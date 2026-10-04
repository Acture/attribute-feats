using System;
using System.Collections.Generic;
using BlueprintCore.Blueprints.Components.Replacements;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts.Restrictions;
using Kingmaker.EntitySystem.Properties;
using Kingmaker.EntitySystem.Properties.BaseGetter;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace AttributeFeats.New_Feats
{
    internal static class StatReplacementFeats
    {
        private static readonly WeaponSubCategory[] WeaponInsightCategories =
        {
            WeaponSubCategory.Melee,
            WeaponSubCategory.Ranged,
            WeaponSubCategory.Thrown,
            WeaponSubCategory.Natural,
        };

        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var weaponInsights = new List<BlueprintFeature>
            {
                CreateWeaponInsight(
                    StatType.Strength,
                    "CrushingForm",
                    Guids.Replacement.WeaponInsight.Str,
                    "WeaponInsight_Str.Name",
                    "Crushing Form",
                    "WeaponInsight_Str.Desc",
                    BuildWeaponInsightDescription(
                        "Strength",
                        "Force Finds the Gap.",
                        "You trust brute force over delicate aim, letting sheer might drive every weapon into the place it needs to land.")),
                CreateWeaponInsight(
                    StatType.Dexterity,
                    "DuelistsEye",
                    Guids.Replacement.WeaponInsight.Dex,
                    "WeaponInsight_Dex.Name",
                    "Duelist's Eye",
                    "WeaponInsight_Dex.Desc",
                    BuildWeaponInsightDescription(
                        "Dexterity",
                        "Perfect Line.",
                        "Grace and precision carry your weapon through the smallest openings, until each strike feels measured to the width of a breath.")),
                CreateWeaponInsight(
                    StatType.Constitution,
                    "IronStance",
                    Guids.Replacement.WeaponInsight.Con,
                    "WeaponInsight_Con.Name",
                    "Iron Stance",
                    "WeaponInsight_Con.Desc",
                    BuildWeaponInsightDescription(
                        "Constitution",
                        "Stand Through the Blow.",
                        "You anchor every attack in endurance, letting balance, pain tolerance, and relentless stability keep your weapon true.")),
                CreateWeaponInsight(
                    StatType.Intelligence,
                    "TacticalStrike",
                    Guids.Replacement.WeaponInsight.Int,
                    "WeaponInsight_Int.Name",
                    "Tactical Strike",
                    "WeaponInsight_Int.Desc",
                    BuildWeaponInsightDescription(
                        "Intelligence",
                        "Measure, Then Cut.",
                        "Angles, tempo, and reach resolve into a visible pattern, and every attack follows the line your analysis has already solved.")),
                CreateWeaponInsight(
                    StatType.Wisdom,
                    "PredictiveCut",
                    Guids.Replacement.WeaponInsight.Wis,
                    "WeaponInsight_Wis.Name",
                    "Predictive Cut",
                    "WeaponInsight_Wis.Desc",
                    BuildWeaponInsightDescription(
                        "Wisdom",
                        "See the Opening.",
                        "Patient awareness lets you read a fight before it fully forms, turning instinct into perfect timing.")),
                CreateWeaponInsight(
                    StatType.Charisma,
                    "TheatricalCombat",
                    Guids.Replacement.WeaponInsight.Cha,
                    "WeaponInsight_Cha.Name",
                    "Theatrical Combat",
                    "WeaponInsight_Cha.Desc",
                    BuildWeaponInsightDescription(
                        "Charisma",
                        "Own the Exchange.",
                        "Every clash becomes a display of presence and daring, and your confidence pulls the enemy exactly where your weapon wants them.")),
            };

            for (var i = 0; i < weaponInsights.Count; i++)
            {
                for (var j = i + 1; j < weaponInsights.Count; j++)
                {
                    Common.AddBidirectionalMutex(weaponInsights[i], weaponInsights[j]);
                }
            }

            CreateExtendedFeat(
                StatType.Wisdom,
                StatType.AC,
                "InnerSentinel",
                Guids.Replacement.Extended.InnerSentinel,
                "Extended_InnerSentinel.Name",
                "Inner Sentinel",
                "Extended_InnerSentinel.Desc",
                BuildExtendedDescription(
                    "Wisdom",
                    "Still at the Center.",
                    "Calm perception settles over your stance like a second skin, and the danger you understand is the danger that fails to touch you.",
                    "Adds your Wisdom modifier as an untyped bonus to AC while wearing no armor or light armor.",
                    "Applies only while wearing no armor or light armor. This feat can be combined with other Extended Replacement feats."),
                CreateLightOrNoArmorRestriction());

            CreateExtendedFeat(
                StatType.Intelligence,
                StatType.AdditionalCMB,
                "CalculatedGrip",
                Guids.Replacement.Extended.CalculatedGrip,
                "Extended_CalculatedGrip.Name",
                "Calculated Grip",
                "Extended_CalculatedGrip.Desc",
                BuildExtendedDescription(
                    "Intelligence",
                    "Leverage by Design.",
                    "Every hold begins as geometry in your mind, and your understanding of balance turns technique into control.",
                    "Adds your Intelligence modifier as an untyped bonus to CMB.",
                    "This feat can be combined with other Extended Replacement feats."));

            CreateExtendedFeat(
                StatType.Charisma,
                StatType.AdditionalCMD,
                "UnyieldingWill",
                Guids.Replacement.Extended.UnyieldingWill,
                "Extended_UnyieldingWill.Name",
                "Unyielding Will",
                "Extended_UnyieldingWill.Desc",
                BuildExtendedDescription(
                    "Charisma",
                    "Refusal Made Steel.",
                    "The force of your presence hardens into defiance, making every attempt to move or break you feel like a personal insult.",
                    "Adds your Charisma modifier as an untyped bonus to CMD.",
                    "This feat can be combined with other Extended Replacement feats."));

            CreateExtendedFeat(
                StatType.Strength,
                StatType.AdditionalCMD,
                "BrutalDefender",
                Guids.ExtendedReplacement2.BrutalDefender,
                "Extended_BrutalDefender.Name",
                "Brutal Defender",
                "Extended_BrutalDefender.Desc",
                BuildExtendedDescription(
                    "Strength",
                    "Hold the Line by Force.",
                    "Raw power sets your footing and dares the enemy to move you, turning every contest of balance into a test of whose body yields first.",
                    "Adds your Strength modifier as an untyped bonus to CMD.",
                    "This feat can be combined with other Extended Replacement feats."));

            CreateExtendedFeat(
                StatType.Dexterity,
                StatType.AC,
                "LightfootDefense",
                Guids.ExtendedReplacement2.LightfootDefense,
                "Extended_LightfootDefense.Name",
                "Lightfoot Defense",
                "Extended_LightfootDefense.Desc",
                BuildExtendedDescription(
                    "Dexterity",
                    "Untouched in Motion.",
                    "Footwork keeps danger a step behind you, and every clean shift of weight turns light armor or bare skin into enough defense to matter.",
                    "Adds your Dexterity modifier as an untyped bonus to AC while wearing no armor or light armor.",
                    "Applies only while wearing no armor or light armor. This feat can be combined with other Extended Replacement feats."),
                CreateLightOrNoArmorRestriction());

            CreateExtendedFeat(
                StatType.Constitution,
                StatType.HitPoints,
                "IronEndurance",
                Guids.ExtendedReplacement2.IronEndurance,
                "Extended_IronEndurance.Name",
                "Iron Endurance",
                "Extended_IronEndurance.Desc",
                BuildExtendedDescription(
                    "Constitution",
                    "Reserve in the Flesh.",
                    "Endurance does more than keep you upright; it deepens the body's reserve, leaving more of you standing between pain and collapse.",
                    "Adds your Constitution modifier as an untyped bonus to Hit Points.",
                    "This feat can be combined with other Extended Replacement feats."));
        }

        private static BlueprintFeature CreateWeaponInsight(
            StatType baseStat,
            string internalName,
            string guid,
            string nameKey,
            string nameValue,
            string descKey,
            string descValue)
        {
            var cfg = FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L(nameKey, nameValue))
                .SetDescription(Common.L(descKey, descValue, tagEncyclopediaEntries: true));

            AddWeaponInsightReplacements(cfg, baseStat);
            return cfg.Configure();
        }

        private static BlueprintFeature CreateExtendedFeat(
            StatType baseStat,
            StatType targetStat,
            string internalName,
            string guid,
            string nameKey,
            string nameValue,
            string descKey,
            string descValue,
            RestrictionCalculator restriction = null)
        {
            var cfg = FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L(nameKey, nameValue))
                .SetDescription(Common.L(descKey, descValue, tagEncyclopediaEntries: true));

            Common.AddRank(cfg, baseStat, AbilityRankType.Default, ContextRankProgression.AsIs);
            cfg.AddContextStatBonus(
                stat: targetStat,
                value: Common.Rank(),
                descriptor: ModifierDescriptor.None,
                restrictions: restriction);
            return cfg.Configure();
        }

        private static void AddWeaponInsightReplacements(FeatureConfigurator cfg, StatType baseStat)
        {
            foreach (var subCategory in WeaponInsightCategories)
            {
                cfg.AddAttackStatReplacementFixed(new AttackStatReplacementFixed(baseStat, subCategory));
            }
        }

        private static RestrictionCalculator CreateLightOrNoArmorRestriction()
            => new()
            {
                Property = new PropertyCalculator
                {
                    Operation = PropertyCalculator.OperationType.Sum,
                    TargetType = PropertyTargetType.CurrentEntity,
                    Getters = new PropertyGetter[]
                    {
                        new LightOrNoArmorPropertyGetter(),
                    },
                },
            };

        private static string BuildWeaponInsightDescription(string attributeName, string loreTitle, string loreBody)
            => $"<i>Weapon Insight · {attributeName}</i>\n<i>{loreTitle}</i> {loreBody}\n\n<b>Effect:</b> Your weapon attack rolls use your {attributeName} modifier instead of Strength or Dexterity whenever {attributeName} would be better.\n\n<b>Restrictions:</b> Mutually exclusive with other Weapon Insight feats.";

        private static string BuildExtendedDescription(
            string attributeName,
            string loreTitle,
            string loreBody,
            string effectText,
            string restrictionText)
            => $"<i>Extended Replacement · {attributeName}</i>\n<i>{loreTitle}</i> {loreBody}\n\n<b>Effect:</b> {effectText}\n\n<b>Restrictions:</b> {restrictionText}";
    }

    [Serializable]
    internal sealed class LightOrNoArmorPropertyGetter : UnitPropertyGetter
    {
        protected override int GetBaseValue()
        {
            var unit = CurrentEntity;
            if (unit?.Body == null)
            {
                return 1;
            }

            foreach (var slot in unit.Body.CurrentEquipmentSlots)
            {
                if (slot is ArmorSlot armorSlot)
                {
                    if (!armorSlot.HasArmor)
                    {
                        return 1;
                    }

                    var proficiencyGroup = armorSlot.Armor?.Blueprint?.ProficiencyGroup ?? ArmorProficiencyGroup.None;
                    return proficiencyGroup == ArmorProficiencyGroup.None || proficiencyGroup == ArmorProficiencyGroup.Light ? 1 : 0;
                }
            }

            return 1;
        }

        protected override string GetInnerCaption() => "No or light armor";
    }
}
