using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	class CreativeInventoryAction : Package<CreativeInventoryAction>
	{
		public CreativeInventoryAction(ClientWrapper client) : base(client)
		{
			ReadId = 0x10;
		}

		public CreativeInventoryAction(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x10;
		}

		public override void Read()
		{
			var slot = Buffer.ReadShort();
			var itemid = Buffer.ReadShort();
			byte itemCount = 1;
			short itemDamage = 0;
			byte meta = 0;

			if (itemid != -1)
			{
				itemCount = (byte)Buffer.ReadByte();
				itemDamage = Buffer.ReadShort();
				meta = (byte)Buffer.ReadByte();
			}

			Client.Player.PlayerInventory.SetInventorySlot((byte)slot, itemid, itemCount, meta);
		}
	}
}
