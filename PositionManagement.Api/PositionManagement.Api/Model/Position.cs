namespace PositionManagement.Api.Model {
    public class Position {
        public required string SecurityCode { get; set; }
        public required int Quantity { get; set; }
        public required Call Call { get; set; }
    }

    public enum Call {
        Buy,
        Sell
    }
}
