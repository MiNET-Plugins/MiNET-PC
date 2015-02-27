using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using MiNET.Utils;
using MiNET.Worlds;
using MiNETPC.Classes;
using MiNETPC.Packages;

namespace MiNETPC
{
	class PluginGlobals
	{
		public static TcpListener ServerListener = new TcpListener(IPAddress.Any, 25565);
		public static Level Level;
		public static List<Player> PcPlayers = new List<Player>();
		public static List<Player> PePlayers = new List<Player>(); 
		public static Dictionary<string, string> Users = new Dictionary<string, string>();

		public static int LastEntityId = 0;
		public static string ProtocolName = "MiNET PC 1.8";
		public static int ProtocolVersion = 47;
		public static int MaxPlayers = 10;
		public static string Motd = "MiNET TEST";
		public static int PeidOffset = 10000;
		public static int PcidOffset = 50000;
		private static List<int> _gaps;
		private static List<int> _ignore;

		public static byte GetBlockId(ushort blockId)
		{
			//if (_gaps.Contains(itemid)) return 0;
		//	else return (byte) itemid;
			if (blockId == 125) blockId = 5;
			else if (blockId == 126) blockId = 158;
			else if (blockId == 75) blockId = 50;
			else if (blockId == 76) blockId = 50;
			else if (blockId == 123) blockId = 89;
			else if (blockId == 124) blockId = 89;
			else if (_ignore.BinarySearch(blockId) >= 0) blockId = 0;
			else if (_gaps.BinarySearch(blockId) >= 0)
			{
				blockId = 133;
			}
			if (blockId > 255) blockId = 41;
			return (byte) blockId;
		}

		public static void SendChunk(ChunkCoordinates position)
		{
			ChunkColumn targetchunk = Level._worldProvider.GenerateChunkColumn(position);
			PcChunkColumn converted = new PcChunkColumn {X = position.X, Z = position.Z};
			converted.Pe2Pc(targetchunk);

			foreach (var player in PcPlayers)
			{
				//new BlockChange(player.Wrapper, new MSGBuffer(player.Wrapper)) {BlockID = data.block, MetaData = data.meta, Location = new Vector3(data.x, data.y, data.z)}.Write();
				new ChunkData(player.Wrapper) { Chunk = converted, Quee = false}.Write();
			}
		}

		public static void SendBlockUpdate(Vector3 position, short blockId, byte metadata)
		{
			//ChunkColumn targetchunk = Level._worldProvider.GenerateChunkColumn(position);
			//PcChunkColumn converted = new PcChunkColumn { X = position.X, Z = position.Z };
			//converted.Pe2Pc(targetchunk);

			foreach (var player in PcPlayers)
			{
				new BlockChange(player.Wrapper, new MsgBuffer(player.Wrapper)) {BlockId = blockId, MetaData = metadata, Location = position}.Write();
				//new ChunkData(player.Wrapper) { Chunk = converted, Quee = false }.Write();
			}
		}

		public static void Initialize()
		{
			_ignore = new List<int>
			{
				23,
				25,
				28,
				29,
				33,
				34,
				36,
				55,
				69,
				70,
				71,
				72,
				77,
				84,
				87,
				88,
				93,
				94,
				97,
				113,
				115,
				117,
				118,
				131,
				132,
				138,
				140,
				143,
				144,
				145
			};
			_ignore.Sort();

			_gaps = new List<int>
			{
				23,
				25,
				28,
				29,
				33,
				34,
				36,
				55,
				69,
				70,
				72,
				75,
				76,
				77,
				84,
				88,
				90,
				93,
				94,
				95,
				97,
				115,
				116,
				117,
				118,
				119,
				122,
				123,
				124,
				125,
				126,
				130,
				131,
				132,
				137,
				138,
				140,
				143,
				144,
				145,
				146,
				147,
				148,
				149,
				150,
				151,
				152,
				153,
				154,
				160,
				165,
				166,
				167,
				168,
				169
			};
			_gaps.Sort();
		}

		public static List<Player> GetPlayers()
		{
			List<Player> templist = new List<Player>();
			templist.AddRange(PePlayers);
			templist.AddRange(PcPlayers);
			return templist;
		}

		public static Player GetPlayer(int entityId)
		{
			return GetPlayers().FirstOrDefault(i => i.EntityId == entityId);
		}

		public static void BroadcastChat(string message)
		{
			foreach (Player player in PcPlayers)
			{
				new ChatMessage(player.Wrapper) {Message = message}.Write();
			}
		}
	}
}
