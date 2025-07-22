using BepInEx;
using BepInEx.Logging;
using StrongerBaseGlass.Patches;

namespace StrongerBaseGlass
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.hydronautica.strongerbaseglass";
        public const string PLUGIN_NAME = "StrongerBaseGlass";
        public const string PLUGIN_VERSION = "1.0.1";
        
        public new static ManualLogSource Logger { get; private set; }

        private void Awake()
        {
            Logger = base.Logger;
            
            // Initialize hull strength modifications first
            BasePatches.Initialize();
            
            Logger.LogInfo($"Plugin {PLUGIN_NAME} is loaded!");
        }
    }
}
