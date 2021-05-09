﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageLib.SocketBase;
using MessageLib.SocketBase.Command;
using MessageLib.SocketBase.Protocol;

namespace MessageLib.WebSocket.SubProtocol
{
    /// <summary>
    /// SubCommand interface
    /// </summary>
    /// <typeparam name="TWebSocketSession">The type of the web socket session.</typeparam>
    public interface ISubCommand<TWebSocketSession> : ICommand
        where TWebSocketSession : WebSocketSession<TWebSocketSession>, new()
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="requestInfo">The request info.</param>
        void ExecuteCommand(TWebSocketSession session, SubRequestInfo requestInfo);
    }
}
