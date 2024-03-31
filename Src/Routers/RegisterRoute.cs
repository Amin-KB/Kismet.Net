namespace Src.Routers;

public static class RegisterRoute
{
    
    internal static void Register(Type type)
    {
        foreach (var methodInfo in type.GetMethods())
        {
            var routeAttribute = (RouteAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(RouteAttribute));
            if (routeAttribute != null)
            {
                Route.Routes[routeAttribute.Path] = () => (string)methodInfo.Invoke(null, null);
            }
        }
    }
}