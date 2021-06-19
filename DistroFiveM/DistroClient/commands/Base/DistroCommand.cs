using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class DistroCommand : BaseScript {
        public DistroCommand() {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        public virtual async void OnAccept(string strParam) {

        }

        public virtual void OnStart() {

        }

        private async void OnClientResourceStart(string resourceName) {
            if (GetCurrentResourceName() != resourceName) {
                return;
            }

            OnStart();
        }
    }
}