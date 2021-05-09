using System.Linq;
using System.Windows.Forms;
using MessageLib.SocketBase.Logging;

namespace MessageServer.Logging
{

    /// <summary>
    /// Server log factory
    /// </summary>
    public class ServerLogFactory : ILogFactory
    {
        ServerLogHandler logHandler;

        public ServerLogFactory()
        {
            var frmMain = Application.OpenForms.Cast<Form>().FirstOrDefault(p => typeof(FrmMain).IsInstanceOfType(p)) as FrmMain;
            if (frmMain == null)
                return;
            logHandler = new ServerLogHandler(frmMain.Log);
        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ILog GetLog(string name)
        {
            return new ServerLog(name, logHandler);
        }
    }
}
