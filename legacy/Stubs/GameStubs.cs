// Stub classes for Subnautica game types - these are just for compilation
// At runtime, the actual Subnautica assemblies will be used

using System;

// Base game types
public enum TechType
{
    None = 0,
    BaseWindow = 1,
    BaseGlassDome = 2,
    BaseLargeGlassDome = 3,
    BaseObservatory = 4,
    BaseCorridorI = 5,
    BaseCorridorL = 6,
    BaseRoom = 7,
    BaseCorridorGlassI = 8,
    BaseCorridorGlassL = 9
}

public class Base
{
    public enum CellType
    {
        Empty = 0,
        Room = 1,
        Corridor = 2,
        Observatory = 3
    }

    public static TechType[] FaceToRecipe = new TechType[0];
    public static float[] FaceHullStrength = new float[0];
    public static float[] CellHullStrength = new float[0];

    public void Awake() { }
    public float GetHullStrength(int face) { return 0; }
}

// Unity stubs
namespace UnityEngine
{
    public class MonoBehaviour
    {
        public void Awake() { }
    }
}
