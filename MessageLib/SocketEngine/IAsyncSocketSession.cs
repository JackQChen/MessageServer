using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using MessageLib.Common;
using MessageLib.SocketBase;
using MessageLib.SocketBase.Logging;
using MessageLib.SocketEngine.AsyncSocket;

namespace MessageLib.SocketEngine
{
    interface IAsyncSocketSessionBase : ILoggerProvider
    {
        SocketAsyncEventArgsProxy SocketAsyncProxy { get; }
        
        Socket Client { get; }
    }

    interface IAsyncSocketSession : IAsyncSocketSessionBase
    {
        void ProcessReceive(SocketAsyncEventArgs e);
    }
}
