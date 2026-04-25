using System.Reflection;

namespace Respublica;

public class RespublicaAPI
{
    private static RespublicaAPI? _instance;

    public static RespublicaAPI getInstance() => _instance ??= new RespublicaAPI();

    public static void registerExternal(string type, string name, object classfunc)
    {
        var ctype = classfunc.GetType();

        MethodInfo? method = ctype.GetMethod(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, Type.EmptyTypes);
        if (method == null) return;

        var exex = new ExternalExtra
        {
            type = type,
            name = name,
            func = method
        };
        Respublica.getInstance()?.extRegisterFunc.Add(exex);
    }
}