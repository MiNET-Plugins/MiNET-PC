using MiNET;
using MiNET.Items;
using MiNETPC.Classes;

namespace MiNETPC.Packages
{
	class UseEntity : Package<UseEntity>
	{
		public UseEntity(ClientWrapper client) : base(client)
		{
			ReadId = 0x02;
		}

		public UseEntity(ClientWrapper client, MsgBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x02;
		}

		public override void Read()
		{
			var target = Buffer.ReadVarInt();
			var type = Buffer.ReadVarInt();
			float targetX;
			float targetY;
			float targetZ;

			if (type == 2)
			{
				targetX = Buffer.ReadFloat();
				targetY = Buffer.ReadFloat();
				targetZ = Buffer.ReadFloat();
			}
			else if (type == 1)
			{
				var t = PluginGlobals.GetPlayer(target);
				var d = Client.Player.PlayerInventory.GetSlot(Client.Player.CurrentSlot);
				var heldItem = ItemFactory.GetItem(d.Id, d.Metadata);

				if (t.EntityId - PluginGlobals.PeidOffset > 0)
				{
					//Probably a pocket edition player :P
					t.PlayerEntity.HealthManager.TakeHit(null, heldItem.GetDamage(), DamageCause.Custom);
					new Animation(Client) { AnimationId = 1, TargetPlayer = t }.Broadcast();
				}
				else
				{
					//PC Player? :D
				}
			}
		}
	}
}
