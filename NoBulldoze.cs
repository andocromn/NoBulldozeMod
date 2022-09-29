using ICities;
using ColossalFramework;
using ColossalFramework.UI;
using ColossalFramework.Plugins;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoBulldoze
{
    public class NoBulldoze : IUserMod {
        public string Name {
            get { return "NoBulldoze mod"; }
        }

        public string Description {
            get { return "NoBulldoze mod description"; }
        }
    }

    public class Loading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode) {
			GameObject go = new GameObject("Test Object");
			go.AddComponent<BulldozeBehave>();
		}
	}
    public class BulldozeBehave : MonoBehaviour {

		void Start(){
			DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "IT Works!!!  Now what?");
		}

		void Update(){
			//
		}
	}
    public class Monitor : ThreadingExtensionBase {
        private BuildingManager _buildingManager;
        FastList<ushort> abandonedBuildings;

        public override void OnAfterSimulationTick() {
            if (threadingManager.simulationTick % 1024 == 0 && !threadingManager.simulationPaused) {
				for (ushort buildingId = 1; buildingId < BuildingManager.instance.m_buildings.m_buffer.Length; buildingId++)
				{
					var building = BuildingManager.instance.m_buildings.m_buffer[buildingId];
					if (!building.m_flags.IsFlagSet(Building.Flags.Created)) continue;

					if (building != ?"?Abandoned?"?) continue;

					var buildingAi = building.Info.m_buildingAI;
					if (buildingAi == null) continue;

					buildingAi.SetHistorical(buildingId, ref BuildingManager.instance.m_buildings.m_buffer[buildingId], historical);
				}
            }
			base.OnAfterSimulationTick();
        }
    }
}