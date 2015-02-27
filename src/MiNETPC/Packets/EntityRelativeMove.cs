﻿using MiNET.Utils;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class EntityRelativeMove : Package<EntityRelativeMove>
	{
		public Vector3 Movement;
		public Player Player;

		public EntityRelativeMove(ClientWrapper client) : base(client)
		{
			SendId = 0x15;
		}

		public EntityRelativeMove(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x15;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteVarInt(Player.EntityId);
			Buffer.WriteByte((byte) (Movement.X*32));
			Buffer.WriteByte((byte) (Movement.Y*32));
			Buffer.WriteByte((byte) (Movement.Z*32));
			Buffer.WriteBool(Player.OnGround);
			Buffer.FlushData();
		}
	}
}