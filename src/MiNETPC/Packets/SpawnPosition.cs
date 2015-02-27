using MiNET.Utils;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class SpawnPosition : Package<SpawnPosition>
	{
		public SpawnPosition(ClientWrapper client) : base(client)
		{
			SendId = 0x05;
		}

		public SpawnPosition(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x05;
		}

		public override void Write()
		{
			Vector3 D = PluginGlobals.Level.SpawnPoint;
			Buffer.WriteVarInt(SendId);
			Buffer.WritePosition(D);
			Buffer.FlushData();
		}
	}
}