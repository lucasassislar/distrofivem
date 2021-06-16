using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroServer.Model {
    public class ServerStatus {
        public ObjectId id { get; set; }

        public int version { get; set; }
    }
}
