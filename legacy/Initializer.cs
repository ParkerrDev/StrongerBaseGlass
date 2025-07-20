using StrongerBaseGlass.Utils;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongerBaseGlass
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Patch()
        {
            BaseInitializer.Init(nameof(StrongerBaseGlass), typeof(Initializer).Assembly);
            
            // Log to console/file
            ModUtils.LogInfo("StrongerBaseGlass mod loaded successfully");
            
            // Show on-screen message
            ModUtils.LogOnScreen("StrongerBaseGlass: Base glass hull penalties removed!");
            
            ModUtils.LogDebug("Loaded " + nameof(StrongerBaseGlass), false);
        }

    }
}
