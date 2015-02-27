﻿using System;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	internal class SpawnPlayer : Package<SpawnPlayer>
	{
		public Player Player;

		public SpawnPlayer(ClientWrapper client) : base(client)
		{
			SendId = 0x0C;
		}

		public SpawnPlayer(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			SendId = 0x0C;
		}

		public override void Write()
		{
			Buffer.WriteVarInt(SendId);
			Buffer.WriteVarInt(Player.EntityId);
			Buffer.WriteUUID(new Guid(Player.Uuid));
			Buffer.WriteInt((int) Player.Coordinates.X*32);
			Buffer.WriteInt((int) Player.Coordinates.Y*32);
			Buffer.WriteInt((int) Player.Coordinates.Z*32);
			Buffer.WriteByte((byte) Player.Yaw);
			Buffer.WriteByte((byte) Player.Pitch);
			Buffer.WriteShort(0);
			Buffer.WriteByte(127);
			Buffer.FlushData();
		}
	}
}