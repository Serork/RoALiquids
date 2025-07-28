using System;

using Terraria;

namespace RoALiquids;

static class CustomLiquidsModCalls {
    public static object Call(params object[] args) {
        RoALiquids riseOfAgesLiquids = RoALiquids.Instance;
        Array.Resize(ref args, 2);
        string success = "Success";
        try {
            string message = args[0] as string;
            if (message == "IsTarWet") {
                if (args[1] is not NPC npc) {
                    throw new Exception($"{args[1]} is not NPC");
                }
                return npc.GetGlobalNPC<CustomLiquidCollision_NPC>().tarWet;
            }
        }
        catch (Exception e) {
            riseOfAgesLiquids.Logger.Error($"Call Error: {e.StackTrace} {e.Message}");
        }
        return null;
    }
}
