using System.Collections.Generic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;

namespace AttributeFeats.New_Feats
{
    internal static class SummonerSacrificeFeats
    {
        private const ModifierDescriptor Desc = ModifierDescriptor.None;
        private static readonly StatType[] AllAttributes =
        {
            StatType.Strength,
            StatType.Dexterity,
            StatType.Constitution,
            StatType.Intelligence,
            StatType.Wisdom,
            StatType.Charisma,
        };

        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var feats = new[]
            {
                CreateFeat(
                    internalName: "BodyOfMyPact",
                    featureGuid: Guids.SummonerSacrifice.Feature.BodyOfMyPact,
                    outerBuffGuid: Guids.SummonerSacrifice.OuterBuff.BodyOfMyPact,
                    innerBuffGuid: Guids.SummonerSacrifice.InnerBuff.BodyOfMyPact,
                    displayName: "Body of My Pact",
                    description: BuildDescription(
                        modeName: "1:1 Trade",
                        loreText: "<i>What you yield, your servants inherit.</i> You divide your strength, wit, and presence across every calling so your summoned allies arrive bearing the full weight of your sacrificed essence.",
                        effectText: "You take a -4 untyped penalty to Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma. Your summoned creatures gain a +4 untyped bonus to Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma."),
                    selfBonuses: CreateUniformBonuses(-4),
                    summonBonuses: CreateUniformBonuses(4)),
                CreateFeat(
                    internalName: "DoubledBond",
                    featureGuid: Guids.SummonerSacrifice.Feature.DoubledBond,
                    outerBuffGuid: Guids.SummonerSacrifice.OuterBuff.DoubledBond,
                    innerBuffGuid: Guids.SummonerSacrifice.InnerBuff.DoubledBond,
                    displayName: "Doubled Bond",
                    description: BuildDescription(
                        modeName: "1:2 Amplification",
                        loreText: "<i>A lesser loss, a greater echo.</i> Your pact stretches each fragment of sacrificed essence across the circle twice over, letting your summons claim more power than you surrender.",
                        effectText: "You take a -2 untyped penalty to Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma. Your summoned creatures gain a +4 untyped bonus to Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma."),
                    selfBonuses: CreateUniformBonuses(-2),
                    summonBonuses: CreateUniformBonuses(4)),
                CreateFeat(
                    internalName: "EmpoweredSacrifice",
                    featureGuid: Guids.SummonerSacrifice.Feature.EmpoweredSacrifice,
                    outerBuffGuid: Guids.SummonerSacrifice.OuterBuff.EmpoweredSacrifice,
                    innerBuffGuid: Guids.SummonerSacrifice.InnerBuff.EmpoweredSacrifice,
                    displayName: "Empowered Sacrifice",
                    description: BuildDescription(
                        modeName: "Focused Trade",
                        loreText: "<i>Command traded for force.</i> You surrender a portion of your personal presence to drive raw physical might into the creatures that answer your call.",
                        effectText: "You take a -4 untyped penalty to Charisma. Your summoned creatures gain a +8 untyped bonus to Strength."),
                    selfBonuses: new[] { new StatBonus(StatType.Charisma, -4) },
                    summonBonuses: new[] { new StatBonus(StatType.Strength, 8) }),
            };

            AddFamilyMutex(feats);
        }

        private static BlueprintFeature CreateFeat(
            string internalName,
            string featureGuid,
            string outerBuffGuid,
            string innerBuffGuid,
            string displayName,
            string description,
            IReadOnlyList<StatBonus> selfBonuses,
            IReadOnlyList<StatBonus> summonBonuses)
        {
            var localizedName = Common.L($"SummonerSacrifice_{internalName}.Name", displayName);
            var localizedDescription = Common.L($"SummonerSacrifice_{internalName}.Desc", description, tagEncyclopediaEntries: true);

            var innerBuff = BuffConfigurator.New($"{internalName}InnerBuff", innerBuffGuid)
                .SetDisplayName(localizedName)
                .SetDescription(localizedDescription)
                .AddSummonedUnitBuff();
            AddStatBonuses(innerBuff, summonBonuses);
            var configuredInnerBuff = innerBuff.Configure();

            var outerBuff = BuffConfigurator.New($"{internalName}OuterBuff", outerBuffGuid)
                .SetDisplayName(localizedName)
                .SetDescription(localizedDescription)
                .AddOnSpawnBuff(buff: configuredInnerBuff, isInfinity: true);
            AddStatBonuses(outerBuff, selfBonuses);
            var configuredOuterBuff = outerBuff.Configure();

            return FeatureConfigurator.New(internalName, featureGuid, FeatureGroup.Feat)
                .SetDisplayName(localizedName)
                .SetDescription(localizedDescription)
                .AddFacts(new() { configuredOuterBuff })
                .Configure();
        }

        private static void AddStatBonuses(BuffConfigurator cfg, IReadOnlyList<StatBonus> bonuses)
        {
            foreach (var bonus in bonuses)
            {
                cfg.AddStatBonus(descriptor: Desc, stat: bonus.Stat, value: bonus.Value);
            }
        }

        private static StatBonus[] CreateUniformBonuses(int value)
        {
            var bonuses = new StatBonus[AllAttributes.Length];
            for (var i = 0; i < AllAttributes.Length; i++)
            {
                bonuses[i] = new StatBonus(AllAttributes[i], value);
            }

            return bonuses;
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

        private static string BuildDescription(string modeName, string loreText, string effectText)
            => $"<i>Summoner Sacrifice · {modeName}</i>\n{loreText}\n\n<b>Effect:</b> {effectText}\n\n<b>Restrictions:</b> Mutually exclusive with other Summoner Sacrifice feats.";

        private readonly struct StatBonus
        {
            public StatBonus(StatType stat, int value)
            {
                Stat = stat;
                Value = value;
            }

            public StatType Stat { get; }
            public int Value { get; }
        }
    }
}
