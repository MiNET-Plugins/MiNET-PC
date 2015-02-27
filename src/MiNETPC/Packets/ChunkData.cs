using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class ChunkData : Package<ChunkData>
	{
		public PcChunkColumn Chunk;
		public bool Quee = true;

		public ChunkData(ClientWrapper client)
			: base(client)
		{
			SendId = 0x21;
		}

		public ChunkData(ClientWrapper client, MsgBuffer buffer)
			: base(client, buffer)
		{
			SendId = 0x21;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.Write(Chunk.GetBytes());
			Buffer.FlushData(Quee);
		}


		public static void Broadcast(PcChunkColumn chunk, bool self = true, Player source = null)
		{
			foreach (var i in PluginGlobals.PcPlayers)
			{
				if (!self && i == source)
				{
					continue;
				}
				//Client = i.Wrapper;
				//Buffer = new MSGBuffer(i.Wrapper);
				//_stream = i.Wrapper.TCPClient.GetStream();
				//Write();
				new ChunkData(i.Wrapper, new MsgBuffer(i.Wrapper)) {Chunk = chunk}.Write();
			}
		}
	}
}