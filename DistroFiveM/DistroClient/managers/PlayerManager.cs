using CitizenFX.Core;
using DistroClient.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Managers {
    public class PlayerManager : BaseScript {
        public static PlayerManager Singleton { get; private set; }

        private List<BaseItem> items;

        public PlayerManager() {
            Singleton = this;
            Tick += PlayerManager_Tick;

            items = new List<BaseItem>();

            EventHandlers["ClearInventory"] += new Action(OnClearInventory);
            EventHandlers["UpdateInventory"] += new Action<string>(OnReceiveInventory);
        }

        private void OnClearInventory() {
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Lavo ta novo" }
            });

            items.Clear();
        }

        private void OnReceiveInventory(string strItemName) {
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Recebeu {strItemName}" }
            });

            BaseItem item = ItemFactory.Create(strItemName);

            if (item == null) {
                return;
            }

            items.Add(item);
        }

        private async Task PlayerManager_Tick() {
            for (int i = 0; i < items.Count; i++) {
                items[i].Tick();
            }
        }
    }
}
