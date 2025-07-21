using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RoALiquids.Content.Projectiles;

sealed class TarExplosion : ModProjectile {
    public override void SetStaticDefaults() {
        ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
        ProjectileID.Sets.Explosive[Type] = true;
    }


    public override void SetDefaults() {
        Projectile.width = 10;
        Projectile.height = 10;
        Projectile.friendly = true;
        Projectile.penetrate = -1;

        // 5 second fuse.
        Projectile.timeLeft = 300;

        // These help the projectile hitbox be centered on the projectile sprite.
        DrawOffsetX = -2;
        DrawOriginOffsetY = -5;
    }

    public override void AI() {
        Projectile.timeLeft = 0;
        Projectile.PrepareBombToBlow();

        Projectile.Kill();
    }

    public override bool PreDraw(ref Color lightColor) => false;

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
        // Vanilla explosions do less damage to Eater of Worlds in expert mode, so we will too.
        if (Main.expertMode) {
            if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) {
                modifiers.FinalDamage /= 5;
            }
        }
    }

    public override void PrepareBombToBlow() {
        Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the bomb explodes on slopes.
        Projectile.alpha = 255; // Set to transparent. This projectile technically lives as transparent for about 3 frames

        // Change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
        Projectile.Resize(75, 75);

        Projectile.damage = 100; // Bomb: 100, Dynamite: 250
        Projectile.knockBack = 8f; // Bomb: 8f, Dynamite: 10f
    }

    public override void OnKill(int timeLeft) {
        // Play explosion sound
        SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        // Smoke Dust spawn
        for (int i = 0; i < 50; i++) {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
            dust.velocity *= 1.4f;
        }

        // Fire Dust spawn
        for (int i = 0; i < 80; i++) {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
            dust.noGravity = true;
            dust.velocity *= 5f;
            dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
            dust.velocity *= 3f;
        }

        // Large Smoke Gore spawn
        for (int g = 0; g < 2; g++) {
            var goreSpawnPosition = new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f);
            Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
            gore.scale = 1.5f;
            gore.velocity.X += 1.5f;
            gore.velocity.Y += 1.5f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
            gore.scale = 1.5f;
            gore.velocity.X -= 1.5f;
            gore.velocity.Y += 1.5f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
            gore.scale = 1.5f;
            gore.velocity.X += 1.5f;
            gore.velocity.Y -= 1.5f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
            gore.scale = 1.5f;
            gore.velocity.X -= 1.5f;
            gore.velocity.Y -= 1.5f;
        }
        // reset size to normal width and height.
        Projectile.Resize(10, 10);
    }
}
