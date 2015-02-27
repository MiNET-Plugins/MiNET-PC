using System;
using MiNET.Blocks;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class PlayerBlockPlacement : Package<PlayerBlockPlacement>
	{
		public PlayerBlockPlacement(ClientWrapper client) : base(client)
		{
			ReadId = 0x08;
		}

		public PlayerBlockPlacement(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x08;
		}

		public override void Read()
		{
			var position = Buffer.ReadPosition();

			if (position.Y > 127)
			{
				return;
			}

			var face = Buffer.ReadByte();

			switch (face)
			{
				case 0:
					position.Y--;
					break;
				case 1:
					position.Y++;
					break;
				case 2:
					position.Z--;
					break;
				case 3:
					position.Z++;
					break;
				case 4:
					position.X--;
					break;
				case 5:
					position.X++;
					break;
			}

			var heldItem = Buffer.ReadUShort();
			if (heldItem <= UInt16.MinValue || heldItem >= UInt16.MaxValue) return;

			var itemCount = Buffer.ReadByte();
			var itemDamage = Buffer.ReadByte();
			var itemMeta = (byte) Buffer.ReadByte();

			var cursorX = Buffer.ReadByte(); //Unused
			var cursorY = Buffer.ReadByte(); //Unused
			var cursorZ = Buffer.ReadByte(); //Unused



			var b = BlockFactory.GetBlockById(PluginGlobals.GetBlockId(heldItem));
			b.Coordinates = position;
			b.Metadata = itemMeta;
			PluginGlobals.Level.SetBlock(b);

			PluginGlobals.SendBlockUpdate(position, PluginGlobals.GetBlockId(heldItem), itemMeta);
		}
	}
}