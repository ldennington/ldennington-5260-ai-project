namespace TradeGame
{
    public interface IReader
    {
         void ReadResources(string path);

         void ReadCountries(string path);

         void ReadTransformTemplates(string path);
    }
}
