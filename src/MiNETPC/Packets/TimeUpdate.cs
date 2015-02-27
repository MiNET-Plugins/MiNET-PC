using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class TimeUpdate : Package<TimeUpdate>
	{
		public int Day = 0;
		public int Time = 0;

		public TimeUpdate(ClientWrapper client) : base(client)
		{
			SendId = 0x03;
		}

		public TimeUpdate(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x03;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteLong(Day);
			Buffer.WriteLong(Time);
			Buffer.FlushData();
		}
	}
}