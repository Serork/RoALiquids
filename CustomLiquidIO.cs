using System.Collections.Generic;
using System.Linq;

using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RoALiquids;

sealed class CustomLiquidIO : ModSystem {
    private static HashSet<Point16> _customLiquidPositions = [];

    public override void Load() {
        On_WorldMap.Load += On_WorldMap_Load;
    }

    private void On_WorldMap_Load(On_WorldMap.orig_Load orig, WorldMap self) {
        orig(self);
        foreach (Point16 customLiquidPosition in _customLiquidPositions) {
            int x = customLiquidPosition.X, y = customLiquidPosition.Y;
            Tile tile = Main.tile[x, y];
            MapTile tile2 = MapHelper.CreateMapTile(x, y, Main.Map[x, y].Light);
            Main.Map.SetTile(x, y, ref tile2);
        }
    }

    public override void SaveWorldData(TagCompound tag) {
        for (int i = 0; i < Main.maxTilesX; i++) {
            for (int j = 0; j < Main.maxTilesY; j++) {
                Tile tile = Main.tile[i, j];
                if (tile.LiquidType == 5) {
                    _customLiquidPositions.Add(new Point16(i, j));
                }
            }
        }
        if (_customLiquidPositions.Count != 0) {
            tag[nameof(CustomLiquidIO)] = _customLiquidPositions.ToList();
        }
    }

    public override void LoadWorldData(TagCompound tag) {
        var customLiquidPositions = tag.GetList<Point16>(nameof(CustomLiquidIO));
        _customLiquidPositions = [.. customLiquidPositions];
        foreach (Point16 customLiquidPosition in _customLiquidPositions) {
            int x = customLiquidPosition.X, y = customLiquidPosition.Y;
            Tile tile = Main.tile[x, y];
            tile.LiquidType = 5;
        }
    }
}
