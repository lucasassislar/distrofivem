using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroServer.Commands {
    public class WeaponCommand {
        public WeaponCommand() {
        }

        public static bool HandleRequest(Player player, ref string strRequestParam) {
            return Core.Singleton.Database.HasPermission(player, UserRole.Admin);
        }
    }
}
