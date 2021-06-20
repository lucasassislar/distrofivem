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
        private int nOptionHighlighted = -1;

        public PlayerManager() {
            Singleton = this;
            Tick += PlayerManager_Tick;

            Items = new List<BaseItem>();

            EventHandlers["ClearInventory"] += new Action(OnClearInventory);
            EventHandlers["UpdateInventory"] += new Action<string, int>(OnReceiveInventory);
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
            SendNuiMessage("{ \"type\": \"clearInventory\" }");

            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Lavo ta novo" }
            });

            Items.Clear();
        }

        private void OnReceiveInventory(string strItemName, int nSlot) {
            SendNuiMessage("{ \"type\": \"receiveInventory\", \"item\": \"" + strItemName + "\", \"slot\": " + nSlot + "  }");

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

            DisableControlAction(0, (int)Control.WeaponWheelLeftRight, true);
            DisableControlAction(0, (int)Control.WeaponWheelNext, true);
            DisableControlAction(0, (int)Control.WeaponWheelPrev, true);
            DisableControlAction(0, (int)Control.WeaponWheelUpDown, true);
            DisableControlAction(0, (int)Control.SelectWeapon, true);
            DisableControlAction(0, (int)Control.SelectWeaponAutoRifle, true);
            DisableControlAction(0, (int)Control.SelectWeaponHandgun, true);
            DisableControlAction(0, (int)Control.SelectWeaponHeavy, true);
            DisableControlAction(0, (int)Control.SelectWeaponMelee, true);
            DisableControlAction(0, (int)Control.SelectWeaponShotgun, true);
            DisableControlAction(0, (int)Control.SelectWeaponSmg, true);
            DisableControlAction(0, (int)Control.SelectWeaponSniper, true);
            DisableControlAction(0, (int)Control.SelectWeaponSpecial, true);
            DisableControlAction(0, (int)Control.SelectWeaponUnarmed, true);

            int nNewOption = -1;
            if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponUnarmed)) {
                // 1
                nNewOption = 0;
            } else if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponMelee)) {
                // 2
                nNewOption = 1;
            } else if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponShotgun)) {
                // 3
                nNewOption = 2;
            } else if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponHeavy)) {
                // 4
                nNewOption = 3;
            } else if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponSpecial)) {
                // 5
                nNewOption = 4;
            } else if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponHandgun)) {
                // 6
                nNewOption = 5;
            } else if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponSmg)) {
                // 7
                nNewOption = 6;
            } else if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponAutoRifle)) {
                // 8
                nNewOption = 7;
            } else if (IsDisabledControlJustPressed(0, (int)Control.SelectWeaponSniper)) {
                // 9
                nNewOption = 8;
            }

            //Items[nOptionHighlighted].OnStartControl();

            if (nNewOption != -1) {
                if (nOptionHighlighted != -1) {
                    if (Items.Count > nOptionHighlighted) {
                        Items[nOptionHighlighted].OnEndedControl();
                    }
                }

                nOptionHighlighted = nNewOption;
                SendNuiMessage("{ \"type\": \"inventoryHighlight\", \"slot\": " + nOptionHighlighted + " }");
            }

            if (IsControlJustPressed(1, (int)GameKey.E)) {
                SendNuiMessage("{ \"type\": \"showFullscreenUI\" }");
                SetNuiFocus(true, true);
            }

            for (int i = 0; i < Items.Count; i++) {
                Items[i].Tick();
            }
        }
    }
}
