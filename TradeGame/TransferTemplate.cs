namespace TradeGame
{
    /* The TransferTemplate class was adapted from Katherine Studzinski's
       explanation of her solution at https://piazza.com/class/kyz01i5gip25bn?cid=60_f1 */
    internal class TransferTemplate : Action
    {
        public string TransferringCountry { get; set; }
        public string ReceivingCountry { get; set; }
        public Resource Resource { get; set; }
        public int Amount { get; set; }
        public string Type { get; } = "transfer";
    }
}
