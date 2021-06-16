using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroServer.Model {
    public class User {
        public ObjectId id { get; set; }

        public string[] identifiers { get; set; }

        public string fiveM { get; set; }

        public string role { get; set; }
    }
}
