using CitizenFX.Core;
using DistroServer.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroServer.Managers {
    public class DatabaseManager {
        public IMongoCollection<ServerStatus> DbStatus { get; private set; }
        public IMongoCollection<User> DbUser { get; private set; }
        public IMongoCollection<Item> DbItem { get; private set; }

        private IMongoDatabase db;

        public DatabaseManager() {

        }

        public bool HasPermission(Player player, string role) {
            User user = GetUser(player);
            return user.role == role;
        }

        public User GetUser(Player player) {
            string[] identifiers = player.Identifiers.ToList().ToArray();
            string fiveM = GetFiveM(identifiers);

            var find = DbUser.Find(_ => _.fiveM == fiveM);
            return find.FirstOrDefault();
        }

        private string GetFiveM(string[] identifiers) {
            string fiveM = null;
            for (int i = 0; i < identifiers.Length; i++) {
                string strIdentifier = identifiers[i];
                if (strIdentifier.StartsWith("fivem:")) {
                    string[] arrIdentifierSplit = strIdentifier.Split(':');
                    fiveM = arrIdentifierSplit[1];
                }
            }
            return fiveM;
        }

        public void SavePlayer(Player player) {
            string[] identifiers = player.Identifiers.ToList().ToArray();
            string fiveM = GetFiveM(identifiers);

            var find = DbUser.Find(_ => _.fiveM == fiveM);

            User user;
            if (find.CountDocuments() == 0) {
                user = new User();
                user.identifiers = identifiers;
                user.fiveM = fiveM;
                user.role = UserRole.Standard;
                user.name = player.Name.ToLower();
                
                Item item = DbItem.Find(c => c.type == "drone").FirstOrDefault();
                if (item == null) {
                    item = new Item();
                    item.description = "Drone V200";
                    item.type = "drone";
                    DbItem.InsertOne(item);

                    item = DbItem.Find(c => c.type == "drone").FirstOrDefault();
                }
                user.inventory = new ObjectId[] { item.id };

                DbUser.InsertOne(user);
            }
        }

        public bool PromoteToAdmin(string strName) {
            var find = DbUser.Find(_ => _.name == strName);

            User user = find.FirstOrDefault();
            if (user == null) {
                return false;
            }

            DbUser.UpdateOne(_ => _.name == strName, Builders<User>.Update.Set("role", UserRole.Admin));
            return true;
        }

        public bool DemoteToStandard(string strName) {
            var find = DbUser.Find(_ => _.name == strName);

            User user = find.FirstOrDefault();
            if (user == null) {
                return false;
            }

            DbUser.UpdateOne(_ => _.name == strName, Builders<User>.Update.Set("role", UserRole.Standard));
            return true;
        }

        public void UpdateInventory(User user) {
            Item item = DbItem.Find(c => c.type == "drone").FirstOrDefault();
            DbUser.UpdateOne(c => c.id == user.id, Builders<User>.Update.Set("inventory", new ObjectId[] { item.id }));
        }

        public int IncreaseVersion() {
            var find = DbStatus.Find(_ => true);
            ServerStatus status;
            if (find.CountDocuments() == 0) {
                status = new ServerStatus();
                status.version = 1;

                DbStatus.InsertOne(status);
            } else {
                status = find.FirstOrDefault();
                DbStatus.UpdateOne(_ => true, Builders<ServerStatus>.Update.Set("version", status.version + 1));
            }
            Globals.Version = status.version;

            return status.version;
        }

        public void Initialize() {
            MongoUrl url = new MongoUrl("mongodb://localhost:27017/fivem");
            MongoClient client = new MongoClient(url);
            db = client.GetDatabase("fivem");

            DbStatus = db.GetCollection<ServerStatus>("server_status");
            DbUser = db.GetCollection<User>("users");
            DbItem = db.GetCollection<Item>("items");

            //IncreaseVersion();
        }
    }
}
