using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient {
    public class DistroCommand : BaseScript {
        public DistroCommand() {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

            EventHandlers["DistroReceive"] += new Action<int>(OnDistroReceive);
        }

        private void OnDistroReceive(int version) {
            // tell the player
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Bem vindo a versao {version}" }
            });
        }

        private void OnClientResourceStart(string resourceName) {
            if (GetCurrentResourceName() != resourceName) {
                return;
            }

            RegisterCommand("distro", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo versao ao servidor..." }
                });

                TriggerServerEvent("DistroRequest");
            }), false);
        }
    }
}
