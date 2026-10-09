using UnityModManagerNet;

namespace WotRHomebrew
{
    /// <summary>Runtime state set by the UMM entry point and shared by all feature assemblies.</summary>
    public static class Mod
    {
        public static UnityModManager.ModEntry Entry;
        public static UnityModManager.ModEntry.ModLogger Log;
        public static ModSettings Settings;
    }
}
