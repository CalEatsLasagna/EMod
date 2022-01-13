using EMod.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EMod.NPCs.Bosses
{
	public class ZAHANDO : ModNPC
	{
		public override string Texture => "EMod/NPCs/Bosses/reach";

		public int grabbedPlayer = -1;

		public bool HasGrabbedPlayer => grabbedPlayer >= 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hand");

			NPCID.Sets.TrailCacheLength[npc.type] = 5;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 128;
			npc.height = 128;
			npc.scale = 0.6f * 1.5f;
			npc.damage = 100;
			npc.defense = 20;
			npc.lifeMax = 2000;
			npc.knockBackResist = 0.2f;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath12;
			npc.netAlways = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(grabbedPlayer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			grabbedPlayer = reader.ReadInt32();
		}

		public override void AI()
		{
			if (npc.ai[0] == 0)
				Main.PlaySound(SoundID.Item84, npc.Center);

			if (!npc.HasValidTarget)
				npc.TargetClosest(false);

			Player player = Main.player[npc.target];
			var mp = player.GetModPlayer<GrabbedPlayer>();

			if (mp.grabbedByNPC >= 0)
			{
				int oldAggro = player.aggro;
				player.aggro = 100000000;  //Comically large number
				npc.TargetClosest(false);
				player.aggro = oldAggro;

				player = Main.player[npc.target];
				mp = player.GetModPlayer<GrabbedPlayer>();
			}

			//Slow down if the NPC is moving away from the player
			if (Vector2.DistanceSquared(npc.Center + npc.velocity, player.Center) > npc.DistanceSQ(player.Center) || mp.HasBeenGrabbed)
				npc.velocity *= 1f - 3.4f / 60f;

			if (!mp.HasBeenGrabbed)
			{
				const float accel = 2.7f / 60f;
				var dir = npc.DirectionTo(player.Center);
				npc.velocity += dir * accel;

				float rotation = dir.ToRotation();
				npc.rotation = rotation - MathHelper.PiOver2;

				npc.spriteDirection = rotation < -MathHelper.PiOver2 || rotation > MathHelper.PiOver2 ? -1 : 1;
			}
			else if (mp.grabbedByNPC == npc.whoAmI)
				player.Center = npc.Center + npc.DirectionTo(player.Center) * 2f;

			float vel = npc.velocity.LengthSquared();
			if (vel > 16 * 16)
				npc.velocity = Vector2.Normalize(npc.velocity) * 16;
			else if (vel < 0.02f * 0.02f)
				npc.velocity = Vector2.Zero;

			//Make nearby hands move away
			for (int n = 0; n < Main.maxNPCs; n++)
			{
				NPC other = Main.npc[n];
				if (other.active && other.modNPC is ZAHANDO otherHand && npc.Hitbox.Intersects(other.Hitbox))
				{
					Vector2 dir = npc.DirectionTo(other.Center);

					const float accel = 4.3f / 60f;
					if (!HasGrabbedPlayer)
						npc.velocity += -dir * accel;

					if (!otherHand.HasGrabbedPlayer)
						other.velocity += dir * accel;
				}
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (!HasGrabbedPlayer)
			{
				grabbedPlayer = target.whoAmI;

				var mp = target.GetModPlayer<GrabbedPlayer>();

				mp.grabbedByNPC = npc.whoAmI;
				mp.grabCounter = 20;
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
			=> !HasGrabbedPlayer || (grabbedPlayer != target.whoAmI);

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("EMod/NPCs/Bosses/" + (HasGrabbedPlayer ? "grab" : "reach"));
			Vector2 origin = HasGrabbedPlayer ? new Vector2(102, 109) : new Vector2(55, 74);

			if (npc.spriteDirection == -1)
				origin.X = texture.Width - origin.X;

			float scale = 0.6f;
			if (!HasGrabbedPlayer)
				scale *= 1.5f;

			SpriteEffects effect = npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, drawColor, npc.rotation, origin, scale, effect, 0);

			//Afterimage effect
			for (int k = 0; k < npc.oldPos.Length; k++)
			{
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + npc.Size / 2f;
					
				Color color = npc.GetAlpha(drawColor) * (((float)npc.oldPos.Length - k) / npc.oldPos.Length);

				spriteBatch.Draw(texture, drawPos, null, color, npc.rotation, origin, npc.scale, effect, 0f);
			}

			return false;
		}
	}
}
