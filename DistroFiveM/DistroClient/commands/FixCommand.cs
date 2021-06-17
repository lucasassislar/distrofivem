using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class FixCommand : DistroCommand {

        public FixCommand() {

        }

        public override void OnStart() {
            RoleManager.Singleton.RegisterCommand("fix", this);

            RegisterCommand("fix", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo reparos ao servidor..." }
                });

                TriggerServerEvent("Request", "fix");
            }), false);
        }

        public override async void OnAccept(string strCarName) {
            int nPlayerPed = GetPlayerPed(-1);
            if (!IsPedSittingInAnyVehicle(nPlayerPed)) {
                return;
            }

            int nVehiclePed = GetVehiclePedIsIn(nPlayerPed, false);
            SetVehicleEngineHealth(nVehiclePed, 1000);
            SetVehicleEngineOn(nVehiclePed, true, true, false);
            SetVehicleFixed(nVehiclePed);

            SetVehicleDirtLevel(nVehiclePed, 0);

            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Carro reparado!" }
            });
        }

    }
}
