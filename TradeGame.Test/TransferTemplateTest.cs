using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TradeGame.Test
{
    [TestClass]
    public class TransferTemplateTest
    {
        [TestMethod]
        public void Execute()
        {
            TransferTemplate transferTemplate = new TransferTemplate()
            {
                TransferringCountry = "Erewhon",
                ReceivingCountry = "Atlantis",
                Resource = "metallicElements",
                Amount = 5
            };

            IList<Country> countries = new List<Country>()
            {
                new Country() { Name = "Atlantis", State = new Dictionary<string, int>()
                    {
                        { "metallicElements",  10 },
                    }
                },
                new Country() { Name = "Erewhon", State = new Dictionary<string, int>()
                    {
                        { "metallicElements", 15 },
                    }
                }
            };

            transferTemplate.Execute(countries);

            Country receiver = countries.FirstOrDefault(c => c.Name == "Atlantis");
            receiver.State["metallicElements"].Should().Be(15);

            Country transferrer = countries.Where(c => c.Name == "Erewhon").FirstOrDefault();
            transferrer.State["metallicElements"].Should().Be(10);

        }
    }
}
