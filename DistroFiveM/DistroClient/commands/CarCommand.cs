using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class CarCommand : DistroCommand {

        public CarCommand() {

        }

        public override void OnStart() {
            RoleManager.Singleton.RegisterCommand("car", this);

            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo carro ao servidor..." }
                });

                // account for the argument not being passed
                var strCarModelName = "t20";
                if (args.Count > 0) {
                    strCarModelName = args[0].ToString();
                }

                TriggerServerEvent("Request", "car", strCarModelName);
            }), false);
        }

        public override async void OnAccept(string strCarName) {
            // check if the model actually exists
            var hash = (uint)GetHashKey(strCarName);
            if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash)) {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[CarSpawner]", $"It might have been a good thing that you tried to spawn a {strCarName}. Who even wants their spawning to actually ^*succeed?" }
                });
                return;
            }

            // create the vehicle
            var vehicle = await World.CreateVehicle(strCarName, Game.PlayerPed.Position, Game.PlayerPed.Heading);

            // set the player ped into the vehicle and driver seat
            Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);

            // tell the player
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Curta seu novo ^*{strCarName}!" }
            });
        }

    }
}
