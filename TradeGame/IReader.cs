﻿namespace TradeGame
{
    internal interface IReader
    {
        public IDictionary<string, Resource> ReadResources(string path);

        public IList<Country> ReadCountries(string path);

        public IList<TransformTemplate> ReadTransformTemplates(string path);
    }
}
