using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace EMod.Sounds.Custom
{
	public class BassDrop : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			if (soundInstance is null)
			{
				//This is a new sound instance
				
				soundInstance = sound.CreateInstance();
				soundInstance.Volume = volume;
				soundInstance.Pan = pan;
				Main.PlaySoundInstance(soundInstance);
				return soundInstance;
			}
 
			//This is an existing sound instance that's still playing
			soundInstance.Volume = volume;
			soundInstance.Pan = pan;
			return soundInstance;
		}
	}
}
