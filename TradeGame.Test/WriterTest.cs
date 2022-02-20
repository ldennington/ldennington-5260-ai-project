using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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

            PriorityQueue<Schedule, double> schedules = new PriorityQueue<Schedule, double>();
            schedules.Enqueue(new Schedule()
            {
                ExpectedUtility = 14.2,
                Steps = new List<Action>() {
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
                        }
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
                        }
                    }
                }
            }, 14.2);

            schedules.Enqueue(new Schedule()
            {
                ExpectedUtility = 14.2,
                Steps = new List<Action>() {
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
                        }
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
                        }
                    }
                }
            }, 9.5);

            string expectedOutput = @"[
                {
                  ""Steps"": [
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
                      ""Type"": ""transform""
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
                      ""Type"": ""transform""
                    }
                  ],
                  ""ExpectedUtility"": 14.2
                },
                {
                  ""Steps"": [
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
                      ""Type"": ""transform""
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
                      ""Type"": ""transform""
                    }
                  ],
                  ""ExpectedUtility"": 14.2
                }
              ]";

            IWriter writer = new Writer(fileSystemMock);
            writer.WriteSchedules(schedules);
            fileSystemMock.AllFiles.Should().Contain(@"C:\output_schedules.json");
            // use Regex to remove all whitespace from strings for comparison
            Regex.Replace(fileSystemMock.File.ReadAllText(@"C:\output_schedules.json"), @"\s+", "").Should().BeEquivalentTo(Regex.Replace(expectedOutput, @"\s+", ""));
        }
    }
}
