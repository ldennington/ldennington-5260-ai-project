namespace TradeGame
{
    internal class Env : IEnv
    {
        public string Get(string name)
        {
            return Get(name, EnvironmentVariableTarget.Process);
        }

        public string Get(string name, EnvironmentVariableTarget environmentVariableTarget)
        {
            return Environment.GetEnvironmentVariable(name, environmentVariableTarget);
        }
    }
}
