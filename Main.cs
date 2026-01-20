using System;
using System.Reflection;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;   // ✅ 关键：BlueprintsCache 在这里
using UnityModManagerNet;
using AttributeFeats.New_Feats;          // ✅ 你的 ConfigureAll 在这里

namespace AttributeFeats
{
	public static class Main
	{
		internal static Harmony HarmonyInstance;
		internal static UnityModManager.ModEntry.ModLogger Log;
		internal static UnityModManager.ModEntry Entry;

		public static bool Enabled = true;   // 默认启用；UMM toggle 只是给你个开关

		public static bool Load(UnityModManager.ModEntry modEntry)
		{
			Entry = modEntry;
			Log = modEntry.Logger;

			modEntry.OnToggle = OnToggle;

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
