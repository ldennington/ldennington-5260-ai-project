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
        public IList<Action> Steps { get; set; } = new List<Action>();
    }
}
