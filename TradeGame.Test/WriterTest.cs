using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Text.RegularExpressions;

namespace TradeGame.Test
{
    [TestClass]
    public class WriterTest
    {
        [TestMethod]
        public void WriteSchedules()
        {
            MockFileSystem fileSystemMock = new MockFileSystem();
            Mock<IEnv> envMock = new Mock<IEnv>();

            string tempDirectory = Path.Combine("C:", "temp");
            envMock.Setup(e => e.Get("TEMP")).Returns(tempDirectory);
            fileSystemMock.Directory.CreateDirectory(tempDirectory);

            Global.Solutions.Enqueue(new Schedule()
            {
                Actions = new List<Action>() {
                    new TransformTemplate()
                    {
                        Name = "housing",
                        Country = "atlantis",
                        Inputs = new Dictionary<string, int>()
                        {
                            { "population", 5 },
                            { "metallicElements", 1 },
                            { "timber", 5 },
                            { "metallicAlloys", 3 },
                        },
                        Outputs = new Dictionary<string, int>()
                        {
                            { "housing", 1 },
                            { "housingWaste", 1 },
                            { "population", 5 },
                        },
                        ExpectedUtility = 14.5
                    },
                    new TransformTemplate()
                    {
                        Name = "electronics",
                        Country = "atlantis",
                        Inputs = new Dictionary<string, int>()
                        {
                            { "population", 1 },
                            { "metallicElements", 3 },
                            { "metallicAlloys", 2 },
                        },
                        Outputs = new Dictionary<string, int>()
                        {
                            { "population", 1 },
                            { "electronics", 2 },
                            { "electronicsWaste", 1 },
                        },
                        ExpectedUtility = 11.2
                    }
                }
            }, 11.2);

            Global.Solutions.Enqueue(new Schedule()
            {
                Actions = new List<Action>() {
                    new TransformTemplate()
                    {
                        Name = "electronics",
                        Country = "atlantis",
                        Inputs = new Dictionary<string, int>()
                        {
                            { "population", 1 },
                            { "metallicElements", 3 },
                            { "metallicAlloys", 2 },
                        },
                        Outputs = new Dictionary<string, int>()
                        {
                            { "population", 1 },
                            { "electronics", 2 },
                            { "electronicsWaste", 1 },
                        },
                        ExpectedUtility = 7.9
                    },
                    new TransformTemplate()
                    {
                        Name = "alloys",
                        Country = "atlantis",
                        Inputs = new Dictionary<string, int>()
                        {
                            { "population", 1 },
                            { "metallicElements", 2 },
                        },
                        Outputs = new Dictionary<string, int>()
                        {
                            { "population", 1 },
                            { "metallicAlloys", 1 },
                            { "metallicAlloysWaste", 1 },
                        },
                        ExpectedUtility = 10.0
                    }
                }
            }, 10.0);

            string expectedOutput = @"[
                {
                  ""Actions"": [
                    {
                      ""name"": ""housing"",
                      ""inputs"": {
                        ""population"": 5,
                        ""metallicElements"": 1,
                        ""timber"": 5,
                        ""metallicAlloys"": 3
                      },
                      ""outputs"": {
                        ""housing"": 1,
                        ""housingWaste"": 1,
                        ""population"": 5
                      },
                      ""country"": ""atlantis"",
                      ""type"": ""transform"",

                      ""expectedUtility"": 14.5
                    },
                    {
                      ""name"": ""electronics"",
                      ""inputs"": {
                        ""population"": 1,
                        ""metallicElements"": 3,
                        ""metallicAlloys"": 2
                      },
                      ""outputs"": {
                        ""population"": 1,
                        ""electronics"": 2,
                        ""electronicsWaste"": 1
                      },
                      ""country"": ""atlantis"",
                      ""type"": ""transform"",
                      ""expectedUtility"": 11.2
                    }
                  ]
                },
                {
                  ""Actions"": [
                    {
                      ""name"": ""electronics"",
                      ""inputs"": {
                        ""population"": 1,
                        ""metallicElements"": 3,
                        ""metallicAlloys"": 2
                      },
                      ""outputs"": {
                        ""population"": 1,
                        ""electronics"": 2,
                        ""electronicsWaste"": 1
                      },
                      ""country"": ""atlantis"",
                      ""type"": ""transform"",
                      ""expectedUtility"": 7.9
                    },
                    {
                      ""name"": ""alloys"",
                      ""inputs"": {
                        ""population"": 1,
                        ""metallicElements"": 2
                      },
                      ""outputs"": {
                        ""population"": 1,
                        ""metallicAlloys"": 1,
                        ""metallicAlloysWaste"": 1
                      },
                      ""country"": ""atlantis"",
                      ""type"": ""transform"",
                      ""expectedUtility"": 10       
                    }
                  ]
                }
              ]";

            IWriter writer = new Writer(fileSystemMock, envMock.Object);
            writer.WriteSchedules();

            string fullPath = Path.Combine(tempDirectory, "output-schedules.json");
            fileSystemMock.AllFiles.Should().Contain(fullPath);
            // use Regex to remove all whitespace from strings for comparison
            Regex.Replace(fileSystemMock.File.ReadAllText(fullPath), @"\s+", "").Should().BeEquivalentTo(Regex.Replace(expectedOutput, @"\s+", ""));
        }
    }
}
