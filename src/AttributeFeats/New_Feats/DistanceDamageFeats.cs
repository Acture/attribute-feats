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
                displayName: "Aggressor's Edge",
                description: BuildDescription(
                    rangeLabel: "Close Range",
                    loreTitle: "Crowd the Guard.",
                    loreBody: "You hit hardest once you step inside hesitation and force the fight into immediate reach.",
                    effectText: "When your weapon attack target is within 10 feet, that attack gains a +4 untyped damage bonus."),
                distanceConditions: ConditionsBuilder.New()
                    .DistanceToTarget(AggressorsEdgeMaxDistance, negate: true));

            CreateFeat(
                internalName: "MarksmansFocus",
                featureGuid: Guids.DistanceDamage.MarksmansFocus,
                displayName: "Marksman's Focus",
                description: BuildDescription(
                    rangeLabel: "Long Range",
                    loreTitle: "Set the Line.",
                    loreBody: "You settle into the shot as the space opens, turning measured distance into cleaner impact.",
                    effectText: "When your weapon attack target is 30 feet or farther away, that attack gains a +4 untyped damage bonus."),
                distanceConditions: ConditionsBuilder.New()
                    .DistanceToTarget(MarksmansFocusMinDistanceExclusive));

            CreateFeat(
                internalName: "OptimalRange",
                featureGuid: Guids.DistanceDamage.OptimalRange,
                displayName: "Optimal Range",
                description: BuildDescription(
                    rangeLabel: "Mid Range",
                    loreTitle: "Right Where It Matters.",
                    loreBody: "You know the band of distance where timing, angle, and pressure align into the cleanest hit.",
                    effectText: "When your weapon attack target is between 15 and 25 feet away, that attack gains a +4 untyped damage bonus."),
                distanceConditions: ConditionsBuilder.New()
                    .DistanceToTarget(OptimalRangeMinDistanceExclusive)
                    .DistanceToTarget(OptimalRangeMaxDistance, negate: true));
        }

        private static void ConfigureDamageBuff()
        {
            BuffConfigurator.New("DistanceDamageFlatBonusBuff", Guids.DistanceDamage.Buff.FlatBonus)
                .SetDisplayName(Common.L("DistanceDamage.Buff.Name", "Distance Damage"))
                .SetDescription(Common.L("DistanceDamage.Buff.Desc", "Distance Damage is active, granting a +4 untyped bonus to damage for the current weapon attack."))
                .SetStacking(StackingType.Replace)
                .AddContextStatBonus(StatType.AdditionalDamage, SimpleValue(4), descriptor: Desc)
                .Configure();
        }

        private static BlueprintFeature CreateFeat(
            string internalName,
            string featureGuid,
            string displayName,
            string description,
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
                .SetDisplayName(Common.L($"DistanceDamage_{internalName}.Name", displayName))
                .SetDescription(Common.L($"DistanceDamage_{internalName}.Desc", description, tagEncyclopediaEntries: true))
                .AddInitiatorAttackWithWeaponTrigger(action: applyBuff, triggerBeforeAttack: true)
                .AddInitiatorAttackWithWeaponTrigger(
                    action: ActionsBuilder.New().RemoveBuff(Guids.DistanceDamage.Buff.FlatBonus, toCaster: false),
                    actionsOnInitiator: true)
                .Configure();
        }

        private static ContextValue SimpleValue(int value)
            => new() { ValueType = ContextValueType.Simple, Value = value };

        private static string BuildDescription(string rangeLabel, string loreTitle, string loreBody, string effectText)
            => $"<i>Distance Damage · {rangeLabel}</i>\n<i>{loreTitle}</i> {loreBody}\n\n<b>Effect:</b> {effectText}\n\n<b>Restrictions:</b> Distance Damage feats are independent and do not apply any intra-family mutex.";
    }
}
