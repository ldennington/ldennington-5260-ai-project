using System.Text.Json.Serialization;

namespace TradeGame
{
    internal class TransformTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("inputs")]
        [JsonConverter(typeof(ResourceConverter))]
        public IDictionary<Resource, int> Inputs { get; set; }
        [JsonPropertyName("outputs")]
        [JsonConverter(typeof(ResourceConverter))]
        public IDictionary<Resource, int> Outputs { get; set; }
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
