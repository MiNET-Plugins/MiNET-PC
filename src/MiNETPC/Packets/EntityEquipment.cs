using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class EntityEquipment : Package<EntityEquipment>
	{
		public short Slot = 0;
		public short ItemID = -1;
		public byte ItemCount = 1;
		public short ItemDamage = 0;
		public byte NBT = 0;
		public int EntityID = 0;

		public EntityEquipment(ClientWrapper client) : base(client)
		{
			SendId = 0x04;
		}

		public EntityEquipment(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x04;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);

			Buffer.WriteVarInt(EntityID);
			Buffer.WriteShort(Slot);
			Buffer.WriteShort(ItemID);
			if (ItemID != -1)
			{
				Buffer.WriteByte(ItemCount);
				Buffer.WriteShort(ItemDamage);
				Buffer.WriteByte(NBT);
			}
			Buffer.FlushData();
		}
	}
}