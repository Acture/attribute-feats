using System;
using System.Reflection;
using AttributeFeats.New_Feats;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using UnityEngine;
using UnityModManagerNet;

namespace AttributeFeats
{
    public static class Main
    {
        internal static Harmony HarmonyInstance;
        internal static UnityModManager.ModEntry.ModLogger Log;
        internal static UnityModManager.ModEntry Entry;
        internal static ModSettings Settings;

        public static bool Enabled = true;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Entry = modEntry;
            Log = modEntry.Logger;
            Settings = UnityModManager.ModSettings.Load<ModSettings>(modEntry);

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            HarmonyInstance = new Harmony(modEntry.Info.Id);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            Log.Log("AttributeFeats loaded.");
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            Log.Log(value ? "AttributeFeats enabled." : "AttributeFeats disabled (restart required to fully unload).");
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var s = Settings;
            var changed = false;

            GUILayout.Label("<color=cyan><b>[ Global Scaling Configuration ] — Restart Required</b></color>");

            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Power Level: <color=yellow>{s.powerLevel}</color>", GUILayout.Width(260));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Cycle Power Level", GUILayout.Width(140)))
            {
                s.powerLevel = s.powerLevel == PowerLevel.Balanced ? PowerLevel.Legacy_AllFull : PowerLevel.Balanced;
                changed = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.Label($"<color=grey><size=11>{GetPowerLevelDescription(s.powerLevel)}</size></color>");
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("<color=green><b>[ Build Enablers ]</b></color>");
            GUILayout.BeginVertical("box");
            GUILayout.Label("<color=green><size=11>These defaults keep Main feats focused on build-enabling stats rather than raw power.</size></color>");
            changed |= ToggleSetting(ref s.EnableAttributes, "Enable Attributes — adds to the six ability scores (default: ON)");
            changed |= ToggleSetting(ref s.EnableDefenses, "Enable Defenses — AC, CMD, saves, and initiative (default: ON)");
            changed |= ToggleSetting(ref s.EnableManeuvers, "Enable Maneuvers — Combat Maneuver Bonus only (default: ON)");
            changed |= ToggleSetting(ref s.EnableChecks, "Enable Checks — Bluff, Diplomacy, and Intimidate checks (default: ON)");
            changed |= ToggleSetting(ref s.EnableSkills, "Enable Skills — all skill bonuses (default: ON)");
            changed |= ToggleSetting(ref s.EnableCasterDC, "Enable Caster DC — spell save DC scaling (default: ON)");
            changed |= ToggleSetting(ref s.EnableCasterLevel, "Enable Caster Level — caster level scaling (default: ON)");
            changed |= ToggleSetting(ref s.EnableSpellPenetration, "Enable Spell Penetration — bonus vs. spell resistance (default: ON)");
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("<color=orange><b>[ Stacking Options ]</b></color>");
            GUILayout.BeginVertical("box");
            changed |= ToggleSetting(ref s.IncludeSelfInAttributeStack, "Include Self in Attribute Stack — a Main feat may add its chosen attribute to itself (default: OFF)");
            GUILayout.Label("<color=grey><size=11>Leave this off for the redesign baseline. Turning it on restores recursive self-stacking behavior.</size></color>");
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("<color=red><b>[ Power Mode ]</b></color>");
            GUILayout.BeginVertical("box");
            GUILayout.Label("<color=red><size=11>Warning: these settings materially increase combat power and are intentionally off by default.</size></color>");
            changed |= ToggleSetting(ref s.EnableBAB, "Enable BAB — adds reduced scaling to Base Attack Bonus (default: OFF)");
            changed |= ToggleSetting(ref s.EnablePowerMode, "Enable Power Mode — enables attack bonus, damage, AoOs, sneak attack, HP, speed, and fixed +1 Reach (default: OFF)");
            GUILayout.EndVertical();

            if (changed)
            {
                Settings.Save(modEntry);
            }
        }

        private static bool ToggleSetting(ref bool value, string label)
        {
            var newValue = GUILayout.Toggle(value, label);
            if (newValue == value) return false;
            value = newValue;
            return true;
        }

        private static string GetPowerLevelDescription(PowerLevel level) => level switch
        {
            PowerLevel.Balanced => "Balanced: full scaling for attributes, defenses, maneuvers, skills, caster level, and spell penetration; reduced scaling for spell DC, BAB, and Power Mode bonuses.",
            PowerLevel.Legacy_AllFull => "Legacy_AllFull: every rank-based Main feat bonus uses full modifier scaling, matching the old all-full behavior.",
            _ => level.ToString(),
        };

        [HarmonyPatch(typeof(BlueprintsCache))]
        private static class BlueprintsCache_Patch
        {
            private static bool Initialized;

            [HarmonyPriority(Priority.Last)]
            [HarmonyPatch(nameof(BlueprintsCache.Init))]
            [HarmonyPostfix]
            private static void Init_Postfix()
            {
                try
                {
                    if (Initialized) return;
                    Initialized = true;

                    if (!Enabled)
                    {
                        Log.Log("AttributeFeats: skipped (disabled).");
                        return;
                    }

                    Log.Log("AttributeFeats: patching blueprints...");
                    FeatRegistry.ConfigureAll();
                    Log.Log("AttributeFeats: done.");
                }
                catch (Exception e)
                {
                    Log.Log("AttributeFeats: failed to initialize.\n" + e);
                }
            }
        }
    }
}
