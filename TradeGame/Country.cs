using CsvHelper.Configuration.Attributes;

namespace TradeGame
{
    /* The Country class was adapted from Jeff Baranski's Python file parser
    at https://gist.github.com/jbaranski/209d475c21fe0459c2499ed606cfad9b */
    public class Country
    {
        [Name("Country")]
        public string Name { get; set; }

        [Name("Self")]
        public bool IsSelf { get; set; }

        public IDictionary<string, int> State { get; set; }

        public double StateQuality { get; set; }
    }
}
