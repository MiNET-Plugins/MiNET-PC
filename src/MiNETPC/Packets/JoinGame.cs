using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class JoinGame : Package<JoinGame>
	{
		public Player Player;

		public JoinGame(ClientWrapper client) : base(client)
		{
			SendId = 0x01;
		}

		public JoinGame(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x01;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteInt(Player.EntityId);
			Buffer.WriteByte((byte) Player.Gamemode);
			Buffer.WriteByte(Player.Dimension);
			Buffer.WriteByte((byte) PluginGlobals.Level.Difficulty);
			Buffer.WriteByte((byte) PluginGlobals.MaxPlayers);
			Buffer.WriteString("flat");
			Buffer.WriteBool(false);
			Buffer.FlushData();
		}
	}
}