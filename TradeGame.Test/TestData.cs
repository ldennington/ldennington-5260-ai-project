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
    }
}
