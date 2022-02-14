namespace TradeGame
{
    class ScheduleComparer : IComparer<double>
    {
        public int Compare(double scheduleAScore, double scheduleBScore)
        {
            if (scheduleAScore == scheduleBScore) // scores are equal
            {
                return 0;
            }
            else if (scheduleAScore > scheduleBScore) // make sure we dequeue larger scores first
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
