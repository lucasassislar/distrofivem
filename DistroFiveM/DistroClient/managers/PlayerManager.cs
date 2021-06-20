using CitizenFX.Core;
using CitizenFX.Core.Native;
using DistroClient.Items;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Managers {
    public class PlayerManager : BaseScript {
        public static PlayerManager Singleton { get; private set; }

        public List<BaseItem> Items { get; private set; }

        private bool bLoaded;

        public PlayerManager() {
            Singleton = this;
            Tick += PlayerManager_Tick;

            Items = new List<BaseItem>();

            EventHandlers["ClearInventory"] += new Action(OnClearInventory);
            EventHandlers["UpdateInventory"] += new Action<string>(OnReceiveInventory);
        }

        private void Load() {
            if (bLoaded) {
                return;
            }
            bLoaded = true;

            API.SetNuiFocus(false, false);
            RegisterNUICallback("CloseFullscreenMenu", ReceiveCloseFullScreenMenu);
        }

        private CallbackDelegate ReceiveCloseFullScreenMenu(IDictionary<string, object> data, CallbackDelegate del) {
            SetNuiFocus(false, false);

            return del;
        }

        private void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback) {
            API.RegisterNuiCallbackType(msg); // Remember API calls must be executed on the first tick at the earliest!

            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) => {
                callback.Invoke(body, resultCallback);
            });
        }

        private void OnClearInventory() {
            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Lavo ta novo" }
            });

            Items.Clear();
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

            Items.Add(item);
        }

        private async Task PlayerManager_Tick() {
            Load();

            if (IsControlJustPressed(1, (int)GameKey.E)) {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Abrindo inventário" }
                });

                SendNuiMessage("{ \"type\": \"showFullscreenUI\" }");
                SetNuiFocus(true, true);
            }

            for (int i = 0; i < Items.Count; i++) {
                Items[i].Tick();
            }
        }
    }
}
