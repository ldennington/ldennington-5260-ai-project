namespace TradeGame
{
    class ScheduleComparer : IComparer<double>
    {
        public int Compare(double stateA, double stateB)
        {
            if (stateA == stateB)
            {
                return 0;
            }
            else if (stateA > stateB) // make sure we dequeue highest expected utilities first
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
