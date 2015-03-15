using System.Threading;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNETPC.Packages;

namespace MiNETPC
{
	[Plugin(Author = "kennyvv", PluginName = "MiNET PC", PluginVersion = "Pre Alpha 1.0", Description = "Add PC Support to MiNET")]
    public partial class MiNetpc : IPlugin
    {
		public void OnEnable(PluginContext context)
		{
			ConsoleFunctions.WriteInfoLine("[MiNET PC] Loading...");
			PluginGlobals.Initialize();
			PluginGlobals.PluginContext = context;
			PluginGlobals.Level = context.Levels;
			ConsoleFunctions.WriteInfoLine("[MiNET PC] Initiating server...");
			new Thread(() => new Networking.BasicListener().ListenForClients()).Start();
			new Ticks().StartTimeOfDayTimer();
		}

		public void OnDisable()
		{
			ConsoleFunctions.WriteInfoLine("[MiNET PC] Kicking players...");
			Disconnect.Broadcast("Server shutting down...");
		}
    }
}
