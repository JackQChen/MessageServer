using System.Text;
using MessageLib;

namespace WebSocketService
{
    public class Session : WsSession
    {

        public Session()
        {
        }

        public override void OnWsConnected(HttpRequest request)
        {
            this.SendTextAsync("[ID=" + this.Id + "] Connected!");
        }

        public override void OnWsReceived(byte[] buffer, int offset, int size)
        {
            this.SendTextAsync("Server echo: " + Encoding.UTF8.GetString(buffer, offset, size));
        }

        protected override void OnReceivedRequest(HttpRequest request)
        {
            if (request.Method == "GET")
            {
                if (request.Url == "/")
                {
                    var response = Cache.Find("/index");
                    if (response.Item1)
                        SendAsync(response.Item2);
                    else
                        SendResponseAsync(Response.MakeErrorResponse(404));
                }
                else
                    SendResponseAsync(Response.MakeErrorResponse(404));
            }
            else
                SendResponseAsync(Response.MakeErrorResponse("Unsupported HTTP method: " + request.Method));
        }

    }
}
