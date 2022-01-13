using EMod.NPCs.Bosses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EMod.Items
{
	public class SunglassesPlusPlus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Extra Cool Sunglasses");
			Tooltip.SetDefault("Summons an extra-cool dude who will kick your ass" +
				"\nNot consumable");

			ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13;
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 12;
			item.scale = 1.2f;
			item.maxStack = 20;
			item.rare = ItemRarityID.Pink;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item44;
			item.consumable = false;
		}

		public override bool CanUseItem(Player player)
			=> player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<PicardiaBoss>());

		public override bool UseItem(Player player)
		{
			//Spawn near the player
			Vector2 position = player.Center + Main.rand.NextVector2Unit() * 30 * 16;

			int spawned = NPC.NewNPC((int)position.X, (int)position.Y, ModContent.NPCType<PicardiaBoss>(), Target: player.whoAmI);

			string typeName = Main.npc[spawned].TypeName;
			if (Main.netMode == NetmodeID.SinglePlayer)
				Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName), 175, 75);
			else if (Main.netMode == NetmodeID.Server)
				NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Main.npc[spawned].GetTypeNetName()), new Color(175, 75, 255));

			return true;
		}

		public override void AddRecipes()	
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sunglasses);
			recipe.AddIngredient(ItemID.FallenStar, 20);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddRecipeGroup(EMod.RecipeGroup_HmBars_Tier2, 28);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofSight, 3);
			recipe.AddIngredient(ItemID.SoulofMight, 3);
			recipe.AddIngredient(ItemID.SoulofFright, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
