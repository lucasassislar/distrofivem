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
        }

        private void OnServerResourceStart(string resourceName) {
            if (GetCurrentResourceName() != resourceName) {
                return;
            }

            Database.Initialize();
        }
    }
}