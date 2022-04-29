using System.Text.Json;
using System.Text.Json.Serialization;

namespace TradeGame
{
    /* The Action class was adapted from Katherine Studzinski's explanation
       of her solution at https://piazza.com/class/kyz01i5gip25bn?cid=60_f1 */

    [JsonConverter(typeof(ActionConverter))]
    public abstract class Action
    {
        [JsonPropertyName("Type")]
        public string? Type { get; set; }
        [JsonPropertyName("Expected Utility")]
        public double ExpectedUtility { get; set; }

        [JsonIgnore]
        public bool ShouldKeep { get; set; } = true;
    }

    // System.Text.Json cannot serialize/deserialize abstract classes.
    // this custom converter allows us to get the underlying types
    // (TransferTemplate/TransformTemplate) and work with those instead
    internal class ActionConverter : JsonConverter<Action>
    {
        public override Action Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // only need to override write
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Action value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    JsonSerializer.Serialize(writer, (Action)null, options);
                    break;
                default:
                    {
                        var type = value.GetType();

                        switch(type.Name)
                        {
                            case "TransformTemplate":
                                value.Type = "transform";
                                break;
                            case "TransferTemplate":
                                value.Type = "transfer";
                                break;
                        }

                        JsonSerializer.Serialize(writer, value, type, options);
                        break;
                    }
            }
        }
    }
}
