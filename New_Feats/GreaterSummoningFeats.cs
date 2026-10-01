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
                    nameEn: "Behemoth's Heritage",
                    nameZh: "比蒙遗脉",
                    desc: BuildDescription(
                        "Strength",
                        "力量",
                        "<i>Predatory Primacy.</i> The raw, untamed savagery of prehistoric behemoths answers your conjuration. Your summoned beasts arrive swollen with colossal sinew, tearing into the vanguard with primeval ferocity.",
                        "<i>荒古蛮性。</i>史前比蒙的洪荒凶性响应你的召唤阵式。降临于现世的灵兽肌骨暴涨，以撕裂山岳的狂暴巨力撕碎当面之敌。",
                        "Your summoned creatures gain an untyped bonus to Strength equal to your Strength modifier.",
                        "你召唤的生物获得等同于你的力量调整值的无类型力量加值。"),
                    baseStat: StatType.Strength,
                    buffedStats: new[] { StatType.Strength }),
                CreateFeat(
                    internalName: "QuickenedPact",
                    featureGuid: Guids.Summon.Feature.QuickenedPact,
                    outerBuffGuid: Guids.Summon.OuterBuff.QuickenedPact,
                    innerBuffGuid: Guids.Summon.InnerBuff.QuickenedPact,
                    nameEn: "Zephyr's Covenant",
                    nameZh: "风灵疾契",
                    desc: BuildDescription(
                        "Dexterity",
                        "敏捷",
                        "<i>Gale-Rider's Shroud.</i> You bind your summons to the capricious currents of the elemental planes of air. Your allies manifest surrounded by swirling updrafts, darting and flanking with supernatural velocity.",
                        "<i>疾风咒契。</i>你将召唤盟约与气元素位面的无羁狂风相缔结。现身的生物周身裹挟风暴旋流，穿梭于刀光剑影间，身如脱兔、迅疾莫测。",
                        "Your summoned creatures gain untyped bonuses to Dexterity and Speed equal to your Dexterity modifier.",
                        "你召唤的生物获得等同于你的敏捷调整值的无类型敏捷与移动速度加值。"),
                    baseStat: StatType.Dexterity,
                    buffedStats: new[] { StatType.Dexterity, StatType.Speed }),
                CreateFeat(
                    internalName: "VitalPact",
                    featureGuid: Guids.Summon.Feature.VitalPact,
                    outerBuffGuid: Guids.Summon.OuterBuff.VitalPact,
                    innerBuffGuid: Guids.Summon.InnerBuff.VitalPact,
                    nameEn: "Titan's Lifespring",
                    nameZh: "巨怪生机",
                    desc: BuildDescription(
                        "Constitution",
                        "体质",
                        "<i>The Indomitable Tether.</i> Your conjurations draw from the boundless vitality of the earth's deep roots. Called creatures possess thick, fibrous hides and unflagging endurance, remaining standing through apocalyptic onslaughts.",
                        "<i>大地生机。</i>你的通灵印记自地脉深处的生命泉眼汲取活力。受召之物皮糙肉厚、耐力无穷，纵受狂轰滥炸亦能昂然伫立。",
                        "Your summoned creatures gain an untyped bonus to Constitution equal to your Constitution modifier.",
                        "你召唤的生物获得等同于你的体质调整值的无类型体质加值。"),
                    baseStat: StatType.Constitution,
                    buffedStats: new[] { StatType.Constitution }),
                CreateFeat(
                    internalName: "TacticalBinding",
                    featureGuid: Guids.Summon.Feature.TacticalBinding,
                    outerBuffGuid: Guids.Summon.OuterBuff.TacticalBinding,
                    innerBuffGuid: Guids.Summon.InnerBuff.TacticalBinding,
                    nameEn: "Aegis of the Schema",
                    nameZh: "天元魔阵",
                    desc: BuildDescription(
                        "Intelligence",
                        "智力",
                        "<i>Geometric Abjuration.</i> Exact planar coordinates and rigorous arcane geometries envelop your summons in interlocking kinetic wards. Foes find their blows deflected by visible mathematical equations.",
                        "<i>天元轨则。</i>以严谨至极的位面坐标与几何符阵构筑召唤回路。受召生物周身覆以流转的几何力场，使敌手的刃锋沿着折射偏角滑开。",
                        "Your summoned creatures gain an untyped bonus to AC equal to your Intelligence modifier.",
                        "你召唤的生物获得等同于你的智力调整值的无类型防御等级（AC）加值。"),
                    baseStat: StatType.Intelligence,
                    buffedStats: new[] { StatType.AC }),
                CreateFeat(
                    internalName: "InsightfulSummons",
                    featureGuid: Guids.Summon.Feature.InsightfulSummons,
                    outerBuffGuid: Guids.Summon.OuterBuff.InsightfulSummons,
                    innerBuffGuid: Guids.Summon.InnerBuff.InsightfulSummons,
                    nameEn: "Empathic Communion",
                    nameZh: "神契灵犀",
                    desc: BuildDescription(
                        "Wisdom",
                        "感知",
                        "<i>Shared Awareness.</i> A quiet, transcendent thread connects your spiritual awareness to the minds of your servants. Forewarned by your third eye, they sidestep spells and shrug off curses as if sharing your foresight.",
                        "<i>心印相通。</i>以神识灵犀为纽带，将超然直觉投射于受召生物心窍。它们如获先知之眼，在法术呼啸与诅咒降临前敏锐躲避、心如止水。",
                        "Your summoned creatures gain untyped bonuses to Fortitude, Reflex, and Will saves equal to your Wisdom modifier.",
                        "你召唤的生物获得等同于你的感知调整值的强韧、反射与意志豁免检定无类型加值。"),
                    baseStat: StatType.Wisdom,
                    buffedStats: new[] { StatType.SaveFortitude, StatType.SaveReflex, StatType.SaveWill }),
                CreateFeat(
                    internalName: "MagneticCalling",
                    featureGuid: Guids.Summon.Feature.MagneticCalling,
                    outerBuffGuid: Guids.Summon.OuterBuff.MagneticCalling,
                    innerBuffGuid: Guids.Summon.InnerBuff.MagneticCalling,
                    nameEn: "Dominator's Calling",
                    nameZh: "御统王令",
                    desc: BuildDescription(
                        "Charisma",
                        "魅力",
                        "<i>Imperial Decree.</i> Your conjuration is no mere plea across planar boundaries, but an absolute mandate. Infused with your indomitable majesty, your creatures strike with fearless ferocity and sovereign intent.",
                        "<i>帝令敕召。</i>你的召唤非是祈求位面生灵的援手，而是降下无可违抗的君王敕令。受令而来的异界大军沐浴皇威，舍生忘死、击无不克。",
                        "Your summoned creatures gain an untyped bonus to attack rolls equal to your Charisma modifier.",
                        "你召唤的生物获得等同于你的魅力调整值的无类型攻击检定加值。"),
                    baseStat: StatType.Charisma,
                    buffedStats: new[] { StatType.AdditionalAttackBonus }),
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
            StatType baseStat,
            IReadOnlyList<StatType> buffedStats)
        {
            var displayName = Common.L($"Summon_{internalName}.Name", nameEn, nameZh);
            var description = Common.L(
                $"Summon_{internalName}.Desc",
                desc.en,
                desc.zh,
                tagEncyclopediaEntries: true);

            var innerBuff = BuffConfigurator.New($"{internalName}InnerBuff", innerBuffGuid)
                .SetDisplayName(displayName)
                .SetDescription(description)
                .SetIconIfPresent(internalName);
            AddRank(innerBuff, baseStat);
            AddContextBonuses(innerBuff, buffedStats);
            var configuredInnerBuff = innerBuff.Configure();

            var outerBuff = BuffConfigurator.New($"{internalName}OuterBuff", outerBuffGuid)
                .SetDisplayName(displayName)
                .SetDescription(description)
                .SetIconIfPresent(internalName)
                .AddOnSpawnBuff(buff: configuredInnerBuff, isInfinity: true)
                .Configure();

            return FeatureConfigurator.New(internalName, featureGuid, FeatureGroup.Feat)
                .SetDisplayName(displayName)
                .SetDescription(description)
                .SetIconIfPresent(internalName)
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

        private static (string en, string zh) BuildDescription(
            string attrEn,
            string attrZh,
            string loreEn,
            string loreZh,
            string effectEn,
            string effectZh)
        {
            var en = $"<i>Greater Summoning · {attrEn}</i>\n{loreEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Restrictions:</b> Mutually exclusive with other Greater Summoning feats.";
            var zh = $"<i>高等召唤 · {attrZh}</i>\n{loreZh}\n\n<b>效果：</b>{effectZh}\n\n<b>限制：</b>与其他“高等召唤”专长互斥。";
            return (en, zh);
        }
    }
}
