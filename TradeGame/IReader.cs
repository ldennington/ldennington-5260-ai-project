namespace TradeGame
{
    internal interface IReader
    {
        public IList<Resource> ReadCsv(string path);
    }
}
