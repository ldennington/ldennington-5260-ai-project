using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;

namespace TradeGame.Test
{
    [TestClass]
    public class InputOutputTest
    {
        private readonly MockFileSystem fileSystemMock = new MockFileSystem();
        private IReader reader;

        private string resourceInput;
        private string countryInput;
        private string transformTemplateInput;

        [TestInitialize]
        public void Initialize()
        {
            resourceInput = string.Concat("Resource,Weight,Notes\n",
                                        "R1,0,analog to population\n",
                                        "R2,0,analog to metallic elements\n",
                                        "R3,0,analog to timber\n",
                                        "R21,0.2,analog to metallic alloys\n",
                                        "R22,0.5,analog to electronics\n",
                                        "R23,0.8,analog to housing (and housing sufficiency)\n",
                                        "R21',0.5,waste\n",
                                        "R22',0.8,waste\n",
                                        "R23',0.4,waste");

            countryInput = string.Concat(string.Concat("Country,R1,R2,R3,R21,R22,R23\n",
                                        "Atlantis,100,700,2000,0,0,0\n",
                                        "Brobdingnag,50,300,1200,0,0,0\n",
                                        "Carpania,25,100,300,0,0,0\n",
                                        "Dinotopia,30,200,200,0,0,0\n",
                                        "Erewhon,70,500,1700,0,0,0"));

            transformTemplateInput = @"[{
	            ""name"": ""housing"",
	            ""inputs"": {
		            ""population"": ""5"",
		            ""metallicElements"": ""1"",
		            ""timber"": ""5"",
		            ""metallicAlloys"": ""3""
	            },
	            ""outputs"": {
		            ""housing"": ""1"",
		            ""housingWaste"": ""1"",
		            ""population"": ""5""
	            }
            }]";

            reader = new Reader(fileSystemMock);
        }

        [TestMethod]
        public void ReadResourceInput()
        {
            string resourceFilePath = Path.Combine("C:", "resources.csv");
            IDictionary<string, Resource> expectedResources = new Dictionary<string, Resource>()
            {
                { "R1", new Resource() { Name = "R1", Weight = 0, Notes = "analog to population" }},
                { "R2", new Resource() { Name = "R2", Weight = 0, Notes = "analog to metallic elements" }},
                { "R3", new Resource() { Name = "R3", Weight = 0, Notes = "analog to timber" }},
                { "R21", new Resource() { Name = "R21", Weight = 0.2, Notes = "analog to metallic alloys" }},
                { "R22", new Resource() { Name = "R22", Weight = 0.5, Notes = "analog to electronics" }},
                { "R23", new Resource() { Name = "R23", Weight = 0.8, Notes = "analog to housing (and housing sufficiency)" }},
                { "R21'", new Resource() { Name = "R21'", Weight = 0.5, Notes = "waste" }},
                { "R22'", new Resource() { Name = "R22'", Weight = 0.8, Notes = "waste" }},
                { "R23'", new Resource() { Name = "R23'", Weight = 0.4, Notes = "waste" }}
            };
            fileSystemMock.AddFile(resourceFilePath, resourceInput);

            reader.ReadResources(resourceFilePath);
            Global.Resources.Should().BeEquivalentTo(expectedResources);
        }

        [TestMethod]
        public void ReadCountryInput()
        {
            string countryFilePath = Path.Combine("C:", "countries.csv");
            fileSystemMock.AddFile(countryFilePath, countryInput);
            IList<Country> expectedCountriesAndResources = new List<Country>()
                {
                    new Country() { Name = "Atlantis", State = new Dictionary<string, int>()
                        {
                            { "R1",  100 },
                            { "R2",  700 },
                            { "R3",  2000 },
                            { "R21", 0 },
                            { "R22", 0 },
                            { "R23", 0 },
                        }
                    },
                    new Country() { Name = "Brobdingnag", State = new Dictionary<string, int>()
                        {
                            { "R1", 50 },
                            { "R2", 300 },
                            { "R3", 1200 },
                            { "R21", 0 },
                            { "R22", 0 },
                            { "R23", 0 },
                        }
                    },
                    new Country() { Name = "Carpania", State = new Dictionary<string, int>()
                        {
                            {"R1", 25 },
                            {"R2", 100 },
                            {"R3", 300 },
                            {"R21", 0 },
                            {"R22", 0 },
                            {"R23", 0 },
                        }
                    },
                    new Country() { Name = "Dinotopia", State = new Dictionary<string, int>()
                        {
                            { "R1", 30 },
                            { "R2", 200 },
                            { "R3", 200 },
                            { "R21", 0 },
                            { "R22", 0 },
                            { "R23", 0 },
                        }
                    },
                    new Country() { Name = "Erewhon", State = new Dictionary<string, int>()
                        {
                            { "R1", 70 },
                            { "R2", 500 },
                            { "R3", 1700 },
                            { "R21", 0 },
                            { "R22", 0 },
                            { "R23", 0 },
                        }
                    },
                };

            reader.ReadCountries(countryFilePath).Should().BeEquivalentTo(expectedCountriesAndResources);
        }

        [TestMethod]
        public void ReadTransformTemplateInput()
        {
            string templateFilePath = Path.Combine("transform_templates.csv");
            fileSystemMock.AddFile(templateFilePath, transformTemplateInput);
            IList<TransformTemplate> expected = new List<TransformTemplate>()
            {
                new TransformTemplate()
                {
                    Name = "housing",
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
                        { "population", 5 }
                    },
                }
            };

            reader.ReadTransformTemplates(templateFilePath).Should().BeEquivalentTo(expected);
        }
    }
}