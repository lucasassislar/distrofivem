using CitizenFX.Core;
using DistroClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace DistroClient {
    public class RoleManager : BaseScript {
        public static RoleManager Singleton { get; private set; }

        private Dictionary<string, DistroCommand> dictCommands;

        public RoleManager() {
            Singleton = this;

            dictCommands = new Dictionary<string, DistroCommand>();

            EventHandlers["Accept"] += new Action<string, string>(OnServerResponse);
        }

        public void RegisterCommand(string cmd, DistroCommand command) {
            this.dictCommands.Add(cmd, command);
        }

        private async void OnServerResponse(string resourceName, string resourceParam) {
            DistroCommand command;
            if (this.dictCommands.TryGetValue(resourceName, out command)) {
                command.OnAccept(resourceParam);
            }
        }
    }
}
