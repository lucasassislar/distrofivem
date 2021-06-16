﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Commands {
    public class NoClipCommand : BaseScript {
        public NoClipCommand() {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

            EventHandlers["Accept_NoClip"] += new Action(NoClipAccept);

            Tick += NoClipCommand_Tick;
        }

        private bool bHasNoClipPermission;

        private bool bNoClipEnabled;

        private int nEntity;

        private float fSpeed = 1;

        private float fY = 0.5f;
        private float fZ = 0.2f;
        private float fH = 3;

        private async Task NoClipCommand_Tick() {
            if (!bHasNoClipPermission) {
                return;
            }

            if (IsControlJustPressed(1, (int)GameKey.F1)) {
                bNoClipEnabled = !bNoClipEnabled;
                
                UpdateNoClip();
            }

            if (bNoClipEnabled) {
                float fCurrentSpeed = fSpeed;

                float yOff = 0.0f;
                float zOff = 0.0f;

                if (IsDisabledControlPressed(0, (int)GameKey.F2)) {
                    bNoClipEnabled = false;
                    UpdateNoClip();
                    return;
                }

                DisableAllControlActions(PlayerPedId());
                if (IsDisabledControlPressed(0, (int)GameKey.W)) {
                    yOff = fY;
                } else if (IsDisabledControlPressed(0, (int)GameKey.S)) {
                    yOff = -fY;
                }

                if (IsDisabledControlPressed(0, (int)GameKey.Q)) {
                    zOff = -fZ;
                } else if (IsDisabledControlPressed(0, (int)GameKey.E)) {
                    zOff = fZ;
                }

                if (IsDisabledControlPressed(0, (int)GameKey.A)) {
                    SetEntityHeading(nEntity, GetEntityHeading(nEntity) + fH);
                } else if (IsDisabledControlPressed(0, (int)GameKey.D)) {
                    SetEntityHeading(nEntity, GetEntityHeading(nEntity) - fH);
                }

                Vector3 vNewPos = GetOffsetFromEntityInWorldCoords(nEntity, 0.0f, yOff * (fCurrentSpeed + 0.3f), zOff * (fCurrentSpeed + 0.3f));
                float fHeading = GetEntityHeading(nEntity);
                SetEntityVelocity(nEntity, 0.0f, 0.0f, 0.0f);
                SetEntityRotation(nEntity, 0.0f, 0.0f, 0.0f, 0, false);
                SetEntityHeading(nEntity, fHeading);
                SetEntityCoordsNoOffset(nEntity, vNewPos.X, vNewPos.Y, vNewPos.Z, true, true, true);
            }
        }

        private void UpdateNoClip() {
            if (IsPedInAnyVehicle(PlayerPedId(), false)) {
                nEntity = GetVehiclePedIsIn(PlayerPedId(), false);
            } else {
                nEntity = PlayerPedId();
            }

            SetEntityCollision(nEntity, !bNoClipEnabled, !bNoClipEnabled);
            FreezeEntityPosition(nEntity, bNoClipEnabled);
            SetEntityInvincible(nEntity, bNoClipEnabled);
            SetVehicleRadioEnabled(nEntity, !bNoClipEnabled);
        }

        private void NoClipAccept() {
            bHasNoClipPermission = true;
            bNoClipEnabled = true;
            UpdateNoClip();

            TriggerEvent("chat:addMessage", new {
                color = new[] { 255, 0, 0 },
                args = new[] { "[d1str0]", $"Curta seu no clip :)" }
            });
        }

        private void OnClientResourceStart(string resourceName) {
            if (GetCurrentResourceName() != resourceName) {
                return;
            }

            RegisterCommand("noclip", new Action<int, List<object>, string>(async (source, args, raw) => {
                TriggerEvent("chat:addMessage", new {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[d1str0]", $"Pedindo no clip ao servidor..." }
                });

                if (this.bHasNoClipPermission) {
                    bNoClipEnabled = !bNoClipEnabled;
                    UpdateNoClip();
                } else {
                    TriggerServerEvent("Request_NoClip");
                }
            }), false);


        }
    }
}