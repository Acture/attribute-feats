using System.Collections.Generic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;

namespace ACHomebrew.Feats
{
    internal static class SummonerSacrificeFeats
    {
        private const ModifierDescriptor Desc = ModifierDescriptor.None;
        private static readonly StatType[] PhysicalAttributes =
        {
            StatType.Strength,
            StatType.Dexterity,
            StatType.Constitution,
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
                    nameEn: Common.Text("SummonerSacrifice_BodyOfMyPact.Name", "Martyr's Transference"),
                    nameZh: Common.Text("SummonerSacrifice_BodyOfMyPact.Name", "形神替生", true),
                    desc: BuildDescription(
                        modeEn: "1:1 Trade",
                        modeZh: "等价献祭",
                        loreEn: Common.Text("SummonerSacrifice_BodyOfMyPact.Lore", "You accept a body's burden so that the creatures bound to your call may stand stronger."),
                        loreZh: Common.Text("SummonerSacrifice_BodyOfMyPact.Lore", "你甘愿让自身承受负担，使回应你召唤的生物更加强健。", true),
                        effectEn: "You take a -4 untyped penalty to Strength, Dexterity, and Constitution. Your summoned creatures gain a +4 untyped bonus to Strength, Dexterity, and Constitution.",
                        effectZh: "你的力量、敏捷和体质承受-4无类型减值。你召唤的生物的力量、敏捷和体质获得+4无类型加值。"),
                    selfBonuses: CreateUniformBonuses(-4),
                    summonBonuses: CreateUniformBonuses(4)),
                CreateFeat(
                    internalName: "DoubledBond",
                    featureGuid: Guids.SummonerSacrifice.Feature.DoubledBond,
                    outerBuffGuid: Guids.SummonerSacrifice.OuterBuff.DoubledBond,
                    innerBuffGuid: Guids.SummonerSacrifice.InnerBuff.DoubledBond,
                    nameEn: Common.Text("SummonerSacrifice_DoubledBond.Name", "Eldritch Crucible"),
                    nameZh: Common.Text("SummonerSacrifice_DoubledBond.Name", "双生法炼", true),
                    desc: BuildDescription(
                        modeEn: "1:2 Amplification",
                        modeZh: "倍率谐振",
                        loreEn: Common.Text("SummonerSacrifice_DoubledBond.Lore", "An exacting pact magnifies what you surrender, letting sacrifice feed a companion's strength."),
                        loreZh: Common.Text("SummonerSacrifice_DoubledBond.Lore", "严密的契约放大你付出的代价，让牺牲转为伙伴的力量。", true),
                        effectEn: "You take a -2 untyped penalty to Strength, Dexterity, and Constitution. Your summoned creatures gain a +4 untyped bonus to Strength, Dexterity, and Constitution.",
                        effectZh: "你的力量、敏捷和体质承受-2无类型减值。你召唤的生物的力量、敏捷和体质获得+4无类型加值。"),
                    selfBonuses: CreateUniformBonuses(-2),
                    summonBonuses: CreateUniformBonuses(4)),
                CreateFeat(
                    internalName: "EmpoweredSacrifice",
                    featureGuid: Guids.SummonerSacrifice.Feature.EmpoweredSacrifice,
                    outerBuffGuid: Guids.SummonerSacrifice.OuterBuff.EmpoweredSacrifice,
                    innerBuffGuid: Guids.SummonerSacrifice.InnerBuff.EmpoweredSacrifice,
                    nameEn: Common.Text("SummonerSacrifice_EmpoweredSacrifice.Name", "Tribute of Iron Dominion"),
                    nameZh: Common.Text("SummonerSacrifice_EmpoweredSacrifice.Name", "夺冕化蛮", true),
                    desc: BuildDescription(
                        modeEn: "Focused Trade",
                        modeZh: "极意倾注",
                        loreEn: Common.Text("SummonerSacrifice_EmpoweredSacrifice.Lore", "You yield some commanding presence to give a summoned body greater physical force."),
                        loreZh: Common.Text("SummonerSacrifice_EmpoweredSacrifice.Lore", "你让渡部分统御的气势，为召来的躯体换取更强的筋骨之力。", true),
                        effectEn: "You take a -4 untyped penalty to Charisma. Your summoned creatures gain a +8 untyped bonus to Strength.",
                        effectZh: "你的魅力承受-4无类型减值。你召唤的生物获得+8无类型力量加值。"),
                    selfBonuses: new[] { new StatBonus(StatType.Charisma, -4) },
                    summonBonuses: new[] { new StatBonus(StatType.Strength, 8) }),
            };

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
                .SetIconIfPresent(internalName)
                .AddSummonedUnitBuff();
            AddStatBonuses(innerBuff, summonBonuses);
            var configuredInnerBuff = innerBuff.Configure();

            var outerBuff = BuffConfigurator.New($"{internalName}OuterBuff", outerBuffGuid)
                .SetDisplayName(localizedName)
                .SetDescription(localizedDescription)
                .SetIconIfPresent(internalName)
                .AddOnSpawnBuff(buff: configuredInnerBuff, isInfinity: true);
            AddStatBonuses(outerBuff, selfBonuses);
            var configuredOuterBuff = outerBuff.Configure();

            return FeatSelection.SummonerSacrifice.NewFeat(internalName, featureGuid)
                .SetDisplayName(localizedName)
                .SetDescription(localizedDescription)
                .SetIconIfPresent(internalName)
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
            var bonuses = new StatBonus[PhysicalAttributes.Length];
            for (var i = 0; i < PhysicalAttributes.Length; i++)
            {
                bonuses[i] = new StatBonus(PhysicalAttributes[i], value);
            }

            return bonuses;
        }


        private static (string en, string zh) BuildDescription(
            string modeEn,
            string modeZh,
            string loreEn,
            string loreZh,
            string effectEn,
            string effectZh)
        {
            var en = $"<i>Summoner Sacrifice · {modeEn}</i>\n{loreEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Restrictions:</b> When EnableMutex is enabled, mutually exclusive with other Summoner Sacrifice feats.";
            var zh = $"<i>召唤献祭 · {modeZh}</i>\n{loreZh}\n\n<b>效果：</b>{effectZh}\n\n<b>限制：</b>启用EnableMutex时，与其他“召唤献祭”专长互斥。";
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
