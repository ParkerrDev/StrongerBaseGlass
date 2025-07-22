using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace StrongerBaseGlass.Patches
{
    public static class BasePatches
    {
        static bool patched = false;

        static readonly Dictionary<TechType, float> FaceHullStrReplacement = new Dictionary<TechType, float>()
        {
            { TechType.BaseWindow, 0 },
            { TechType.BaseGlassDome, 0 },
            { TechType.BaseLargeGlassDome, 0 },
            { TechType.BaseHatch, 0 },
        };

        static readonly Dictionary<Base.CellType, float> CellHullStrReplacement = new Dictionary<Base.CellType, float>()
        {
            { Base.CellType.Room, 0 },
            { Base.CellType.Corridor, 0 },
            { Base.CellType.Observatory, 0 },
            { Base.CellType.LargeRoom, 0 },
            { Base.CellType.Moonpool, 0 },
            { Base.CellType.MapRoom, 0 },
        };

        /// <summary>
        /// Initialize hull strength modifications once when the game starts
        /// </summary>
        public static void Initialize()
        {
            if (patched)
            {
                return;
            }

            patched = true;

            Plugin.Logger.LogInfo("StrongerBaseGlass: Starting hull strength modifications...");

            // Debug: List all fields in Base class
            var allFields = typeof(Base).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            Plugin.Logger.LogInfo($"StrongerBaseGlass: Found {allFields.Length} static fields in Base class:");
            foreach (var field in allFields)
            {
                Plugin.Logger.LogInfo($"StrongerBaseGlass: Field: {field.Name} ({field.FieldType})");
            }

            // Try to find hull strength fields with different approaches
            var faceHullField = FindHullStrengthField("face");
            var cellHullField = FindHullStrengthField("cell");

            // Face modifications
            if (faceHullField != null)
            {
                Plugin.Logger.LogInfo($"StrongerBaseGlass: Found face hull field: {faceHullField.Name}");
                
                var facesStr = faceHullField.GetValue(null) as float[];
                if (facesStr != null)
                {
                    var faces = Base.FaceToRecipe;
                    if (faces != null)
                    {
                        var len = faces.Length;
                        Plugin.Logger.LogInfo($"StrongerBaseGlass: Found {len} face types to check");
                        
                        for (int i = 0; i < len && i < facesStr.Length; i++)
                        {
                            Plugin.Logger.LogInfo($"StrongerBaseGlass: Checking face {i}: {faces[i]} (current strength: {facesStr[i]})");
                            if (FaceHullStrReplacement.TryGetValue(faces[i], out var str))
                            {
                                var originalValue = facesStr[i];
                                facesStr[i] = str;
                                Plugin.Logger.LogInfo($"StrongerBaseGlass: Modified {faces[i]} hull strength: {originalValue} -> {str}");
                            }
                        }
                        
                        // Log any TechTypes we wanted to modify but didn't find
                        foreach (var kvp in FaceHullStrReplacement)
                        {
                            bool found = false;
                            for (int i = 0; i < faces.Length; i++)
                            {
                                if (faces[i] == kvp.Key)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                Plugin.Logger.LogWarning($"StrongerBaseGlass: TechType {kvp.Key} not found in FaceToRecipe array");
                            }
                        }
                    }
                    else
                    {
                        Plugin.Logger.LogWarning("StrongerBaseGlass: Base.FaceToRecipe is null!");
                    }
                }
                else
                {
                    Plugin.Logger.LogWarning($"StrongerBaseGlass: {faceHullField.Name} is not a float array!");
                }
            }
            else
            {
                Plugin.Logger.LogWarning("StrongerBaseGlass: Could not find face hull strength field!");
            }

            // Cell modifications
            if (cellHullField != null)
            {
                Plugin.Logger.LogInfo($"StrongerBaseGlass: Found cell hull field: {cellHullField.Name}");
                
                var cellsStr = cellHullField.GetValue(null) as float[];
                if (cellsStr != null)
                {
                    foreach (var cell in CellHullStrReplacement)
                    {
                        int index = (int)cell.Key;
                        if (index >= 0 && index < cellsStr.Length)
                        {
                            var originalValue = cellsStr[index];
                            cellsStr[index] = cell.Value;
                            Plugin.Logger.LogInfo($"StrongerBaseGlass: Modified {cell.Key} cell hull strength: {originalValue} -> {cell.Value}");
                        }
                        else
                        {
                            Plugin.Logger.LogWarning($"StrongerBaseGlass: Index {index} out of bounds for cell array (length: {cellsStr.Length})");
                        }
                    }
                }
                else
                {
                    Plugin.Logger.LogWarning($"StrongerBaseGlass: {cellHullField.Name} is not a float array!");
                }
            }
            else
            {
                Plugin.Logger.LogWarning("StrongerBaseGlass: Could not find cell hull strength field!");
            }

            // Check for moonpool-specific hull strength fields
            var moonpoolFields = allFields.Where(f => 
                f.FieldType == typeof(float[]) && 
                f.Name.ToLower().Contains("moonpool")).ToArray();

            if (moonpoolFields.Length > 0)
            {
                Plugin.Logger.LogInfo($"StrongerBaseGlass: Found {moonpoolFields.Length} moonpool-specific float arrays:");
                foreach (var field in moonpoolFields)
                {
                    Plugin.Logger.LogInfo($"StrongerBaseGlass: Moonpool field: {field.Name}");
                    var values = field.GetValue(null) as float[];
                    if (values != null)
                    {
                        Plugin.Logger.LogInfo($"StrongerBaseGlass: {field.Name} values: [{string.Join(", ", values)}]");
                        
                        // Set all moonpool-related hull strength values to 0
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] < 0) // Only modify negative values (penalties)
                            {
                                var originalValue = values[i];
                                values[i] = 0;
                                Plugin.Logger.LogInfo($"StrongerBaseGlass: Modified {field.Name}[{i}]: {originalValue} -> 0");
                            }
                        }
                    }
                }
            }
            else
            {
                Plugin.Logger.LogInfo("StrongerBaseGlass: No moonpool-specific float arrays found");
            }

            Plugin.Logger.LogInfo("StrongerBaseGlass: Hull strength modifications complete!");
        }

        private static FieldInfo FindHullStrengthField(string type)
        {
            var allFields = typeof(Base).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            
            // Look for fields containing hull and the type
            var candidates = allFields.Where(f => 
                f.FieldType == typeof(float[]) && 
                f.Name.ToLower().Contains("hull") && 
                f.Name.ToLower().Contains(type)).ToArray();

            if (candidates.Length > 0)
            {
                return candidates[0];
            }

            // Fallback: look for any float array with hull in the name
            candidates = allFields.Where(f => 
                f.FieldType == typeof(float[]) && 
                f.Name.ToLower().Contains("hull")).ToArray();

            if (candidates.Length > 0)
            {
                Plugin.Logger.LogInfo($"StrongerBaseGlass: Found hull-related float array: {candidates[0].Name}");
                return candidates[0];
            }

            return null;
        }

    }
}
