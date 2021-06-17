using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class StatusCommand : DistroCommand {
        public StatusCommand() {
        }

        public override void OnStart() {
            RoleManager.Singleton.RegisterCommand("status", this);

            RegisterCommand("status", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo status ao servidor..." }
                });

                TriggerServerEvent("Request", "status");
            }), false);
        }

        public override void OnAccept(string strParam) {
            string[] strs = strParam.Split(':');

            // tell the player
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Bem vindo a versao {strs[0]} - {strs[1]}" }
            });
        }
    }
}
