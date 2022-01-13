using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria;
using Terraria.Localization;

namespace EMod
{
	public class EMod : Mod
	{
		public const string RecipeGroup_HmBars_Tier2 = "EMod: Mythril/Orichalcum Bars";

		public override void Load()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Ref<Effect> FlashShader = new Ref<Effect>(GetEffect("Effects/FlashBang"));
				Filters.Scene["FlashShader"] = new Filter(new ScreenShaderData(FlashShader, "FlashBang"), EffectPriority.VeryHigh);
				

			}

		}

		public override void AddRecipeGroups()
		{
			RegisterRecipeGroup(RecipeGroup_HmBars_Tier2, ItemID.MythrilBar, new int[]{ ItemID.MythrilBar, ItemID.OrichalcumBar });
		}

		private static void RegisterRecipeGroup(string groupName, int itemForAnyName, int[] validTypes)
			=> RecipeGroup.RegisterGroup(groupName, new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(itemForAnyName)}", validTypes));
	}
}