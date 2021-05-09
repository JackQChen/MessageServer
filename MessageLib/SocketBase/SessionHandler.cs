using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageLib.SocketBase
{
    /// <summary>
    /// Used for session level event handler
    /// </summary>
    /// <typeparam name="TAppSession">the type of the target session</typeparam>
    /// <param name="session">the target session</param>
    public delegate void SessionHandler<in TAppSession>(TAppSession session)
        where TAppSession : IAppSession;

    /// <summary>
    /// Used for session level event handler
    /// </summary>
    /// <typeparam name="TAppSession">the type of the target session</typeparam>
    /// <typeparam name="TParam">the target session</typeparam>
    /// <param name="session">the target session</param>
    /// <param name="value">the event parameter</param>
    public delegate void SessionHandler<in TAppSession, TParam>(TAppSession session, TParam value)
        where TAppSession : IAppSession;
}
