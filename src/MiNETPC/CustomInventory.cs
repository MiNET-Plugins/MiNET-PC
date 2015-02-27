using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Net;
using MiNET.Utils;
using MiNETPC.Classes;

namespace MiNETPC
{
	public class PlayerInventory
	{
		public MetadataSlots Armor { get; private set; }
		public MetadataSlots Slots { get; private set; }
		public MetadataInts ItemHotbar { get; private set; }
		public MetadataSlot ItemInHand { get; private set; }
		private Player _player;

		public PlayerInventory(Player player)
		{
			_player = player;
			Armor = new MetadataSlots();
			Slots = new MetadataSlots();
			ItemHotbar = new MetadataInts();
			ItemInHand = new MetadataSlot(new ItemStack());

			Armor[0] = new MetadataSlot(new ItemStack());
			Armor[1] = new MetadataSlot(new ItemStack());
			Armor[2] = new MetadataSlot(new ItemStack());
			Armor[3] = new MetadataSlot(new ItemStack());

			for (byte i = 0; i < 44; i++)
			{
				Slots[i] = new MetadataSlot(new ItemStack((short)(i + 1), 10));
			}

			byte c = 0;
			Slots[c++] = new MetadataSlot(new ItemStack(54, 10));
			Slots[c++] = new MetadataSlot(new ItemStack(58, 10));
			Slots[c++] = new MetadataSlot(new ItemStack(61, 10));
			Slots[c++] = new MetadataSlot(new ItemStack(325, 1, 10));
			Slots[c++] = new MetadataSlot(new ItemStack(173, 10));
			Slots[c++] = new MetadataSlot(new ItemStack(263, 10));
			Slots[c++] = new MetadataSlot(new ItemStack(268, 10));
			Slots[c++] = new MetadataSlot(new ItemStack(280, 10));

			for (byte i = 0; i < 6; i++)
			{
				ItemHotbar[i] = new MetadataInt(i + 9);
			}
			//ItemHotbar[0] = new MetadataInt(9);
		}

		/// <summary>
		///     Set a players slot to the specified item.
		/// </summary>
		/// <param name="slot">The slot to set</param>
		/// <param name="itemId">The item id</param>
		/// <param name="amount">Amount of items</param>
		/// <param name="metadata">Metadata for the item</param>
		public void SetInventorySlot(byte slot, short itemId, byte amount = 1, short metadata = 0)
		{
			if (slot > 44) throw new IndexOutOfRangeException("slot");
			Slots[slot] = new MetadataSlot(new ItemStack(itemId, amount, metadata));

			//_player.SendPackage(new McpeContainerSetContent
			//{
			//	windowId = 0,
			//	slotData = Slots,
			//	hotbarData = ItemHotbar
			//});
		}

		/// <summary>
		///     Empty the specified slot
		/// </summary>
		/// <param name="slot">The slot to empty.</param>
		public void ClearInventorySlot(byte slot)
		{
			SetInventorySlot(slot, 0, 0);
		}

		public bool HasItem(MetadataSlot item)
		{
			for (byte i = 0; i < Slots.Count; i++)
			{
				if (((MetadataSlot)Slots[i]).Value.Id == item.Value.Id)
				{
					return true;
				}
			}
			return false;
		}

		public ItemStack GetSlot(int slotid)
		{
			if (slotid < Slots.Count)
			{
				MetadataSlot i = (MetadataSlot) Slots[(byte) slotid];
				return i.Value;
			}
			else
			{
				return null;
			}
		}
	}
}
