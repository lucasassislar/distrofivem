using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroServer {
    public delegate bool DistroCommand(Player player, ref string strRequestParam);
}
