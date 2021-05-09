namespace MessageLib.SocketBase.Logging
{
    /// <summary>
    /// Service log factory
    /// </summary>
    public class ServiceLogFactory : ILogFactory
    {
        public ILog GetLog(string name)
        {
            return new ServiceLog(name);
        }
    }
}
