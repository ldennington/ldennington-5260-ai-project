using System.Collections.Generic;

namespace TradeGame.Test
{
    internal static class TestData
    {
        public static string RESOURCE_INPUT = string.Concat("Resource,Weight,Notes\n",
                                        "R1,0,analog to population\n",
                                        "R2,0,analog to metallic elements\n",
                                        "R3,0,analog to timber\n",
                                        "R21,0.2,analog to metallic alloys\n",
                                        "R22,0.5,analog to electronics\n",
                                        "R23,0.8,analog to housing (and housing sufficiency)\n",
                                        "R21',0.5,waste\n",
                                        "R22',0.8,waste\n",
                                        "R23',0.4,waste");

        public static string COUNTRY_INPUT = string.Concat("Country,R1,R2,R3,R21,R22,R23\n",
                                        "Atlantis,100,700,2000,0,0,0\n",
                                        "Brobdingnag,50,300,1200,0,0,0\n",
                                        "Carpania,25,100,300,0,0,0\n",
                                        "Dinotopia,30,200,200,0,0,0\n",
                                        "Erewhon,70,500,1700,0,0,0");


        public static string TRANSFORM_TEMPLATE_INPUT = @"[{
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

        public static TransformTemplate TRANSFORM_TEMPLATE = new TransformTemplate()
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
                        { "population", 5 },
                    }
        };

        public static Country COUNTRY = new Country()
        {
            Name = "Atlantis",
            State = new Dictionary<string, int>()
                {
                    { "population", 50 },
                    { "metallicElements", 300 },
                    { "timber", 1200 },
                    { "metallicAlloys", 3 },
                    { "electronics", 0 },
                    { "housing", 0 }
                }
        };

        public static IList<Country> INITIAL_STATE = new List<Country>()
        {
            new Country()
            {
                Name = "Atlantis",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "Population", 100 },
                    { "Metallic Elements", 700 },
                    { "Timber", 2000 },
                    { "Metallic Alloys", 0 },
                    { "Electronics", 0 },
                    { "Housing", 0 }
                }
            },
            new Country()
            {
                Name = "Brobdingnag",
                State = new Dictionary<string, int>()
                {
                    { "Population", 50 },
                    { "Metallic Elements", 300 },
                    { "Timber", 1200 },
                    { "Metallic Alloys", 0 },
                    { "Electronics", 0 },
                    { "Housing", 0 }
                }
            },
            new Country()
            {
                Name = "Carpania",
                State = new Dictionary<string, int>()
                {
                    { "Population", 25 },
                    { "Metallic Elements", 100 },
                    { "Timber", 300 },
                    { "Metallic Alloys", 0 },
                    { "Electronics", 0 },
                    { "Housing", 0 }
                }
            },
            new Country()
            {
                Name = "Dinotopia",
                State = new Dictionary<string, int>()
                {
                    { "Population", 30 },
                    { "Metallic Elements", 200 },
                    { "Timber", 200 },
                    { "Metallic Alloys", 0 },
                    { "Electronics", 0 },
                    { "Housing", 0 }
                }
            },
            new Country()
            {
                Name = "Erewhon",
                State = new Dictionary<string, int>()
                {
                    { "Population", 70 },
                    { "Metallic Elements", 500 },
                    { "Timber", 1700 },
                    { "Metallic Alloys", 0 },
                    { "Electronics", 0 },
                    { "Housing", 0 }
                }
            },
        };
    }
}
