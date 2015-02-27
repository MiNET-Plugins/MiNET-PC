using System;
using System.Net;
using System.Text;
using MiNET.Worlds;
using MiNETPC.Classes;
using Player = MiNET.Player;

namespace MiNETPC.Packages
{
	internal class Handshake : Package<Handshake>
	{
		public Handshake(ClientWrapper client)
			: base(client)
		{
			ReadId = 0x00;
			SendId = 0x00;
		}

		public Handshake(ClientWrapper client, MsgBuffer buffer)
			: base(client, buffer)
		{
			ReadId = 0x00;
			SendId = 0x00;
		}

		public override void Read()
		{
			var protocol = Buffer.ReadVarInt();
			var host = Buffer.ReadString();
			var port = Buffer.ReadShort();
			var state = Buffer.ReadVarInt();

			switch (@state)
			{
				case 1:
					HandleStatusRequest();
					break;
				case 2:
					HandleLogin();
					break;
			}
		}

		private void HandleStatusRequest()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteString("{\"version\": {\"name\": \"" + PluginGlobals.ProtocolName + "\",\"protocol\": " +
			                   PluginGlobals.ProtocolVersion + "},\"players\": {\"max\": " + PluginGlobals.MaxPlayers +
			                   ",\"online\": " + PluginGlobals.GetPlayers().Count + "},\"description\": {\"text\":\"" +
			                   PluginGlobals.Motd + "\"}}");
			Buffer.FlushData();
		}

		private void HandleLogin()
		{
			var username = Buffer.ReadUsername();
			var uuid = getUUID(username);

			new LoginSucces(Client) {Username = username, UUID = uuid}.Write();

			if (Encoding.UTF8.GetBytes(username).Length == 0)
			{
				new Disconnect(Client) {Reason = "Something went wrong while decoding your username!"}.Write();
				return;
			}

			if (PluginGlobals.Level == null)
			{
				new Disconnect(Client) {Reason = "Server not fully initiated yet!"}.Write();
				return;
			}

			PluginGlobals.LastEntityId++;

			var p = new Player(null, null, PluginGlobals.Level, 5); //For later usage ;P
			//PluginGlobals.Level.AddPlayer(p);

			Client.Player = new Classes.Player
			{
				Uuid = uuid,
				Username = username,
				EntityId = PluginGlobals.LastEntityId,
				Wrapper = Client,
				Gamemode = GameMode.Creative,
				PlayerEntity = p
			};
			Client.PlayMode = true;

			new SetCompression(Client).Write();

			new JoinGame(Client) {Player = Client.Player}.Write();
			new SpawnPosition(Client).Write();
			Client.StartKeepAliveTimer();
			Client.Player.SendChunksForKnownPosition();
		}

		private string getUUID(string username)
		{
			try
			{
				var wc = new WebClient();
				var result = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username).Split('"');
				if (result.Length > 1)
				{
					var uuid = result[3];
					return new Guid(uuid).ToString();
				}
				return Guid.NewGuid().ToString();
			}
			catch
			{
				return Guid.NewGuid().ToString();
			}
		}
	}
}