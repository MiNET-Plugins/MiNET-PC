using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class OnGround : Package<OnGround>
	{
		public OnGround(ClientWrapper client) : base(client)
		{
			ReadId = 0x03;
		}

		public OnGround(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x03;
		}

		public override void Read()
		{
			Client.Player.OnGround = Buffer.ReadBool();
		}
	}
}