using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	public abstract class Package
	{
		public int SendId;
		public int ReadId;
		public ClientWrapper Client;
		public MsgBuffer Buffer;

		protected Package(ClientWrapper client)
		{
			if (!client.TCPClient.Connected) return;
			Client = client;
			Buffer = new MsgBuffer(client);
		}

		protected Package(ClientWrapper client, MsgBuffer buffer)
		{
			if (!client.TCPClient.Connected) return;
			Client = client;
			Buffer = buffer;
		}

		public virtual void Read()
		{
		}

		public virtual void Write()
		{
		}

		public void Broadcast(bool self = true, Player source = null)
		{
			foreach (var i in PluginGlobals.PcPlayers)
			{
				if (!self && i == source)
				{
					continue;
				}
				Client = i.Wrapper;
				Buffer = new MsgBuffer(i.Wrapper);
				Write();
			}
		}
	}

	public abstract class Package<T> : Package where T : Package<T>
	{
		protected Package(ClientWrapper client)
			: base(client)
		{
		}

		protected Package(ClientWrapper client, MsgBuffer buffer)
			: base(client, buffer)
		{
		}
	}
}