using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageLib.SocketBase.Metadata;

namespace MessageLib.SocketBase
{
    /// <summary>
    /// StatusInfo source interface
    /// </summary>
    public interface IStatusInfoSource
    {
        /// <summary>
        /// Gets the server status metadata.
        /// </summary>
        /// <returns></returns>
        StatusInfoAttribute[] GetServerStatusMetadata();

        /// <summary>
        /// Collects the bootstrap status.
        /// </summary>
        /// <param name="bootstrapStatus">The bootstrap status.</param>
        /// <returns></returns>
        StatusInfoCollection CollectServerStatus(StatusInfoCollection bootstrapStatus);

        /// <summary>
        /// Update the bootstrap status.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void UpdateServerStatus(string name, object value);
    }
}
