using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroServer.Commands {
    public class NoClipCommand : BaseScript {
        public NoClipCommand() {
            EventHandlers["Request_NoClip"] += new Action<Player>(NoClipRequest);
        }

        private void NoClipRequest([FromSource] Player player) {
            if (GameManager.Singleton.Database.HasPermission(player, UserRole.Admin)) {
                player.TriggerEvent("Accept_NoClip");
            }
        }
    }
}
