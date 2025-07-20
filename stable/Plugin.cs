using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace StrongerBaseGlass
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.hydronautica.strongerbaseglass";
        public const string PLUGIN_NAME = "StrongerBaseGlass";
        public const string PLUGIN_VERSION = "1.0.0";
        
        public new static ManualLogSource Logger { get; private set; }
        private static Harmony harmony;

        private void Awake()
        {
            Logger = base.Logger;
            
            // Initialize Harmony
            harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
            
            Logger.LogInfo($"Plugin {PLUGIN_NAME} is loaded!");
        }

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }
    }
}
