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
                nameEn: "Point-Blank Ruin",
                nameZh: "咫尺绝杀",
                desc: BuildDescription(
                    rangeLabelEn: "Close Range",
                    rangeLabelZh: "近距爆发",
                    loreEn: "<i>Crowd the Guard.</i> You hit hardest once you step inside an enemy's reach, crowding their guard and driving your weapon into gaps with suffocating, bone-crushing violence.",
                    loreZh: "<i>近身封喉。</i>欺身步入敌刃中门以内，以贴身挤压封死敌之招架空间。在毫厘咫尺间全力迸发破坏力，刃碎重铠、骨断筋折。",
                    effectEn: "When your weapon attack target is within 10 feet, that attack gains a +4 untyped damage bonus.",
                    effectZh: "当你的武器攻击目标在10英尺以内时，该次攻击获得+4无类型伤害加值。"),
                distanceConditions: ConditionsBuilder.New()
                    .DistanceToTarget(AggressorsEdgeMaxDistance, negate: true));

            CreateFeat(
                internalName: "MarksmansFocus",
                featureGuid: Guids.DistanceDamage.MarksmansFocus,
                nameEn: "Horizon's Deadeye",
                nameZh: "苍穹神击",
                desc: BuildDescription(
                    rangeLabelEn: "Long Range",
                    rangeLabelZh: "远距绝杀",
                    loreEn: "<i>Draw of the Distant String.</i> Distance grants clarity. As space opens, you read wind, drop, and motion with supernatural calm, releasing your projectile on an arc that strikes with catastrophic kinetic force.",
                    loreZh: "<i>长空夺魄。</i>旷阔的视野赐予神识绝对清明。风向、重力与敌踪位移尽在推演之中，箭离弦如流星贯日，在远距终端迸发致命贯穿力。",
                    effectEn: "When your weapon attack target is 30 feet or farther away, that attack gains a +4 untyped damage bonus.",
                    effectZh: "当你的武器攻击目标在30英尺或更远时，该次攻击获得+4无类型伤害加值。"),
                distanceConditions: ConditionsBuilder.New()
                    .DistanceToTarget(MarksmansFocusMinDistanceExclusive));

            CreateFeat(
                internalName: "OptimalRange",
                featureGuid: Guids.DistanceDamage.OptimalRange,
                nameEn: "Harmonic Cleave",
                nameZh: "流光截角",
                desc: BuildDescription(
                    rangeLabelEn: "Mid Range",
                    rangeLabelZh: "中距定势",
                    loreEn: "<i>The Golden Threshold.</i> Combat is measured in zones of maximum leverage. You instinctually maintain the ideal middle band of engagement, where the weapon's centrifugal acceleration and your balance reach their devastating apex.",
                    loreZh: "<i>得机得势。</i>交锋胜负系于杠杆发力的最佳截角。在敌我相距的中距黄金带内，兵刃离心加速与身体重心的协调达到极点，挥砍轰杀如雷霆破空。",
                    effectEn: "When your weapon attack target is between 15 and 25 feet away, that attack gains a +4 untyped damage bonus.",
                    effectZh: "当你的武器攻击目标在15至25英尺之间时，该次攻击获得+4无类型伤害加值。"),
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

            return FeatureConfigurator.New(internalName, featureGuid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"DistanceDamage_{internalName}.Name", nameEn, nameZh))
                .SetDescription(Common.L($"DistanceDamage_{internalName}.Desc", desc.en, desc.zh, tagEncyclopediaEntries: true))
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
