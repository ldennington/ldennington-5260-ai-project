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
            fileSystemMock.AddFile(resourceFilePath, TestData.RESOURCE_INPUT);

            IReader reader = new Reader(fileSystemMock);
            reader.ReadResources(resourceFilePath);
            Global.Resources.Should().BeEquivalentTo(expectedResources);
        }

        [TestMethod]
        public void ReadCountryInput()
        {
            string countryFilePath = Path.Combine("C:", "countries.csv");
            fileSystemMock.AddFile(countryFilePath, TestData.COUNTRY_INPUT);
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

            IReader reader = new Reader(fileSystemMock);
            reader.ReadCountries(countryFilePath).Should().BeEquivalentTo(expectedCountriesAndResources);
        }

        [TestMethod]
        public void ReadTransformTemplateInput()
        {
            string templateFilePath = Path.Combine("transform_templates.csv");
            fileSystemMock.AddFile(templateFilePath, TestData.TRANSFORM_TEMPLATE_INPUT);
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
            IReader reader = new Reader(fileSystemMock);
            reader.ReadTransformTemplates(templateFilePath).Should().BeEquivalentTo(expected);
        }
    }
}