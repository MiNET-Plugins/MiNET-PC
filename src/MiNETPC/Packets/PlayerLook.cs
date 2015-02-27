using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class PlayerLook : Package<PlayerLook>
	{
		public PlayerLook(ClientWrapper client) : base(client)
		{
			ReadId = 0x05;
		}

		public PlayerLook(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x05;
		}

		public override void Read()
		{
			Client.Player.Yaw = Buffer.ReadFloat();
			Client.Player.Pitch = Buffer.ReadFloat();
			Client.Player.OnGround = Buffer.ReadBool();
			Client.Player.SendMovePlayer();
		}
	}
}