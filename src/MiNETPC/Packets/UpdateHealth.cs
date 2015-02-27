using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class UpdateHealth : Package<UpdateHealth>
	{
		public UpdateHealth(ClientWrapper client) : base(client)
		{
			SendId = 0x06;
		}

		public UpdateHealth(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x06;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteFloat(Client.Player.HealthManager.Health);
			Buffer.WriteVarInt(20);
			Buffer.WriteFloat(0.0f);
			Buffer.FlushData();
		}
	}
}