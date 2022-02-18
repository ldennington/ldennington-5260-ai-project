using System.Text.Json.Serialization;

namespace TradeGame
{
    // ensures we can deserialize numbers as numbers instead of strings
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    internal class TransformTemplate : ITemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("inputs")]
        public IDictionary<string, int> Inputs { get; set; }
        [JsonPropertyName("outputs")]
        public IDictionary<string, int> Outputs { get; set; }
        [JsonIgnore]
        public string Country { get; set; }
        [JsonIgnore]
        public int Scale { get; set; }

        public TransformTemplate DeepCopy()
        {
            return new TransformTemplate()
            {
                Name = this.Name,
                Inputs = this.Inputs,
                Outputs = this.Outputs
            };
        }
    }
}
