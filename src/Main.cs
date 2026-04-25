namespace Respublica;

using Minecraft.Server.FourKit.Plugin;
using Commands;
using Minecraft.Server.FourKit;
using System.Reflection;

internal class ExternalExtra {
	public string type = "subtown";
	public required string name;
	public required MethodInfo func;
}

public class Respublica : ServerPlugin
{
	private static Respublica? _instance;
	public static Respublica? getInstance() => _instance;

	internal List<ExternalExtra> extRegisterFunc = [];

	public override string name => "Respublica";
	public override string version => "1.0.0-alpha.1";
	public override string author => "UniPM";

	private const string path = @"./plugindb"; // i have it as ./ bc it executes as the server exe - uni

	public override void onEnable() {
		_instance = this;
		Init.InitPnt();
		Init.InitCmd();

		FourKit.addListener(new RespublicaListener());
	}

    public override void onDisable()
    {
        Database.Instance.Dispose();
		if (!Directory.Exists(path)) {
			Console.WriteLine("[RESPUBLICA] PluginDB directory missing! Make sure the server folder isn't read-only or administrator.");
			Console.WriteLine("You can also just create it manually.");
		}
    }
}
