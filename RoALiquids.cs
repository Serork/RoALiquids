using ReLogic.Content.Sources;

using RoA;

using System.IO;

using Terraria.ModLoader;

namespace RoALiquids;

sealed class RoALiquids : Mod {
    public static string LiquidTexturesPath => $"{nameof(RoALiquids)}/Resources/Liquids/";

    private static RoALiquids? _instance;

    public RoALiquids() {
        _instance = this;
    }

    public static RoALiquids Instance => _instance ??= ModContent.GetInstance<RoALiquids>();

    public override IContentSource CreateDefaultContentSource() => new CustomContentSource(base.CreateDefaultContentSource());

    public override void PostSetupContent() {
        foreach (IPostSetupContent type in GetContent<IPostSetupContent>()) {
            type.PostSetupContent();
        }
    }

    public override void HandlePacket(BinaryReader reader, int sender) {
        MultiplayerSystem.HandlePacket(reader, sender);
    }

    public override object Call(params object[] args) => CustomLiquidsModCalls.Call(args);
}
