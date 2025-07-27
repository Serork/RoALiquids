using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using RoA;

using System;
using System.Runtime.CompilerServices;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.WaterfallManager;

namespace RoALiquids;

sealed class CustomWaterfallRenderer : IPostSetupContent {
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "waterfallTexture")]
    public extern static ref Asset<Texture2D>[] WaterfallManager_waterfallTexture(WaterfallManager self);

    void ILoadable.Load(Mod mod) {
        On_WaterfallManager.FindWaterfalls += On_WaterfallManager_FindWaterfalls;
        On_WaterfallManager.AddLight += On_WaterfallManager_AddLight;
        On_WaterfallManager.StylizeColor += On_WaterfallManager_StylizeColor;
        On_WaterfallManager.DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects += On_WaterfallManager_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects;
        On_WaterfallManager.DrawWaterfall_int_float += On_WaterfallManager_DrawWaterfall_int_float;
        On_WaterfallManager.UpdateFrame += On_WaterfallManager_UpdateFrame;
    }

    private static int _wFallFrCounter2;
    private static int _slowFrame;

    private void On_WaterfallManager_UpdateFrame(On_WaterfallManager.orig_UpdateFrame orig, WaterfallManager self) {
        orig(self);

        _wFallFrCounter2++;
        if (_wFallFrCounter2 > 8) {
            _wFallFrCounter2 = 0;
            _slowFrame++;
            if (_slowFrame > 15)
                _slowFrame = 0;
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "slowFrame")]
    public extern static ref int WaterfallManager_slowFrame(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "rainFrameForeground")]
    public extern static ref int WaterfallManager_rainFrameForeground(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "rainFrameBackground")]
    public extern static ref int WaterfallManager_rainFrameBackground(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "snowFrameForeground")]
    public extern static ref int WaterfallManager_snowFrameForeground(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "regularFrame")]
    public extern static ref int WaterfallManager_regularFrame(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "AddLight")]
    public extern static void WaterfallManager_AddLight(WaterfallManager self, int waterfallType, int x, int y);

    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TrySparkling")]
    public extern static void WaterfallManager_TrySparkling(WaterfallManager self, int x, int y, int direction, Color aColor2);

    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "GetAlpha")]
    public extern static float WaterfallManager_GetAlpha(WaterfallManager self, float Alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache);

    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "StylizeColor")]
    public extern static Color WaterfallManager_StylizeColor(WaterfallManager self, float alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache, Color aColor);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "DrawWaterfall")]
    public extern static void WaterfallManager_DrawWaterfall(WaterfallManager self, int waterfallType, int x, int y, float opacity, Vector2 position, Rectangle sourceRect, Color color, SpriteEffects effects);

    private void On_WaterfallManager_DrawWaterfall_int_float(On_WaterfallManager.orig_DrawWaterfall_int_float orig, WaterfallManager self, int Style, float Alpha) {
        Main.tileSolid[546] = false;
        float num = 0f;
        float num2 = 99999f;
        float num3 = 99999f;
        int num4 = -1;
        int num5 = -1;
        float num6 = 0f;
        float num7 = 99999f;
        float num8 = 99999f;
        int num9 = -1;
        int num10 = -1;
        ref int currentMax = ref WaterfallManager_currentMax(self);
        ref WaterfallData[] waterfalls = ref WaterfallManager_waterfalls(self);
        ref Asset<Texture2D>[] waterfallTexture = ref WaterfallManager_waterfallTexture(self);
        ref int slowFrame = ref WaterfallManager_slowFrame(self);
        ref int waterfallDist = ref WaterfallManager_waterfallDist(self);
        ref int rainFrameForeground = ref WaterfallManager_rainFrameForeground(self);
        ref int rainFrameBackground = ref WaterfallManager_rainFrameBackground(self);
        ref int snowFrameForeground = ref WaterfallManager_snowFrameForeground(self);
        ref int regularFrame = ref WaterfallManager_regularFrame(self);
        for (int i = 0; i < currentMax; i++) {
            int num11 = 0;
            int num12 = waterfalls[i].type;
            int num13 = waterfalls[i].x;
            int num14 = waterfalls[i].y;
            int num15 = 0;
            int num16 = 0;
            int num17 = 0;
            int num18 = 0;
            int num19 = 0;
            int num20 = 0;
            int num21;
            int num22;
            bool tar = num12 == MaxWaterfalls - 1;
            bool custom = num12 >= MaxWaterfalls - 2;
            if (num12 == 1 || num12 == 14 || num12 == 25 || custom) {
                if (Main.drewLava || waterfalls[i].stopAtStep == 0)
                    continue;

                num21 = 32 * (tar ? _slowFrame : slowFrame);
            }
            else {
                switch (num12) {
                    case 11:
                    case 22: {
                            if (Main.drewLava)
                                continue;

                            num22 = waterfallDist / 4;
                            if (num12 == 22)
                                num22 = waterfallDist / 2;

                            if (waterfalls[i].stopAtStep > num22)
                                waterfalls[i].stopAtStep = num22;

                            if (waterfalls[i].stopAtStep == 0 || (float)(num14 + num22) < Main.screenPosition.Y / 16f || (float)num13 < Main.screenPosition.X / 16f - 20f || (float)num13 > (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 20f)
                                continue;

                            int num23;
                            int num24;
                            if (num13 % 2 == 0) {
                                num23 = rainFrameForeground + 3;
                                if (num23 > 7)
                                    num23 -= 8;

                                num24 = rainFrameBackground + 2;
                                if (num24 > 7)
                                    num24 -= 8;

                                if (num12 == 22) {
                                    num23 = snowFrameForeground + 3;
                                    if (num23 > 7)
                                        num23 -= 8;
                                }
                            }
                            else {
                                num23 = rainFrameForeground;
                                num24 = rainFrameBackground;
                                if (num12 == 22)
                                    num23 = snowFrameForeground;
                            }

                            Rectangle value = new Rectangle(num24 * 18, 0, 16, 16);
                            Rectangle value2 = new Rectangle(num23 * 18, 0, 16, 16);
                            Vector2 origin = new Vector2(8f, 8f);
                            Vector2 position = ((num14 % 2 != 0) ? (new Vector2(num13 * 16 + 8, num14 * 16 + 8) - Main.screenPosition) : (new Vector2(num13 * 16 + 9, num14 * 16 + 8) - Main.screenPosition));
                            Tile tile = Main.tile[num13, num14 - 1];
                            if (tile.HasTile && tile.BottomSlope)
                                position.Y -= 16f;

                            bool flag = false;
                            float rotation = 0f;
                            for (int j = 0; j < num22; j++) {
                                Color color = Lighting.GetColor(num13, num14);
                                float num25 = 0.6f;
                                float num26 = 0.3f;
                                if (j > num22 - 8) {
                                    float num27 = (float)(num22 - j) / 8f;
                                    num25 *= num27;
                                    num26 *= num27;
                                }

                                Color color2 = color * num25;
                                Color color3 = color * num26;
                                if (num12 == 22) {
                                    Main.spriteBatch.Draw(waterfallTexture[22].Value, position, value2, color2, 0f, origin, 1f, SpriteEffects.None, 0f);
                                }
                                else {
                                    Main.spriteBatch.Draw(waterfallTexture[12].Value, position, value, color3, rotation, origin, 1f, SpriteEffects.None, 0f);
                                    Main.spriteBatch.Draw(waterfallTexture[11].Value, position, value2, color2, rotation, origin, 1f, SpriteEffects.None, 0f);
                                }

                                if (flag)
                                    break;

                                num14++;
                                Tile tile2 = Main.tile[num13, num14];
                                if (WorldGen.SolidTile(tile2))
                                    flag = true;

                                if (tile2.LiquidAmount > 0) {
                                    int num28 = (int)(16f * ((float)(int)tile2.LiquidAmount / 255f)) & 0xFE;
                                    if (num28 >= 15)
                                        break;

                                    value2.Height -= num28;
                                    value.Height -= num28;
                                }

                                if (num14 % 2 == 0)
                                    position.X += 1f;
                                else
                                    position.X -= 1f;

                                position.Y += 16f;
                            }

                            waterfalls[i].stopAtStep = 0;
                            continue;
                        }
                    case 0:
                        num12 = Style;
                        break;
                    case 2:
                        if (Main.drewLava)
                            continue;
                        break;
                }

                num21 = 32 * regularFrame;
            }

            int num29 = 0;
            num22 = waterfallDist;
            Color color4 = Color.White;
            for (int k = 0; k < num22; k++) {
                if (num29 >= 2)
                    break;

                WaterfallManager_AddLight(self, num12, num13, num14);
                Tile tile3 = Main.tile[num13, num14];

                if (tile3.HasUnactuatedTile && Main.tileSolid[tile3.TileType] && !Main.tileSolidTop[tile3.TileType] && !TileID.Sets.Platforms[tile3.TileType] && tile3.BlockType == 0)
                    break;

                Tile tile4 = Main.tile[num13 - 1, num14];

                Tile tile5 = Main.tile[num13, num14 + 1];

                Tile tile6 = Main.tile[num13 + 1, num14];

                if (WorldGen.SolidTile(tile5) && !tile3.IsHalfBlock)
                    num11 = 8;
                else if (num16 != 0)
                    num11 = 0;

                int num30 = 0;
                int num31 = num18;
                int num32 = 0;
                int num33 = 0;
                bool flag2 = false;
                if (tile5.TopSlope && !tile3.IsHalfBlock && tile5.TileType != 19) {
                    flag2 = true;
                    if (tile5.Slope == (SlopeType)1) {
                        num30 = 1;
                        num32 = 1;
                        num17 = 1;
                        num18 = num17;
                    }
                    else {
                        num30 = -1;
                        num32 = -1;
                        num17 = -1;
                        num18 = num17;
                    }

                    num33 = 1;
                }
                else if ((!WorldGen.SolidTile(tile5) && !tile5.BottomSlope && !tile3.IsHalfBlock) || (!tile5.HasTile && !tile3.IsHalfBlock)) {
                    num29 = 0;
                    num33 = 1;
                    num32 = 0;
                }
                else if ((WorldGen.SolidTile(tile4) || tile4.TopSlope || tile4.LiquidAmount > 0) && !WorldGen.SolidTile(tile6) && tile6.LiquidAmount == 0) {
                    if (num17 == -1)
                        num29++;

                    num32 = 1;
                    num33 = 0;
                    num17 = 1;
                }
                else if ((WorldGen.SolidTile(tile6) || tile6.TopSlope || tile6.LiquidAmount > 0) && !WorldGen.SolidTile(tile4) && tile4.LiquidAmount == 0) {
                    if (num17 == 1)
                        num29++;

                    num32 = -1;
                    num33 = 0;
                    num17 = -1;
                }
                else if (((!WorldGen.SolidTile(tile6) && !tile3.TopSlope) || tile6.LiquidAmount == 0) && !WorldGen.SolidTile(tile4) && !tile3.TopSlope && tile4.LiquidAmount == 0) {
                    num33 = 0;
                    num32 = num17;
                }
                else {
                    num29++;
                    num33 = 0;
                    num32 = 0;
                }

                if (num29 >= 2) {
                    num17 *= -1;
                    num32 *= -1;
                }

                int num34 = -1;
                if (num12 != 1 && num12 != 14 && num12 != 25) {
                    if (tile5.HasTile)
                        num34 = tile5.TileType;

                    if (tile3.HasTile)
                        num34 = tile3.TileType;
                }

                switch (num34) {
                    case 160:
                        num12 = 2;
                        break;
                    case 262:
                    case 263:
                    case 264:
                    case 265:
                    case 266:
                    case 267:
                    case 268:
                        num12 = 15 + num34 - 262;
                        // Patch note: ^ Both are used below.
                        break;
                }

                if (num34 != -1)
                    TileLoader.ChangeWaterfallStyle(num34, ref num12);

                Color color5 = Lighting.GetColor(num13, num14);
                if (k > 50 && !tar)
                    WaterfallManager_TrySparkling(self, num13, num14, num17, color5);

                float alpha = WaterfallManager_GetAlpha(self, Alpha, num22, num12, num14, k, tile3);
                color5 = WaterfallManager_StylizeColor(self, alpha, num22, num12, num14, k, tile3, color5);
                if (num12 == 1) {
                    float num35 = Math.Abs((float)(num13 * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
                    float num36 = Math.Abs((float)(num14 * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
                    if (num35 < (float)(Main.screenWidth * 2) && num36 < (float)(Main.screenHeight * 2)) {
                        float num37 = (float)Math.Sqrt(num35 * num35 + num36 * num36);
                        float num38 = 1f - num37 / ((float)Main.screenWidth * 0.75f);
                        if (num38 > 0f)
                            num6 += num38;
                    }

                    if (num35 < num7) {
                        num7 = num35;
                        num9 = num13 * 16 + 8;
                    }

                    if (num36 < num8) {
                        num8 = num35;
                        num10 = num14 * 16 + 8;
                    }
                }
                else if (num12 != 1 && num12 != 14 && num12 != 25 && num12 != 11 && num12 != 12 && num12 != 22) {
                    float num39 = Math.Abs((float)(num13 * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
                    float num40 = Math.Abs((float)(num14 * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
                    if (num39 < (float)(Main.screenWidth * 2) && num40 < (float)(Main.screenHeight * 2)) {
                        float num41 = (float)Math.Sqrt(num39 * num39 + num40 * num40);
                        float num42 = 1f - num41 / ((float)Main.screenWidth * 0.75f);
                        if (num42 > 0f)
                            num += num42;
                    }

                    if (num39 < num2) {
                        num2 = num39;
                        num4 = num13 * 16 + 8;
                    }

                    if (num40 < num3) {
                        num3 = num39;
                        num5 = num14 * 16 + 8;
                    }
                }

                int num43 = (int)tile3.LiquidAmount / 16;
                if (flag2 && num17 != num31) {
                    int num44 = 2;
                    if (num31 == 1)
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16 + 16 - num44) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43 - num44), color5, SpriteEffects.FlipHorizontally);
                    else
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + 16 - num44) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43 - num44), color5, SpriteEffects.None);
                }

                if (num15 == 0 && num30 != 0 && num16 == 1 && num17 != num18) {
                    num30 = 0;
                    num17 = num18;
                    color5 = Color.White;
                    if (num17 == 1)
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16 + 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.FlipHorizontally);
                    else
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16 + 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.FlipHorizontally);
                }

                if (num19 != 0 && num32 == 0 && num33 == 1) {
                    if (num17 == 1) {
                        if (num20 != num12)
                            WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11 + 8) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color4, SpriteEffects.FlipHorizontally);
                        else
                            WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11 + 8) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color5, SpriteEffects.FlipHorizontally);
                    }
                    else {
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11 + 8) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color5, SpriteEffects.None);
                    }
                }

                if (num11 == 8 && num16 == 1 && num19 == 0) {
                    if (num18 == -1) {
                        if (num20 != num12)
                            WaterfallManager_DrawWaterfall(self, num20, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color4, SpriteEffects.None);
                        else
                            WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color5, SpriteEffects.None);
                    }
                    else if (num20 != num12) {
                        WaterfallManager_DrawWaterfall(self, num20, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color4, SpriteEffects.FlipHorizontally);
                    }
                    else {
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color5, SpriteEffects.FlipHorizontally);
                    }
                }

                if (num30 != 0 && num15 == 0) {
                    if (num31 == 1) {
                        if (num20 != num12)
                            WaterfallManager_DrawWaterfall(self, num20, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color4, SpriteEffects.FlipHorizontally);
                        else
                            WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.FlipHorizontally);
                    }
                    else if (num20 != num12) {
                        WaterfallManager_DrawWaterfall(self, num20, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color4, SpriteEffects.None);
                    }
                    else {
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.None);
                    }
                }

                if (num33 == 1 && num30 == 0 && num19 == 0) {
                    if (num17 == -1) {
                        if (num16 == 0)
                            WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43), color5, SpriteEffects.None);
                        else if (num20 != num12)
                            WaterfallManager_DrawWaterfall(self, num20, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color4, SpriteEffects.None);
                        else
                            WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.None);
                    }
                    else if (num16 == 0) {
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43), color5, SpriteEffects.FlipHorizontally);
                    }
                    else if (num20 != num12) {
                        WaterfallManager_DrawWaterfall(self, num20, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color4, SpriteEffects.FlipHorizontally);
                    }
                    else {
                        WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.FlipHorizontally);
                    }
                }
                else {
                    switch (num32) {
                        case 1:
                            if (Main.tile[num13, num14].LiquidAmount > 0 && !Main.tile[num13, num14].IsHalfBlock)
                                break;
                            if (num30 == 1) {
                                for (int m = 0; m < 8; m++) {
                                    int num48 = m * 2;
                                    int num49 = 14 - m * 2;
                                    int num50 = num48;
                                    num11 = 8;
                                    if (num15 == 0 && m < 2)
                                        num50 = 4;

                                    WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16 + num48, num14 * 16 + num11 + num50) - Main.screenPosition, new Rectangle(16 + num21 + num49, 0, 2, 16 - num11), color5, SpriteEffects.FlipHorizontally);
                                }
                            }
                            else {
                                int height2 = 16;
                                if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14].TileType])
                                    height2 = 8;
                                else if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14 + 1].TileType])
                                    height2 = 8;

                                WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, height2), color5, SpriteEffects.FlipHorizontally);
                            }
                            break;
                        case -1:
                            if (Main.tile[num13, num14].LiquidAmount > 0 && !Main.tile[num13, num14].IsHalfBlock)
                                break;
                            if (num30 == -1) {
                                for (int l = 0; l < 8; l++) {
                                    int num45 = l * 2;
                                    int num46 = l * 2;
                                    int num47 = 14 - l * 2;
                                    num11 = 8;
                                    if (num15 == 0 && l > 5)
                                        num47 = 4;

                                    WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16 + num45, num14 * 16 + num11 + num47) - Main.screenPosition, new Rectangle(16 + num21 + num46, 0, 2, 16 - num11), color5, SpriteEffects.FlipHorizontally);
                                }
                            }
                            else {
                                int height = 16;
                                if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14].TileType])
                                    height = 8;
                                else if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14 + 1].TileType])
                                    height = 8;

                                WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, height), color5, SpriteEffects.None);
                            }
                            break;
                        case 0:
                            if (num33 == 0) {
                                if (Main.tile[num13, num14].LiquidAmount <= 0 || Main.tile[num13, num14].IsHalfBlock)
                                    WaterfallManager_DrawWaterfall(self, num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, 16), color5, SpriteEffects.None);

                                k = 1000;
                            }
                            break;
                    }
                }

                if (tile3.LiquidAmount > 0 && !tile3.IsHalfBlock)
                    k = 1000;

                num16 = num33;
                num18 = num17;
                num15 = num32;
                num13 += num32;
                num14 += num33;
                num19 = num30;
                color4 = color5;
                if (num20 != num12)
                    num20 = num12;

                if ((tile4.HasTile && (tile4.TileType == 189 || tile4.TileType == 196)) || (tile6.HasTile && (tile6.TileType == 189 || tile6.TileType == 196)) || (tile5.HasTile && (tile5.TileType == 189 || tile5.TileType == 196)))
                    num22 = (int)(40f * ((float)Main.maxTilesX / 4200f) * Main.gfxQuality);
            }
        }

        Main.ambientWaterfallX = num4;
        Main.ambientWaterfallY = num5;
        Main.ambientWaterfallStrength = num;
        Main.ambientLavafallX = num9;
        Main.ambientLavafallY = num10;
        Main.ambientLavafallStrength = num6;
        Main.tileSolid[546] = true;
    }

    private void On_WaterfallManager_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects(On_WaterfallManager.orig_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects orig, WaterfallManager self, int waterfallType, int x, int y, float opacity, Vector2 position, Rectangle sourceRect, Color color, SpriteEffects effects) {
        if (waterfallType == MaxWaterfalls - 1) {
            Texture2D value = WaterfallManager_waterfallTexture(self)[waterfallType].Value;
            Lighting.GetCornerColors(x, y, out var vertices);
            CustomLiquidHandler.SetTarVertexColors(ref vertices, opacity, x, y);
            Main.tileBatch.Draw(value, position + new Vector2(0f, 0f), sourceRect, vertices, default(Vector2), 1f, effects);

            return;
        }

        orig(self, waterfallType, x, y, opacity, position, sourceRect, color, effects);
    }

    private Microsoft.Xna.Framework.Color On_WaterfallManager_StylizeColor(On_WaterfallManager.orig_StylizeColor orig, float alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache, Microsoft.Xna.Framework.Color aColor) {
        if (waterfallType >= MaxWaterfalls - 2) {
            float num = (float)(int)aColor.R * alpha;
            float num2 = (float)(int)aColor.G * alpha;
            float num3 = (float)(int)aColor.B * alpha;
            float num4 = (float)(int)aColor.A * alpha;

            if (waterfallType == MaxWaterfalls - 1) {
                if (num < 190f * alpha)
                    num = 190f * alpha;
                if (num2 < 190f * alpha)
                    num2 = 190f * alpha;
                if (num3 < 190f * alpha)
                    num3 = 190f * alpha;
            }

            aColor = new Color((int)num, (int)num2, (int)num3, (int)num4);
            return aColor;
        }

        return orig(alpha, maxSteps, waterfallType, y, s, tileCache, aColor);
    }

    private static int MaxWaterfalls => WaterfallManager_waterfallTexture(Main.instance.waterfallManager).Length;

    private void On_WaterfallManager_AddLight(On_WaterfallManager.orig_AddLight orig, int waterfallType, int x, int y) {
        if (waterfallType >= MaxWaterfalls - 2) {
            return;
        }

        orig(waterfallType, x, y);
    }

    void IPostSetupContent.PostSetupContent() {
        ref Asset<Texture2D>[] waterfallTexture = ref WaterfallManager_waterfallTexture(Main.instance.waterfallManager);
        Array.Resize(ref waterfallTexture, waterfallTexture.Length + 2);
        waterfallTexture[^2] = ModContent.Request<Texture2D>(RoALiquids.LiquidTexturesPath + "Permafrost_WaterfallStyle");
        waterfallTexture[^1] = ModContent.Request<Texture2D>(RoALiquids.LiquidTexturesPath + "Tar_WaterfallStyle");
    }


    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "findWaterfallCount")]
    public extern static ref int WaterfallManager_findWaterfallCount(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "waterfallDist")]
    public extern static ref int WaterfallManager_waterfallDist(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "qualityMax")]
    public extern static ref int WaterfallManager_qualityMax(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "currentMax")]
    public extern static ref int WaterfallManager_currentMax(WaterfallManager self);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "waterfalls")]
    public extern static ref WaterfallData[] WaterfallManager_waterfalls(WaterfallManager self);

    private void On_WaterfallManager_FindWaterfalls(On_WaterfallManager.orig_FindWaterfalls orig, WaterfallManager self, bool forced) {
        ref int findWaterfallCount = ref WaterfallManager_findWaterfallCount(self);
        findWaterfallCount++;
        if (findWaterfallCount < 30 && !forced)
            return;

        findWaterfallCount = 0;
        ref int waterfallDist = ref WaterfallManager_waterfallDist(self);
        waterfallDist = (int)(75f * Main.gfxQuality) + 25;
        ref int qualityMax = ref WaterfallManager_qualityMax(self);
        qualityMax = (int)((float)self.maxWaterfallCount * Main.gfxQuality);
        ref int currentMax = ref WaterfallManager_currentMax(self);
        currentMax = 0;
        int num = (int)(Main.screenPosition.X / 16f - 1f);
        int num2 = (int)((Main.screenPosition.X + (float)Main.screenWidth) / 16f) + 2;
        int num3 = (int)(Main.screenPosition.Y / 16f - 1f);
        int num4 = (int)((Main.screenPosition.Y + (float)Main.screenHeight) / 16f) + 2;
        num -= waterfallDist;
        num2 += waterfallDist;
        num3 -= waterfallDist;
        num4 += 20;
        if (num < 0)
            num = 0;

        if (num2 > Main.maxTilesX)
            num2 = Main.maxTilesX;

        if (num3 < 0)
            num3 = 0;

        if (num4 > Main.maxTilesY)
            num4 = Main.maxTilesY;

        ref WaterfallData[] waterfalls = ref WaterfallManager_waterfalls(self);
        ref Asset<Texture2D>[] waterfallTexture = ref WaterfallManager_waterfallTexture(Main.instance.waterfallManager);
        for (int i = num; i < num2; i++) {
            for (int j = num3; j < num4; j++) {
                Tile tile = Main.tile[i, j];

                if (!tile.HasTile)
                    continue;

                if (tile.IsHalfBlock) {
                    Tile tile2 = Main.tile[i, j - 1];

                    if (tile2.LiquidAmount < 16 || WorldGen.SolidTile(tile2)) {
                        Tile tile3 = Main.tile[i - 1, j];
                        Tile tile4 = Main.tile[i + 1, j];
                        if ((tile3.LiquidAmount > 160 || tile4.LiquidAmount > 160) && ((tile3.LiquidAmount == 0 && !WorldGen.SolidTile(tile3) && tile3.Slope == 0) || (tile4.LiquidAmount == 0 && !WorldGen.SolidTile(tile4) && tile4.Slope == 0)) && currentMax < qualityMax) {
                            waterfalls[currentMax].type = 0;
                            if (tile2.LiquidType == LiquidID.Lava || tile4.LiquidType == LiquidID.Lava || tile3.LiquidType == LiquidID.Lava)
                                waterfalls[currentMax].type = 1;
                            else if (tile2.LiquidType == LiquidID.Honey || tile4.LiquidType == LiquidID.Honey || tile3.LiquidType == LiquidID.Honey)
                                waterfalls[currentMax].type = 14;
                            else if (tile2.LiquidType == LiquidID.Shimmer || tile4.LiquidType == LiquidID.Shimmer || tile3.LiquidType == LiquidID.Shimmer)
                                waterfalls[currentMax].type = 25;
                            else if (tile2.LiquidType == 4 || tile4.LiquidType == 4 || tile3.LiquidType == 4)
                                waterfalls[currentMax].type = MaxWaterfalls - 2;
                            else if (tile2.LiquidType == 5 || tile4.LiquidType == 5 || tile3.LiquidType == 5)
                                waterfalls[currentMax].type = MaxWaterfalls - 1;
                            else
                                waterfalls[currentMax].type = 0;

                            waterfalls[currentMax].x = i;
                            waterfalls[currentMax].y = j;
                            currentMax++;
                        }
                    }
                }

                if (tile.TileType == 196) {
                    Tile tile5 = Main.tile[i, j + 1];
                    if (!WorldGen.SolidTile(tile5) && tile5.Slope == 0 && currentMax < qualityMax) {
                        waterfalls[currentMax].type = 11;
                        waterfalls[currentMax].x = i;
                        waterfalls[currentMax].y = j + 1;
                        currentMax++;
                    }
                }

                if (tile.TileType == 460) {
                    Tile tile6 = Main.tile[i, j + 1];
                    if (!WorldGen.SolidTile(tile6) && tile6.Slope == 0 && currentMax < qualityMax) {
                        waterfalls[currentMax].type = 22;
                        waterfalls[currentMax].x = i;
                        waterfalls[currentMax].y = j + 1;
                        currentMax++;
                    }
                }
            }
        }
    }
}
