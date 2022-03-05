namespace TradeGame
{
    internal class Node
    {
        public Node Parent { get; set; } = null;
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
            Node copy = new Node()
            {
                Parent = this
            };

            IList<Country> state = new List<Country>();
            foreach (Country country in State)
            {
                state.Add(country);
            }
            copy.State = state;

            Schedule schedule = new Schedule();
            foreach (Action action in Schedule.Steps)
            {
                schedule.Steps.Add(action);
            }
            schedule.ExpectedUtility = Schedule.ExpectedUtility;
            copy.Schedule = schedule;

            return copy;
        }
    }
}
