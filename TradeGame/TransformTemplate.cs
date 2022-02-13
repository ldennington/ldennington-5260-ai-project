using Newtonsoft.Json;

namespace TradeGame
{
    internal class TransformTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("inputs")]
        public IDictionary<string, int> Inputs { get; set; }
        [JsonProperty("outputs")]
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
