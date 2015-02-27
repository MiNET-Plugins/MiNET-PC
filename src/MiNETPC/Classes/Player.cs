using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using MiNET;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;
using MiNETPC.Packages;

namespace MiNETPC.Classes
{
	public class Player
	{
		public string Username { get; set; }
		public string Uuid { get; set; }
		public ClientWrapper Wrapper { get; set; }
		public int EntityId { get; set; }
		public GameMode Gamemode { get; set; }
		public bool IsSpawned { get; set; }
		public bool Digging { get; set; }

		//Location stuff
		public byte Dimension { get; set; }
		public Vector3 Coordinates { get; set; }
		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public bool OnGround { get; set; }

		//Client settings
		public string Locale { get; set; }
		public byte ViewDistance { get; set; }
		public byte ChatFlags { get; set; }
		public bool ChatColours { get; set; }
		public byte SkinParts { get; set; }

		//Map stuff
		private readonly Vector2 _currentChunkPosition = new Vector2(0, 0);
		public bool ForceChunkReload { get; set; }
		private readonly Dictionary<Tuple<int, int>, ChunkColumn> _chunksUsed;

		//Inventory stuff
		public byte CurrentSlot = 0;

		public MiNET.Player PlayerEntity;
		public PlayerInventory PlayerInventory;
		public PCHealthManager HealthManager;

		public Player()
		{
			_chunksUsed = new Dictionary<Tuple<int, int>, ChunkColumn>();
			HealthManager = new PCHealthManager(this);
			if (PlayerEntity == null) PlayerEntity = new MiNET.Player(null, null, PluginGlobals.Level, -1);
			PlayerInventory = new PlayerInventory(this);
		}

		public void Respawn()
		{
			HealthManager.ResetHealth();
			new Respawn(Wrapper) {}.Write();
		}

		public void SendHealth()
		{
			if (Wrapper != null) new UpdateHealth(Wrapper).Write();
		}

		public void SendChunksForKnownPosition(bool force = false)
		{
			int centerX = (int)Coordinates.X >> 4;
			int centerZ = (int)Coordinates.Z >> 4;

			if (!force && IsSpawned && _currentChunkPosition == new Vector2(centerX, centerZ)) return;

			_currentChunkPosition.X = centerX;
			_currentChunkPosition.Z = centerZ;

			new Thread(() =>
			{
				int counted = 0;

				foreach (
					var chunk in
						PluginGlobals.Level.GenerateChunks(new ChunkCoordinates((int) Coordinates.X, (int) Coordinates.Z),
							force ? new Dictionary<Tuple<int, int>, ChunkColumn>() : _chunksUsed))
				{
					PcChunkColumn pcchunk = new PcChunkColumn {X = chunk.x, Z = chunk.z};
					pcchunk.Pe2Pc(chunk);
					

					new ChunkData(Wrapper, new MsgBuffer(Wrapper)) {Chunk = pcchunk}.Write();
					//new ChunkData().Write(Wrapper, new MSGBuffer(Wrapper), new object[]{ chunk.GetBytes() });

					Thread.Yield();

					if (counted >= 5 && !IsSpawned)
					{

						new PlayerPositionAndLook(Wrapper).Write();

						IsSpawned = true;
						PluginGlobals.PcPlayers.Add(this);

						foreach (var targetPlayer in PluginGlobals.Level.Players)
						{
							targetPlayer.SendPackage( new McpeAddPlayer
								{
									clientId = 0,
									username = Username,
									entityId = PluginGlobals.PcidOffset + EntityId,
									x = (float)Coordinates.X,
									y = (float)Coordinates.Y,
									z = (float)Coordinates.Z,
									yaw = (byte)Yaw,
									pitch = (byte)Pitch,
									metadata = new byte[0]
								});

							PluginGlobals.Level.RelayBroadcast(new McpeAddEntity
							{
								entityType = -1,
								entityId = PluginGlobals.PcidOffset + EntityId,
								x = (float)Coordinates.X,
								y = (float)Coordinates.Y,
								z = (float)Coordinates.Z,
							});
						}

							foreach (var player2 in PluginGlobals.GetPlayers())
							{
								new PlayerListItem(Wrapper)
								{
									Action = 0,
									Username = player2.Username,
									Gamemode = player2.Gamemode,
									UUID = player2.Uuid
								}.Write();

								if (player2 != this)
								{
									new SpawnPlayer(Wrapper)
									{
										Player = player2
									}.Write();
								}
							}

						SendMovePlayer();

						PluginGlobals.Level.BroadcastTextMessage(Username  + " joined the game!");
					}
					counted++;
				}
			}).Start();
		}

		public void SendMovePlayer()
		{
			var package = McpeMovePlayer.CreateObject();
			package.entityId = PluginGlobals.PcidOffset + EntityId;
			package.x = (float)Coordinates.X;
			package.y = (float)Coordinates.Y + 1.62f;
			package.z = (float)Coordinates.Z;
			package.yaw = Yaw;
			package.pitch = Pitch;
			package.bodyYaw = Yaw;
			package.teleport = 0x80;

			foreach (MiNET.Player pl in PluginGlobals.Level.GetSpawnedPlayers())
			{
				pl.SendPackage(package);
			}
		}
	}
}
