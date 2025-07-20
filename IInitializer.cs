using Terraria.ModLoader;

namespace RoALiquids;

interface IInitializer : ILoadable {
    void ILoadable.Unload() { }
}
