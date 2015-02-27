using System.Threading;
using MiNET.API;
using MiNET.PluginSystem.Attributes;
using MiNET.Worlds;
using MiNETPC.Packages;

namespace MiNETPC
{
	[Plugin("MiNETPC", "Add's MC:PC Support to MiNET", "pre 1.0", "kennyvv")]
    public class MiNetpc : MiNETPlugin
    {
		public override void OnEnable(Level level)
		{
			ConsoleFunctions.WriteInfoLine("[MiNET PC] Loading...");
			PluginGlobals.Initialize();
			PluginGlobals.Level = level;
			ConsoleFunctions.WriteInfoLine("[MiNET PC] Initiating server...");
			new Thread(() => new Networking.BasicListener().ListenForClients()).Start();
			new Ticks().StartTimeOfDayTimer();
		}

		public override void OnDisable()
		{
			ConsoleFunctions.WriteInfoLine("[MiNET PC] Kicking players...");
			Disconnect.Broadcast("Server shutting down...");
		}
    }
}
