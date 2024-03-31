namespace Src.Routers;

[AttributeUsage(AttributeTargets.Method)]
public class RouteAttribute : Attribute
{
    public string Path { get; }

    public RouteAttribute(string path)
    {
        Path = path;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Path: {Path}");
        Console.ResetColor();
    }
}