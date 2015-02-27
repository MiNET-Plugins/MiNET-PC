using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using MiNET.Entities;
using MiNET.Utils;
using MiNETPC.Packages;

namespace MiNETPC
{
	public class PCHealthManager
	{
		public Classes.Player Player { get; set; }
		public int Health { get; set; }
		public short Air { get; set; }
		public bool IsDead { get; set; }
		public int FireTick { get; set; }
		public bool IsOnFire { get; set; }
		public DamageCause LastDamageCause { get; set; }
		public Classes.Player LastDamageSource { get; set; }

		public PCHealthManager(Classes.Player entity)
		{
			Player = entity;
			ResetHealth();
		}

		public void TakeHit(Classes.Player source, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
			if (LastDamageCause == DamageCause.Unknown) LastDamageCause = cause;

			LastDamageSource = source;

			//Untested code below, should work fine, however this is not sure yet.
			//	int Damage = ItemFactory.GetItem(sourcePlayer.Inventory.ItemInHand.Value.Id).GetDamage();
			//	Health -= Damage - Entity.Armour;

			Health -= damage;
			if (Player != null)
				Player.SendHealth();
			new Animation(Player.Wrapper) { AnimationId = 1, TargetPlayer = Player }.Broadcast();
		}

		public void Kill()
		{
			if (IsDead) return;

			IsDead = true;
			Health = 0;

			if (Player != null)
			{
				Player.SendHealth();
				//Player.BroadcastEntityEvent();
				//Player.BroadcastSetEntityData();
			}
		}

		public void ResetHealth()
		{
			Health = 20;
			Air = 300;
			IsOnFire = false;
			FireTick = 0;
			IsDead = false;
			LastDamageCause = DamageCause.Unknown;

			if (Player != null)
			{
				Player.SendHealth();
			}
		}

		public void OnTick()
		{
			//TODO: Rewrite to fit all entities

			if (IsDead) return;

			if (Health <= 0)
			{
				Kill();
				return;
			}

			if (Player.Coordinates.Y < 0 && !IsDead)
			{
				TakeHit(null, 10);
				LastDamageCause = DamageCause.Void;
				return;
			}

			if (IsInWater(new PlayerLocation((float)Player.Coordinates.X, (float)Player.Coordinates.Y, (float)Player.Coordinates.Z)))
			{
				Air--;
				if (Air <= 0)
				{
					if (Math.Abs(Air) % 10 == 0)
					{
						Health--;
						
						if (Player != null)
						{
							Player.SendHealth();
							//player.BroadcastEntityEvent();
							//player.BroadcastSetEntityData();
							LastDamageCause = DamageCause.Drowning;
						}
					}
				}

				if (IsOnFire)
				{
					IsOnFire = false;
					FireTick = 0;
					
					//if (Player != null)
						//Player.BroadcastSetEntityData();
					//Remove player fire....
				}
			}
			else
			{
				Air = 300;
			}

			if (!IsOnFire && IsInLava(new PlayerLocation((float)Player.Coordinates.X, (float)Player.Coordinates.Y, (float)Player.Coordinates.Z)))
			{
				FireTick = 300;
				IsOnFire = true;
			
				//if (Player != null)
				//	player.BroadcastSetEntityData();
				//Set player on fire :P
			}

			if (IsOnFire)
			{
				FireTick--;
				if (FireTick <= 0)
				{
					IsOnFire = false;
				}

				if (Math.Abs(FireTick) % 20 == 0)
				{
					Health--;
					
					if (Player != null)
					{
						Player.SendHealth();
						//player.BroadcastEntityEvent();
						//player.BroadcastSetEntityData();
						LastDamageCause = DamageCause.FireTick;
					}
				}
			}
		}

		private bool IsInWater(PlayerLocation playerPosition)
		{
			float y = playerPosition.Y + 1.62f;

			BlockCoordinates waterPos = new BlockCoordinates
			{
				X = (int)Math.Floor(playerPosition.X),
				Y = (int)Math.Floor(y),
				Z = (int)Math.Floor(playerPosition.Z)
			};

			var block = PluginGlobals.Level.GetBlock(waterPos);

			if (block == null || (block.Id != 8 && block.Id != 9)) return false;

			return y < Math.Floor(y) + 1 - ((1 / 9) - 0.1111111);
		}

		private bool IsInLava(PlayerLocation playerPosition)
		{
			var block = PluginGlobals.Level.GetBlock(playerPosition);

			if (block == null || (block.Id != 10 && block.Id != 11)) return false;

			return playerPosition.Y < Math.Floor(playerPosition.Y) + 1 - ((1 / 9) - 0.1111111);
		}

		public static string GetDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}
	}
}
