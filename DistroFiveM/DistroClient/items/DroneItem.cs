using CitizenFX.Core;
using DistroClient.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Items {
    public class DroneItem : BaseItem {
        private int nEntity;
        private int nCamera;

        private Vector3 vCamOffsetPos;
        private Vector3 vCamRot;

        private bool bSpawned;
        
        private float fRotationSpeed;
        private Vector3 vSpeed;

        public DroneItem() {
            fRotationSpeed = 3;
            vSpeed = new Vector3(5, 5, 3);
            vCamOffsetPos = new Vector3(0, 0, -1);

            Vector3 vPos = GetEntityCoords(PlayerPedId(), false);

            float fFOV = GetGameplayCamFov();
            nCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", vPos.X, vPos.Y, vPos.Z, 0, 0, 0, fFOV, false, 2);

            nEntity = CreateObject(788747387, vPos.X, vPos.Y, vPos.Z, true, false, false);
            //SetEntityVisible(nEntity, false, false);
        }

        public override void OnEndedControl() {
            base.OnEndedControl();

            RenderScriptCams(false, false, 0, true, false);
            //SetEntityVisible(nEntity, false, false);
        }

        public override void OnStartControl() {
            base.OnStartControl();

            SetCamActive(nCamera, true);
            RenderScriptCams(true, false, 0, true, false);
        }

        private Quaternion GetEntityQuat() {
            float x = 0;
            float y = 0;
            float z = 0;
            float w = 0;
            GetEntityQuaternion(nEntity, ref x, ref y, ref z, ref w);

            return new Quaternion(x, y, z, w);
        }

        public override void Tick() {
            base.Tick();

            if (!bSpawned) {
                bSpawned = true;
                Vector3 vPlayerPos = GetEntityCoords(PlayerPedId(), false);
                SetEntityCoords(nEntity, vPlayerPos.X, vPlayerPos.Y, vPlayerPos.Z, true, false, false, false);
            }

            if (!IsControlling) {
                SetEntityVelocity(nEntity, 0, 0, 0);
                return;
            }

            DisableFirstPersonCamThisFrame();

            Vector3 vToMove = Vector3.Zero;
            if (IsDisabledControlPressed(0, (int)GameKey.A)) {
                vToMove.X -= vSpeed.X;
            } else if (IsDisabledControlPressed(0, (int)GameKey.D)) {
                vToMove.X += vSpeed.X;
            }

            if (IsDisabledControlPressed(0, (int)GameKey.W)) {
                vToMove.Y += vSpeed.Y;
            } else if (IsDisabledControlPressed(0, (int)GameKey.S)) {
                vToMove.Y -= vSpeed.Y;
            }

            if (IsDisabledControlPressed(0, (int)GameKey.Z)) {
                vToMove.Z += vSpeed.Z;
            } else if (IsDisabledControlPressed(0, (int)GameKey.Q)) {
                vToMove.Z -= vSpeed.Z;
            }

            Quaternion quat = GetEntityQuat();
            vToMove = Vector3.Transform(vToMove, quat);

            SetEntityVelocity(nEntity, vToMove.X, vToMove.Y, vToMove.Z);


            float fOffsetX = GetDisabledControlNormal(1, 1) * fRotationSpeed;
            float fOffsetY = GetDisabledControlNormal(1, 2) * fRotationSpeed;

            vCamRot.Z -= fOffsetX;
            vCamRot.X -= fOffsetY;

            SetCamRot(nCamera, vCamRot.X, vCamRot.Y, vCamRot.Z, 0);
            SetEntityRotation(nEntity, 0, vCamRot.Y, vCamRot.Z, 0, false);

            SetObjectPhysicsParams(nEntity, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            Vector3 vCamPos = GetEntityCoords(nEntity, false) + vCamOffsetPos;
            SetCamCoord(nCamera, vCamPos.X, vCamPos.Y, vCamPos.Z);
        }
    }
}