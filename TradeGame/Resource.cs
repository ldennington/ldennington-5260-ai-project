using CsvHelper.Configuration.Attributes;
using System.Text.Json.Serialization;

namespace TradeGame
{
    public class Resource
    {
        [Name("Resource")]
        public string Name { get; set; }
        public double Weight { get; set; } = 0.0;

        [Optional]
        public string Notes { get; set; } = "";
    }
}
