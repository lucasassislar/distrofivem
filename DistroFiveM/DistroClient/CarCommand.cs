using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class CarCommand : BaseScript {
        public CarCommand() {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

            EventHandlers["CarAccept"] += new Action<string>(OnCarAccept);
        }

        private async void OnCarAccept(string strCarName) {
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
                args = new[] { "[CarSpawner]", $"Woohoo! Enjoy your new ^*{strCarName}!" }
            });
        }

        private void OnClientResourceStart(string resourceName) {
            if (GetCurrentResourceName() != resourceName) {
                return;
            }

            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo carro ao servidor..." }
                });

                // account for the argument not being passed
                var strCarModelName = "adder";
                if (args.Count > 0) {
                    strCarModelName = args[0].ToString();
                }

                TriggerServerEvent("CarRequest", strCarModelName);

                
            }), false);
        }
    }
}
