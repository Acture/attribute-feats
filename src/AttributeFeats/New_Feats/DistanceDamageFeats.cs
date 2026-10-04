using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace AttributeFeats.New_Feats
{
    internal static class DistanceDamageFeats
    {
        private const ModifierDescriptor Desc = ModifierDescriptor.None;
        private static readonly Feet AggressorsEdgeMaxDistance = new(10);
        private static readonly Feet MarksmansFocusMinDistanceExclusive = new(29);
        private static readonly Feet OptimalRangeMinDistanceExclusive = new(14);
        private static readonly Feet OptimalRangeMaxDistance = new(25);
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            ConfigureDamageBuff();

            CreateFeat(
                internalName: "AggressorsEdge",
                featureGuid: Guids.DistanceDamage.AggressorsEdge,
                nameEn: Common.Text("DistanceDamage_AggressorsEdge.Name", "Point-Blank Ruin"),
                nameZh: Common.Text("DistanceDamage_AggressorsEdge.Name", "咫尺绝杀", true),
                desc: BuildDescription(
                    rangeLabelEn: "Close Range",
                    rangeLabelZh: "近距爆发",
                    loreEn: Common.Text("DistanceDamage_AggressorsEdge.Lore", "You train to deliver a committed blow in the cramped space inside an opponent's guard."),
                    loreZh: Common.Text("DistanceDamage_AggressorsEdge.Lore", "你磨炼在逼近敌人守势的狭窄空间内全力出手的技巧。", true),
                    effectEn: "When your weapon attack target is within 10 feet, that attack gains a +4 untyped damage bonus.",
                    effectZh: "当你的武器攻击目标在10英尺以内时，该次攻击获得+4无类型伤害加值。"),
                distanceConditions: ConditionsBuilder.New()
                    .DistanceToTarget(AggressorsEdgeMaxDistance, negate: true));

            CreateFeat(
                internalName: "MarksmansFocus",
                featureGuid: Guids.DistanceDamage.MarksmansFocus,
                nameEn: Common.Text("DistanceDamage_MarksmansFocus.Name", "Horizon's Deadeye"),
                nameZh: Common.Text("DistanceDamage_MarksmansFocus.Name", "苍穹神击", true),
                desc: BuildDescription(
                    rangeLabelEn: "Long Range",
                    rangeLabelZh: "远距绝杀",
                    loreEn: Common.Text("DistanceDamage_MarksmansFocus.Lore", "Distance gives you room to read a target's line and settle the weapon before release."),
                    loreZh: Common.Text("DistanceDamage_MarksmansFocus.Lore", "距离为你留出判断目标轨迹的余地，也让兵刃在出手前更加稳定。", true),
                    effectEn: "When your weapon attack target is farther than 29 feet away, that attack gains a +4 untyped damage bonus.",
                    effectZh: "当你的武器攻击目标超过29英尺时，该次攻击获得+4无类型伤害加值。"),
                distanceConditions: ConditionsBuilder.New()
                    .DistanceToTarget(MarksmansFocusMinDistanceExclusive));

            CreateFeat(
                internalName: "OptimalRange",
                featureGuid: Guids.DistanceDamage.OptimalRange,
                nameEn: Common.Text("DistanceDamage_OptimalRange.Name", "Harmonic Cleave"),
                nameZh: Common.Text("DistanceDamage_OptimalRange.Name", "流光截角", true),
                desc: BuildDescription(
                    rangeLabelEn: "Mid Range",
                    rangeLabelZh: "中距定势",
                    loreEn: Common.Text("DistanceDamage_OptimalRange.Lore", "You study the middle ground of an engagement, where spacing lets a weapon do its best work."),
                    loreZh: Common.Text("DistanceDamage_OptimalRange.Lore", "你研究交锋的中间距离，让恰当间隔帮助兵刃发挥所长。", true),
                    effectEn: "When your weapon attack target is farther than 14 feet but no farther than 25 feet away, that attack gains a +4 untyped damage bonus.",
                    effectZh: "当你的武器攻击目标超过14英尺且不超过25英尺时，该次攻击获得+4无类型伤害加值。"),
                distanceConditions: ConditionsBuilder.New()
                    .DistanceToTarget(OptimalRangeMinDistanceExclusive)
                    .DistanceToTarget(OptimalRangeMaxDistance, negate: true));
        }

        private static void ConfigureDamageBuff()
        {
            BuffConfigurator.New("DistanceDamageFlatBonusBuff", Guids.DistanceDamage.Buff.FlatBonus)
                .SetDisplayName(Common.L("DistanceDamage.Buff.Name", "Distance Damage", "距离伤害"))
                .SetDescription(Common.L(
                    "DistanceDamage.Buff.Desc",
                    "Distance Damage is active, granting a +4 untyped bonus to damage for the current weapon attack.",
                    "距离伤害已激活，为当前武器攻击提供+4无类型伤害加值。"))
                .SetIconIfPresent("AggressorsEdge")
                .SetStacking(StackingType.Replace)
                .AddContextStatBonus(StatType.AdditionalDamage, SimpleValue(4), descriptor: Desc)
                .Configure();
        }

        private static BlueprintFeature CreateFeat(
            string internalName,
            string featureGuid,
            string nameEn,
            string nameZh,
            (string en, string zh) desc,
            ConditionsBuilder distanceConditions)
        {
            var applyBuff = ActionsBuilder.New()
                .Conditional(
                    conditions: distanceConditions.Build(),
                    ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Guids.DistanceDamage.Buff.FlatBonus, ContextDuration.Fixed(1), toCaster: true)
                        .Build(),
                    ifFalse: ActionsBuilder.New().Build());

            return FeatSelection.DistanceDamage.NewFeat(internalName, featureGuid)
                .SetDisplayName(Common.L($"DistanceDamage_{internalName}.Name", nameEn, nameZh))
                .SetDescription(Common.L($"DistanceDamage_{internalName}.Desc", desc.en, desc.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(internalName)
                .AddInitiatorAttackWithWeaponTrigger(action: applyBuff, triggerBeforeAttack: true)
                .AddInitiatorAttackWithWeaponTrigger(
                    action: ActionsBuilder.New().RemoveBuff(Guids.DistanceDamage.Buff.FlatBonus, toCaster: false),
                    actionsOnInitiator: true)
                .Configure();
        }

        private static ContextValue SimpleValue(int value)
            => new() { ValueType = ContextValueType.Simple, Value = value };

        private static (string en, string zh) BuildDescription(
            string rangeLabelEn,
            string rangeLabelZh,
            string loreEn,
            string loreZh,
            string effectEn,
            string effectZh)
        {
            var en = $"<i>Distance Damage · {rangeLabelEn}</i>\n{loreEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Restrictions:</b> Distance Damage feats are independent and do not apply any intra-family mutex.";
            var zh = $"<i>距离特化 · {rangeLabelZh}</i>\n{loreZh}\n\n<b>效果：</b>{effectZh}\n\n<b>限制：</b>距离伤害专长各自独立生效，无同类互斥限制。";
            return (en, zh);
        }
    }
}
