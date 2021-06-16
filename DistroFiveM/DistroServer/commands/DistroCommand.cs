using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroServer.Commands {
    public class DistroCommand : BaseScript {
        public DistroCommand() {
            EventHandlers["DistroRequest"] += new Action<Player>(OnDistro);
        }

        private void OnDistro([FromSource] Player player) {
            GameManager.Singleton.Database.IncreaseVersion();
            GameManager.Singleton.Database.SavePlayer(player);

            player.TriggerEvent("DistroReceive", Globals.Version);
        }
    }
}
