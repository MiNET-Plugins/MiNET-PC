using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	class Respawn : Package<Respawn>
	{
		public int Dimension = 0;
		public byte GameMode = 1;
		public byte Difficulty = 0;

		public Respawn(ClientWrapper client) : base(client)
		{
			SendId = 0x07;
		}

		public Respawn(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x07;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteInt(Dimension);
			Buffer.WriteByte(Difficulty);
			Buffer.WriteByte(GameMode);
			Buffer.WriteString("flat");
			Buffer.FlushData();
		}
	}
}
