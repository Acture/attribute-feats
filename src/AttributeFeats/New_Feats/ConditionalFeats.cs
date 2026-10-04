using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;

namespace AttributeFeats.New_Feats
{
    internal static class ConditionalFeats
    {
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var endlessResolveBuff = CreateEndlessResolveBuff();
            var firstBloodBuff = CreateFirstBloodBuff();
            var vendettaBuff = CreateVendettaBuff();
            var patientHunterBuff = CreatePatientHunterBuff();
            var berserkersLastStandBuff = CreateBerserkersLastStandBuff();
            var tacticalReadingBuff = CreateTacticalReadingBuff();

            CreateEndlessResolve(endlessResolveBuff);
            CreateFirstBlood(firstBloodBuff);
            CreateVendetta(vendettaBuff);
            CreatePatientHunter(patientHunterBuff);
            CreateBerserkersLastStand(berserkersLastStandBuff);
            CreateTacticalReading(tacticalReadingBuff);
        }

        private static ContextValue SimpleValue(int value)
            => new() { ValueType = ContextValueType.Simple, Value = value };

        private static ActionsBuilder ApplySelfBuff(BlueprintBuff buff, int rounds)
            => ActionsBuilder.New().ApplyBuff(buff.ToReference<BlueprintBuffReference>(), ContextDuration.Fixed(rounds), toCaster: true);

        private static ActionsBuilder ApplySelfBuff(string buffGuid, int duration, DurationRate rate)
            => ActionsBuilder.New().ApplyBuff(buffGuid, ContextDuration.Fixed(duration, rate), toCaster: true);

        private static ActionsBuilder ApplySelfBuffIfMissing(string buffGuid, int duration, DurationRate rate)
            => ActionsBuilder.New().Conditional(
                conditions: ConditionsBuilder.New().CasterHasFact(buffGuid, negate: true).Build(),
                ifTrue: ApplySelfBuff(buffGuid, duration, rate).Build(),
                ifFalse: ActionsBuilder.New().Build());

        private static ActionsBuilder RemoveSelfBuff(BlueprintBuff buff)
            => ActionsBuilder.New().RemoveBuff(buff.ToReference<BlueprintBuffReference>(), toCaster: true);

        private static ActionsBuilder RemoveSelfBuff(string buffGuid)
            => ActionsBuilder.New().RemoveBuff(BlueprintTool.GetRef<BlueprintBuffReference>(buffGuid), toCaster: true);

        private static BuffConfigurator NewBuff(
            string internalName,
            string guid,
            string nameKey,
            string nameValue,
            string descKey,
            string descValue,
            StatType baseStat)
        {
            return BuffConfigurator.New(internalName, guid)
                .SetDisplayName(Common.L(nameKey, nameValue))
                .SetDescription(Common.L(descKey, descValue))
                .SetStacking(StackingType.Replace)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(baseStat, ModifierDescriptor.None, AbilityRankType.Default, min: 0))
                .AddRecalculateOnStatChange(stat: baseStat);
        }

        private static FeatureConfigurator NewFeature(
            string internalName,
            string guid,
            string nameKey,
            string nameValue,
            string descKey,
            string descValue)
        {
            return FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L(nameKey, nameValue))
                .SetDescription(Common.L(descKey, descValue, tagEncyclopediaEntries: true));
        }

        private static BlueprintBuff CreateEndlessResolveBuff()
        {
            return NewBuff(
                    internalName: "EndlessResolveTriggerBuff",
                    guid: Guids.Conditional.TriggerBuff.EndlessResolve,
                    nameKey: "Conditional_EndlessResolve.Buff.Name",
                    nameValue: "Endless Resolve",
                    descKey: "Conditional_EndlessResolve.Buff.Desc",
                    descValue: "Endless Resolve is active, adding your Constitution modifier as an untyped bonus to AC and all saving throws.",
                    baseStat: StatType.Constitution)
                .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddContextStatBonus(StatType.SaveFortitude, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddContextStatBonus(StatType.SaveReflex, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddContextStatBonus(StatType.SaveWill, Common.Rank(), descriptor: ModifierDescriptor.None)
                .Configure();
        }

        private static BlueprintBuff CreateFirstBloodBuff()
        {
            return NewBuff(
                    internalName: "FirstBloodTriggerBuff",
                    guid: Guids.Conditional.TriggerBuff.FirstBlood,
                    nameKey: "Conditional_FirstBlood.Buff.Name",
                    nameValue: "First Blood",
                    descKey: "Conditional_FirstBlood.Buff.Desc",
                    descValue: "First Blood is active, adding your Dexterity modifier as an untyped bonus to attack rolls and automatically confirming your critical threats.",
                    baseStat: StatType.Dexterity)
                .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddInitiatorCritAutoconfirm()
                .Configure();
        }

        private static BlueprintBuff CreateVendettaBuff()
        {
            return NewBuff(
                    internalName: "VendettaTriggerBuff",
                    guid: Guids.Conditional.TriggerBuff.Vendetta,
                    nameKey: "Conditional_Vendetta.Buff.Name",
                    nameValue: "Vendetta",
                    descKey: "Conditional_Vendetta.Buff.Desc",
                    descValue: "Vendetta is active, adding your Charisma modifier as an untyped bonus to attack rolls and damage for 3 rounds.",
                    baseStat: StatType.Charisma)
                .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: ModifierDescriptor.None)
                .Configure();
        }

        private static BlueprintBuff CreatePatientHunterBuff()
        {
            return NewBuff(
                    internalName: "PatientHunterTriggerBuff",
                    guid: Guids.Conditional.TriggerBuff.PatientHunter,
                    nameKey: "Conditional_PatientHunter.Buff.Name",
                    nameValue: "Patient Hunter",
                    descKey: "Conditional_PatientHunter.Buff.Desc",
                    descValue: "Patient Hunter is active, adding twice your Wisdom modifier as an untyped bonus to damage on your first attack each combat round.",
                    baseStat: StatType.Wisdom)
                .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddInitiatorAttackWithWeaponTrigger(action: RemoveSelfBuff(Guids.Conditional.TriggerBuff.PatientHunter), actionsOnInitiator: true)
                .Configure();
        }

        private static BlueprintBuff CreateBerserkersLastStandBuff()
        {
            return NewBuff(
                    internalName: "BerserkersLastStandTriggerBuff",
                    guid: Guids.Conditional2.TriggerBuff.BerserkersLastStand,
                    nameKey: "Conditional_BerserkersLastStand.Buff.Name",
                    nameValue: "Berserker's Last Stand",
                    descKey: "Conditional_BerserkersLastStand.Buff.Desc",
                    descValue: "Berserker's Last Stand is active, adding your Strength modifier as an untyped bonus to attack rolls and damage while applying an equal untyped penalty to AC.",
                    baseStat: StatType.Strength)
                .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: ModifierDescriptor.None)
                .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: ModifierDescriptor.None, multiplier: -1)
                .Configure();
        }

        private static BlueprintBuff CreateTacticalReadingBuff()
        {
            return NewBuff(
                    internalName: "TacticalReadingTriggerBuff",
                    guid: Guids.Conditional2.TriggerBuff.TacticalReading,
                    nameKey: "Conditional_TacticalReading.Buff.Name",
                    nameValue: "Tactical Reading",
                    descKey: "Conditional_TacticalReading.Buff.Desc",
                    descValue: "Tactical Reading is active, adding your Intelligence modifier as an untyped bonus to attack rolls until combat ends.",
                    baseStat: StatType.Intelligence)
                .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: ModifierDescriptor.None)
                .Configure();
        }

        private static BlueprintFeature CreateEndlessResolve(BlueprintBuff buff)
        {
            return NewFeature(
                    internalName: "EndlessResolve",
                    guid: Guids.Conditional.EndlessResolve,
                    nameKey: "Conditional_EndlessResolve.Name",
                    nameValue: "Endless Resolve",
                    descKey: "Conditional_EndlessResolve.Desc",
                    descValue: "<i>Conditional · Constitution</i>\n<i>Iron at the Brink.</i> Even as your body buckles, stubborn vitality hardens into a final reserve of defense.\n\n<b>Effect:</b> While you are below half health, gain a buff that adds your Constitution modifier as an untyped bonus to AC and all saving throws.\n\n<b>Restrictions:</b> Mutually exclusive with the Constitution Main Attribute Mastery feat.")
                .AddBuffOnHealthTickingTrigger(healthPercent: 0.5f, triggeredBuff: buff.ToReference<BlueprintBuffReference>())
                .AddRecalculateOnStatChange(stat: StatType.Constitution)
                .Configure();
        }

        private static BlueprintFeature CreateFirstBlood(BlueprintBuff buff)
        {
            return NewFeature(
                    internalName: "FirstBlood",
                    guid: Guids.Conditional.FirstBlood,
                    nameKey: "Conditional_FirstBlood.Name",
                    nameValue: "First Blood",
                    descKey: "Conditional_FirstBlood.Desc",
                    descValue: "<i>Conditional · Dexterity</i>\n<i>Opening Cut.</i> The first heartbeat of battle belongs to the swift, when precision lands before hesitation can answer.\n\n<b>Effect:</b> During the first round of combat, gain a 1-round buff that adds your Dexterity modifier as an untyped bonus to attack rolls and automatically confirms your critical threats.\n\n<b>Restrictions:</b> Mutually exclusive with the Dexterity Main Attribute Mastery feat.")
                .AddCombatStateTrigger(
                    combatStartActions: ApplySelfBuff(buff, 1),
                    combatEndActions: RemoveSelfBuff(buff))
                .Configure();
        }

        private static BlueprintFeature CreateVendetta(BlueprintBuff buff)
        {
            return NewFeature(
                    internalName: "Vendetta",
                    guid: Guids.Conditional.Vendetta,
                    nameKey: "Conditional_Vendetta.Name",
                    nameValue: "Vendetta",
                    descKey: "Conditional_Vendetta.Desc",
                    descValue: "<i>Conditional · Charisma</i>\n<i>Wrath Made Manifest.</i> Another's fall sharpens your presence into a terrible promise, turning grief into a vow your enemies cannot ignore.\n\n<b>Effect:</b> When a nearby ally dies, gain a 3-round buff that adds your Charisma modifier as an untyped bonus to attack rolls and damage.\n\n<b>Restrictions:</b> Mutually exclusive with the Charisma Main Attribute Mastery feat.")
                .AddUnitDeathTrigger(
                    actions: ApplySelfBuff(buff, 3),
                    deathTrigger: UnitDeathTrigger.DeathTrigger.OnUnitDeath,
                    faction: UnitDeathTrigger.FactionType.Ally,
                    radiusInMeters: SimpleValue(30),
                    withUnconsciousLifeState: false)
                .Configure();
        }

        private static BlueprintFeature CreatePatientHunter(BlueprintBuff buff)
        {
            return NewFeature(
                    internalName: "PatientHunter",
                    guid: Guids.Conditional.PatientHunter,
                    nameKey: "Conditional_PatientHunter.Name",
                    nameValue: "Patient Hunter",
                    descKey: "Conditional_PatientHunter.Desc",
                    descValue: "<i>Conditional · Wisdom</i>\n<i>Stillness Before the Strike.</i> Patience lets you read the battle's rhythm, storing every quiet heartbeat for the blow that finally matters.\n\n<b>Effect:</b> At the start of each combat round, your first attack gains twice your Wisdom modifier as an untyped bonus to damage.\n\n<b>Restrictions:</b> Mutually exclusive with the Wisdom Main Attribute Mastery feat.")
                .AddCombatStateTrigger(
                    combatStartActions: ApplySelfBuff(buff, 1),
                    combatEndActions: RemoveSelfBuff(buff))
                .AddNewRoundTrigger(newRoundActions: ApplySelfBuff(buff, 1))
                .Configure();
        }

        private static BlueprintFeature CreateBerserkersLastStand(BlueprintBuff buff)
        {
            return NewFeature(
                    internalName: "BerserkersLastStand",
                    guid: Guids.Conditional2.BerserkersLastStand,
                    nameKey: "Conditional_BerserkersLastStand.Name",
                    nameValue: "Berserker's Last Stand",
                    descKey: "Conditional_BerserkersLastStand.Desc",
                    descValue: "<i>Conditional · Strength</i>\n<i>Rage at the Brink.</i> With death close enough to taste, survival becomes a brutal bargain: strike harder, stand looser, and trust fury more than caution.\n\n<b>Effect:</b> While you are below 25% health, gain a buff that adds your Strength modifier as an untyped bonus to weapon attack rolls and damage, and applies an untyped penalty to AC equal to your Strength modifier.\n\n<b>Restrictions:</b> Mutually exclusive with the Strength Main Attribute Mastery feat.")
                .AddBuffOnHealthTickingTrigger(healthPercent: 0.25f, triggeredBuff: buff.ToReference<BlueprintBuffReference>())
                .AddRecalculateOnStatChange(stat: StatType.Strength)
                .Configure();
        }

        private static BlueprintFeature CreateTacticalReading(BlueprintBuff buff)
        {
            return NewFeature(
                    internalName: "TacticalReading",
                    guid: Guids.Conditional2.TacticalReading,
                    nameKey: "Conditional_TacticalReading.Name",
                    nameValue: "Tactical Reading",
                    descKey: "Conditional_TacticalReading.Desc",
                    descValue: "<i>Conditional · Intelligence</i>\n<i>Pattern Secured.</i> One exchange is enough to reveal the fight's hidden logic, and once the pattern is solved your weapon follows conclusions instead of guesses.\n\n<b>Effect:</b> After your first weapon attack in combat resolves, gain a buff until the end of combat that adds your Intelligence modifier as an untyped bonus to attack rolls.\n\n<b>Restrictions:</b> Mutually exclusive with the Intelligence Main Attribute Mastery feat.")
                .AddCombatStateTrigger(
                    combatStartActions: RemoveSelfBuff(buff),
                    combatEndActions: RemoveSelfBuff(buff))
                .AddInitiatorAttackWithWeaponTrigger(
                    action: ApplySelfBuffIfMissing(Guids.Conditional2.TriggerBuff.TacticalReading, 10, DurationRate.Minutes),
                    actionsOnInitiator: true,
                    triggerBeforeAttack: false)
                .Configure();
        }
    }
}
