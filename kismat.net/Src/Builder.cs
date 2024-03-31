using System.Net;
using System.Net.Sockets;
using System.Text;

namespace kismat.net;

public class Builder
{
     static Dictionary<string, Func<string>> routes = new Dictionary<string, Func<string>>();

   public static void Start()
    {
        // Define routes
        routes["/"] = () => "Hello, world!";
        routes["/hello"] = () => "Hello!";
        routes["/time"] = GetCurrentTime;

        TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();
        Console.WriteLine("Server started. Listening on port 8080...");

        while (true)
        {
            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Request: \n" + request);

                string response = HandleRequest(request);
                byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBuffer, 0, responseBuffer.Length);
            }
        }
    }

    static string HandleRequest(string request)
    {
        // Run middleware
        request = LogMiddleware(request);

        string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);
        string[] firstLineParts = lines[0].Split(' ');

        string method = firstLineParts[0];
        string path = firstLineParts[1];

        if (method == "GET")
        {
            if (routes.ContainsKey(path))
            {
                // Call the handler associated with the path
                return "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n" + routes[path]();
            }
            else
            {
                return "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n404 Not Found";
            }
        }
        else
        {
            return "HTTP/1.1 405 Method Not Allowed\r\nContent-Type: text/plain\r\n\r\nMethod not allowed";
        }
    }

    static string LogMiddleware(string request)
    {
        // Example middleware: Logging
        Console.WriteLine("Log: " + request);
        return request;
    }

    static string GetCurrentTime()
    {
        return "The current time is: " + DateTime.Now.ToString("HH:mm:ss");
    }
}