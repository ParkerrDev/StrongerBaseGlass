// Stub classes for QModManager - these are just for compilation
// At runtime, the actual QModManager assemblies will be used

using System;

namespace QModManager.API.ModLoading
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QModCoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class QModPatchAttribute : Attribute
    {
    }
}

namespace QModManager.Utility
{
    public static class Logger
    {
        public enum Level
        {
            Debug,
            Info,
            Warn,
            Error
        }

        public static void Log(Level level, string message, Exception exception = null, bool show = true)
        {
            // Stub implementation - actual logging will be done by QModManager at runtime
            Console.WriteLine($"[{level}] {message}");
        }
    }
}
