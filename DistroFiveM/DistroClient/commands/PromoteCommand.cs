using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class PromoteCommand : DistroCommand {
        public PromoteCommand() {
        }

        public override void OnStart() {
            RoleManager.Singleton.RegisterCommand("promote", this);

            RegisterCommand("promote", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo promoção ao servidor..." }
                });

                if (args.Count == 0) {
                    TriggerEvent("chat:addMessage", new {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[d1str0]", "Use assim '/promote [nome da pessoa]'" }
                    });
                    return;
                }

                TriggerServerEvent("Request", "promote", args[0]);
            }), false);
        }

        public override void OnAccept(string strParam) {
            // tell the player
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Player promovido: {strParam}" }
            });
        }
    }
}
