using MiNET.Utils;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class PlayerPositionAndLook : Package<PlayerPositionAndLook>
	{
		public PlayerPositionAndLook(ClientWrapper client) : base(client)
		{
			SendId = 0x08;
			ReadId = 0x06;
		}

		public PlayerPositionAndLook(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x08;
			ReadId = 0x06;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteDouble(PluginGlobals.Level.SpawnPoint.X);
			Buffer.WriteDouble(PluginGlobals.Level.SpawnPoint.Y - 1.62);
			Buffer.WriteDouble(PluginGlobals.Level.SpawnPoint.Z);
			Buffer.WriteFloat(0f);
			Buffer.WriteFloat(0f);
			Buffer.WriteByte(111);
			Buffer.FlushData();
		}

		public override void Read()
		{
			var X = Buffer.ReadDouble();
			var FeetY = Buffer.ReadDouble();
			var Z = Buffer.ReadDouble();
			var Yaw = Buffer.ReadFloat();
			var Pitch = Buffer.ReadFloat();
			var OnGround = Buffer.ReadBool();

			Client.Player.OnGround = OnGround;
			Client.Player.Yaw = Yaw;
			Client.Player.Pitch = Pitch;
			Client.Player.Coordinates = new Vector3(X, FeetY, Z);
			Client.Player.SendMovePlayer();
		}
	}
}