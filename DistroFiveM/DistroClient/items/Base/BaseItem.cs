using DistroClient.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Items {
    public class BaseItem {
        private bool bSpawnedItem;
        private bool bControllingItem;

        private int nPlayerEntity;
        private int nTimer;

        public bool IsControlling { get; private set; }

        public BaseItem() {
        }

        public virtual void OnControl() {

        }

        public virtual void OnStartControl() {
            IsControlling = true;
            PlayerManager.TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Comecou drone" }
            });
        }

        public virtual void OnEndedControl() {
            IsControlling = false;
            PlayerManager.TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Finalizou drone" }
            });
        }

        public virtual void Tick() {
            if (IsControlling) {
                DisableAllControlActions(PlayerPedId());
            }

            //if (bControllingItem) {
            //    DisableAllControlActions(PlayerPedId());

            //    int nGameTimer = GetGameTimer();
            //    if (nGameTimer - nTimer > 500) {
            //        if (IsDisabledControlPressed(0, (int)GameKey.E)) {
            //            bControllingItem = false;
            //            
            //            OnEndedControl();

            //            UpdateEntity();
            //            return;
            //        }
            //    }
            //} else {
            //    if (IsControlJustPressed(1, (int)GameKey.E)) {
            //        bControllingItem = true;
            //        
            //        OnStartControl();

            //        nTimer = GetGameTimer();

            //        UpdateEntity();
            //    }
            //}
        }

        private void UpdateEntity() {
            if (IsPedInAnyVehicle(PlayerPedId(), false)) {
                nPlayerEntity = GetVehiclePedIsIn(PlayerPedId(), false);
            } else {
                nPlayerEntity = PlayerPedId();
            }

            SetEntityCollision(nPlayerEntity, !bControllingItem, !bControllingItem);
            FreezeEntityPosition(nPlayerEntity, bControllingItem);
            SetEntityInvincible(nPlayerEntity, bControllingItem);
            SetVehicleRadioEnabled(nPlayerEntity, !bControllingItem);
        }
    }
}
