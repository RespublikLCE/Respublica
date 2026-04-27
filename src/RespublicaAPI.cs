using LiteDB;
using Minecraft.Server.FourKit;

namespace Respublica;

public sealed class RespublicaAPI
{
    internal static Respublica getRespublica()
    {
        if (Respublica.getInstance() == null) Respublica.setInstance(new Respublica());
        return Respublica.getInstance()!;
    }

    /// <summary>
    /// Registers an external function.
    /// <para>
    /// Allows you to register functions into Respublica, including subcommands.
    /// </para>
    /// </summary>
    /// <param name="type">The ExternalType of the function.</param>
    /// <param name="name">The name of the function.</param>
    /// <param name="cmd">The subcommand name for the function, only used for subcommands.</param>
    /// <param name="func">The delegate function of the external function.</param>
    public static void RegisterExternal(ExternalType type, string name, string cmd, Delegate func)
    {
        if (getRespublica().extRegisterFunc.Any(x => x.name == name || x.cmd == cmd))
        {
            Console.WriteLine("[RESPUBLICA] Function with name or command already exists!");
            Console.WriteLine("There might be conflicting addons. Please check your /plugins folder!");
            return;
        }
        var exex = new ExternalFunc(type, cmd, name, func);
        getRespublica().extRegisterFunc.Add(exex);
    }

    /// <summary>
    /// Registers an external function.
    /// <para>
    /// Allows you to register functions into Respublica, including subcommands.
    /// </para>
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="func">The delegate function of the external function.</param>
    public static void RegisterExternal(string name, Delegate func)
    {
        var exex = new ExternalFunc(name, func);
        getRespublica().extRegisterFunc.Add(exex);
    }

    /// <summary>
    /// Executes an external function.
    /// </summary>
    /// <param name="name">Name of the external function.</param>
    /// <param name="args">Arguments for the function.</param>
    /// <returns></returns>
    public static object? ExecuteExternal(string name, object[] args)
    {
        var external = getRespublica().extRegisterFunc.Find(x => x.name == name);
        if (external == null) return null;
        return external.func.DynamicInvoke(args);
    }

    /// <summary>
    /// Retrieves a collection from the Respublica DB.
    /// </summary>
    /// <typeparam name="T">DB type for collection</typeparam>
    /// <param name="name">Name of the collection</param>
    /// <returns></returns>
    public static ILiteCollection<T> GetDBCollection<T>(string name) => Database.Instance.GetCollection<T>(name);
}