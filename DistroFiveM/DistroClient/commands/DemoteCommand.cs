using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class DemoteCommand : DistroCommand {
        public DemoteCommand() {
        }

        public override void OnStart() {
            RoleManager.Singleton.RegisterCommand("demote", this);

            RegisterCommand("demote", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo demoção ao servidor..." }
                });

                if (args.Count == 0) {
                    TriggerEvent("chat:addMessage", new {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[d1str0]", "Use assim '/demote [nome da pessoa]'" }
                    });
                    return;
                }

                TriggerServerEvent("Request", "demote", args[0]);
            }), false);
        }

        public override void OnAccept(string strParam) {
            // tell the player
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Player demovido: {strParam}" }
            });
        }
    }
}
