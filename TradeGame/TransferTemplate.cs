namespace TradeGame
{
    /* adapted from Katherine Studzinski's explanation of her solution
    at https://piazza.com/class/kyz01i5gip25bn?cid=60_f1 */
    internal class TransferTemplate : IAction
    {
        public string TransferringCountry { get; set; }
        public string ReceivingCountry { get; set; }
        public Resource Resource { get; set; }
        public int Amount { get; set; }
    }
}
