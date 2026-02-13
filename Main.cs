using System;
using System.Reflection;
using AttributeFeats.New_Feats;          
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;  
using Kingmaker.UnitLogic.Mechanics.Components;
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

		static void OnGUI(UnityModManager.ModEntry modEntry)
		{
			var s = Settings;
			GUILayout.Label("<color=cyan><b>[ Global Scaling Configuration ] - Restart Required</b></color>");

			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			GUILayout.Label($"Global Progression Mode: <color=yellow>{s.progression}</color>", GUILayout.Width(300));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Cycle Ratio", GUILayout.Width(120)))
			{
				s.progression = s.progression switch
				{
					ContextRankProgression.AsIs => ContextRankProgression.Div2,
					ContextRankProgression.Div2 => ContextRankProgression.HalfMore,
					_ => ContextRankProgression.AsIs
				};
			}
			GUILayout.EndHorizontal();
			GUILayout.Label($"<color=grey><size=11>Current Effect: {GetDescription(s.progression)}</size></color>");
			GUILayout.EndVertical();

			GUILayout.Space(10);

			GUILayout.Label("<color=cyan><b>[ Feature Toggles ]</b></color>");
			GUILayout.BeginVertical("box");
			s.EnableAttributes = GUILayout.Toggle(s.EnableAttributes, " Ability Score Stacking (e.g., Con added to Str)");
			s.EnableBattles = GUILayout.Toggle(s.EnableBattles, " Combat Stats (AC, BAB, Initiative, HP)");
			s.EnableSavings = GUILayout.Toggle(s.EnableSavings, " Saving Throws (Fortitude, Reflex, Will)");
			s.EnableChecks = GUILayout.Toggle(s.EnableChecks, " Attribute Checks (Bluff, Diplomacy, etc.)");
			s.EnableSkills = GUILayout.Toggle(s.EnableSkills, " Skill Proficiencies (Athletics, Perception, etc.)");
			s.EnableCaster = GUILayout.Toggle(s.EnableCaster, " Caster Progression (DC, CL, Echo Spell, All Spell Lists)");
			GUILayout.EndVertical();

			if (GUI.changed) { Settings.Save(modEntry); }
		}

		private static string GetDescription(ContextRankProgression p) => p switch
		{
			ContextRankProgression.AsIs => "100% (1:1 Ratio)",
			ContextRankProgression.Div2 => "50% (2:1 Ratio - Balanced)",
			ContextRankProgression.HalfMore => "150% (1:1.5 Ratio - Overpowered)",
			_ => p.ToString()
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
					MainAbilityToEverything_Feats.ConfigureAll();
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
