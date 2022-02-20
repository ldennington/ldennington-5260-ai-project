namespace TradeGame
{
    public interface IEnv
    {
        string Get(string name);
        string Get(string name, EnvironmentVariableTarget environmentVariableTarget);
    }
}
