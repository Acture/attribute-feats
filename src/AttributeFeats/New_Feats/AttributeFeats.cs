using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace AttributeFeats.New_Feats
{
    internal static class MainAbilityToEverything_Feats
    {
        private static readonly ModifierDescriptor Desc = ModifierDescriptor.Inherent;

        private static readonly StatType[] AttributeStats =
        {
            StatType.Strength,
            StatType.Dexterity,
            StatType.Constitution,
            StatType.Intelligence,
            StatType.Wisdom,
            StatType.Charisma,
        };

        private static readonly StatType[] DefenseStats =
        {
            StatType.AC,
            StatType.AdditionalCMD,
            StatType.SaveFortitude,
            StatType.SaveReflex,
            StatType.SaveWill,
            StatType.Initiative,
        };

        private static readonly StatType[] ManeuverStats =
        {
            StatType.AdditionalCMB,
        };

        private static readonly StatType[] BABStats =
        {
            StatType.BaseAttackBonus,
        };

        private static readonly StatType[] PowerStats =
        {
            StatType.AdditionalAttackBonus,
            StatType.AdditionalDamage,
            StatType.AttackOfOpportunityCount,
            StatType.SneakAttack,
            StatType.HitPoints,
            StatType.Speed,
        };

        private static readonly StatType[] CheckStats =
        {
            StatType.CheckBluff,
            StatType.CheckDiplomacy,
            StatType.CheckIntimidate,
        };

        private static readonly StatType[] SkillStats =
        {
            StatType.SkillAthletics,
            StatType.SkillKnowledgeArcana,
            StatType.SkillKnowledgeWorld,
            StatType.SkillLoreNature,
            StatType.SkillLoreReligion,
            StatType.SkillMobility,
            StatType.SkillPerception,
            StatType.SkillPersuasion,
            StatType.SkillStealth,
            StatType.SkillThievery,
            StatType.SkillUseMagicDevice,
        };

        private static readonly StatType[] SkilledStats = SkillStats.Concat(CheckStats).ToArray();
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var feats = new List<BlueprintFeature>
            {
                CreateOne(
                    StatType.Strength,
                    "ApexPredator",
                    Guids.str_main_to_everything,
                    "MainAttr_Str.Name",
                    "Apex Predator",
                    "MainAttr_Str.Desc",
                    BuildDescription(
                        "Strength",
                        "Unchallenged Hunger.",
                        "You rule through raw might, turning every contest into proof that physical dominance is destiny.")),
                CreateOne(
                    StatType.Dexterity,
                    "EmbodiedGrace",
                    Guids.dex_main_to_everything,
                    "MainAttr_Dex.Name",
                    "Embodied Grace",
                    "MainAttr_Dex.Desc",
                    BuildDescription(
                        "Dexterity",
                        "Perfect Motion.",
                        "Grace and precision make every motion deliberate, until the world seems to move at the pace you allow.")),
                CreateOne(
                    StatType.Constitution,
                    "LivingBulwark",
                    Guids.con_main_to_everything,
                    "MainAttr_Con.Name",
                    "Living Bulwark",
                    "MainAttr_Con.Desc",
                    BuildDescription(
                        "Constitution",
                        "The Body Endures.",
                        "Endurance answers every threat; what others call punishment, you survive until it becomes power.")),
                CreateOne(
                    StatType.Intelligence,
                    "ArchitectOfSelf",
                    Guids.int_main_to_everything,
                    "MainAttr_Int.Name",
                    "Architect of Self",
                    "MainAttr_Int.Desc",
                    BuildDescription(
                        "Intelligence",
                        "Self as Design.",
                        "You refine yourself like a theorem, reshaping weakness through calculation, discipline, and exact design.")),
                CreateOne(
                    StatType.Wisdom,
                    "WellspringOfInsight",
                    Guids.wis_main_to_everything,
                    "MainAttr_Wis.Name",
                    "Wellspring of Insight",
                    "MainAttr_Wis.Desc",
                    BuildDescription(
                        "Wisdom",
                        "Seeing the Pattern.",
                        "Insight threads through every choice, letting perception and intuition guide even the smallest action.")),
                CreateOne(
                    StatType.Charisma,
                    "CrownOfWill",
                    Guids.cha_main_to_everything,
                    "MainAttr_Cha.Name",
                    "Crown of Will",
                    "MainAttr_Cha.Desc",
                    BuildDescription(
                        "Charisma",
                        "Sovereign Presence.",
                        "The force of your will bends the moment around you, making confidence a weapon and resolve a crown.")),
            };

            foreach (var feat in feats)
            {
                FeatureConfigurator.For(feat).Configure();
            }

            for (var i = 0; i < feats.Count; i++)
            {
                for (var j = i + 1; j < feats.Count; j++)
                {
                    Common.AddBidirectionalMutex(feats[i], feats[j]);
                }
            }
        }

        private static string BuildDescription(string attributeName, string loreTitle, string loreBody)
            => $"<i>Main Attribute Mastery · {attributeName}</i>\n<i>{loreTitle}</i> {loreBody}\n\n<b>Effect:</b> Adds your {attributeName} modifier as an inherent bonus to enabled attributes, defenses, maneuvers, skills/checks, caster bonuses, Base Attack Bonus, and Power Mode bonuses from the mod settings. If self-stacking is enabled it also adds to {attributeName}, and Reach becomes a fixed +1 when Power Mode is enabled.\n\n<b>Restrictions:</b> Mutually exclusive with the other Main Attribute Mastery feats.";

        private static BlueprintFeature CreateOne(
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

            var settings = Main.Settings ?? new ModSettings();
            Common.AddRank(cfg, baseStat, AbilityRankType.Default, Common.ResolveProgression(settings.powerLevel, ScalingIntent.Full));
            Common.AddRank(cfg, baseStat, AbilityRankType.StatBonus, Common.ResolveProgression(settings.powerLevel, ScalingIntent.Half));

            void AddContextBonuses(IEnumerable<StatType> stats, AbilityRankType rankType)
            {
                foreach (var stat in stats)
                {
                    cfg.AddComponent<AddContextStatBonus>(c =>
                    {
                        c.Stat = stat;
                        c.Descriptor = Desc;
                        c.Value = Common.Rank(rankType);
                    });
                }
            }

            if (settings.EnableAttributes)
            {
                var attributeStats = settings.IncludeSelfInAttributeStack
                    ? AttributeStats
                    : AttributeStats.Where(stat => stat != baseStat);
                AddContextBonuses(attributeStats, AbilityRankType.Default);
            }

            if (settings.EnableDefenses)
                AddContextBonuses(DefenseStats, AbilityRankType.Default);

            if (settings.EnableManeuvers)
                AddContextBonuses(ManeuverStats, AbilityRankType.Default);

            if (settings.EnableSkills && settings.EnableChecks)
            {
                AddContextBonuses(SkilledStats, AbilityRankType.Default);
            }
            else
            {
                if (settings.EnableSkills)
                    AddContextBonuses(SkillStats, AbilityRankType.Default);

                if (settings.EnableChecks)
                    AddContextBonuses(CheckStats, AbilityRankType.Default);
            }

            if (settings.EnableBAB)
                AddContextBonuses(BABStats, AbilityRankType.StatBonus);

            if (settings.EnablePowerMode)
            {
                AddContextBonuses(PowerStats, AbilityRankType.StatBonus);
                cfg.AddComponent<AddStatBonus>(c =>
                {
                    c.Stat = StatType.Reach;
                    c.Value = 1;
                    c.Descriptor = Desc;
                });
            }

            if (settings.EnableCasterDC)
            {
                cfg.AddComponent<IncreaseAllSpellsDC>(c =>
                {
                    c.Value = Common.Rank(AbilityRankType.StatBonus);
                    c.Descriptor = Desc;
                    c.SpellsOnly = false;
                });
            }

            if (settings.EnableCasterLevel)
            {
                cfg.AddComponent<IncreaseCasterLevel>(c =>
                {
                    c.Value = Common.Rank(AbilityRankType.Default);
                    c.Descriptor = Desc;
                });
            }

            if (settings.EnableSpellPenetration)
            {
                cfg.AddComponent<SpellPenetrationBonus>(c =>
                {
                    c.Value = Common.Rank(AbilityRankType.Default);
                    c.Descriptor = Desc;
                });
            }

            cfg.AddRecalculateOnStatChange(stat: baseStat);
            return cfg.Configure();
        }
    }
}
