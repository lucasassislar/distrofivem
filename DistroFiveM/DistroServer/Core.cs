using System;
using System.Collections.Generic;
using CitizenFX.Core;
using DistroServer.Managers;
using DistroServer.Model;
using static CitizenFX.Core.Native.API;

namespace DistroServer {
    public class Core : BaseScript {
        public static Core Singleton { get; private set; }

        public DatabaseManager Database { get; private set; }

        public Core() {
            Singleton = this;

            Database = new DatabaseManager();

            EventHandlers["onServerResourceStart"] += new Action<string>(OnServerResourceStart);
            EventHandlers["playerJoining"] += new Action<Player>(OnPlayerJoining);
        }

        private void OnServerResourceStart(string resourceName) {
            if (GetCurrentResourceName() != resourceName) {
                return;
            }

            Database.Initialize();
        }

        private void OnPlayerJoining([FromSource] Player player) {
            UpdateInventory(player);
        }

        public void UpdateInventory(Player player) {
            User user = Database.GetUser(player);
            player.TriggerEvent("ClearInventory");

            for (int i = 0; i < user.inventory.Length; i++) {
                Item item = Database.GetItem(user.inventory[i]);
                player.TriggerEvent("UpdateInventory", item.type, i);
            }
        }
    }
}