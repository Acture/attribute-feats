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

        private static BuffConfigurator NewBuff(
            string internalName,
            string guid,
            string nameKey,
            string nameEn,
            string nameZh,
            string descKey,
            string descEn,
            string descZh,
            StatType baseStat)
        {
            return BuffConfigurator.New(internalName, guid)
                .SetDisplayName(Common.L(nameKey, nameEn, nameZh))
                .SetDescription(Common.L(descKey, descEn, descZh))
                .SetIconIfPresent(internalName)
                .SetStacking(StackingType.Replace)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(baseStat, ModifierDescriptor.None, AbilityRankType.Default, min: 0))
                .AddRecalculateOnStatChange(stat: baseStat);
        }

        private static FeatureConfigurator NewFeature(
            string internalName,
            string guid,
            string nameKey,
            string nameEn,
            string nameZh,
            string descKey,
            (string en, string zh) desc)
        {
            return FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L(nameKey, nameEn, nameZh))
                .SetDescription(Common.L(descKey, desc.en, desc.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(internalName);
        }

        private static (string en, string zh) BuildConditionalDescription(
            string attrEn,
            string attrZh,
            string loreTextEn,
            string loreTextZh,
            string effectEn,
            string effectZh)
        {
            var en = $"<i>Conditional · {attrEn}</i>\n{loreTextEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Restrictions:</b> None. This feat is independent and stacks with other feats.";
            var zh = $"<i>触发 · {attrZh}</i>\n{loreTextZh}\n\n<b>效果：</b>{effectZh}\n\n<b>限制：</b>无。此专长独立生效，可与其他专长正常叠加。";
            return (en, zh);
        }

        private static BlueprintBuff CreateEndlessResolveBuff()
        {
            return NewBuff(
                    internalName: "EndlessResolveTriggerBuff",
                    guid: Guids.Conditional.TriggerBuff.EndlessResolve,
                    nameKey: "Conditional_EndlessResolve.Buff.Name",
                    nameEn: "Defiance at the Precipice",
                    nameZh: "绝境砥柱",
                    descKey: "Conditional_EndlessResolve.Buff.Desc",
                    descEn: "Defiance at the Precipice is active, adding your Constitution modifier as an untyped bonus to AC and all saving throws.",
                    descZh: "绝境砥柱已激活，将你的体质调整值作为无类型加值附加至防御等级（AC）与所有豁免检定。",
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
                    nameEn: "Ambush of the Viper",
                    nameZh: "封喉首刃",
                    descKey: "Conditional_FirstBlood.Buff.Desc",
                    descEn: "Ambush of the Viper is active, adding your Dexterity modifier as an untyped bonus to attack rolls and automatically confirming your critical threats.",
                    descZh: "封喉首刃已激活，将你的敏捷调整值作为无类型加值附加至攻击检定，并自动确认重击威胁。",
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
                    nameEn: "Oath of Retribution",
                    nameZh: "复仇血誓",
                    descKey: "Conditional_Vendetta.Buff.Desc",
                    descEn: "Oath of Retribution is active, adding your Charisma modifier as an untyped bonus to attack rolls and damage for 3 rounds.",
                    descZh: "复仇血誓已激活，将你的魅力调整值作为无类型加值附加至攻击检定与伤害检定，持续3轮。",
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
                    nameEn: "Crane's Severance",
                    nameZh: "蓄势孤峰",
                    descKey: "Conditional_PatientHunter.Buff.Desc",
                    descEn: "Crane's Severance is active, adding twice your Wisdom modifier as an untyped bonus to damage on your first attack each combat round.",
                    descZh: "蓄势孤峰已激活，在每个战斗轮开始时的首次攻击中获得相当于感知调整值两倍的无类型伤害加值。",
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
                    nameEn: "Gorum's Last Stand",
                    nameZh: "狂神绝唱",
                    descKey: "Conditional_BerserkersLastStand.Buff.Desc",
                    descEn: "Gorum's Last Stand is active, adding your Strength modifier as an untyped bonus to attack rolls and damage while applying an equal untyped penalty to AC.",
                    descZh: "狂神绝唱已激活，将你的力量调整值作为无类型加值附加至攻击检定与伤害检定，同时防御等级（AC）承受等同于力量调整值的无类型减值。",
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
                    nameEn: "Cadence Decoded",
                    nameZh: "阅破机宜",
                    descKey: "Conditional_TacticalReading.Buff.Desc",
                    descEn: "Cadence Decoded is active, adding your Intelligence modifier as an untyped bonus to attack rolls until combat ends.",
                    descZh: "阅破机宜已激活，将你的智力调整值作为无类型加值附加至攻击检定，持续至战斗结束。",
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
                    nameEn: "Defiance at the Precipice",
                    nameZh: "绝境砥柱",
                    descKey: "Conditional_EndlessResolve.Desc",
                    desc: BuildConditionalDescription(
                        "Constitution",
                        "体质",
                        "<i>Mettle of the Dying Boar.</i> When blood blinds your vision and the body totters at death's brink, your marrow ignites with primeval obstinacy. Your bruised bones harden into adamantine against the reaper's scythe.",
                        "<i>困兽之斗。</i>当鲜血迷障双目、身躯摇摇欲坠之时，骨髓深处的蛮荒血性轰然觉醒。伤痕反成重甲，濒危之躯硬撼死神镰刀，化作不倒之坚壁。",
                        "While you are below half health, gain a buff that adds your Constitution modifier as an untyped bonus to AC and all saving throws.",
                        "当生命值低于50%时，获得一个增益状态，将你的体质调整值作为无类型加值附加至防御等级（AC）与所有豁免检定。"))
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
                    nameEn: "Ambush of the Viper",
                    nameZh: "封喉首刃",
                    descKey: "Conditional_FirstBlood.Desc",
                    desc: BuildConditionalDescription(
                        "Dexterity",
                        "敏捷",
                        "<i>The Fatal Opening.</i> In the first heartbeat of engagement, before the enemy has set their footing or adjusted their shield, your weapon strikes with lethal, unhesitating finality.",
                        "<i>机先必杀。</i>双兵方触，战端甫启。乘敌阵未稳、心神仓惶之初，发迅雷之刃直取咽喉要害，一瞬锁定胜局。",
                        "During the first round of combat, gain a 1-round buff that adds your Dexterity modifier as an untyped bonus to attack rolls and automatically confirms your critical threats.",
                        "在战斗的第一轮内，获得持续1轮的增益状态，将你的敏捷调整值作为无类型加值附加至攻击检定，并自动确认重击威胁。"))
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
                    nameEn: "Oath of Retribution",
                    nameZh: "复仇血誓",
                    descKey: "Conditional_Vendetta.Desc",
                    desc: BuildConditionalDescription(
                        "Charisma",
                        "魅力",
                        "<i>A Companion's Requiem.</i> A trusted ally's fall does not break your spirit—it unleashes a holy conflagration. Grief hardens into a merciless vow of vengeance that drives your weapon with apocalyptic fury.",
                        "<i>同袍血祭。</i>知交战殁，不折其志，反激起焚天怒火。悲恸化为诛绝仇寇之血誓，每记挥砍皆携带着为逝者索命的狂怒威能。",
                        "When a nearby ally dies, gain a 3-round buff that adds your Charisma modifier as an untyped bonus to attack rolls and damage.",
                        "当附近的盟友阵亡时，获得持续3轮的增益状态，将你的魅力调整值作为无类型加值附加至攻击检定与伤害检定。"))
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
                    nameEn: "Crane's Severance",
                    nameZh: "蓄势孤峰",
                    descKey: "Conditional_PatientHunter.Desc",
                    desc: BuildConditionalDescription(
                        "Wisdom",
                        "感知",
                        "<i>Stillness Before the Severance.</i> While lesser warriors flail in reckless frenzy, you breathe with the rhythm of the duel. When the inevitable opening aligns, your patient strike descends with crushing karmic weight.",
                        "<i>定海断流。</i>庸夫贪刀乱舞，智者观时待变。于喧嚣中静候因果契合之一瞬，雷霆发轫，一刀斩尽千重因果。",
                        "At the start of each combat round, your first attack gains twice your Wisdom modifier as an untyped bonus to damage.",
                        "在每个战斗轮开始时，你的第一次攻击获得相当于你感知调整值两倍的无类型伤害加值。"))
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
                    nameEn: "Gorum's Last Stand",
                    nameZh: "狂神绝唱",
                    descKey: "Conditional_BerserkersLastStand.Desc",
                    desc: BuildConditionalDescription(
                        "Strength",
                        "力量",
                        "<i>Rage of the Doomed.</i> Staring into the open jaws of the abyss, all thought of self-preservation evaporates. If you are to fall, your dying fury will carve an army's worth of foes into the earth beside you.",
                        "<i>绝境死决。</i>身陷九死一生之绝域，尽焚求生之念。纵命陨当场，亦要以毕生神力抡碎山峦，拖拽千百强敌共葬深渊。",
                        "While you are below 25% health, gain a buff that adds your Strength modifier as an untyped bonus to weapon attack rolls and damage, and applies an untyped penalty to AC equal to your Strength modifier.",
                        "当生命值低于25%时，获得一个增益状态，将你的力量调整值作为无类型加值附加至武器攻击检定与伤害检定，同时防御等级（AC）承受等同于力量调整值的无类型减值。"))
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
                    nameEn: "Cadence Decoded",
                    nameZh: "阅破机宜",
                    descKey: "Conditional_TacticalReading.Desc",
                    desc: BuildConditionalDescription(
                        "Intelligence",
                        "智力",
                        "<i>The Theorem Solved.</i> A single exchange of steel provides all the data your keen intellect needs. Having deciphered the opponent's balance, reach, and habits, every subsequent stroke strikes like an answered equation.",
                        "<i>算无遗策。</i>刃锋初接，已收尽敌势。洞悉其步法转折与破绽宿疾，解构其战术图谱，自此招招命中命门，克敌制胜。",
                        "After your first weapon attack in combat resolves, gain a buff until the end of combat that adds your Intelligence modifier as an untyped bonus to attack rolls.",
                        "在战斗中的首次武器攻击结算后，获得持续至战斗结束的增益状态，将你的智力调整值作为无类型加值附加至攻击检定。"))
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
