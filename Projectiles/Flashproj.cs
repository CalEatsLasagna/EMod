using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace EMod.Projectiles
{
    public class FlashProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Grenade);
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.timeLeft = 125;
        }
        const int flashrange = 9999;
        public override void Kill(int timeLeft)
        {
            //maybe play a loud sound?
            for(int i = 0; i< Main.player.Length; i++)
            {
                Player p = Main.player[i];
                if (p.active)
                {
                    p.GetModPlayer<EmodPlayer>().FlashbangedTime = 120;
                    p.GetModPlayer<EmodPlayer>().StartedFlash = true;
                }
               
            }
        }
    }
}
