using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;

namespace TradeGame.Test
{
    [TestClass]
    public class InputOutputTest
    {
        private readonly MockFileSystem fileSystemMock = new MockFileSystem();

        [TestMethod]
        public void ReadResourceInput()
        {
            string resourceFilePath = Path.Combine("C:", "resources.csv");
            IList<Resource> expectedResources = new List<Resource>()
            {
                new Resource() { Name = "R1", Weight = 0, Notes = "analog to population" },
                new Resource() { Name = "R2", Weight = 0, Notes = "analog to metallic elements" },
                new Resource() { Name = "R3", Weight = 0, Notes = "analog to timber" },
                new Resource() { Name = "R21", Weight = 0.2, Notes = "analog to metallic alloys" },
                new Resource() { Name = "R22", Weight = 0.5, Notes = "analog to electronics" },
                new Resource() { Name = "R23", Weight = 0.8, Notes = "analog to housing (and housing sufficiency)" },
                new Resource() { Name = "R21'", Weight = 0.5, Notes = "waste" },
                new Resource() { Name = "R22'", Weight = 0.8, Notes = "waste" },
                new Resource() { Name = "R23'", Weight = 0.4, Notes = "waste" }
            };
            fileSystemMock.AddFile(resourceFilePath, TestData.RESOURCE_INPUT);

            IReader reader = new Reader(fileSystemMock);
            reader.ReadCsv(resourceFilePath).Should().BeEquivalentTo(expectedResources);
        }

    }
}