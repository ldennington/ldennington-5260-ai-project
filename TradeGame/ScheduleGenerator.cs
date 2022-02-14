namespace TradeGame
{
    internal class ScheduleGenerator
    {
        public void GenerateSuccessors(IList<Country> world, IList<TransformTemplate> transformTemplates)
        {
            IList<IList<Country>> states = new List<IList<Country>>();
            PriorityQueue<ITemplate, double> schedulesAndUtilities = new PriorityQueue<ITemplate, double>();
            // ensure all transactions involve self to limit state space
            Country self = world.Where(c => c.IsSelf).FirstOrDefault();

            // iterate over transform templates
            foreach (TransformTemplate template in transformTemplates)
            {
                TransformTemplate grounded = template.DeepCopy();
                grounded.Country = self.Name;

                // maximize the number of resources in transform to limit state space
                SetScale(grounded, self);
                ExecuteTransform(grounded, self);

                schedulesAndUtilities.Enqueue(grounded, 0);
                states.Add(world);
            }
            TransferTemplate transferTemplate = new TransferTemplate();
        }

        public void SetScale(TransformTemplate template, Country country)
        {
            int[] maxes = new int[template.Inputs.Count];
            int i = 0;
            foreach (string resource in template.Inputs.Keys)
            {
                if (!country.State.Keys.Contains(resource))
                {
                    maxes[i] = 0;
                }
                else
                {
                    maxes[i] = country.State[resource] / template.Inputs[resource];
                }
                i++;
            }

            template.Scale = maxes.Min();
        }

        public void ExecuteTransform(TransformTemplate template, Country country)
        {
            foreach (string resource in template.Inputs.Keys)
            {
                country.State[resource] -= template.Inputs[resource] * template.Scale;
            }

            foreach (string resource in template.Outputs.Keys)
            {
                if (!country.State.Keys.Contains(resource))
                {
                    country.State.Add(resource, 0);
                }

                country.State[resource] += template.Outputs[resource] * template.Scale;
            }
        }
    }
}
