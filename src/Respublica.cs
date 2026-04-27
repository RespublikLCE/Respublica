namespace Respublica;

using Minecraft.Server.FourKit.Plugin;
using Commands;
using Minecraft.Server.FourKit;
using System.Reflection;

public enum ExternalType
{
	SubTown, // Town Subcommand
	SubNation, // Nation Subcommand
	SubPlot, // Plot Subcommand
	SubInvite, // Invite Subcommand
	SubRespublica, // Respublica Subcommand
	Empty // General Use Function
}

internal class ExternalFunc {
	public ExternalType type = ExternalType.Empty;
	public string name;
	public string cmd = "";
	public Delegate func;

	public ExternalFunc(string _name, Delegate _func) {
		name = _name;
		func = _func;
	}

	public ExternalFunc(ExternalType _type, string _cmd, string _name, Delegate _func)
	{
		type = _type;
		cmd = _cmd;
		name = _name;
		func = _func;
	}
}

internal sealed class Respublica : ServerPlugin
{
	private static Respublica? _instance;
	public static Respublica? getInstance() => _instance;
	internal static void setInstance(Respublica inst) => _instance = inst;

	internal List<ExternalFunc> extRegisterFunc = [];

	public override string name => "Respublica";
	public override string version => Assembly.GetExecutingAssembly().GetName().Version!.ToString();
	public override string author => "UniPM";

	private const string path = @"./plugindb"; // i have it as ./ bc it executes as the server exe - uni

	public override void onEnable() {
		_instance ??= this;
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
