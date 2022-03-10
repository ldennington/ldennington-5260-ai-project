namespace TradeGame
{
    public class Schedule
    {
        public IList<Action> Steps { get; set; } = new List<Action>();
        public double ExpectedUtility { get; set; }
    }
}
