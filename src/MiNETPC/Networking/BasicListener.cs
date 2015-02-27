using System;
using System.Net.Sockets;
using System.Threading;
using MiNET.Net;
using MiNETPC.Classes;
using MiNETPC.Packages;
using PackageFactory = MiNETPC.Packages.PackageFactory;

namespace MiNETPC.Networking
{
	public class BasicListener
	{
		public void ListenForClients()
		{
			PluginGlobals.ServerListener.Start();
			ConsoleFunctions.WriteServerLine("Ready for connections...");
			while (true)
			{
				TcpClient client = PluginGlobals.ServerListener.AcceptTcpClient();
				ConsoleFunctions.WriteDebugLine("A new connection has been made!");

				Thread clientThread = new Thread(() => HandleClientCommNew(client));
				clientThread.Start();
			}
		}
		private void HandleClientCommNew(object client)
		{
			TcpClient tcpClient = (TcpClient)client;
			NetworkStream clientStream = tcpClient.GetStream();
			ClientWrapper Client = new ClientWrapper(tcpClient);

			while (true)
			{
				try
				{
					MsgBuffer Buf = new MsgBuffer(Client);
					int ReceivedData = clientStream.Read(Buf.BufferedData, 0, Buf.BufferedData.Length);
					if (ReceivedData > 0)
					{
						int length = Buf.ReadVarInt();
						Buf.Size = length;
						int packid = Buf.ReadVarInt();
						bool found = false;
						if (!new PackageFactory(Client, Buf).Handle(packid)) ;
						else found = true;

						if (!found)
						{
							ConsoleFunctions.WriteWarningLine("Unknown packet received! \"0x" + packid.ToString("X2") + "\"");
							// Client.Player.SendChat("We received an unknown packet from you! 0x" + packid.ToString("X2") + "");
						}
					}
					else
					{
						//Stop the while loop. Client disconnected!
						break;
					}
				}
				catch (Exception ex)
				{
					//Exception, disconnect!
					ConsoleFunctions.WriteDebugLine("Error: \n" + ex);
					new Disconnect(Client) { Reason = "§4MiNET PC\n§fServer threw an exception!\n\nFor the nerdy people: \n" + ex.Message }.Write();
					break;
				}
			}
			//Close the connection with the client. :)
			Client.StopKeepAliveTimer();

			if (Client.Player != null)
			{
				//Globals.Level.RemovePlayer(Client.Player);
				//Globals.Level.BroadcastPlayerRemoval(Client);
			}
			Client.TCPClient.Close();
		}
	}

}
