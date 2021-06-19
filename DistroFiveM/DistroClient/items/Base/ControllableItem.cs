using DistroClient.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Items {
    public class ControllableItem : BaseItem {
        private bool bSpawnedItem;
        private bool bControllingItem;

        private int nPlayerEntity;
        private int nTimer;

        public ControllableItem() {
        }

        public virtual void OnControl() {

        }

        public override void Tick() {
            base.Tick();

            if (bControllingItem) {
                DisableAllControlActions(PlayerPedId());

                int nGameTimer = GetGameTimer();
                if (nGameTimer - nTimer > 100) {
                    if (IsDisabledControlPressed(0, (int)GameKey.E)) {
                        bControllingItem = false;
                        UpdateEntity();
                        return;
                    }
                }
            } else {
                if (IsControlJustPressed(1, (int)GameKey.E)) {
                    nTimer = GetGameTimer();

                    bControllingItem = true;
                    UpdateEntity();
                }
            }
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
