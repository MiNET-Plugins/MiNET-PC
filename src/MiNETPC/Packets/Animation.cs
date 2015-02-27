using MiNET.Net;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class Animation : Package<Animation>
	{
		public byte AnimationId;
		public Player TargetPlayer;

		public Animation(ClientWrapper client) : base(client)
		{
			SendId = 0x0B;
			ReadId = 0x0A;
		}

		public Animation(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x0B;
			ReadId = 0x0A;
		}

		public override void Read()
		{
			new Animation(Client) {AnimationId = 0, TargetPlayer = Client.Player}.Broadcast(false, Client.Player);

			PluginGlobals.Level.RelayBroadcast(new McpeAnimate()
			{
				actionId = 1,
				entityId = PluginGlobals.PcidOffset + Client.Player.EntityId
			});
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteVarInt(TargetPlayer.EntityId);
			Buffer.WriteByte(AnimationId);
			Buffer.FlushData();
		}
	}
}