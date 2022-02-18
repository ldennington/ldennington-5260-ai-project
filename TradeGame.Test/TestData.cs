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

        public static Country INITIAL_COUNTRY = new Country()
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

        public static Country FINAL_COUNTRY = new Country()
        {
            Name = "Atlantis",
            State = new Dictionary<string, int>()
                {
                    { "population", 80 },
                    { "metallicElements", 900 },
                    { "timber", 600 },
                    { "metallicAlloys", 106 },
                    { "electronics", 380 },
                    { "housing", 170 }
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
                    { "population", 100 },
                    { "metallicElements", 700 },
                    { "timber", 2000 },
                    { "metallicAlloys", 0 },
                    { "electronics", 0 },
                    { "housing", 0 }
                }
            },
            new Country()
            {
                Name = "Brobdingnag",
                State = new Dictionary<string, int>()
                {
                    { "population", 50 },
                    { "metallicElements", 300 },
                    { "timber", 1200 },
                    { "metallicAlloys", 0 },
                    { "electronics", 0 },
                    { "housing", 0 }
                }
            },
            new Country()
            {
                Name = "Carpania",
                State = new Dictionary<string, int>()
                {
                    { "population", 25 },
                    { "metallicElements", 100 },
                    { "timber", 300 },
                    { "metallicAlloys", 0 },
                    { "electronics", 0 },
                    { "housing", 0 }
                }
            },
            new Country()
            {
                Name = "Dinotopia",
                State = new Dictionary<string, int>()
                {
                    { "population", 30 },
                    { "metallicElements", 200 },
                    { "timber", 200 },
                    { "metallicAlloys", 0 },
                    { "electronics", 0 },
                    { "housing", 0 }
                }
            },
            new Country()
            {
                Name = "Erewhon",
                State = new Dictionary<string, int>()
                {
                    { "population", 70 },
                    { "metallicElements", 500 },
                    { "timber", 1700 },
                    { "metallicAlloys", 0 },
                    { "electronics", 0 },
                    { "housing", 0 }
                }
            },
        };

        public static IList<Country> FINAL_STATE = new List<Country>()
        {
            new Country()
            {
                Name = "Atlantis",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "population", 150 },
                    { "metallicElements", 100 },
                    { "timber", 1100 },
                    { "metallicAlloys", 300 },
                    { "electronics", 506 },
                    { "housing", 200 }
                }
            },
            new Country()
            {
                Name = "Brobdingnag",
                State = new Dictionary<string, int>()
                {
                    { "population", 120 },
                    { "metallicElements", 950 },
                    { "timber", 600 },
                    { "metallicAlloys", 1000 },
                    { "electronics", 204 },
                    { "housing", 105 }
                }
            },
            new Country()
            {
                Name = "Carpania",
                State = new Dictionary<string, int>()
                {
                    { "population", 130 },
                    { "metallicElements", 1200 },
                    { "timber", 150 },
                    { "metallicAlloys", 450 },
                    { "electronics", 197 },
                    { "housing", 55 }
                }
            },
            new Country()
            {
                Name = "Dinotopia",
                State = new Dictionary<string, int>()
                {
                    { "population", 35 },
                    { "metallicElements", 986 },
                    { "timber", 122 },
                    { "metallicAlloys", 253 },
                    { "electronics", 157 },
                    { "housing", 68 }
                }
            },
            new Country()
            {
                Name = "Erewhon",
                State = new Dictionary<string, int>()
                {
                    { "population", 220 },
                    { "metallicElements", 1700 },
                    { "timber", 1000 },
                    { "metallicAlloys", 1100 },
                    { "electronics", 656 },
                    { "housing", 180 }
                }
            },
        };

        // a smaller list of countries makes calculation tests easier
        public static IList<Country> TWO_COUNTRY_STATE = new List<Country>()
        {
            new Country()
            {
                Name = "Atlantis",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "population", 150 },
                    { "metallicElements", 100 },
                    { "timber", 1100 },
                    { "metallicAlloys", 300 },
                    { "electronics", 506 },
                    { "housing", 200 }
                }
            },
            new Country()
            {
                Name = "Brobdingnag",
                State = new Dictionary<string, int>()
                {
                    { "population", 120 },
                    { "metallicElements", 950 },
                    { "timber", 600 },
                    { "metallicAlloys", 1000 },
                    { "electronics", 204 },
                    { "housing", 105 }
                }
            },
        };

        public static IList<ITemplate> SCHEDULE = new List<ITemplate>()
        {
            new TransformTemplate()
            {
                Name = "housing",
                Country = "Atlantis",
                Inputs = new Dictionary<string, int>()
                        {
                            { "population", 25 },
                            { "metallicElements", 5 },
                            { "timber", 25 },
                            { "metallicAlloys", 15 },
                        },
                Outputs = new Dictionary<string, int>()
                        {
                            { "housing", 5 },
                            { "housingWaste", 5 },
                            { "population", 25 },
                        }
            },
            new TransformTemplate()
            {
                Name = "electronics",
                Country = "Atlantis",
                Inputs = new Dictionary<string, int>()
                        {
                            { "population", 2 },
                            { "metallicElements", 6 },
                            { "metallicAlloys", 4 },
                        },
                Outputs = new Dictionary<string, int>()
                        {
                            { "electronicsWaste", 1 },
                            { "electronics", 2 },
                            { "population", 2 },
                        }
            },
        };
    }
}
