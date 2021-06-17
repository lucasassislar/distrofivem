using System;
using System.Collections.Generic;
using CitizenFX.Core;
using DistroServer.Model;
using static CitizenFX.Core.Native.API;

namespace DistroServer.Commands {
    public class PromoteCommand {
        public PromoteCommand() {
        }

        public static bool HandleRequest(Player player, ref string strRequestParam) {
            User user = Core.Singleton.Database.GetUser(player);

            if (user.role == UserRole.Admin) {
                return Core.Singleton.Database.PromoteToAdmin(strRequestParam);
            }

            return false;
        }
    }
}
