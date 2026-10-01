using System;
using System.Collections.Generic;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace AttributeFeats.New_Feats
{
    internal static class StanceFeats
    {
        private const ModifierDescriptor Desc = ModifierDescriptor.None;
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var feats = new List<BlueprintFeature>
            {
                CreateStance(
                    baseStat: StatType.Strength,
                    internalName: "BrutalStance",
                    featureGuid: Guids.Stance.Feature.Str,
                    buffGuid: Guids.Stance.Buff.Str,
                    activatableGuid: Guids.Stance.Activatable.Str,
                    keyPrefix: "Stance_Str",
                    displayNameEn: "Berserker's Overrun",
                    displayNameZh: "破阵裂山势",
                    description: BuildDescription(
                        "Strength",
                        "力量",
                        "Gorum's Reckless Abandon.",
                        "In the bloody ethos of the Iron God, armor is a craven distraction. Dropping all defense, you throw the entirety of your body weight and reckless fury into unstoppable, earth-cleaving assaults.",
                        "狂神破阵。",
                        "遵奉铁甲战神之霸道信条：守御乃战阵懦夫之伪饰。尽弃铠甲之护，将全副身量与嗜血狂意倾注于每一次挥击之上，每击皆带裂山荡寇之威。",
                        "While active, adds your Strength modifier (untyped) to attack rolls and damage. You suffer a penalty equal to your Strength modifier to AC.",
                        "激活时，将你的力量调整值（无类型加值）附加至攻击检定与伤害检定。你的防御等级（AC）承受等同于力量调整值的减值。"),
                    configureBuff: buff => buff
                        .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc, multiplier: -1)),
                CreateStance(
                    baseStat: StatType.Dexterity,
                    internalName: "LiquidForm",
                    featureGuid: Guids.Stance.Feature.Dex,
                    buffGuid: Guids.Stance.Buff.Dex,
                    activatableGuid: Guids.Stance.Activatable.Dex,
                    keyPrefix: "Stance_Dex",
                    displayNameEn: "Willow in the Gale",
                    displayNameZh: "惊鸿穿林势",
                    description: BuildDescription(
                        "Dexterity",
                        "敏捷",
                        "Sinuous Evasion.",
                        "Yielding like willow branches before an axe, you twist and skim through the air. Blades find only empty space, though prioritizing total avoidance leaves little momentum for counter-attacks.",
                        "惊鸿避刃。",
                        "如狂风过隙中的柔韧杨柳，身如惊鸿，穿花掠影。任凭敌刃狂澜倾泻，唯求不沾微尘，虽令反击招架暂失先手，却教强敌连连空挥。",
                        "While active, adds your Dexterity modifier (untyped) to AC and Reflex saves. You suffer a penalty equal to your Dexterity modifier to attack rolls.",
                        "激活时，将你的敏捷调整值（无类型加值）附加至防御等级（AC）与反射豁免。你的攻击检定承受等同于敏捷调整值的减值。"),
                    configureBuff: buff => buff
                        .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.SaveReflex, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc, multiplier: -1)),
                CreateStance(
                    baseStat: StatType.Constitution,
                    internalName: "EndlessVigor",
                    featureGuid: Guids.Stance.Feature.Con,
                    buffGuid: Guids.Stance.Buff.Con,
                    activatableGuid: Guids.Stance.Activatable.Con,
                    keyPrefix: "Stance_Con",
                    displayNameEn: "Mountain's Deep Roots",
                    displayNameZh: "不动磐峰势",
                    description: BuildDescription(
                        "Constitution",
                        "体质",
                        "Anchor of the Earth.",
                        "Planting your heels into subterranean stone and drawing deep grounding breaths, you turn your body into an immovable monolith of endurance, absorbing battering blows while sacrificing reach and tempo.",
                        "磐岳归根。",
                        "含胸拔背，气沉丹田，双足若古松生根深扎厚土。化作不动之肉身峰峦，源源吞纳重击摧折，固若金汤却不逞口舌攻伐。",
                        "While active, you gain temporary hit points equal to your Constitution modifier. Your attack rolls and Attack of Opportunity count are reduced by your Constitution modifier.",
                        "激活时，获得等同于体质调整值的临时生命值。你的攻击检定与借机攻击次数承受等同于体质调整值的减值。"),
                    configureBuff: buff =>
                    {
                        return buff
                            .AddTemporaryHitPointsFromAbilityValue(descriptor: Desc, removeWhenHitPointsEnd: false, value: Common.Rank())
                            .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc, multiplier: -1)
                            .AddContextStatBonus(StatType.AttackOfOpportunityCount, Common.Rank(), descriptor: Desc, multiplier: -1);
                    }),
                CreateStance(
                    baseStat: StatType.Intelligence,
                    internalName: "TacticalMind",
                    featureGuid: Guids.Stance.Feature.Int,
                    buffGuid: Guids.Stance.Buff.Int,
                    activatableGuid: Guids.Stance.Activatable.Int,
                    keyPrefix: "Stance_Int",
                    displayNameEn: "Grandmaster's Gambit",
                    displayNameZh: "弈者静待势",
                    description: BuildDescription(
                        "Intelligence",
                        "智力",
                        "The Waiting Blade.",
                        "Like a grandmaster studying a chessboard, you hold your weapon in poised repose, refusing to initiate until an enemy overextends into the exact trap you have orchestrated.",
                        "弈者断局。",
                        "若国手对弈，横剑藏锋而隐忍不发。任凭强敌喧嚣，唯俟其步伐失序、露出一瞬破绽，立时以寒光霆击断其胜局。",
                        "While active, adds your Intelligence modifier (untyped) to attacks of opportunity and to your Attack of Opportunity count. You suffer a penalty equal to your Intelligence modifier to attack rolls that are not attacks of opportunity.",
                        "激活时，将你的智力调整值（无类型加值）附加至借机攻击的攻击检定与借机攻击次数。非借机攻击的普通攻击检定承受等同于智力调整值的减值。"),
                    includeNegativeRank: true,
                    configureBuff: buff => buff
                        .AddAttackOfOpportunityAttackBonus(Common.Rank(), descriptor: Desc, notAttackOfOpportunity: false)
                        .AddAttackOfOpportunityAttackBonus(Common.Rank(AbilityRankType.StatBonus), descriptor: Desc, notAttackOfOpportunity: true)
                        .AddContextStatBonus(StatType.AttackOfOpportunityCount, Common.Rank(), descriptor: Desc)),
                CreateStance(
                    baseStat: StatType.Wisdom,
                    internalName: "CenteredMind",
                    featureGuid: Guids.Stance.Feature.Wis,
                    buffGuid: Guids.Stance.Buff.Wis,
                    activatableGuid: Guids.Stance.Activatable.Wis,
                    keyPrefix: "Stance_Wis",
                    displayNameEn: "Mirror of Still Waters",
                    displayNameZh: "明镜止水势",
                    description: BuildDescription(
                        "Wisdom",
                        "感知",
                        "Void of the Ascetic.",
                        "Retreating into a state of absolute spiritual equilibrium, the battlefield reflects upon your surface without creating a ripple. Defenses become impregnable, though violence holds no charm for you.",
                        "澄澈自照。",
                        "收敛神识入于虚空明镜之中。战尘喧扰皆过眼云烟，百邪莫侵、万法不破，于极致清宁间消弭兵戈戾气。",
                        "While active, adds your Wisdom modifier (untyped) to AC and all saving throws. You suffer a penalty equal to your Wisdom modifier to attack rolls and damage.",
                        "激活时，将你的感知调整值（无类型加值）附加至防御等级（AC）与所有豁免检定。你的攻击检定与伤害检定承受等同于感知调整值的减值。"),
                    configureBuff: buff => buff
                        .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.SaveFortitude, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.SaveReflex, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.SaveWill, Common.Rank(), descriptor: Desc)
                        .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc, multiplier: -1)
                        .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: Desc, multiplier: -1)),
                CreateStance(
                    baseStat: StatType.Charisma,
                    internalName: "CommandingPresence",
                    featureGuid: Guids.Stance.Feature.Cha,
                    buffGuid: Guids.Stance.Buff.Cha,
                    activatableGuid: Guids.Stance.Activatable.Cha,
                    keyPrefix: "Stance_Cha",
                    displayNameEn: "Vanguard's Banner",
                    displayNameZh: "金戈铁旌势",
                    description: BuildDescription(
                        "Charisma",
                        "魅力",
                        "Warlord's Rallying Banner.",
                        "Exposing yourself fearlessly at the front of the battle line, your radiant presence and ringing war-cries banish dread from your companions' hearts, elevating their swords with the certainty of triumph.",
                        "铁旌号令。",
                        "挺身屹立于两军锋矢交错之处，铠光耀目，叱喝惊雷。舍身立威以定军心，令三十步内同袍热血沸腾，剑锋所指无坚不摧。",
                        "While active, allies within 30 feet gain an untyped bonus to attack rolls equal to your Charisma modifier. You suffer a penalty equal to your Charisma modifier to AC.",
                        "激活时，30尺内的所有盟友在攻击检定上获得等同于你的魅力调整值的无类型加值。你的防御等级（AC）承受等同于魅力调整值的减值。"),
                    configureBuff: buff =>
                    {
                        var allyBuff = BuffConfigurator.New("CommandingPresenceAllyBuff", Guids.Stance.AllyBuff.CommandingPresence)
                            .SetDisplayName(Common.L("Stance_Cha.AllyBuff.Name", "Vanguard's Banner", "金戈铁旌势"))
                            .SetDescription(Common.L(
                                "Stance_Cha.AllyBuff.Desc",
                                "<i>Stance · Charisma</i>\n<i>The Warlord's Standard.</i> Your ally fights under the inspiring canopy of your sovereign courage.\n\n<b>Effect:</b> Grants an untyped bonus to attack rolls equal to the user's Charisma modifier while within 30 feet.",
                                "<i>姿态 · 魅力</i>\n<i>百战铁旌。</i>沐浴在战帅无畏勇烈之光辉下，盟军同仇敌忾。\n\n<b>效果：</b>在30尺内，攻击检定获得等同于专长持有者魅力调整值的无类型加值。",
                                tagEncyclopediaEntries: true));
                        AddRanks(allyBuff, StatType.Charisma, includeNegativeRank: false);
                        var configuredAllyBuff = allyBuff
                            .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: Desc)
                            .AddRecalculateOnStatChange(stat: StatType.Charisma)
                            .Configure();

                        var areaEffect = AbilityAreaEffectConfigurator.New("CommandingPresenceArea", Guids.Stance.AreaEffect.CommandingPresence)
                            .SetAffectEnemies(false)
                            .SetAffectDead(false)
                            .SetShape(AreaEffectShape.Cylinder)
                            .SetSize(new Feet(30))
                            .AddAbilityAreaEffectBuff(
                                configuredAllyBuff,
                                checkConditionEveryRound: true,
                                condition: ConditionsBuilder.New().IsAlly().IsCaster(negate: true))
                            .Configure();

                        return buff
                            .AddAreaEffect(areaEffect)
                            .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc, multiplier: -1);
                    }),
            };

            ApplyIntraFamilyMutex(feats);
        }

        private static (string en, string zh) BuildDescription(
            string attrEn,
            string attrZh,
            string loreTitleEn,
            string loreBodyEn,
            string loreTitleZh,
            string loreBodyZh,
            string effectEn,
            string effectZh)
        {
            var en = $"<i>Stance · {attrEn}</i>\n<i>{loreTitleEn}</i> {loreBodyEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Activation:</b> Free action to toggle. Only one Stance feat may be active at a time.\n\n<b>Restrictions:</b> Mutually exclusive with other Stance feats.";
            var zh = $"<i>姿态 · {attrZh}</i>\n<i>{loreTitleZh}</i> {loreBodyZh}\n\n<b>效果：</b>{effectZh}\n\n<b>启动：</b>切换姿态为自由动作。同一时间只能激活一种姿态专长。\n\n<b>限制：</b>与其他姿态专长互相排斥。";
            return (en, zh);
        }

        private static void AddRanks(BuffConfigurator cfg, StatType baseStat, bool includeNegativeRank)
        {
            cfg.AddContextRankConfig(ContextRankConfigs.StatBonus(baseStat, Desc, AbilityRankType.Default, min: 0));
            if (includeNegativeRank)
            {
                cfg.AddContextRankConfig(
                    ContextRankConfigs.StatBonus(baseStat, Desc, AbilityRankType.StatBonus, min: 0)
                        .WithMultiplyByModifierProgression(-1));
            }
        }

        private static void ApplyIntraFamilyMutex(IReadOnlyList<BlueprintFeature> feats)
        {
            for (var i = 0; i < feats.Count; i++)
            {
                for (var j = i + 1; j < feats.Count; j++)
                {
                    Common.AddBidirectionalMutex(feats[i], feats[j]);
                }
            }
        }

        private static BlueprintFeature CreateStance(
            StatType baseStat,
            string internalName,
            string featureGuid,
            string buffGuid,
            string activatableGuid,
            string keyPrefix,
            string displayNameEn,
            string displayNameZh,
            (string en, string zh) description,
            Func<BuffConfigurator, BuffConfigurator> configureBuff,
            bool includeNegativeRank = false)
        {
            var buff = BuffConfigurator.New($"{internalName}Buff", buffGuid)
                .SetDisplayName(Common.L($"{keyPrefix}.Buff.Name", displayNameEn, displayNameZh))
                .SetDescription(Common.L($"{keyPrefix}.Buff.Desc", description.en, description.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(internalName);
            AddRanks(buff, baseStat, includeNegativeRank);
            var configuredBuff = configureBuff(buff)
                .AddRecalculateOnStatChange(stat: baseStat)
                .Configure();

            var activatable = ActivatableAbilityConfigurator.New($"{internalName}Activatable", activatableGuid)
                .SetDisplayName(Common.L($"{keyPrefix}.Activatable.Name", displayNameEn, displayNameZh))
                .SetDescription(Common.L($"{keyPrefix}.Activatable.Desc", description.en, description.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(internalName)
                .SetBuff(configuredBuff)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateIfCombatEnded(false)
                .SetDeactivateIfOwnerDisabled(true)
                .SetDeactivateIfOwnerUnconscious(true)
                .Configure();

            return FeatureConfigurator.New(internalName, featureGuid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"{keyPrefix}.Feature.Name", displayNameEn, displayNameZh))
                .SetDescription(Common.L($"{keyPrefix}.Feature.Desc", description.en, description.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(internalName)
                .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> { activatable })
                .Configure();
        }
    }
}
