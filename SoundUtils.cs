using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace EMod
{
	public static class SoundUtils
	{
		public static SoundEffectInstance PlayCustomSound(this Mod mod, Vector2 position, string sound, float volumeScale = 1f, float pitchOffset = 0f)
			=> Main.PlaySound(SoundLoader.customSoundType, (int)position.X, (int)position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/" + sound), volumeScale, pitchOffset);
	}
}
