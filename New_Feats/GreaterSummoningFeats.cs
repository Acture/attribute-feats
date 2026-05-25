using System.Collections.Generic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace AttributeFeats.New_Feats
{
    internal static class GreaterSummoningFeats
    {
        private static readonly ModifierDescriptor Desc = ModifierDescriptor.None;
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var feats = new[]
            {
                CreateFeat(
                    internalName: "BloodlineOfBeasts",
                    featureGuid: Guids.Summon.Feature.BloodlineOfBeasts,
                    outerBuffGuid: Guids.Summon.OuterBuff.BloodlineOfBeasts,
                    innerBuffGuid: Guids.Summon.InnerBuff.BloodlineOfBeasts,
                    flavorName: "Bloodline of Beasts",
                    attributeName: "Strength",
                    baseStat: StatType.Strength,
                    buffedStats: new[] { StatType.Strength },
                    loreText: "<i>Predatory Heritage.</i> The savage hunger in your blood answers every conjuration, urging your summoned creatures to meet the world fang-first.",
                    effectText: "Your summoned creatures gain an untyped bonus to Strength equal to your Strength modifier."),
                CreateFeat(
                    internalName: "QuickenedPact",
                    featureGuid: Guids.Summon.Feature.QuickenedPact,
                    outerBuffGuid: Guids.Summon.OuterBuff.QuickenedPact,
                    innerBuffGuid: Guids.Summon.InnerBuff.QuickenedPact,
                    flavorName: "Quickened Pact",
                    attributeName: "Dexterity",
                    baseStat: StatType.Dexterity,
                    buffedStats: new[] { StatType.Dexterity, StatType.Speed },
                    loreText: "<i>Swift Beyond Sight.</i> Your bargains favor motion without hesitation, so every creature you call arrives already poised to dart, weave, and pounce.",
                    effectText: "Your summoned creatures gain untyped bonuses to Dexterity and Speed equal to your Dexterity modifier."),
                CreateFeat(
                    internalName: "VitalPact",
                    featureGuid: Guids.Summon.Feature.VitalPact,
                    outerBuffGuid: Guids.Summon.OuterBuff.VitalPact,
                    innerBuffGuid: Guids.Summon.InnerBuff.VitalPact,
                    flavorName: "Vital Pact",
                    attributeName: "Constitution",
                    baseStat: StatType.Constitution,
                    buffedStats: new[] { StatType.Constitution },
                    loreText: "<i>Breath Shared.</i> You bind your summons with the same stubborn life that anchors you, lending them the endurance to remain standing through the worst of the fight.",
                    effectText: "Your summoned creatures gain an untyped bonus to Constitution equal to your Constitution modifier."),
                CreateFeat(
                    internalName: "TacticalBinding",
                    featureGuid: Guids.Summon.Feature.TacticalBinding,
                    outerBuffGuid: Guids.Summon.OuterBuff.TacticalBinding,
                    innerBuffGuid: Guids.Summon.InnerBuff.TacticalBinding,
                    flavorName: "Tactical Binding",
                    attributeName: "Intelligence",
                    baseStat: StatType.Intelligence,
                    buffedStats: new[] { StatType.AC },
                    loreText: "<i>The Summoned Formation.</i> Careful diagrams and exact sigils turn every conjuration into a planned deployment, your creatures warded by the geometry of your will.",
                    effectText: "Your summoned creatures gain an untyped bonus to AC equal to your Intelligence modifier."),
                CreateFeat(
                    internalName: "InsightfulSummons",
                    featureGuid: Guids.Summon.Feature.InsightfulSummons,
                    outerBuffGuid: Guids.Summon.OuterBuff.InsightfulSummons,
                    innerBuffGuid: Guids.Summon.InnerBuff.InsightfulSummons,
                    flavorName: "Insightful Summons",
                    attributeName: "Wisdom",
                    baseStat: StatType.Wisdom,
                    buffedStats: new[] { StatType.SaveFortitude, StatType.SaveReflex, StatType.SaveWill },
                    loreText: "<i>Guided Instinct.</i> Your summons move beneath a quiet current of warning, sensing danger with the same intuition that keeps you centered amid chaos.",
                    effectText: "Your summoned creatures gain untyped bonuses to Fortitude, Reflex, and Will saves equal to your Wisdom modifier."),
                CreateFeat(
                    internalName: "MagneticCalling",
                    featureGuid: Guids.Summon.Feature.MagneticCalling,
                    outerBuffGuid: Guids.Summon.OuterBuff.MagneticCalling,
                    innerBuffGuid: Guids.Summon.InnerBuff.MagneticCalling,
                    flavorName: "Magnetic Calling",
                    attributeName: "Charisma",
                    baseStat: StatType.Charisma,
                    buffedStats: new[] { StatType.AdditionalAttackBonus },
                    loreText: "<i>Irresistible Command.</i> The force of your presence does not end at the circle's edge; creatures you call lean into battle with the confidence of your own will.",
                    effectText: "Your summoned creatures gain an untyped bonus to attack rolls equal to your Charisma modifier."),
            };

            AddFamilyMutex(feats);
        }

        private static BlueprintFeature CreateFeat(
            string internalName,
            string featureGuid,
            string outerBuffGuid,
            string innerBuffGuid,
            string flavorName,
            string attributeName,
            StatType baseStat,
            IReadOnlyList<StatType> buffedStats,
            string loreText,
            string effectText)
        {
            var displayName = Common.L($"Summon_{internalName}.Name", flavorName);
            var description = Common.L(
                $"Summon_{internalName}.Desc",
                BuildDescription(attributeName, loreText, effectText),
                tagEncyclopediaEntries: true);

            var innerBuff = BuffConfigurator.New($"{internalName}InnerBuff", innerBuffGuid)
                .SetDisplayName(displayName)
                .SetDescription(description);
            AddRank(innerBuff, baseStat);
            AddContextBonuses(innerBuff, buffedStats);
            var configuredInnerBuff = innerBuff.Configure();

            var outerBuff = BuffConfigurator.New($"{internalName}OuterBuff", outerBuffGuid)
                .SetDisplayName(displayName)
                .SetDescription(description)
                .AddOnSpawnBuff(buff: configuredInnerBuff, isInfinity: true)
                .Configure();

            return FeatureConfigurator.New(internalName, featureGuid, FeatureGroup.Feat)
                .SetDisplayName(displayName)
                .SetDescription(description)
                .AddFacts(new() { outerBuff })
                .Configure();
        }

        private static void AddRank(BuffConfigurator cfg, StatType baseStat)
        {
            cfg.AddContextRankConfig(ContextRankConfigs.StatBonus(baseStat, ModifierDescriptor.None, AbilityRankType.Default, min: 0));
        }

        private static void AddContextBonuses(BuffConfigurator cfg, IReadOnlyList<StatType> stats)
        {
            foreach (var stat in stats)
            {
                cfg.AddContextStatBonus(stat, Common.Rank(), Desc);
            }
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

        private static string BuildDescription(string attributeName, string loreText, string effectText)
            => $"<i>Greater Summoning · {attributeName}</i>\n{loreText}\n\n<b>Effect:</b> {effectText}\n\n<b>Restrictions:</b> Mutually exclusive with other Greater Summoning feats.";
    }
}
