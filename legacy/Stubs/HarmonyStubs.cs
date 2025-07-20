// Stub classes for HarmonyLib - these are just for compilation
// At runtime, the actual HarmonyX/HarmonyLib assemblies will be used

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
    // Harmony main class
    public class Harmony
    {
        public Harmony(string id) { }
        public void PatchAll(Assembly assembly) { }
    }

    // Harmony attributes
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class HarmonyPatchAttribute : Attribute
    {
        public HarmonyPatchAttribute(Type declaringType) { }
        public HarmonyPatchAttribute(string methodName) { }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HarmonyPostfixAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HarmonyTranspilerAttribute : Attribute
    {
    }

    // AccessTools for reflection
    public static class AccessTools
    {
        public static FieldInfo Field(Type type, string name) { return null; }
    }

    // CodeInstruction for transpilers
    public class CodeInstruction
    {
        public OpCode opcode;
        public object operand;

        public CodeInstruction(OpCode opcode) { this.opcode = opcode; }
        public CodeInstruction(OpCode opcode, object operand) { this.opcode = opcode; this.operand = operand; }
    }
}
