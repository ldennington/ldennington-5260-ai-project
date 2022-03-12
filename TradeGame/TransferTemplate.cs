using System.Text.Json.Serialization;

namespace TradeGame
{
    /* The TransferTemplate class was adapted from Katherine Studzinski's
       explanation of her solution at https://piazza.com/class/kyz01i5gip25bn?cid=60_f1 */
    internal class TransferTemplate : Action
    {
        [JsonPropertyName("Transferring Country")]
        public string TransferringCountry { get; set; }
        [JsonPropertyName("Receiving Country")]
        public string ReceivingCountry { get; set; }
        public string Resource { get; set; }
        public int Amount { get; set; }

        public void Execute(IList<Country> state)
        {
            Country receivingCountry = state.Where(c => c.Name == ReceivingCountry).FirstOrDefault();
            Country transferringCountry = state.Where(c => c.Name == TransferringCountry).FirstOrDefault();

            receivingCountry.State[Resource] = receivingCountry.State[Resource] += Amount;
            transferringCountry.State[Resource] = transferringCountry.State[Resource] -= Amount;
        }
    }
}
