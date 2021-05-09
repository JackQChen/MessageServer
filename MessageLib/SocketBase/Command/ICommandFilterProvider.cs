using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageLib.SocketBase.Metadata;

namespace MessageLib.SocketBase.Command
{
    /// <summary>
    /// The basic interface for CommandFilter
    /// </summary>
    public interface ICommandFilterProvider
    {
        /// <summary>
        /// Gets the filters which assosiated with this command object.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CommandFilterAttribute> GetFilters();
    }
}
