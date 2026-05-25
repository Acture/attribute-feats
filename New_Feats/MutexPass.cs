using System;
using System.Collections.Generic;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;

namespace AttributeFeats.New_Feats
{
    internal static class MutexPass
    {
        private static bool Applied;

        public static void ApplyAll()
        {
            if (Applied) return;
            Applied = true;

            ApplyAttributeGroup("Strength",
                Guids.str_main_to_everything,
                Guids.Specialized.Defensive.Str,
                Guids.Specialized.Maneuver.Str,
                Guids.Specialized.Skilled.Str,
                Guids.Specialized.Arcane.Str,
                Guids.Stance.Feature.Str,
                Guids.Replacement.WeaponInsight.Str);

            ApplyAttributeGroup("Dexterity",
                Guids.dex_main_to_everything,
                Guids.Specialized.Defensive.Dex,
                Guids.Specialized.Maneuver.Dex,
                Guids.Specialized.Skilled.Dex,
                Guids.Specialized.Arcane.Dex,
                Guids.Stance.Feature.Dex,
                Guids.Replacement.WeaponInsight.Dex,
                Guids.Conditional.FirstBlood,
                Guids.SpellTag.Descriptor.StormChannel);

            ApplyAttributeGroup("Constitution",
                Guids.con_main_to_everything,
                Guids.Specialized.Defensive.Con,
                Guids.Specialized.Maneuver.Con,
                Guids.Specialized.Skilled.Con,
                Guids.Specialized.Arcane.Con,
                Guids.Stance.Feature.Con,
                Guids.Replacement.WeaponInsight.Con,
                Guids.Conditional.EndlessResolve);

            ApplyAttributeGroup("Intelligence",
                Guids.int_main_to_everything,
                Guids.Specialized.Defensive.Int,
                Guids.Specialized.Maneuver.Int,
                Guids.Specialized.Skilled.Int,
                Guids.Specialized.Arcane.Int,
                Guids.Stance.Feature.Int,
                Guids.Replacement.WeaponInsight.Int,
                Guids.Replacement.Extended.CalculatedGrip,
                Guids.SpellTag.School.SeersEdge,
                Guids.SpellTag.School.Spellforge,
                Guids.SpellTag.School.ShapeShifter,
                Guids.SpellTag.Descriptor.EtchingMind);

            ApplyAttributeGroup("Wisdom",
                Guids.wis_main_to_everything,
                Guids.Specialized.Defensive.Wis,
                Guids.Specialized.Maneuver.Wis,
                Guids.Specialized.Skilled.Wis,
                Guids.Specialized.Arcane.Wis,
                Guids.Stance.Feature.Wis,
                Guids.Replacement.WeaponInsight.Wis,
                Guids.Conditional.PatientHunter,
                Guids.Replacement.Extended.InnerSentinel,
                Guids.SpellTag.School.PureWarder,
                Guids.SpellTag.School.DeathSpeaker,
                Guids.SpellTag.Descriptor.FrozenHeart);

            ApplyAttributeGroup("Charisma",
                Guids.cha_main_to_everything,
                Guids.Specialized.Defensive.Cha,
                Guids.Specialized.Maneuver.Cha,
                Guids.Specialized.Skilled.Cha,
                Guids.Specialized.Arcane.Cha,
                Guids.Stance.Feature.Cha,
                Guids.Replacement.WeaponInsight.Cha,
                Guids.Conditional.Vendetta,
                Guids.Replacement.Extended.UnyieldingWill,
                Guids.SpellTag.School.MasterCaller,
                Guids.SpellTag.School.HeartsTyrant,
                Guids.SpellTag.School.Veilweaver,
                Guids.SpellTag.Descriptor.InnerFlame,
                Guids.SpellTag.Descriptor.ResonantVoice);
        }

        private static void ApplyAttributeGroup(string attributeName, params string[] guids)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var feats = new List<BlueprintFeature>();

            foreach (var guid in guids)
            {
                if (string.IsNullOrWhiteSpace(guid) || !seen.Add(guid))
                    continue;

                try
                {
                    feats.Add(BlueprintTool.Get<BlueprintFeature>(guid));
                }
                catch (Exception e)
                {
                    Main.Log?.Log($"AttributeFeats: MutexPass could not resolve {attributeName} feat {guid}: {e.Message}");
                }
            }

            for (var i = 0; i < feats.Count; i++)
            {
                for (var j = i + 1; j < feats.Count; j++)
                {
                    Common.AddBidirectionalMutex(feats[i], feats[j]);
                }
            }

            Main.Log?.Log($"AttributeFeats: MutexPass applied {attributeName} mutexes across {feats.Count} feats.");
        }
    }
}
