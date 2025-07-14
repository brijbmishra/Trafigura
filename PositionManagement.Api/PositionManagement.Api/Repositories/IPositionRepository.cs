using Dapper;
using Microsoft.Extensions.Options;
using PositionManagement.Api.Repositories.Entities;

namespace PositionManagement.Api.Repositories {
    public interface IPositionRepository {
        Task<List<PositionEntity>> GetPostionsAsync();
        Task<List<PositionEntity>> GetPositionBySecurityCodeAsync(string securityCode);
        Task CreatePositionAsync(PositionEntity positionEntity);
        Task<int> GetLastTradeIdAsync();
    }

    public class PositionRepository : Repository, IPositionRepository {
        public PositionRepository(IOptions<ConnectionStringProvider> connectionStringProvider) : base(connectionStringProvider) { }

        public async Task CreatePositionAsync(PositionEntity positionEntity) {
            var sql = @"
                INSERT INTO dbo.Positions (TradeId, Version, SecurityCode, Quantity, Call)
                VALUES (@TradeId, @Version, @SecurityCode, @Quantity, @Call)";

            await _dbConnection.ExecuteAsync(sql, positionEntity);
        }

        public async Task<int> GetLastTradeIdAsync() {
            var sql = "select MAX(TradeId) from Positions";

            var maxTradeId = await _dbConnection.ExecuteScalarAsync<int?>(sql);
            // Assuming the value is a single integer or null
            if (maxTradeId.HasValue) {
                return maxTradeId.Value;
            } else {
                return 0;
            }
        }

        public async Task<List<PositionEntity>> GetPositionBySecurityCodeAsync(string securityCode) {
            if (string.IsNullOrEmpty(securityCode)) {
                throw new ArgumentException("Security code cannot be null or empty.", nameof(securityCode));
            }

            var sql = "SELECT * from dbo.Positions where SecurityCode = @sc";

            var positionEntities = await _dbConnection.QueryAsync<PositionEntity>(sql, new { sc = securityCode });
            if (positionEntities == null || !positionEntities.Any()) {
                return new List<PositionEntity>(); // or throw an exception if you prefer
            } else {
                return [.. positionEntities];
            }
        }

        public async Task<List<PositionEntity>> GetPostionsAsync() {
            var sql = "SELECT * from dbo.Positions";

            var positionEntities = await _dbConnection.QueryAsync<PositionEntity>(sql);
            if (positionEntities == null || !positionEntities.Any()) {
                return new List<PositionEntity>(); // or throw an exception if you prefer
            } else {
                return [.. positionEntities];
            }
        }
    }
}