using MiNET.Utils;
using MiNET.Worlds;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class PlayerDigging : Package<PlayerDigging>
	{
		public PlayerDigging(ClientWrapper client) : base(client)
		{
			ReadId = 0x07;
		}

		public PlayerDigging(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x07;
		}

		public override void Read()
		{
			var status = Buffer.ReadByte();

			if (status == 2 || Client.Player.Gamemode == GameMode.Creative)
			{
				var position = Buffer.ReadPosition();
				var face = Buffer.ReadByte();
				var intVector = new Vector3((int) position.X, (int) position.Y, (int) position.Z);

				var block = PluginGlobals.Level.GetBlock(intVector);
				block.BreakBlock(PluginGlobals.Level);
				//Globals.Level.SetBlock(new BlockAir() {Coordinates = intVector});
				Client.Player.Digging = false;

				//PluginGlobals.SendChunk(new ChunkCoordinates((int) Position.X >> 4, (int) Position.Z >> 4));
				PluginGlobals.SendBlockUpdate(intVector, 0, 0);
			}
			else if (status == 0)
			{
				Client.Player.Digging = true;
			}
		}
	}
}