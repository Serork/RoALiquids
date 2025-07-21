using ReLogic.Content.Sources;

using RoA;

using System.IO;

using Terraria.ModLoader;

namespace RoALiquids;

sealed class RoALiquids : Mod {
    public static string LiquidTexturesPath => $"{nameof(RoALiquids)}/Resources/Liquids/";

    public override IContentSource CreateDefaultContentSource() => new CustomContentSource(base.CreateDefaultContentSource());

    public override void PostSetupContent() {
        foreach (IPostSetupContent type in GetContent<IPostSetupContent>()) {
            type.PostSetupContent();
        }
    }

    public override void HandlePacket(BinaryReader reader, int sender) {
        MultiplayerSystem.HandlePacket(reader, sender);
    }
}
