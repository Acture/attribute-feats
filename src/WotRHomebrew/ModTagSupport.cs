using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WotRHomebrew.Feats;

namespace WotRHomebrew
{
    /// <summary>
    /// ModTagEx labels modded content from a downloaded GUID database that does not list
    /// this mod. When ModTagEx is loaded, register our blueprint GUIDs in its lookup table
    /// so tooltips name WotR Homebrew instead of "Unknown mod". Uses reflection only.
    /// </summary>
    internal static class ModTagSupport
    {
        private const string ModName = "WotR Homebrew";

        public static void TryRegister()
        {
            try
            {
                var mainType = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => assembly.GetName().Name == "ModTagEx")
                    .Select(assembly => assembly.GetType("ModTagEx.Main"))
                    .FirstOrDefault(type => type != null);
                if (mainType?.GetField("GuidModDict", BindingFlags.Public | BindingFlags.Static)?.GetValue(null)
                    is not Dictionary<Guid, string> lookup)
                    return;

                var count = 0;
                foreach (var guid in OurGuids())
                {
                    lookup[guid] = ModName;
                    count++;
                }
                Main.Log?.Log($"WotR Homebrew: registered {count} blueprint IDs with ModTagEx.");
            }
            catch (Exception error)
            {
                Main.Log?.Log("WotR Homebrew: ModTagEx registration skipped - " + error.Message);
            }
        }

        /// <summary>Every GUID constant in Guids plus every registered leaf feat.</summary>
        internal static IEnumerable<Guid> OurGuids()
        {
            var constants = AllNestedTypes(typeof(Guids))
                .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.Static))
                .Where(field => field.IsLiteral && field.FieldType == typeof(string))
                .Select(field => (string)field.GetRawConstantValue());
            var feats = FeatSelection.BudgetedFeats().Select(entry => entry.feat.AssetGuid.ToString());
            foreach (var text in constants.Concat(feats).Distinct())
            {
                if (Guid.TryParse(text, out var guid)) yield return guid;
            }
        }

        private static IEnumerable<Type> AllNestedTypes(Type type)
            => new[] { type }.Concat(type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic).SelectMany(AllNestedTypes));
    }
}
