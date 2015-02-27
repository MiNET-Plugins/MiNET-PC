using System;
using MiNET.Net;
using MiNET.PluginSystem.Attributes;
using MiNET.Utils;
using MiNET.Worlds;
using MiNETPC.Classes;
using MiNETPC.Packages;
using Package = MiNET.Net.Package;
using Player = MiNET.Player;

namespace MiNETPC
{
	public class PePacketReader
	{
		[HandlePacket(typeof(McpeAnimate))]
		public void HandleAnimation(Package packet, Player source)
		{
			McpeAnimate data = (McpeAnimate) packet;
			if (data.actionId == 1)
			{
				var tar = PluginGlobals.GetPlayer(source.EntityId + PluginGlobals.PeidOffset);
				foreach (var i in PluginGlobals.PcPlayers)
				{
					new Animation(i.Wrapper) { AnimationId = 0, TargetPlayer = tar }.Broadcast(false, tar);	
				}
			}
		}

		[HandlePacket(typeof(McpeMessage))]
		public void HandleChatPacket(Package packet, Player source)
		{
			McpeMessage data = (McpeMessage) packet;
			if (data.message.StartsWith("/")) return; //Do not show commands in chat!!!

			PluginGlobals.BroadcastChat("<" + source.Username + "> " + data.message.Replace("\\", "\\\\").Replace("\"", "\'\'"));
		}

		[HandleSendPacket(typeof (McpeUpdateBlock))]
		public void ChangeBlockHandler(Package packet, Player source)
		{
			McpeUpdateBlock data = (McpeUpdateBlock) packet;
			ChunkCoordinates target = new ChunkCoordinates(data.x >> 4, data.z >> 4);
			ChunkColumn targetchunk = PluginGlobals.Level._worldProvider.GenerateChunkColumn(target);
			PcChunkColumn converted = new PcChunkColumn { X = target.X, Z = target.Z };

			converted.Pe2Pc(targetchunk);

			//foreach (var player in PluginGlobals.PcPlayers)
			//{
				//new BlockChange(player.Wrapper, new MSGBuffer(player.Wrapper)) {BlockID = data.block, MetaData = data.meta, Location = new Vector3(data.x, data.y, data.z)}.Write();
				//new ChunkData(player.Wrapper){ Chunk = converted, Quee = false}.Write();
			//}
			PluginGlobals.SendBlockUpdate(new Vector3(data.x, data.y, data.z), PluginGlobals.GetBlockId(data.block), data.meta);
		}

		[HandlePacket(typeof (McpeMovePlayer))]
		public void HandleMovePacket(Package packet, Player source)
		{
			McpeMovePlayer data = (McpeMovePlayer) packet;
			if (PluginGlobals.GetPlayer(PluginGlobals.PeidOffset + data.entityId) != null)
			{
				PluginGlobals.GetPlayer(PluginGlobals.PeidOffset + data.entityId).Coordinates = new Vector3(data.x, data.y, data.z);
				PluginGlobals.GetPlayer(PluginGlobals.PeidOffset + data.entityId).Yaw = data.bodyYaw;
				PluginGlobals.GetPlayer(PluginGlobals.PeidOffset + data.entityId).Pitch = data.pitch;

				foreach (var player in PluginGlobals.PcPlayers)
				{
					new EntityTeleport(player.Wrapper, new MsgBuffer(player.Wrapper))
					{
						Coordinates = new Vector3(data.x, data.y, data.z),
						OnGround = false,
						Yaw = (byte) data.bodyYaw,
						Pitch = (byte) data.pitch,
						EntityId = PluginGlobals.PeidOffset + data.entityId
					}.Write();
				}
			}
		}

		[HandlePlayerDisconnect]
		public void HandleDisconnect(Player player)
		{
			foreach (var playerd in PluginGlobals.PePlayers)
			{
				if (playerd.Username == player.Username)
				{
					foreach (var playerd2 in PluginGlobals.PcPlayers)
					{
						new PlayerListItem(playerd2.Wrapper) {Action = 4, Gamemode = GameMode.Creative, Username = playerd.Username, UUID = playerd.Uuid}.Write();
					}
					break;
				}
			}
			PluginGlobals.BroadcastChat("\\u00A7e" + player.Username + " has left the game!");
		}

		[HandlePlayerLogin]
		public void HandleLogin(Player player)
		{
			var p = new Classes.Player
			{
				Username = player.Username,
				EntityId = PluginGlobals.PeidOffset + player.EntityId,
				Wrapper = new ClientWrapper(),
				Uuid = Guid.NewGuid().ToString(),
				PlayerEntity = player,
				Coordinates = new Vector3(player.KnownPosition.X, player.KnownPosition.Y, player.KnownPosition.Z),
				Yaw = player.KnownPosition.Yaw,
				Pitch = player.KnownPosition.Pitch
			};

			PluginGlobals.PePlayers.Add(p);
		
				//PluginGlobals.Level.SendAddForPlayer(targetPlayer, this);
				//PluginGlobals.Level.SendAddForPlayer(newPlayer, targetPlayer);

				foreach (var pc in PluginGlobals.PcPlayers) //Send PC players to PE
				{
					player.SendPackage(new McpeAddPlayer
					{
						clientId = 0,
						username = pc.Username,
						entityId = PluginGlobals.PcidOffset + pc.EntityId,
						x = (float) pc.Coordinates.X,
						y = (float) pc.Coordinates.Y,
						z = (float) pc.Coordinates.Z,
						yaw = (byte) pc.Yaw,
						pitch = (byte) pc.Pitch,
						metadata = new byte[0]
					});

					player.SendPackage(new McpeAddEntity
					{
						entityType = -1,
						entityId = PluginGlobals.PcidOffset + pc.EntityId,
						x = (float) pc.Coordinates.X,
						y = (float)pc.Coordinates.Y,
						z = (float)pc.Coordinates.Z,
					});
				}

				foreach (var playerd in PluginGlobals.PcPlayers) //Send PE Players to PC
				{
					new PlayerListItem(playerd.Wrapper)
					{
						Action = 0,
						Username = p.Username,
						Gamemode = GameMode.Creative,
						UUID = p.Uuid
					}.Write();

					new SpawnPlayer(playerd.Wrapper)
					{
						Player = p
					}.Write();
				}

			PluginGlobals.BroadcastChat("\\u00A7e" + player.Username + " joined the game!");
		}

		[OnPlayerInteract]
		public void PlayerAttacked(int entityId)
		{
			if (entityId >= PluginGlobals.PcidOffset)
			{
				entityId -= PluginGlobals.PcidOffset;
				var target = PluginGlobals.GetPlayer(entityId);
				target.HealthManager.TakeHit(null);
			}
		}
	}
}
