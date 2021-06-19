using System;
using System.Collections.Generic;
using CitizenFX.Core;
using DistroServer.Model;
using static CitizenFX.Core.Native.API;

namespace DistroServer.Commands {
    public class StatusCommand {
        public StatusCommand() {

        }

        public static bool HandleRequest(Player player, ref string strRequestParam) {
            User user = Core.Singleton.Database.GetUser(player);

            //Core.Singleton.Database.UpdateInventory(user);
            int version = Core.Singleton.Database.IncreaseVersion();

            strRequestParam = $"{version}:{user.role}";

            Core.Singleton.UpdateInventory(player);

            return true;
        }
    }
}
