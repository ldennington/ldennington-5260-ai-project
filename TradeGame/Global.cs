namespace TradeGame
{
    internal class Global
    {
        public static IDictionary<string, Resource> Resources = new Dictionary<string, Resource>();

        public static IList<IAction> TransformTemplates = new List<IAction>();
    }
}
