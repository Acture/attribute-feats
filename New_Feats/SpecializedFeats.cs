using System.Collections.Generic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace AttributeFeats.New_Feats
{
    /// <summary>
    /// Creates the 24 Specialized Adept feats.
    /// </summary>
    internal static class SpecializedFeats
    {
        private static readonly ModifierDescriptor Desc = ModifierDescriptor.None;
        private static readonly StatType[] NoStats = new StatType[0];

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

        private static readonly StatType[] CheckStats =
        {
            StatType.CheckBluff,
            StatType.CheckDiplomacy,
            StatType.CheckIntimidate,
        };

        private static bool Initialized;

        private enum SpecializedFamily
        {
            Defensive,
            Maneuver,
            Skilled,
            Arcane,
        }

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var defensive = new[]
            {
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Strength,
                    DefenseStats,
                    Guids.Specialized.Defensive.Str,
                    "TitansStance",
                    "Titan's Stance",
                    "<i>Brutal Defense.</i> You stand against your enemies with the same overwhelming presence that crushes them — your sheer mass and strength making every blow against you feel like striking a mountain."),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Dexterity,
                    DefenseStats,
                    Guids.Specialized.Defensive.Dex,
                    "FlowingForm",
                    "Flowing Form",
                    "<i>Untouchable Grace.</i> You slip away from danger before it manifests; the blade that should have struck you finds only the wind where you once stood."),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Constitution,
                    DefenseStats,
                    Guids.Specialized.Defensive.Con,
                    "IronBulwark",
                    "Iron Bulwark",
                    "<i>Unbreakable.</i> Your body resists what crushes others. Pain, poison, exhaustion — all wash over the fortress of your flesh and find no purchase."),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Intelligence,
                    DefenseStats,
                    Guids.Specialized.Defensive.Int,
                    "CalculatedDefense",
                    "Calculated Defense",
                    "<i>The Analyst's Shield.</i> You read every attack as an equation — its angle, its tempo, its inevitable answer. Knowing the enemy is the first defense; knowing yourself is the last."),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Wisdom,
                    DefenseStats,
                    Guids.Specialized.Defensive.Wis,
                    "StoicVigilance",
                    "Stoic Vigilance",
                    "<i>Aware of All Threats.</i> Long meditation has tied your reflexes to your awareness; danger you see is danger you have already begun to answer."),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Charisma,
                    DefenseStats,
                    Guids.Specialized.Defensive.Cha,
                    "IndomitablePresence",
                    "Indomitable Presence",
                    "<i>Force of Self.</i> Your conviction is a shield no weapon can pierce. Foes hesitate before striking you, finding their resolve unraveling in the face of your unshakable will."),
            };

            var maneuver = new[]
            {
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Strength,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Str,
                    "CrushingGrip",
                    "Crushing Grip",
                    "<i>Overwhelming Leverage.</i> You seize control through raw might, turning every trip, shove, and grapple into an undeniable display of physical dominance."),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Dexterity,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Dex,
                    "DeftHand",
                    "Deft Hand",
                    "<i>Flicker Control.</i> Your hands find openings between heartbeats, letting balance, timing, and precision turn motion itself into leverage."),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Constitution,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Con,
                    "UnyieldingHold",
                    "Unyielding Hold",
                    "<i>Relentless Pressure.</i> You do not need a perfect opening when you can simply outlast resistance, grinding your foe down until the hold is inevitable."),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Intelligence,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Int,
                    "TacticalBind",
                    "Tactical Bind",
                    "<i>Engineered Openings.</i> Every maneuver begins as a solved problem, its angles and counters already mapped before the first contact."),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Wisdom,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Wis,
                    "PredictiveLock",
                    "Predictive Lock",
                    "<i>Read the Motion.</i> By sensing intent before it becomes action, you catch the enemy where they are going rather than where they stand."),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Charisma,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Cha,
                    "DomineeringThrow",
                    "Domineering Throw",
                    "<i>Command of Space.</i> Your presence breaks an opponent's posture before your hands ever touch them, making every throw feel like a public humiliation."),
            };

            var skilled = new[]
            {
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Strength,
                    SkillStats,
                    Guids.Specialized.Skilled.Str,
                    "PracticedHand",
                    "Practiced Hand",
                    "<i>Work Made Mastery.</i> Strength is not only for battle; steady labor, hard practice, and a powerful hand let you force technique into reliable habit."),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Dexterity,
                    SkillStats,
                    Guids.Specialized.Skilled.Dex,
                    "EffortlessSkill",
                    "Effortless Skill",
                    "<i>Natural Technique.</i> Precision turns repetition into instinct, until difficult tasks feel like simple extensions of a perfectly trained motion."),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Constitution,
                    SkillStats,
                    Guids.Specialized.Skilled.Con,
                    "TirelessPractice",
                    "Tireless Practice",
                    "<i>Endurance Refined.</i> Where others tire, you continue. Skill grows through repetition, and no one can match the hours your body can endure."),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Intelligence,
                    SkillStats,
                    Guids.Specialized.Skilled.Int,
                    "PolymathsTouch",
                    "Polymath's Touch",
                    "<i>Breadth of Study.</i> No discipline stays isolated for long in your mind; knowledge spills from one field into every other until mastery multiplies itself."),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Wisdom,
                    SkillStats,
                    Guids.Specialized.Skilled.Wis,
                    "QuietMastery",
                    "Quiet Mastery",
                    "<i>Patient Perception.</i> You notice what others miss, and skill follows from truly seeing the world instead of stumbling through it."),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Charisma,
                    SkillStats,
                    Guids.Specialized.Skilled.Cha,
                    "InspiredVersatility",
                    "Inspired Versatility",
                    "<i>Talent Made Manifest.</i> Confidence carries you through unfamiliar challenges, turning versatility into performance and performance into success."),
            };

            var arcane = new[]
            {
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Strength,
                    NoStats,
                    Guids.Specialized.Arcane.Str,
                    "SpellForgedWill",
                    "Spell-Forged Will",
                    "<i>Magic Under Pressure.</i> You force power through stubborn will and raw might, hammering spellcraft into shape like iron on an anvil."),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Dexterity,
                    NoStats,
                    Guids.Specialized.Arcane.Dex,
                    "QuickcastReflex",
                    "Quickcast Reflex",
                    "<i>Fingers Faster than Thought.</i> Swift gestures and flawless timing let your magic slip into the world before resistance can gather."),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Constitution,
                    NoStats,
                    Guids.Specialized.Arcane.Con,
                    "SpellTemperedBody",
                    "Spell-Tempered Body",
                    "<i>The Body as Crucible.</i> Your flesh bears the strain of power without faltering, steadying spells that would fray lesser casters."),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Intelligence,
                    NoStats,
                    Guids.Specialized.Arcane.Int,
                    "ScholarOfTheWeave",
                    "Scholar of the Weave",
                    "<i>Scholarship Made Spellcraft.</i> You read the weave like a manuscript, refining every incantation through analysis, structure, and exact recall."),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Wisdom,
                    NoStats,
                    Guids.Specialized.Arcane.Wis,
                    "OraclesIntuition",
                    "Oracle's Intuition",
                    "<i>Hear the Hidden Current.</i> Intuition guides your casting along the unseen flow beneath reality, making each spell feel discovered rather than constructed."),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Charisma,
                    NoStats,
                    Guids.Specialized.Arcane.Cha,
                    "SorcerousPresence",
                    "Sorcerous Presence",
                    "<i>Command the Weave.</i> Magic responds to certainty in your voice and presence, bending more readily when you cast as though the world already agrees."),
            };

            AddFamilyMutex(defensive);
            AddFamilyMutex(maneuver);
            AddFamilyMutex(skilled);
            AddFamilyMutex(arcane);
        }

        private static BlueprintFeature CreateSpecialized(
            SpecializedFamily family,
            StatType baseStat,
            IReadOnlyList<StatType> stats,
            string guid,
            string internalName,
            string flavorName,
            string loreText)
        {
            var settings = Main.Settings ?? new ModSettings();
            var attributeKey = GetAttributeKey(baseStat);
            var attributeName = GetAttributeName(baseStat);
            var familyKey = GetFamilyKey(family);
            var familyName = GetFamilyDisplayName(family);

            var cfg = FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"{familyKey}_{attributeKey}.Name", flavorName))
                .SetDescription(Common.L(
                    $"{familyKey}_{attributeKey}.Desc",
                    BuildDescription(
                        familyName,
                        attributeName,
                        loreText,
                        GetEffectText(family, attributeName),
                        GetRestrictionText(family, attributeName)),
                    tagEncyclopediaEntries: true));

            Common.AddRank(cfg, baseStat, AbilityRankType.Default, Common.ResolveProgression(settings.powerLevel, ScalingIntent.Full));
            if (family == SpecializedFamily.Arcane)
            {
                Common.AddRank(cfg, baseStat, AbilityRankType.StatBonus, Common.ResolveProgression(settings.powerLevel, ScalingIntent.Half));
            }

            switch (family)
            {
                case SpecializedFamily.Defensive:
                    if (settings.EnableDefenses)
                    {
                        AddContextBonuses(cfg, stats, AbilityRankType.Default);
                    }
                    break;
                case SpecializedFamily.Maneuver:
                    if (settings.EnableManeuvers)
                    {
                        AddContextBonuses(cfg, stats, AbilityRankType.Default);
                    }
                    break;
                case SpecializedFamily.Skilled:
                    if (settings.EnableSkills)
                    {
                        AddContextBonuses(cfg, SkillStats, AbilityRankType.Default);
                    }

                    if (settings.EnableChecks)
                    {
                        AddContextBonuses(cfg, CheckStats, AbilityRankType.Default);
                    }
                    break;
                case SpecializedFamily.Arcane:
                    if (settings.EnableCasterLevel)
                    {
                        cfg.AddComponent<IncreaseCasterLevel>(c =>
                        {
                            c.Value = Common.Rank(AbilityRankType.Default);
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

                    if (settings.EnableSpellPenetration)
                    {
                        cfg.AddComponent<SpellPenetrationBonus>(c =>
                        {
                            c.Value = Common.Rank(AbilityRankType.Default);
                            c.Descriptor = Desc;
                        });
                    }
                    break;
            }

            cfg.AddRecalculateOnStatChange(stat: baseStat);
            return cfg.Configure();
        }

        private static void AddFamilyMutex(IReadOnlyList<BlueprintFeature> feats)
        {
            for (var i = 0; i < feats.Count; i++)
            {
                for (var j = i + 1; j < feats.Count; j++)
                {
                    Common.AddBidirectionalMutex(feats[i], feats[j]);
                }
            }
        }

        private static void AddContextBonuses(FeatureConfigurator cfg, IReadOnlyList<StatType> stats, AbilityRankType rankType)
        {
            foreach (var stat in stats)
            {
                cfg.AddContextStatBonus(stat, Common.Rank(rankType), Desc);
            }
        }

        private static string BuildDescription(
            string familyName,
            string attributeName,
            string loreText,
            string effectText,
            string restrictionText)
            => $"<i>{familyName} · {attributeName}</i>\n{loreText}\n\n<b>Effect:</b> {effectText}\n\n<b>Restrictions:</b> {restrictionText}";

        private static string GetEffectText(SpecializedFamily family, string attributeName)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return $"Adds your {attributeName} modifier (untyped) to AC, CMD, Initiative, and all saving throws.";
                case SpecializedFamily.Maneuver:
                    return $"Adds your {attributeName} modifier (untyped) to CMB.";
                case SpecializedFamily.Skilled:
                    return $"Adds your {attributeName} modifier (untyped) to all skills, plus Bluff, Diplomacy, and Intimidate checks.";
                case SpecializedFamily.Arcane:
                    return $"Adds your {attributeName} modifier (untyped) to caster level and spell penetration checks, plus half your {attributeName} modifier to spell save DCs.";
                default:
                    return string.Empty;
            }
        }

        private static string GetRestrictionText(SpecializedFamily family, string attributeName)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return $"Mutually exclusive with other Defensive Adept feats and with the {attributeName} Main Attribute Mastery feat.";
                case SpecializedFamily.Maneuver:
                    return $"Mutually exclusive with other Maneuver Adept feats and with the {attributeName} Main Attribute Mastery feat.";
                case SpecializedFamily.Skilled:
                    return $"Mutually exclusive with other Skilled feats and with the {attributeName} Main Attribute Mastery feat.";
                case SpecializedFamily.Arcane:
                    return $"Mutually exclusive with other Arcane Insight feats and with the {attributeName} Main Attribute Mastery feat.";
                default:
                    return string.Empty;
            }
        }

        private static string GetFamilyKey(SpecializedFamily family)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return "Defensive";
                case SpecializedFamily.Maneuver:
                    return "Maneuver";
                case SpecializedFamily.Skilled:
                    return "Skilled";
                case SpecializedFamily.Arcane:
                    return "Arcane";
                default:
                    return family.ToString();
            }
        }

        private static string GetFamilyDisplayName(SpecializedFamily family)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return "Defensive Adept";
                case SpecializedFamily.Maneuver:
                    return "Maneuver Adept";
                case SpecializedFamily.Skilled:
                    return "Skilled";
                case SpecializedFamily.Arcane:
                    return "Arcane Insight";
                default:
                    return family.ToString();
            }
        }

        private static string GetAttributeKey(StatType baseStat)
        {
            switch (baseStat)
            {
                case StatType.Strength:
                    return "Str";
                case StatType.Dexterity:
                    return "Dex";
                case StatType.Constitution:
                    return "Con";
                case StatType.Intelligence:
                    return "Int";
                case StatType.Wisdom:
                    return "Wis";
                case StatType.Charisma:
                    return "Cha";
                default:
                    return baseStat.ToString();
            }
        }

        private static string GetAttributeName(StatType baseStat)
        {
            switch (baseStat)
            {
                case StatType.Strength:
                    return "Strength";
                case StatType.Dexterity:
                    return "Dexterity";
                case StatType.Constitution:
                    return "Constitution";
                case StatType.Intelligence:
                    return "Intelligence";
                case StatType.Wisdom:
                    return "Wisdom";
                case StatType.Charisma:
                    return "Charisma";
                default:
                    return baseStat.ToString();
            }
        }
    }
}
