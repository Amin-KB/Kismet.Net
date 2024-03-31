namespace Src.Handlers;

public class RequestHandler
{
   public static string HandleRequest(string request)
    {
        var routes = Routers.Route.Routes;
        string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);
        string[] firstLineParts = lines[0].Split(' ');

        string method = firstLineParts[0];
        string path = firstLineParts[1];
        if (method == "GET")
        {
            if (Routers.Route.Routes.ContainsKey(path))
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

}