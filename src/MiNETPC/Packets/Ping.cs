using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class Ping : Package<Ping>
	{
		public Ping(ClientWrapper client) : base(client)
		{
			ReadId = 0x01;
		}

		public Ping(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x01;
		}

		public override void Read()
		{
			Client.SendData(Buffer.BufferedData);
		}
	}
}