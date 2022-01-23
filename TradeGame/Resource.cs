using CsvHelper.Configuration.Attributes;

namespace TradeGame
{
    internal class Resource
    {
        [Name("Resource")]
        public string Name { get; set; }
        public double Weight { get; set; }

        [Optional]
        public string Notes { get; set; }
    }
}
