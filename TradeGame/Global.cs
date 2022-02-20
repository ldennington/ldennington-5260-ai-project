namespace TradeGame
{
    internal class Global
    {
        public static IDictionary<string, Resource> Resources = new Dictionary<string, Resource>();

        public static IList<TransformTemplate> TransformTemplates = new List<TransformTemplate>();

        public static IList<Country> InitialState = new List<Country>();

        public static PriorityQueue<Schedule, double> Schedules = new PriorityQueue<Schedule, double>(new ScheduleComparer());
    }
}
