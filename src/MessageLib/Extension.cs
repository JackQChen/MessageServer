using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MessageLib
{
    public static class Extension
    {

        public static void SafeClose(this Socket socket)
        {
            try
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException) { }
                socket.Close();
                socket.Dispose();
            }
            catch (ObjectDisposedException) { }
        }

        public static void DualMode(this Socket socket, bool value)
        {
            if (socket.AddressFamily != AddressFamily.InterNetworkV6)
                throw new NotSupportedException();
            socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, value ? 0 : 1);
        }

        public static void SetKeepAlive(this Socket socket, int keepAliveTime, int keepAliveInterval)
        {
            var size = Marshal.SizeOf(typeof(uint));
            var optionValues = new byte[size * 3];
            BitConverter.GetBytes((uint)1).CopyTo(optionValues, 0);
            BitConverter.GetBytes((uint)(keepAliveTime * 1000)).CopyTo(optionValues, size);
            BitConverter.GetBytes((uint)(keepAliveInterval * 1000)).CopyTo(optionValues, size * 2);
            socket.IOControl(IOControlCode.KeepAliveValues, optionValues, null);
        }

        public static string ToFileSize(this long fileSize)
        {
            if (fileSize < 0)
                return "ErrorSize";
            else if (fileSize >= 1024 * 1024 * 1024)
                return string.Format("{0:########0.00}GB", fileSize / (1024d * 1024 * 1024));
            else if (fileSize >= 1024 * 1024)
                return string.Format("{0:####0.00}MB", fileSize / (1024d * 1024));
            else if (fileSize >= 1024)
                return string.Format("{0:####0.00}KB", fileSize / 1024d);
            else
                return string.Format("{0}B", fileSize);
        }
    }
}
