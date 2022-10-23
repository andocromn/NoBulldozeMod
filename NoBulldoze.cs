using ICities;
using ColossalFramework;
using ColossalFramework.UI;
using ColossalFramework.Plugins;
using UnityEngine;
using System;

namespace NoBulldoze
{
    public class NoBulldoze : IUserMod
    {
        public string Name
        {
            get
            {
                return "NoBulldoze mod";
            }
        }

        public string Description
        {
            get
            {
                return "NoBulldoze mod description";
            }
        }
    }

    public class Loading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            var go = new GameObject("Test Object");
            go.AddComponent<BulldozeBehave>();
        }
    }

    public class BulldozeBehave : MonoBehaviour
    {

        void Start()
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "IT Works!!!  Now what?");
        }
        void Update()
        {

        }
    }

    public class DestroyMonitor : ThreadingExtensionBase
    {
        private readonly BuildingManager _buildingManager;
        private readonly SimulationManager _simulationManager;

        public DestroyMonitor()
        {
            this._buildingManager = Singleton<BuildingManager>.instance;
            this._simulationManager = Singleton<SimulationManager>.instance;
        }

        public override void OnAfterSimulationTick()
        {
            try
            {
                for (var i = (ushort)(_simulationManager.m_currentTickIndex % 1000); i < _buildingManager.m_buildings.m_buffer.Length; i += 1000)
                {
                    if (_buildingManager.m_buildings.m_buffer[i].m_flags == Building.Flags.None)
                        continue;

                    if ((_buildingManager.m_buildings.m_buffer[i].m_flags & Building.Flags.Abandoned) != Building.Flags.None)
                    {
                        var building = BuildingManager.instance.m_buildings.m_buffer[i];
                        if (!building.m_flags.IsFlagSet(Building.Flags.Created))
                            continue;

                        var buildingAi = building.Info.m_buildingAI;
                        if (buildingAi == null)
                            continue;

                        buildingAi.SetHistorical(i, ref BuildingManager.instance.m_buildings.m_buffer[i], true);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("ex: {0} Stack Trace: {1}", ex.Message, ex.StackTrace);
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, errorMessage);
            }
        }
    }
}