namespace TradeGame
{
    class ScheduleComparer : IComparer<double>
    {
        public int Compare(double scheduleAScore, double scheduleBScore)
        {
            if (scheduleAScore == scheduleBScore) // expected utilities are equal
            {
                return 0;
            }
            else if (scheduleAScore > scheduleBScore) // make sure we dequeue higher expected utilities first
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
