using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class HeldItemChange : Package<HeldItemChange>
	{
		public HeldItemChange(ClientWrapper client) : base(client)
		{
			SendId = 0x09;
			ReadId = 0x09;
		}

		public HeldItemChange(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x09;
			ReadId = 0x09;
		}

		public override void Read()
		{
			var slot = (byte) Buffer.ReadByte();
			Client.Player.CurrentSlot = slot;
			ConsoleFunctions.WriteDebugLine("Slotje: " + slot);
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteByte(Client.Player.CurrentSlot);
			Buffer.FlushData();
		}
	}
}