using System.Text.Json.Serialization;

namespace TradeGame
{
    /* The TransformTemplate class was adapted from Katherine Studzinski's explanation
       of her solution at https://piazza.com/class/kyz01i5gip25bn?cid=60_f1 */

    // ensures we can deserialize numbers as numbers instead of strings
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    internal class TransformTemplate : Action
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Inputs")]
        public IDictionary<string, int> Inputs { get; set; }
        [JsonPropertyName("Outputs")]
        public IDictionary<string, int> Outputs { get; set; }
        [JsonPropertyName("Country")]
        public string Country { get; set; }
        [JsonIgnore]
        public int Scale { get; set; }

        public TransformTemplate DeepCopy()
        {
            TransformTemplate copy = new TransformTemplate()
            {
                Name = Name,
                Inputs = new Dictionary<string, int>(),
                Outputs = new Dictionary<string, int>()
            };

            foreach (KeyValuePair<string, int> input in Inputs)
            {
                copy.Inputs.Add(input);
            }

            foreach (KeyValuePair<string, int> output in Outputs)
            {
                copy.Outputs.Add(output);
            }

            return copy;
        }

        /* The Scale-related methods help satisfy preconditions and were adapted from
           Jeff Baranski's explanation of his solution at
           https://piazza.com/class/kyz01i5gip25bn?cid=60_f2 */
        public void SetScale(Country country)
        {
            CalculateScale(country);
            AdjustForScale();
        }

        public void CalculateScale(Country country)
        {
            int[] maxes = new int[Inputs.Count];
            int i = 0;
            foreach (string resource in Inputs.Keys)
            {
                if (!country.State.Keys.Contains(resource) || country.State[resource] == 0)
                {
                    maxes[i] = 0;
                }
                else
                {
                    maxes[i] = country.State[resource] / Inputs[resource];
                }
                i++;
            }

            Scale = maxes.Min();
        }

        public void AdjustForScale()
        {
            foreach (string resource in Inputs.Keys)
            {
                Inputs[resource] *= Scale;
            }

            foreach (string resource in Outputs.Keys)
            {
                Outputs[resource] *= Scale;
            }
        }

        public void Execute(Country country)
        {
            foreach (string resource in Inputs.Keys)
            {
                country.State[resource] -= Inputs[resource];
            }

            foreach (string resource in Outputs.Keys)
            {
                if (!country.State.Keys.Contains(resource))
                {
                    country.State.Add(resource, 0);
                }

                country.State[resource] += Outputs[resource];
            }
        }
    }
}
