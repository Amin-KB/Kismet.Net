using System.Net;
using System.Net.Sockets;
using System.Text;
using Src.Routers;

namespace Src;

public class Server
{
    
    public static void Start()
    {
        Routers.RegisterRoute.Register(typeof(Server));
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Server started. Listening on port 8080...");
        Console.ResetColor();
        while (true)
        {
            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Request: \n" + request);

                string response = Handlers.RequestHandler.HandleRequest(request);
                byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBuffer, 0, responseBuffer.Length);
            }
        }
    }
    [Route("/")]
    public static string Home()
    {
        return "Hello, world!";
    }

}