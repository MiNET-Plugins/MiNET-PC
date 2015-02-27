using System;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNETPC.Classes
{
	public class PcChunkColumn
	{
		public int X { get; set; }
		public int Z { get; set; }
		public byte[] BiomeId = ArrayOf<byte>.Create(256, 1);

		public ushort[] Blocks = new ushort[16 * 16 * 256];
		public NibbleArray Skylight = new NibbleArray(16 * 16 * 256);
		public NibbleArray Blocklight = new NibbleArray(16 * 16 * 256);

		public PcChunkColumn()
		{
			for (int i = 0; i < Skylight.Length; i++)
				Skylight[i] = 0xff;
		}

		public ushort GetBlock(int x, int y, int z)
		{
			var index = x + 16 * z + 16 * 16 * y;
			if (index >= 0 && index < Blocks.Length)
				return Blocks[index];
			else return 900;
		}

		public void SetBlock(int x, int y, int z, int blockid, int metadata)
		{
			var index = x + 16 * z + 16 * 16 * y;
			if (index >= 0 && index < Blocks.Length)
				Blocks[index] = Convert.ToUInt16((blockid << 4) | metadata);
		}

		public void SetBlocklight(int x, int y, int z, byte data)
		{
			Blocklight[(x * 2048) + (z * 256) + y] = data;
		}

		public void SetSkylight(int x, int y, int z, byte data)
		{
			Skylight[(x * 2048) + (z * 256) + y] = data;
		}

		public byte[] GetBytes()
		{
			var writer = new MsgBuffer(new byte[]{});
			writer.WriteInt(X);
			writer.WriteInt(Z);
			writer.WriteBool(true);
			writer.WriteUShort(0xffff); // bitmap
			writer.WriteVarInt((Blocks.Length * 2) + Skylight.Data.Length + Blocklight.Data.Length + BiomeId.Length);

			foreach (var i in Blocks)
				writer.WriteUShort(i);

			writer.Write(Blocklight.Data);
			writer.Write(Skylight.Data);

			writer.Write(BiomeId); //OK
			return writer.ExportWriter;
		}

		public void Pe2Pc(ChunkColumn sourceChunk)
		{
			for (var y = 0; y < 128; y++)
			{
				for (var x = 0; x < 16; x++)
				{
					for (var z = 0; z < 16; z++)
					{
						SetBlock(x, y, z, sourceChunk.GetBlock(x, y, z), sourceChunk.GetMetadata(x, y, z));
					}
				}
			}
		}
	}

	public static class ArrayOf<T> where T : new()
	{
		public static T[] Create(int size, T initialValue)
		{
			var array = new T[size];
			for (var i = 0; i < array.Length; i++)
				array[i] = initialValue;
			return array;
		}

		public static T[] Create(int size)
		{
			var array = new T[size];
			for (var i = 0; i < array.Length; i++)
				array[i] = new T();
			return array;
		}
	}
}
