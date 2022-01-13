using EMod.NPCs.Bosses;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace EMod.Players
{
	public class GrabbedPlayer : ModPlayer
	{
		public int grabbedByNPC = -1;

		public int grabCounter;

		public bool HasBeenGrabbed
		{
			get
			{
				if(grabbedByNPC >= 0)
				{
					bool grabberIsHand = Main.npc[grabbedByNPC].modNPC is ZAHANDO;

					if(!grabberIsHand)
						grabbedByNPC = -1;
					else
						return true;
				}

				return false;
			}
		}

		public override void SetControls()
		{
			if(HasBeenGrabbed){
				if(player.controlLeft && player.releaseLeft)
					grabCounter--;
				if(player.controlRight && player.releaseRight)
					grabCounter--;
				if(player.controlUp && player.releaseUp)
					grabCounter--;
				if(player.controlDown && player.releaseDown)
					grabCounter--;
				if(player.controlJump && player.releaseJump)
					grabCounter--;

				//No jumping, mounting or grappling allowed!
				player.controlJump = false;
				player.releaseJump = false;
				player.controlMount = false;
				player.releaseMount = false;
				player.controlHook = false;
				player.releaseHook = false;

				//No dashing!
				player.controlRight = false;
				player.releaseRight = false;
				player.controlLeft = false;
				player.releaseLeft = false;

				ZAHANDO hand = Main.npc[grabbedByNPC].modNPC as ZAHANDO;
				if(grabCounter <= 0){
					hand.grabbedPlayer = -1;
					
					grabbedByNPC = -1;
				}
			}
		}

		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if(HasBeenGrabbed)
				r = g = b = a = 0;  //Make the player invisible
		}
	}
}
