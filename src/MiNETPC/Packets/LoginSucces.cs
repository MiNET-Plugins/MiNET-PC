﻿using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class LoginSucces : Package<LoginSucces>
	{
		public string Username;
		public string UUID;

		public LoginSucces(ClientWrapper client)
			: base(client)
		{
			ReadId = 0x02;
			SendId = 0x02;
		}

		public LoginSucces(ClientWrapper client, MsgBuffer buffer)
			: base(client, buffer)
		{
			ReadId = 0x02;
			SendId = 0x02;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteString(UUID);
			Buffer.WriteString(Username);
			Buffer.FlushData();
		}
	}
}