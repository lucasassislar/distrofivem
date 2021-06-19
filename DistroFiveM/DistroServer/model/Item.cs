using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroServer.Model {
    public class Item {
        public ObjectId id { get; set; }
        
        public string type { get; set; }

        public string description { get; set; }

        public object data { get; set; }
    }
}
