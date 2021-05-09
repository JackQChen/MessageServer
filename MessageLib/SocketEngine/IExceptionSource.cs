using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageLib.Common;

namespace MessageLib.SocketEngine
{
    interface IExceptionSource
    {
        event EventHandler<ErrorEventArgs> ExceptionThrown;
    }
}
