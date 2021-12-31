using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
namespace EMod.Items
{
	public class FlashBangItem : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Flash Bang");
			Tooltip.SetDefault("think fast chucklenuts");
        }
        public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shootSpeed = 12f;
			item.shoot = ModContent.ProjectileType<Projectiles.FlashProjectile>();
			item.width = 8;
			item.height = 28;
			item.maxStack = 30;
			item.consumable = true;
			//item.UseSound = can we make it do the think fast thing or the csgo throwing flashbang?
			item.useAnimation = 40;
			item.useTime = 40;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
		}
        public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Grenade , 1);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

    }
}