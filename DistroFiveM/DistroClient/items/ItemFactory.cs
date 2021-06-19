using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroClient.Items {
    public static class ItemFactory {
        public static BaseItem Create(string strName) {
            switch (strName) {
                case "drone":
                    return new DroneItem();
                default:
                    return null;
            }
        }
    }
}
