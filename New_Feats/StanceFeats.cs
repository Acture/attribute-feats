using System;
using System.Collections.Generic;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics;

namespace AttributeFeats.New_Feats
{
    internal static class StanceFeats
    {
        private const ModifierDescriptor Desc = ModifierDescriptor.None;
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var feats = new List<BlueprintFeature>
            {
                CreateStance(
                    baseStat: StatType.Strength,
                    internalName: "BrutalStance",
                    featureGuid: Guids.Stance.Feature.Str,
                    buffGuid: Guids.Stance.Buff.Str,
                    activatableGuid: Guids.Stance.Activatable.Str,
                    keyPrefix: "Stance_Str",
                    displayName: "Brutal Stance",
                    description: BuildDescription(
                        "Strength",
                        "Unleashed.",
                        "You drop all guard and channel raw might into every swing, turning physical dominance into a brutal commitment that leaves no strength for defense.",
                        "While active, adds your Strength modifier (untyped) to attack rolls and damage. You suffer a penalty equal to your Strength modifier to AC."),
                    configureBuff: buff => buff
                        .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc, multiplier: -1)),
                CreateStance(
                    baseStat: StatType.Dexterity,
                    internalName: "LiquidForm",
                    featureGuid: Guids.Stance.Feature.Dex,
                    buffGuid: Guids.Stance.Buff.Dex,
                    activatableGuid: Guids.Stance.Activatable.Dex,
                    keyPrefix: "Stance_Dex",
                    displayName: "Liquid Form",
                    description: BuildDescription(
                        "Dexterity",
                        "Flow Without Friction.",
                        "Grace and fluidity carry you through the fight, every motion precise enough to slip between blows before danger ever fully arrives.",
                        "While active, adds your Dexterity modifier (untyped) to AC and Reflex saves. You suffer a penalty equal to your Dexterity modifier to attack rolls."),
                    configureBuff: buff => buff
                        .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.SaveReflex, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc, multiplier: -1)),
                CreateStance(
                    baseStat: StatType.Constitution,
                    internalName: "EndlessVigor",
                    featureGuid: Guids.Stance.Feature.Con,
                    buffGuid: Guids.Stance.Buff.Con,
                    activatableGuid: Guids.Stance.Activatable.Con,
                    keyPrefix: "Stance_Con",
                    displayName: "Endless Vigor",
                    description: BuildDescription(
                        "Constitution",
                        "Breath Without End.",
                        "Endurance and resilience steady every motion, letting vitality carry you past the point where others tire and falter.",
                        "While active, you gain temporary hit points equal to your Constitution modifier. Your attack rolls and Attack of Opportunity count are reduced by your Constitution modifier."),
                    configureBuff: buff =>
                    {
                        // A clean single-attack-only lockout is not exposed here, so this uses the plan's approved attack/AoO penalty fallback.
                        return buff
                            .AddTemporaryHitPointsFromAbilityValue(descriptor: Desc, removeWhenHitPointsEnd: false, value: Common.Rank())
                            .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc, multiplier: -1)
                            .AddContextStatBonus(StatType.AttackOfOpportunityCount, Common.Rank(), descriptor: Desc, multiplier: -1);
                    }),
                CreateStance(
                    baseStat: StatType.Intelligence,
                    internalName: "TacticalMind",
                    featureGuid: Guids.Stance.Feature.Int,
                    buffGuid: Guids.Stance.Buff.Int,
                    activatableGuid: Guids.Stance.Activatable.Int,
                    keyPrefix: "Stance_Int",
                    displayName: "Tactical Mind",
                    description: BuildDescription(
                        "Intelligence",
                        "Calculated Openings.",
                        "Analysis and calculation leave nothing unmeasured; you hold your blade until the enemy presents the exact mistake you predicted.",
                        "While active, adds your Intelligence modifier (untyped) to attacks of opportunity and to your Attack of Opportunity count. You suffer a penalty equal to your Intelligence modifier to attack rolls that are not attacks of opportunity."),
                    includeNegativeRank: true,
                    configureBuff: buff => buff
                        .AddAttackOfOpportunityAttackBonus(Common.Rank(), descriptor: Desc, notAttackOfOpportunity: false)
                        .AddAttackOfOpportunityAttackBonus(Common.Rank(AbilityRankType.StatBonus), descriptor: Desc, notAttackOfOpportunity: true)
                        .AddContextStatBonus(StatType.AttackOfOpportunityCount, Common.Rank(), descriptor: Desc)),
                CreateStance(
                    baseStat: StatType.Wisdom,
                    internalName: "CenteredMind",
                    featureGuid: Guids.Stance.Feature.Wis,
                    buffGuid: Guids.Stance.Buff.Wis,
                    activatableGuid: Guids.Stance.Activatable.Wis,
                    keyPrefix: "Stance_Wis",
                    displayName: "Centered Mind",
                    description: BuildDescription(
                        "Wisdom",
                        "Still Awareness.",
                        "Insight and awareness turn the battlefield quiet in your thoughts, every threat already understood before it can fully form.",
                        "While active, adds your Wisdom modifier (untyped) to AC and all saving throws. You suffer a penalty equal to your Wisdom modifier to attack rolls and damage."),
                    configureBuff: buff => buff
                        .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.SaveFortitude, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.SaveReflex, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.SaveWill, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc, multiplier: -1)
                        .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: Desc, multiplier: -1)),
                CreateStance(
                    baseStat: StatType.Charisma,
                    internalName: "CommandingPresence",
                    featureGuid: Guids.Stance.Feature.Cha,
                    buffGuid: Guids.Stance.Buff.Cha,
                    activatableGuid: Guids.Stance.Activatable.Cha,
                    keyPrefix: "Stance_Cha",
                    displayName: "Commanding Presence",
                    description: BuildDescription(
                        "Charisma",
                        "The Battle Hears You.",
                        "Force of personality bends the rhythm of the fight around you, every command landing with the certainty of a will that expects obedience.",
                        "While active, adds your Charisma modifier (untyped) to attack rolls. You suffer a penalty equal to your Charisma modifier to AC."),
                    configureBuff: buff =>
                    {
                        // A proper ally aura needs extra supporting aura blueprints beyond the 18 preallocated stance assets, so this Wave 1 version keeps the Charisma tradeoff self-only.
                        return buff
                            .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc)
                            .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc, multiplier: -1);
                    }),
            };

            ApplyIntraFamilyMutex(feats);
        }

        private static string BuildDescription(string attributeName, string loreTitle, string loreBody, string effectText)
            => $"<i>Stance · {attributeName}</i>\n<i>{loreTitle}</i> {loreBody}\n\n<b>Effect:</b> {effectText}\n\n<b>Activation:</b> Free action to toggle. Only one Stance feat may be active at a time.\n\n<b>Restrictions:</b> Mutually exclusive with other Stance feats.";

        private static void AddRanks(BuffConfigurator cfg, StatType baseStat, bool includeNegativeRank)
        {
            cfg.AddContextRankConfig(ContextRankConfigs.StatBonus(baseStat, Desc, AbilityRankType.Default, min: 0));
            if (includeNegativeRank)
            {
                cfg.AddContextRankConfig(
                    ContextRankConfigs.StatBonus(baseStat, Desc, AbilityRankType.StatBonus, min: 0)
                        .WithMultiplyByModifierProgression(-1));
            }
        }

        private static void ApplyIntraFamilyMutex(IReadOnlyList<BlueprintFeature> feats)
        {
            for (var i = 0; i < feats.Count; i++)
            {
                for (var j = i + 1; j < feats.Count; j++)
                {
                    Common.AddBidirectionalMutex(feats[i], feats[j]);
                }
            }
        }

        private static BlueprintFeature CreateStance(
            StatType baseStat,
            string internalName,
            string featureGuid,
            string buffGuid,
            string activatableGuid,
            string keyPrefix,
            string displayName,
            string description,
            Func<BuffConfigurator, BuffConfigurator> configureBuff,
            bool includeNegativeRank = false)
        {
            var buff = BuffConfigurator.New($"{internalName}Buff", buffGuid)
                .SetDisplayName(Common.L($"{keyPrefix}.Buff.Name", displayName))
                .SetDescription(Common.L($"{keyPrefix}.Buff.Desc", description, tagEncyclopediaEntries: true));
            AddRanks(buff, baseStat, includeNegativeRank);
            var configuredBuff = configureBuff(buff)
                .AddRecalculateOnStatChange(stat: baseStat)
                .Configure();

            var activatable = ActivatableAbilityConfigurator.New($"{internalName}Activatable", activatableGuid)
                .SetDisplayName(Common.L($"{keyPrefix}.Activatable.Name", displayName))
                .SetDescription(Common.L($"{keyPrefix}.Activatable.Desc", description, tagEncyclopediaEntries: true))
                .SetBuff(configuredBuff)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateIfCombatEnded(false)
                .SetDeactivateIfOwnerDisabled(true)
                .SetDeactivateIfOwnerUnconscious(true)
                .Configure();

            return FeatureConfigurator.New(internalName, featureGuid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"{keyPrefix}.Feature.Name", displayName))
                .SetDescription(Common.L($"{keyPrefix}.Feature.Desc", description, tagEncyclopediaEntries: true))
                .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> { activatable })
                .Configure();
        }
    }
}
