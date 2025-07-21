using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RoALiquids.Content.Dusts;

sealed class SolidifiedTar : ModDust {
    public override void OnSpawn(Dust dust) => UpdateType = DustID.Dirt;
}
