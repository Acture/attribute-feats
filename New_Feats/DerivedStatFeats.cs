using System.Collections.Generic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Properties;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Properties;

namespace AttributeFeats.New_Feats
{
    internal static class DerivedStatFeats
    {
        private const string SkilledDefenderSkillRanksPropertyGuid = "391e1155-2021-473f-a216-ab01f3f5e500";
        private const string SoulBulwarkTempBuffGuid = "dbbbf07b-3c5f-4d4c-84a9-952f531e242e";
        private static readonly ModifierDescriptor Desc = ModifierDescriptor.None;
        private static readonly StatType[] SaveStats =
        {
            StatType.SaveFortitude,
            StatType.SaveReflex,
            StatType.SaveWill,
        };

        private static readonly StatType[] SkillStats =
        {
            StatType.SkillAthletics,
            StatType.SkillKnowledgeArcana,
            StatType.SkillKnowledgeWorld,
            StatType.SkillLoreNature,
            StatType.SkillLoreReligion,
            StatType.SkillMobility,
            StatType.SkillPerception,
            StatType.SkillPersuasion,
            StatType.SkillStealth,
            StatType.SkillThievery,
            StatType.SkillUseMagicDevice,
        };

        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            CreateSkilledDefenderSkillRanksProperty();
            CreateSoulBulwarkTempBuff();
            CreateSoulBulwarkTriggerBuff();
            CreateArcaneAegis();
            CreateMartialInsight();
            CreateSkilledDefender();
            CreateMysticVitality();
            CreateSoulBulwark();
            CreateSwordSaint();
        }

        private static void CreateArcaneAegis()
        {
            NewFeature(
                internalName: "ArcaneAegis",
                guid: Guids.Derived.ArcaneAegis,
                nameKey: "Derived_ArcaneAegis.Name",
                nameEn: "Arcane Aegis",
                nameZh: "魔能天衣",
                descKey: "Derived_ArcaneAegis.Desc",
                desc: BuildDescription(
                    "Caster Level to AC",
                    "施法者等级转化AC",
                    "<i>Weave of Abjuration.</i> Residual arcane energy coats your form like an invisible mantle of deflection. Raw caster discipline turns every stray strand of magic into an instinctive ward that diverts lethal blades before they touch skin.",
                    "<i>法脉流形。</i>奔涌的奥术源能如无形天衣披覆周身。施法者长年修习沉淀的法力底蕴化为本能护体气场，将近身斩杀的锐刃自毫厘间偏转卸劲。",
                    "Adds half your caster level as an untyped bonus to AC.",
                    "将你的施法者等级的一半作为无类型加值附加至防御等级（AC）。"))
                .AddContextRankConfig(ContextRankConfigs.CasterLevel(min: 0).WithDiv2Progression())
                .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc)
                .Configure();
        }

        private static void CreateMartialInsight()
        {
            var cfg = NewFeature(
                internalName: "MartialInsight",
                guid: Guids.Derived.MartialInsight,
                nameKey: "Derived_MartialInsight.Name",
                nameEn: "War-Hardened Reflexes",
                nameZh: "百战身魄",
                descKey: "Derived_MartialInsight.Desc",
                desc: BuildDescription(
                    "Base Attack Bonus to Saves",
                    "基础攻击加值转化豁免",
                    "<i>Battlefield Instincts.</i> Countless skirmishes have conditioned your reflexes into pure survival instinct. When a fireball explodes or a toxin seeps in, your combat-honed muscles and grit react before conscious thought can form.",
                    "<i>铁血砥砺。</i>尸山血海的淬炼令机体生出超凡的求生直觉。无论是法术轰炸的炽烈余波，还是蚀骨剧毒的暗算浸染，千锤百炼的身魄皆能先于心念自发抵御。",
                    "Adds half your base attack bonus as an untyped bonus to Fortitude, Reflex, and Will saving throws.",
                    "将你的基础攻击加值（BAB）的一半作为无类型加值附加至强韧、反射与意志豁免检定。"))
                .AddContextRankConfig(ContextRankConfigs.BaseAttack(min: 0).WithDiv2Progression());

            foreach (var stat in SaveStats)
            {
                cfg.AddContextStatBonus(stat, Common.Rank(), descriptor: Desc);
            }

            cfg.Configure();
        }

        private static void CreateSkilledDefender()
        {
            NewFeature(
                internalName: "SkilledDefender",
                guid: Guids.Derived.SkilledDefender,
                nameKey: "Derived_SkilledDefender.Name",
                nameEn: "Scholar's Positioning",
                nameZh: "通识御敌",
                descKey: "Derived_SkilledDefender.Desc",
                desc: BuildDescription(
                    "Total Skill Ranks to AC",
                    "技能总点数转化AC",
                    "<i>Omnidisciplinary Awareness.</i> From architectural understanding of terrain to biological analysis of anatomy and athletic equilibrium, your encyclopedic expertise informs every step. You evade danger simply by never occupying a disadvantaged position.",
                    "<i>博识兼修。</i>无论是对战场地势的建筑学洞察，还是对敌手骨肉机巧的生理解构，浩瀚的学识化作规避危局的无上准绳。知己知彼，步步先机，自立于不败之地。",
                    "Adds one third of your total skill ranks as an untyped bonus to AC.",
                    "将你所有技能总点数的三分之一作为无类型加值附加至防御等级（AC）。"))
                .AddContextRankConfig(ContextRankConfigs.CustomProperty(SkilledDefenderSkillRanksPropertyGuid, min: 0).WithDivStepProgression(3))
                .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: Desc)
                .Configure();
        }

        private static void CreateMysticVitality()
        {
            NewFeature(
                internalName: "MysticVitality",
                guid: Guids.Derived.MysticVitality,
                nameKey: "Derived_MysticVitality.Name",
                nameEn: "Ley-Infused Vitality",
                nameZh: "灵脉淬体",
                descKey: "Derived_MysticVitality.Desc",
                desc: BuildDescription(
                    "Caster Level to Hit Points",
                    "施法者等级转化生命",
                    "<i>Arcane Font of Flesh.</i> Unbounded magical force saturates your organs, sinew, and blood. Rather than withering under eldritch power, your mortal biology is fundamentally strengthened, sustained by a perpetual reservoir of vital energy.",
                    "<i>灵潮融血。</i>磅礴的奥能奔流日夜冲刷四肢百骸，肉身非但未被异界魔能侵蚀，反与本源法力彻底融汇，气血充盈饱满，化为生生不息的寿元洪流。",
                    "Adds your caster level as an untyped bonus to Hit Points.",
                    "将你的施法者等级作为无类型加值附加至生命值上限（HP）。"))
                .AddContextRankConfig(ContextRankConfigs.CasterLevel(min: 0))
                .AddContextStatBonus(StatType.HitPoints, Common.Rank(), descriptor: Desc)
                .Configure();
        }

        private static void CreateSoulBulwark()
        {
            NewFeature(
                internalName: "SoulBulwark",
                guid: Guids.Derived.SoulBulwark,
                nameKey: "Derived_SoulBulwark.Name",
                nameEn: "Dawn of the Soul",
                nameZh: "法相初明",
                descKey: "Derived_SoulBulwark.Desc",
                desc: BuildDescription(
                    "Caster Level to Temporary Hit Points",
                    "施法者等级转化临时生命",
                    "<i>Spirit's Resplendent Vanguard.</i> At the first scent of bloodshed, your inner soul awakens with incandescent clarity. A blazing psychic barrier surges forth to envelope you, absorbing the initial brunt of hostile fury.",
                    "<i>灵台定照。</i>杀意初现之际，本命法相灵光乍现，神华自生。一道璀璨耀目的心能罡气应激而发，在战端初启之刹那替身躯承受狂暴冲击。",
                    "At the start of combat, you gain temporary hit points equal to your caster level.",
                    "在战斗开始时，获得等同于你的施法者等级的临时生命值。"))
                .AddFacts(facts: new List<Blueprint<BlueprintUnitFactReference>> { Guids.Derived.SoulBulwarkBuff })
                .Configure();
        }

        private static void CreateSwordSaint()
        {
            NewFeature(
                internalName: "SwordSaint",
                guid: Guids.Derived.SwordSaint,
                nameKey: "Derived_SwordSaint.Name",
                nameEn: "Blade of the Spell-Saint",
                nameZh: "剑圣咒痕",
                descKey: "Derived_SwordSaint.Desc",
                desc: BuildDescription(
                    "Base Attack Bonus to Spell DC",
                    "基础攻击加值转化法术DC",
                    "<i>Synthesis of Steel and Sorcery.</i> The deadly discipline of weapon mastery infuses your incantations. Every gesture is delivered with the unerring finality of a master swordsman's coup de grâce, making your spells nearly impossible to resist.",
                    "<i>剑咒合一。</i>将登峰造极的剑道杀意熔炼于每一道法咒之中。施法手势如宗师拔刀般决绝肃杀、无懈可击，令敌手神魂受摄，极难抵御法术威能。",
                    "Adds half your base attack bonus as an untyped bonus to the DC of all your spells.",
                    "将你的基础攻击加值（BAB）的一半作为无类型加值附加至所有法术的豁免难度等级（DC）。"))
                .AddContextRankConfig(ContextRankConfigs.BaseAttack(min: 0).WithDiv2Progression())
                .AddIncreaseAllSpellsDC(descriptor: Desc, spellsOnly: false, value: Common.Rank())
                .Configure();
        }

        private static void CreateSkilledDefenderSkillRanksProperty()
        {
            var cfg = UnitPropertyConfigurator.New("SkilledDefenderSkillRanksProperty", SkilledDefenderSkillRanksPropertyGuid)
                .SetBaseValue(0)
                .SetOperationOnComponents(BlueprintUnitProperty.MathOperation.Sum);

            foreach (var stat in SkillStats)
            {
                cfg.AddSkillRankGetter(new PropertySettings(), stat);
            }

            cfg.Configure();
        }

        private static void CreateSoulBulwarkTempBuff()
        {
            BuffConfigurator.New("SoulBulwarkTempBuff", SoulBulwarkTempBuffGuid)
                .SetDisplayName(Common.L("Derived_SoulBulwark.Temp.Name", "Dawn of the Soul", "法相初明"))
                .SetDescription(Common.L(
                    "Derived_SoulBulwark.Temp.Desc",
                    "<i>Derived · Caster Level to Temporary Hit Points</i>\nA reserve of radiant psychic force gathers around your soul for the span of the fight, absorbing damage before your mortal flesh yields.",
                    "<i>衍生属性 · 施法者等级转化临时生命</i>\n璀璨心能屏障在战斗期间护佑周身，在血肉凡胎受创前优先抵御伤害。",
                    tagEncyclopediaEntries: true))
                .SetIconIfPresent("SoulBulwark")
                .SetStacking(StackingType.Replace)
                .AddContextRankConfig(ContextRankConfigs.CasterLevel(min: 0))
                .AddTemporaryHitPointsFromAbilityValue(descriptor: Desc, removeWhenHitPointsEnd: false, value: Common.Rank())
                .Configure();
        }

        private static void CreateSoulBulwarkTriggerBuff()
        {
            BuffConfigurator.New("SoulBulwarkTriggerBuff", Guids.Derived.SoulBulwarkBuff)
                .SetDisplayName(Common.L("Derived_SoulBulwark.Buff.Name", "Dawn of the Soul", "法相初明"))
                .SetDescription(Common.L(
                    "Derived_SoulBulwark.Buff.Desc",
                    "<i>Derived · Caster Level to Temporary Hit Points</i>\nThe soul keeps its own vigil, calling protective strength into place whenever a fight begins.",
                    "<i>衍生属性 · 施法者等级转化临时生命</i>\n神识常驻清明，每当战端初起便即刻激发生命护壁。",
                    tagEncyclopediaEntries: true))
                .SetIconIfPresent("SoulBulwark")
                .AddCombatStateTrigger(
                    combatStartActions: ActionsBuilder.New().ApplyBuff(
                        buff: SoulBulwarkTempBuffGuid,
                        durationValue: ContextDuration.Fixed(10, DurationRate.Minutes),
                        asChild: true,
                        isNotDispelable: true,
                        toCaster: true),
                    combatEndActions: ActionsBuilder.New().RemoveBuff(
                        buff: SoulBulwarkTempBuffGuid,
                        onlyFromCaster: true,
                        toCaster: true))
                .Configure();
        }

        private static FeatureConfigurator NewFeature(
            string internalName,
            string guid,
            string nameKey,
            string nameEn,
            string nameZh,
            string descKey,
            (string en, string zh) desc)
            => FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L(nameKey, nameEn, nameZh))
                .SetDescription(Common.L(descKey, desc.en, desc.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(internalName);

        private static (string en, string zh) BuildDescription(
            string subtitleEn,
            string subtitleZh,
            string loreEn,
            string loreZh,
            string effectEn,
            string effectZh)
        {
            var en = $"<i>Derived · {subtitleEn}</i>\n{loreEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Restrictions:</b> None. This feat is independent and stacks normally with other feat families.";
            var zh = $"<i>衍生属性 · {subtitleZh}</i>\n{loreZh}\n\n<b>效果：</b>{effectZh}\n\n<b>限制：</b>无。此专长独立生效，可与其他专长正常叠加。";
            return (en, zh);
        }
    }
}
