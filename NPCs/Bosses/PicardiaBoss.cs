using EMod.Sounds.Custom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EMod.NPCs.Bosses
{
	//Made by yours truly -- absoluteAquarian
	[AutoloadBossHead]
	public class PicardiaBoss : ModNPC
	{
		public override string Texture => "EMod/NPCs/Bosses/pe";

		public override string BossHeadTexture => base.Texture + "_Head_Boss";

		public struct AnimationData
		{
			public string texture;
			public Vector2 origin;
			public bool vibrate;
		}

		public static readonly AnimationData coolandgood = new AnimationData()
		{
			texture = "coolandgood",
			origin = new Vector2(61, 49),
			vibrate = false
		};

		public static readonly AnimationData notcoolandgood = new AnimationData()
		{
			texture = "notcoolandgood",
			origin = new Vector2(59, 52),
			vibrate = true
		};

		public static readonly AnimationData picardiaSD = new AnimationData()
		{
			texture = "picardiaSD",
			origin = new Vector2(32, 32),
			vibrate = true
		};

		public static readonly AnimationData pe = new AnimationData()
		{
			texture = "pe",
			origin = new Vector2(64, 63),
			vibrate = false
		};

		public static readonly AnimationData ecstatic = new AnimationData()
		{
			texture = "ecstatic",
			origin = new Vector2(48, 46),
			vibrate = false
		};

		public static readonly AnimationData garf = new AnimationData()
		{
			texture = "garf",
			origin = new Vector2(72, 76),
			vibrate = false
		};

		public static readonly AnimationData sourcat = new AnimationData()
		{
			texture = "sourcat",
			origin = new Vector2(64, 90),
			vibrate = false
		};

		public const int State_Spawning = 0;
		public const int State_FlyMenacingly = 1;
		public const int State_ChargeAtPlayer = 2;
		public const int State_WarpAttack = 3;
		public const int State_KilledAllPlayers = 4;
		public const int State_OhShitItsAStandUser = 5;

		public const int State_Spawning_1_TimerMax = 180;
		public const int State_Spawning_2_TimerMax = 210;

		public int State
		{
			get => (int)npc.ai[0];
			set => npc.ai[0] = value;
		}

		public int StateProgress
		{
			get => (int)npc.ai[1];
			set => npc.ai[1] = value;
		}

		public int Timer
		{
			get => (int)npc.ai[2];
			set => npc.ai[2] = value;
		}

		private bool hasKilledEveryone;

		public AnimationData currentData = pe;

		private float alphaReal;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(":coolandgood:");

			NPCID.Sets.TrailCacheLength[npc.type] = 5;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()	
		{
			npc.width = 80;
			npc.height = 80;
			npc.damage = 80;
			npc.defense = 42;
			npc.knockBackResist = 0f;
			npc.lifeMax = 26000;
			npc.aiStyle = -1;
			npc.netAlways = true;
			npc.noTileCollide = true;
			npc.noGravity = true;

			alphaReal = 255;
			npc.alpha = 255;

			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.boss = true;

			npc.npcSlots = 10f;

			npc.dontTakeDamage = true;
		}

		private SoundEffectInstance bass, bass2;
		private bool playBass, playBass2;
		private float bassVolume, bassVolume2;

		//Esed to make a second, transparent self draw
		private int effectGrowTimer;
		private const int EffectGrowTimerMax = 60;

		public override void AI()
		{
			npc.GivenName = ":" + currentData.texture + ":";

			switch (State)
			{
				case State_Spawning:
					if (StateProgress == 0)
					{
						StateProgress = 1;
						Timer = State_Spawning_1_TimerMax;

						playBass = true;
						npc.spriteDirection = 1;

						SaySpawnText();
					}
					else if (StateProgress == 1)
					{
						bassVolume = 1f - (float)Timer / State_Spawning_1_TimerMax;

						alphaReal -= 255f / State_Spawning_1_TimerMax;
						npc.alpha = (int)alphaReal;

						if (Timer < 0)
						{
							StateProgress = 2;
							Timer = State_Spawning_2_TimerMax;

							playBass = false;
							playBass2 = true;
							bass?.Stop();
							bass = null;

							effectGrowTimer = EffectGrowTimerMax;

							currentData = coolandgood;
						}
					}
					else if (StateProgress == 2)
					{
						bassVolume2 = 1f - (float)Timer / State_Spawning_2_TimerMax;

						if (Timer < 0)
						{
							StateProgress = 3;

							playBass2 = false;

							bass2?.Stop();
							bass2 = null;

							npc.dontTakeDamage = false;

							Timer = 180;
						}
					}
					else if (StateProgress == 3)
					{
						// TODO: more AI
						if (Timer < 0)
							npc.active = false;
					}
					break;
			}

			if (playBass)
				bass = mod.PlayCustomSound(npc.Center, "BassBoosted", bassVolume);
			if (playBass2)
				bass2 = mod.PlayCustomSound(npc.Center, "BassDrop", bassVolume2);

			if (Timer >= 0)
				Timer--;

			if (effectGrowTimer >= 0)
				effectGrowTimer--;
		}

		private static readonly string[] spawnTexts = new string[]
		{
			"Welcome to Clown World!  Population: [c/ff0000:Y O U]",
		//	"I hope you're ready to get rekt, kiddo.",
		//	"You are lukewarm and bad.  Prepare to [c/ff0000:perish].",
		//	"I will eat your [c/ff0000:R I B S]!  I will eat them up!",
		//	"I am going to claw my way down your throat and tear out your very soul!"
		};

		private void SaySpawnText()
		{
			string choice = "<:coolandgood:> " + Main.rand.Next(spawnTexts);

			if (Main.netMode == NetmodeID.SinglePlayer)
				Main.NewText(choice, Color.Orange);
			else
				NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(choice), Color.Orange);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			drawColor *= 1f - npc.alpha / 255f;

			Texture2D texture = mod.GetTexture("NPCs/Bosses/" + currentData.texture);
			Vector2 origin = currentData.origin;

			Vector2 position = npc.Center - Main.screenPosition;

			if (currentData.vibrate)
				position += Main.rand.NextVector2Square(-4f, 4f);

			SpriteEffects effect = npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			if (npc.spriteDirection == -1)
				origin.X = texture.Width - origin.X;

			if (effectGrowTimer >= 0)
			{
				Color color = Color.White;

				float scaleFactor = effectGrowTimer / (float)EffectGrowTimerMax;
				float scale = npc.scale * (1f + 0.6f * (1f - scaleFactor * scaleFactor * scaleFactor));

				spriteBatch.Draw(texture, position, null, effectGrowTimer > 20 ? color * 0.4f : color * (0.4f * effectGrowTimer / 20f), npc.rotation, origin, scale, effect, 0);
			}

			spriteBatch.Draw(texture, position, null, drawColor, npc.rotation, origin, npc.scale, effect, 0);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (State == State_Spawning && StateProgress >= 2)
			{
				//Lens flare!
				DrawLensFlare(spriteBatch, new Vector2(-11, -12));
				DrawLensFlare(spriteBatch, new Vector2(18, -12));
			}
		}

		private void DrawLensFlare(SpriteBatch spriteBatch, Vector2 offset)
		{
			if (Timer > 120 && StateProgress == 2)
				return;

			float alpha = StateProgress == 2 && Timer >= 60 ? 1f - (Timer - 60) / 60f : 1f;

			var texture = mod.GetTexture("NPCs/Bosses/lens-flare-red");
			int dimMax = Math.Max(texture.Width, texture.Height);
			float scale = 75f / dimMax;

			spriteBatch.Draw(texture, npc.Center + offset - Main.screenPosition, null, Color.White * alpha, 0f, texture.Size() / 2f, scale, SpriteEffects.None, 0);
		}
	}
}
