using MiNET.Utils;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class EntityTeleport : Package<EntityTeleport>
	{
		public int EntityId;
		public Vector3 Coordinates;
		public byte Yaw;
		public byte Pitch;
		public bool OnGround;

		public EntityTeleport(ClientWrapper client) : base(client)
		{
			SendId = 0x18;
		}

		public EntityTeleport(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x18;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteVarInt(EntityId);
			Buffer.WriteInt((int) (Coordinates.X*32));
			Buffer.WriteInt((int) (Coordinates.Y*32));
			Buffer.WriteInt((int) (Coordinates.Z*32));
			Buffer.WriteByte(Yaw);
			Buffer.WriteByte(Pitch);
			Buffer.WriteBool(OnGround);
			Buffer.FlushData();
		}
	}
}