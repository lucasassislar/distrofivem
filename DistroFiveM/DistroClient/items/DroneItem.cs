using CitizenFX.Core;
using DistroClient.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace DistroClient.Items {
    public class DroneItem : ControllableItem {
        private int nCamera;

        public DroneItem() {
            Vector3 vPos = GetEntityCoords(PlayerPedId(), false);
            CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", vPos.X, vPos.Y, vPos.Z, 0, 0, 0, fov * 1.0f, false, 2);
        }

        public override void OnControl() {
            base.OnControl();


        }
    }
}
