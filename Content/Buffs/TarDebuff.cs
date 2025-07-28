﻿using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RoALiquids.Content.Buffs;

sealed class TarDebuff : ModBuff {
    public override void SetStaticDefaults() {
        Main.debuff[Type] = true;
        Main.pvpBuff[Type] = true;
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) {
        player.GetModPlayer<TarDebuff_Player>().IsEffectActive = true;

        player.moveSpeed *= 0.333f;
    }

    private class TarDebuff_NPC : ModNPC {
        public bool IsEffectActive;

        public override void ResetEffects() => IsEffectActive = false;

        public override void UpdateLifeRegen(ref int damage) {
            if (!IsEffectActive) {
                return;
            }

            if (NPC.onFire || NPC.onFire2 || NPC.onFire3 || NPC.onFrostBurn || NPC.onFrostBurn2 || NPC.shadowFlame) {
                if (NPC.lifeRegen > 0)
                    NPC.lifeRegen = 0;

                NPC.lifeRegen -= 50;
                if (damage < 10)
                    damage = 10;
            }
        }
    }

    private class TarDebuff_Player : ModPlayer {
        public bool IsEffectActive;

        public override void ResetEffects() => IsEffectActive = false;

        public override void PreUpdateMovement() {
            //if (!IsEffectActive) {
            //    return;
            //}

            //Player.moveSpeed *= 0.333f;
        }

        public override void UpdateBadLifeRegen() {
            if (!IsEffectActive) {
                return;
            }

            if (Player.onFire || Player.onFire2 || Player.onFire3 || Player.onFrostBurn || Player.onFrostBurn2) {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegen -= 50;
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
            if (!IsEffectActive) {
                return;
            }

            Player drawPlayer = drawInfo.drawPlayer;
            if (!drawPlayer.GetModPlayer<CustomLiquidCollision_Player>().tarWet) {
                int alpha2 = Main.rand.Next(50, 100);
                ushort tarDustType = (ushort)ModContent.DustType<Dusts.TarDebuff>();
                if (Main.rand.Next(5) <= 2 && drawInfo.shadow == 0f) {
                    Vector2 position3 = drawInfo.Position;
                    position3.X -= 2f;
                    position3.Y -= 2f;
                    if (Main.rand.Next(4) == 0) {
                        //Color newColor2 = new Color(88 / 255f + 0.1f * Main.rand.NextFloat(), 74 / 255f + 0.1f * Main.rand.NextFloat(), 91 / 255f + 0.1f * Main.rand.NextFloat());
                        //newColor2.A = (byte)(newColor2.A * 0.85f);
                        Dust dust11 = Dust.NewDustDirect(position3, drawPlayer.width + 4, drawPlayer.height + 2, tarDustType, 0f, 0f, alpha2, default, 1f);
                        if (Main.rand.Next(2) == 0)
                            dust11.alpha += 25;

                        if (Main.rand.Next(2) == 0)
                            dust11.alpha += 25;

                        dust11.noLight = true;
                        dust11.velocity *= 0.2f;
                        dust11.velocity += drawPlayer.velocity * 0.7f;
                        dust11.fadeIn = 0.8f;
                        drawInfo.DustCache.Add(dust11.dustIndex);
                    }

                    //if (Main.rand.Next(30) == 0) {
                    //    Color color2 = Main.hslToRgb(88 / 255f, 74 / 255f, 91 / 255f);
                    //    color2.A = (byte)(color2.A * 0.75f);
                    //    Dust dust12 = Dust.NewDustDirect(position3, drawPlayer.width + 4, drawPlayer.height + 2, DustID.TintableDustLighted, 0f, 0f, 254, new Color(88, 74, 91, 0), 0.45f);
                    //    dust12.noLight = true;
                    //    dust12.velocity.X *= 0f;
                    //    dust12.velocity *= 0.03f;
                    //    dust12.fadeIn = 0.6f;
                    //    //drawInfo.DustCache.Add(dust12.dustIndex);
                    //}
                }
            }

            r *= 0.8f;
            g *= 0.8f;
            b *= 0.8f;
        }
    }
}
