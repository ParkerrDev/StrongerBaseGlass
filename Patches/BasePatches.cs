using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace StrongerBaseGlass.Patches
{
    [HarmonyPatch(typeof(Base))]
    public static class BasePatches
    {
        static readonly FieldInfo FaceHullStrength = AccessTools.Field(typeof(Base), "faceHullStrengths");
        static readonly FieldInfo CellHullStrength = AccessTools.Field(typeof(Base), "cellHullStrengths");

        static bool patched = false;

        static readonly Dictionary<TechType, float> FaceHullStrReplacement = new Dictionary<TechType, float>()
        {
            { TechType.BaseWindow, 0 },
            { TechType.BaseObservatory, 0 },
        };

        static readonly Dictionary<Base.CellType, float> CellHullStrReplacement = new Dictionary<Base.CellType, float>()
        {
            { Base.CellType.Observatory, -1.25f },
        };        [HarmonyPatch("Awake"), HarmonyPostfix]
        public static void Awake()
        {
            if (patched)
            {
                return;
            }

            patched = true;

            Plugin.Logger.LogInfo("StrongerBaseGlass: Starting hull strength modifications...");

            // Face - with error handling
            if (FaceHullStrength != null)
            {
                var facesStr = FaceHullStrength.GetValue(null) as float[];
                var faces = Base.FaceToRecipe;
                var len = faces.Length;
                
                Plugin.Logger.LogInfo($"StrongerBaseGlass: Found {len} face types to check");
                
                for (int i = 0; i < len; i++)
                {
                    if (FaceHullStrReplacement.TryGetValue(faces[i], out var str))
                    {
                        var originalValue = facesStr[i];
                        facesStr[i] = str;
                        Plugin.Logger.LogInfo($"StrongerBaseGlass: Modified {faces[i]} hull strength: {originalValue} -> {str}");
                    }
                }
            }
            else
            {
                Plugin.Logger.LogWarning("StrongerBaseGlass: Could not find faceHullStrengths field!");
            }

            // Cell - with error handling
            if (CellHullStrength != null)
            {
                var cellsStr = CellHullStrength.GetValue(null) as float[];
                foreach (var cell in CellHullStrReplacement)
                {
                    var originalValue = cellsStr[(int)cell.Key];
                    cellsStr[(int)cell.Key] = cell.Value;
                    Plugin.Logger.LogInfo($"StrongerBaseGlass: Modified {cell.Key} cell hull strength: {originalValue} -> {cell.Value}");
                }
            }
            else
            {
                Plugin.Logger.LogWarning("StrongerBaseGlass: Could not find cellHullStrengths field!");
            }

            Plugin.Logger.LogInfo("StrongerBaseGlass: Hull strength modifications complete!");
        }

        [HarmonyPatch("GetHullStrength"), HarmonyPostfix]
        public static void GetHullStrengthPostfix(Base __instance, ref float __result)
        {
            Plugin.Logger.LogInfo($"StrongerBaseGlass: Base hull strength calculated: {__result}");
        }

    }
}
