namespace TradeGame
{
    internal interface IWriter
    {
        void WriteSchedules(PriorityQueue<Schedule, double> schedules);
    }
}
