using System.IO;

using Terraria;

namespace RoALiquids;

sealed class PlayLiquidChangeSoundPacket : NetPacket {
    public PlayLiquidChangeSoundPacket(int i, int j, byte liquidChangeType) {
        Writer.Write(i);
        Writer.Write(j);
        Writer.Write(liquidChangeType);
    }

    public override void Read(BinaryReader reader, int sender) {
        int x = reader.ReadInt32();
        int y = reader.ReadInt32();
        CustomLiquidHandler.TileChangeType liquidChangeType = (CustomLiquidHandler.TileChangeType)reader.ReadByte();
        if (!Main.gameMenu)
            CustomLiquidHandler.PlayLiquidChangeSound(liquidChangeType, x, y);
    }
}
