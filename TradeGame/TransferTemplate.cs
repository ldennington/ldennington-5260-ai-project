namespace TradeGame
{
    internal class TransferTemplate : ITemplate
    {
        public string TransferringCountry { get; set; }
        public string ReceivingCountry { get; set; }
        public Resource Resource { get; set; }
        public int Amount { get; set; }
    }
}
