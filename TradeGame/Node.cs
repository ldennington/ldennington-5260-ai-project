namespace TradeGame
{
    public class Node
    {
        public Node? Parent { get; set; } = null;
        public int Depth
        {
            get
            {
                if (Parent != null)
                    return Parent.Depth + 1;
                else
                    return 0;
            }
        }
        public IList<Country> State { get; set; } = new List<Country>();
        public Schedule Schedule { get; set; } = new Schedule();

        public Node DeepCopy()
        {
            Node nodeCopy = new Node()
            {
                Parent = this
            };

            // ensure we make a copy of every non-primitive property
            IList<Country> state = new List<Country>();
            foreach (Country country in State)
            {
                Country countryCopy = new Country()
                {
                    Name = country.Name,
                    IsSelf = country.IsSelf,
                    State = new Dictionary<string, int>(),
                    // reset state quality for correct comparison in Calculator
                    StateQuality = 0.0,
                };

                foreach (KeyValuePair<string, int> resourceAndAmount in country.State)
                {
                    countryCopy.State.Add(resourceAndAmount);
                }

                state.Add(countryCopy);
            }

            nodeCopy.State = state;

            Schedule schedule = new Schedule();
            foreach (Action action in Schedule.Actions)
            {
                schedule.Actions.Add(action);
            }
            nodeCopy.Schedule = schedule;

            return nodeCopy;
        }
    }
}
