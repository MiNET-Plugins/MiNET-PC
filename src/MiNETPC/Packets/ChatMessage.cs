﻿using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	public class ChatMessage : Package<ChatMessage>
	{
		public string Message;

		public ChatMessage(ClientWrapper client)
			: base(client)
		{
			ReadId = 0x01;
			SendId = 0x02;
		}

		public ChatMessage(ClientWrapper client, MsgBuffer buffer)
			: base(client, buffer)
		{
			ReadId = 0x01;
			SendId = 0x02;
		}

		public override void Read()
		{
			Message = Buffer.ReadString();
			//PluginGlobals.Level[0].BroadcastTextMessage("<" + Client.Player.Username + "> " +
			                                        // Message.Replace("\\", "\\\\").Replace("\"", "\'\'"));
			PluginGlobals.BroadcastChat(Message.Replace("\\", "\\\\").Replace("\"", "\'\'"), Client.Player.Username, true);
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteString("{ \"text\": \"" + Message + "\" }");
			Buffer.WriteByte(0);
			Buffer.FlushData();
		}
	}
}