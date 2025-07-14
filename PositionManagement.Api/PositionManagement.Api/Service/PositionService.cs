using PositionManagement.Api.Model;
using PositionManagement.Api.Repositories;
using PositionManagement.Api.Repositories.Entities;

namespace PositionManagement.Api.Service {
    public interface IPositionService {
        Task<List<CurrentPosition>> GetPositions();
        Task InsertAsync(Position position);
        Task UpdateAsync(Position position);
        Task CancelAsync(Position position);
    }

    public class PositionService : IPositionService {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository) {
            _positionRepository = positionRepository ?? throw new ArgumentNullException(nameof(positionRepository));
        }

        public async Task<List<CurrentPosition>> GetPositions() {
            var positions = await _positionRepository.GetPostionsAsync();
            if (positions == null || positions.Count == 0) {
                return new List<CurrentPosition>();
            }

            return positions
                .GroupBy(p => p.SecurityCode)
                .Select(g => new CurrentPosition {
                    SecurityCode = g.Key,
                    Quantity = g.Sum(p => p.Quantity)
                })
                .ToList();
        }

        public async Task CancelAsync(Position position) {
            var positions = await _positionRepository.GetPositionBySecurityCodeAsync(position.SecurityCode);
            if (positions == null || positions.Count() == 0) {
                // Move to TempPositions and return
            }

            var currentPosition = positions.LastOrDefault(p => p.Version == 1);
            if (currentPosition == null) {
                // Move to TempPositions and return
            }

            var tradeId = currentPosition.TradeId;
            var maxVersion = positions.Max(p => p.Version);
            var quantity = positions.Sum(p => p.Quantity) * -1;
            var positionEntity = new PositionEntity {
                TradeId = tradeId,
                Version = maxVersion + 1,
                SecurityCode = position.SecurityCode,
                Quantity = quantity, // Cancel the entire position
                Call = quantity > 0 ? "Buy" : "Sell" // If quantity is negative, it means we are selling the position
            };
            await _positionRepository.CreatePositionAsync(positionEntity);
        }

        public async Task InsertAsync(Position position) {
            // Create position
            var lastTradeId = await _positionRepository.GetLastTradeIdAsync();
            var positionEntity = new PositionEntity {
                TradeId = lastTradeId + 1,
                Version = 1,
                SecurityCode = position.SecurityCode,
                Quantity = position.Call == Call.Buy ? position.Quantity : -position.Quantity,
                Call = getCall(position.Call)
            };
            await _positionRepository.CreatePositionAsync(positionEntity);
        }

        public async Task UpdateAsync(Position position) {
            var positions = await _positionRepository.GetPositionBySecurityCodeAsync(position.SecurityCode);
            if (positions == null || positions.Count() == 0) {
                // Move to TempPositions and return
            }
            var currentPosition = positions.LastOrDefault(p => p.Version == 1);
            if (currentPosition == null) {
                // Move to TempPositions and return
            }

            var tradeId = currentPosition.TradeId;
            var maxVersion = positions.Max(p => p.Version);
            var positionEntity = new PositionEntity {
                TradeId = tradeId,
                Version = maxVersion + 1,
                SecurityCode = position.SecurityCode,
                Quantity = position.Call == Call.Buy ? position.Quantity : -position.Quantity,
                Call = getCall(position.Call)
            };
            await _positionRepository.CreatePositionAsync(positionEntity);

        }

        private string getCall(Call call) {
            return call switch {
                Call.Buy => "Buy",
                Call.Sell => "Sell",
                _ => throw new ArgumentOutOfRangeException(nameof(call), call, null)
            };
        }
    }
}
