using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class DvCommand : DistroCommand {

        public DvCommand() {

        }

        public override void OnStart() {
            RoleManager.Singleton.RegisterCommand("dv", this);

            RegisterCommand("dv", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo devolução ao servidor..." }
                });

                TriggerServerEvent("Request", "dv");
            }), false);
        }

        public override async void OnAccept(string strCarName) {
            int nPlayerPed = GetPlayerPed(-1);
            if (IsPedSittingInAnyVehicle(nPlayerPed)) {
                int nVehiclePed = GetVehiclePedIsIn(nPlayerPed, false);

                SetEntityAsMissionEntity(nVehiclePed, true, true);
                DeleteVehicle(ref nVehiclePed);
            } 
        }

    }
}
