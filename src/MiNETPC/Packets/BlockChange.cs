using MiNET.Utils;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class BlockChange : Package<BlockChange>
	{
		public Vector3 Location;
		public int BlockId;
		public int MetaData;

		public BlockChange(ClientWrapper client)
			: base(client)
		{
			SendId = 0x23;
		}

		public BlockChange(ClientWrapper client, MsgBuffer buffer)
			: base(client, buffer)
		{
			SendId = 0x23;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WritePosition(Location);
			Buffer.WriteVarInt(BlockId << 4 | MetaData);
			Buffer.FlushData();
		}
	}
}