using System.Text.Json;
using System.Text.Json.Serialization;

namespace TradeGame
{
    internal class Schedule
    {
        public IList<Action> Steps { get; set; } = new List<Action>();
        public double ExpectedUtility { get; set; }
    }
}
