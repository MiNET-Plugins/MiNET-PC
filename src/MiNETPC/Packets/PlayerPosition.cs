using MiNET.Utils;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class PlayerPosition : Package<PlayerPosition>
	{
		public PlayerPosition(ClientWrapper client) : base(client)
		{
			ReadId = 0x04;
		}

		public PlayerPosition(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x04;
		}

		public override void Read()
		{
			var X = Buffer.ReadDouble();
			var FeetY = Buffer.ReadDouble();
			var Z = Buffer.ReadDouble();
			var OnGround = Buffer.ReadBool();
			Client.Player.Coordinates = new Vector3(X, FeetY, Z);
			Client.Player.OnGround = OnGround;
			Client.Player.SendChunksForKnownPosition();
			Client.Player.SendMovePlayer();
		}
	}
}