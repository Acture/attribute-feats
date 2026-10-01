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
                    nameEn: "Martyr's Transference",
                    nameZh: "形神替生",
                    desc: BuildDescription(
                        modeEn: "1:1 Trade",
                        modeZh: "等价献祭",
                        loreEn: "<i>What you surrender, your servants inherit.</i> You willingly drain your own vital energies, intellect, and worldly presence across the conjuration circle, feeding raw spirit directly into your minions to elevate them to terrifying heights.",
                        loreZh: "<i>舍己塑灵。</i>割裂自身气血、灵慧与威仪，尽数灌入通灵法阵。以施法者本命元神为薪柴，换取召来异界使者全方位的惊世蜕变。",
                        effectEn: "You take a -4 untyped penalty to Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma. Your summoned creatures gain a +4 untyped bonus to Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma.",
                        effectZh: "你的力量、敏捷、体质、智力、感知和魅力承受-4无类型减值。你召唤的生物的力量、敏捷、体质、智力、感知和魅力获得+4无类型加值。"),
                    selfBonuses: CreateUniformBonuses(-4),
                    summonBonuses: CreateUniformBonuses(4)),
                CreateFeat(
                    internalName: "DoubledBond",
                    featureGuid: Guids.SummonerSacrifice.Feature.DoubledBond,
                    outerBuffGuid: Guids.SummonerSacrifice.OuterBuff.DoubledBond,
                    innerBuffGuid: Guids.SummonerSacrifice.InnerBuff.DoubledBond,
                    nameEn: "Eldritch Crucible",
                    nameZh: "双生法炼",
                    desc: BuildDescription(
                        modeEn: "1:2 Amplification",
                        modeZh: "倍率谐振",
                        loreEn: "<i>Asymmetrical Resonance.</i> Through arcane harmonic resonance, you stretch each spark of sacrificed essence across the summoning circle twofold. A minor toll upon your vessel unlocks disproportionate planar ascendancy.",
                        loreZh: "<i>法脉谐振。</i>洞悉异界位面的回音法则，将献祭的精魄于通灵阵中激荡放大。微损施法本体，即可撬动受召军团成倍的位面威能。",
                        effectEn: "You take a -2 untyped penalty to Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma. Your summoned creatures gain a +4 untyped bonus to Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma.",
                        effectZh: "你的力量、敏捷、体质、智力、感知和魅力承受-2无类型减值。你召唤的生物的力量、敏捷、体质、智力、感知和魅力获得+4无类型加值。"),
                    selfBonuses: CreateUniformBonuses(-2),
                    summonBonuses: CreateUniformBonuses(4)),
                CreateFeat(
                    internalName: "EmpoweredSacrifice",
                    featureGuid: Guids.SummonerSacrifice.Feature.EmpoweredSacrifice,
                    outerBuffGuid: Guids.SummonerSacrifice.OuterBuff.EmpoweredSacrifice,
                    innerBuffGuid: Guids.SummonerSacrifice.InnerBuff.EmpoweredSacrifice,
                    nameEn: "Tribute of Iron Dominion",
                    nameZh: "夺冕化蛮",
                    desc: BuildDescription(
                        modeEn: "Focused Trade",
                        modeZh: "极意倾注",
                        loreEn: "<i>Crown Surrendered to Claws.</i> You strip away the haughty grace of command, channeling raw monarchic authority into pure, brutal muscle. Your minions lose all subtlety, transfigured into hulking juggernauts of annihilation.",
                        loreZh: "<i>折冠铸殛。</i>剥离统御者的从容仪度，将全部支配欲念熔铸为受召者撕碎万物的暴戾蛮力。麾下爪牙褪尽精巧，化作摧山撼岳的嗜血巨灵。",
                        effectEn: "You take a -4 untyped penalty to Charisma. Your summoned creatures gain a +8 untyped bonus to Strength.",
                        effectZh: "你的魅力承受-4无类型减值。你召唤的生物获得+8无类型力量加值。"),
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
            string nameEn,
            string nameZh,
            (string en, string zh) desc,
            IReadOnlyList<StatBonus> selfBonuses,
            IReadOnlyList<StatBonus> summonBonuses)
        {
            var localizedName = Common.L($"SummonerSacrifice_{internalName}.Name", nameEn, nameZh);
            var localizedDescription = Common.L($"SummonerSacrifice_{internalName}.Desc", desc.en, desc.zh, tagEncyclopediaEntries: true);

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

        private static (string en, string zh) BuildDescription(
            string modeEn,
            string modeZh,
            string loreEn,
            string loreZh,
            string effectEn,
            string effectZh)
        {
            var en = $"<i>Summoner Sacrifice · {modeEn}</i>\n{loreEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Restrictions:</b> Mutually exclusive with other Summoner Sacrifice feats.";
            var zh = $"<i>召唤献祭 · {modeZh}</i>\n{loreZh}\n\n<b>效果：</b>{effectZh}\n\n<b>限制：</b>与其他“召唤献祭”专长互斥。";
            return (en, zh);
        }

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
