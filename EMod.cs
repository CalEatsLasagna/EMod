using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria;

namespace EMod
{
	public class EMod : Mod
	{
		public override void Load()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Ref<Effect> FlashShader = new Ref<Effect>(GetEffect("Effects/FlashBang"));
				Filters.Scene["FlashShader"] = new Filter(new ScreenShaderData(FlashShader, "FlashBang"), EffectPriority.VeryHigh);
				

			}

		}
	}
}