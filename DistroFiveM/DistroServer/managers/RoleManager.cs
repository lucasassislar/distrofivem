using CitizenFX.Core;
using DistroServer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroServer.Managers {
    public class RoleManager : BaseScript {
        public static RoleManager Singleton { get; private set; }

        private Dictionary<string, DistroCommand> dictCommands;

        public RoleManager() {
            dictCommands = new Dictionary<string, DistroCommand>();
            InitializeCommands();

            EventHandlers["Request"] += new Action<Player, string, string>(OnClientRequest);
        }

        private void InitializeCommands() {
            dictCommands.Add("car", CarCommand.HandleRequest);
            dictCommands.Add("noclip", NoClipCommand.HandleRequest);
            dictCommands.Add("nc", NoClipCommand.HandleRequest);
            dictCommands.Add("status", StatusCommand.HandleRequest);
            dictCommands.Add("promote", PromoteCommand.HandleRequest);
        }

        private void OnClientRequest([FromSource] Player player, string strRequestName, string strRequestParam) {
            Core.Singleton.Database.SavePlayer(player);

            DistroCommand command;
            if (dictCommands.TryGetValue(strRequestName, out command)) {
                if (command(player, ref strRequestParam)) {
                    player.TriggerEvent("Accept", strRequestName, strRequestParam);
                }
            }
        }

    }
}
