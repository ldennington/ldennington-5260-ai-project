namespace TradeGame
{
    public interface IWriter
    {
        void WriteSchedules();

        void WritePredictedAndActualEUs(double predicted, double actual);
    }
}
