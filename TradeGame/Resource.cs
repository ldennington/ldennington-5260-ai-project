using CsvHelper.Configuration.Attributes;

namespace TradeGame
{
    /* The Resource class was adapted from Jeff Baranski's Python file parser
    at https://gist.github.com/jbaranski/209d475c21fe0459c2499ed606cfad9b */
    public class Resource
    {
        [Name("Resource")]
        public string Name { get; set; }
        public double Weight { get; set; } = 0.0;

        [Optional]
        public string Notes { get; set; } = "";
    }
}
