﻿using System;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class KeepAlive : Package<KeepAlive>
	{
		public KeepAlive(ClientWrapper client) : base(client)
		{
			ReadId = 0x00;
			SendId = 0x00;
		}

		public KeepAlive(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x00;
			SendId = 0x00;
		}

		public override void Write()
		{
			var id = new Random().Next(0, 100);
			Buffer.WriteVarInt(SendId);
			Buffer.WriteVarInt(id);
			Buffer.FlushData();
		}
	}
}