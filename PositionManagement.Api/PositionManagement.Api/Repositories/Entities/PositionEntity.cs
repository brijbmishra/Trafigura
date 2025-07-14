namespace PositionManagement.Api.Repositories.Entities {
    public class PositionEntity {
        public int TansactionId { get; set; }
        public int TradeId { get; set; }
        public int Version { get; set; }
        public string SecurityCode { get; set; }
        public int Quantity { get; set; }
        public string Call { get; set; }
    }
}
