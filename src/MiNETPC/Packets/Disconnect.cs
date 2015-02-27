﻿using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class Disconnect : Package<Disconnect>
	{
		public string Reason = "";

		public Disconnect(ClientWrapper client)
			: base(client)
		{
			SendId = 0x40;
		}

		public Disconnect(ClientWrapper client, MsgBuffer buffer)
			: base(client, buffer)
		{
			SendId = 0x40;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteString("{ \"text\": \"" + Reason + "\" }");
			Buffer.FlushData();
		}

		public static void Broadcast(string reason, bool self = true, Player source = null)
		{
			foreach (var i in PluginGlobals.GetPlayers())
			{
				if (!self && i == source)
				{
					continue;
				}
				//Client = i.Wrapper;
				//Buffer = new MSGBuffer(i.Wrapper);
				//_stream = i.Wrapper.TCPClient.GetStream();
				//Write();
				new Disconnect(i.Wrapper, new MsgBuffer(i.Wrapper)) {Reason = reason}.Write();
			}
		}
	}
}