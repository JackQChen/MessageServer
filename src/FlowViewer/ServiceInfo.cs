using System;

namespace MessageServer.Extension
{
    internal class ServiceInfo : MarshalByRefObject
    {
        internal event Action<DateTime, int, long, long> Updated;

        public ServiceInfo()
        {
        }

        public void Update(DateTime dateTime, int connectionCount, long sendingSpeed, long receivingSpeed)
        {
            if (Updated != null)
                Updated(dateTime, connectionCount, sendingSpeed, receivingSpeed);
        }
    }
}
