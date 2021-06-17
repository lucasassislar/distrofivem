using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class WeaponCommand : DistroCommand {
        public WeaponCommand() {

        }

        public override void OnStart() {
            RoleManager.Singleton.RegisterCommand("weapon", this);

            RegisterCommand("weapon", new Action<int, List<object>, string>(async (source, args, raw) => {
                if (args.Count == 0) {
                    TriggerEvent("chat:addMessage", new {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[d1str0]", "Use assim '/weapon [nome da arma]'" }
                    });
                    return;
                }

                object strWeapon = args[0];
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo arma {strWeapon} ao servidor..." }
                });

                TriggerServerEvent("Request", "weapon", strWeapon);
            }), false);
        }

        public override async void OnAccept(string strWeapon) {
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Curta ai sua {strWeapon}..." }

            });

            var hash = (uint)GetHashKey("weapon_" + strWeapon);

            GiveWeaponToPed(GetPlayerPed(-1), hash, 9999, false, true);
        }

    }
}
