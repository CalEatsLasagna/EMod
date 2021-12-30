using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.Graphics.Effects;

namespace EMod
{
    public class EmodPlayer : ModPlayer
    {
      public bool StartedFlash = false;
        public int FlashbangedTime;
     
        public override void PostUpdate()
        {
            if (FlashbangedTime == 0)
            {
                Filters.Scene["FlashShader"].Deactivate();
            }
            if (FlashbangedTime != 0)
            {
                if (StartedFlash)
                {
                    
                    Filters.Scene.Activate("FlashShader");
                    StartedFlash = false;
                }
                FlashbangedTime--;
                Filters.Scene["FlashShader"].GetShader().UseProgress(FlashbangedTime);
                
            }
         
        }
    }
}