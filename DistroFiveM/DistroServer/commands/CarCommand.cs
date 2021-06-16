﻿using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroServer.Commands {
    public class CarCommand : BaseScript {
        public CarCommand() {
            EventHandlers["Request_Car"] += new Action<Player, string>(CarRequest);
        }

        private void CarRequest([FromSource] Player player, string carName) {
            if (GameManager.Singleton.Database.HasPermission(player, UserRole.Admin)) {
                player.TriggerEvent("Accept_Car", carName);
            }
        }
    }
}
