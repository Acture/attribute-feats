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
                nameValue: "Arcane Aegis",
                descKey: "Derived_ArcaneAegis.Desc",
                description: BuildDescription(
                    "Caster Level to AC",
                    "Spellwork settles around you like a second skin, turning disciplined magic into a ward that answers before steel can bite.",
                    "Adds half your caster level as an untyped bonus to AC."))
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
                nameValue: "Martial Insight",
                descKey: "Derived_MartialInsight.Desc",
                description: BuildDescription(
                    "Base Attack Bonus to Saves",
                    "Battle teaches you more than offense; every honed strike leaves behind the instinct to brace, turn, and endure.",
                    "Adds half your base attack bonus as an untyped bonus to Fortitude, Reflex, and Will saving throws."))
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
                nameValue: "Skilled Defender",
                descKey: "Derived_SkilledDefender.Desc",
                description: BuildDescription(
                    "Total Skill Ranks to AC",
                    "A lifetime of practiced motions teaches you where danger gathers, and every mastered craft becomes another way to stay untouched.",
                    "Adds one third of your total skill ranks as an untyped bonus to AC."))
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
                nameValue: "Mystic Vitality",
                descKey: "Derived_MysticVitality.Desc",
                description: BuildDescription(
                    "Caster Level to Hit Points",
                    "Power does not stop at the edge of a spell; it sinks deeper, fortifying flesh and breath with enduring reserve.",
                    "Adds your caster level as an untyped bonus to Hit Points."))
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
                nameValue: "Soul Bulwark",
                descKey: "Derived_SoulBulwark.Desc",
                description: BuildDescription(
                    "Caster Level to Temporary Hit Points",
                    "Your spirit rises to meet danger first, gathering spare strength into a barrier the moment battle begins.",
                    "At the start of combat, you gain temporary hit points equal to your caster level."))
                .AddFacts(facts: new List<Blueprint<BlueprintUnitFactReference>> { Guids.Derived.SoulBulwarkBuff })
                .Configure();
        }

        private static void CreateSwordSaint()
        {
            NewFeature(
                internalName: "SwordSaint",
                guid: Guids.Derived.SwordSaint,
                nameKey: "Derived_SwordSaint.Name",
                nameValue: "Sword Saint",
                descKey: "Derived_SwordSaint.Desc",
                description: BuildDescription(
                    "Base Attack Bonus to Spell DC",
                    "Martial rigor sharpens your spellcraft into a killing edge, making each incantation harder to deny or escape.",
                    "Adds half your base attack bonus as an untyped bonus to the DC of all your spells."))
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
                .SetDisplayName(Common.L("Derived_SoulBulwark.Temp.Name", "Soul Bulwark"))
                .SetDescription(Common.L(
                    "Derived_SoulBulwark.Temp.Desc",
                    BuildDescription(
                        "Caster Level to Temporary Hit Points",
                        "A reserve of force gathers around your soul for the span of the fight, ready to break before your body does.",
                        "Grants temporary hit points equal to your caster level."),
                    tagEncyclopediaEntries: true))
                .SetStacking(StackingType.Replace)
                .AddContextRankConfig(ContextRankConfigs.CasterLevel(min: 0))
                .AddTemporaryHitPointsFromAbilityValue(descriptor: Desc, removeWhenHitPointsEnd: false, value: Common.Rank())
                .Configure();
        }

        private static void CreateSoulBulwarkTriggerBuff()
        {
            BuffConfigurator.New("SoulBulwarkTriggerBuff", Guids.Derived.SoulBulwarkBuff)
                .SetDisplayName(Common.L("Derived_SoulBulwark.Buff.Name", "Soul Bulwark"))
                .SetDescription(Common.L(
                    "Derived_SoulBulwark.Buff.Desc",
                    BuildDescription(
                        "Caster Level to Temporary Hit Points",
                        "The soul keeps its own vigil, calling protective strength into place whenever a fight begins.",
                        "At the start of combat, this feat refreshes Soul Bulwark's temporary hit points."),
                    tagEncyclopediaEntries: true))
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
            string nameValue,
            string descKey,
            string description)
            => FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L(nameKey, nameValue))
                .SetDescription(Common.L(descKey, description, tagEncyclopediaEntries: true));

        private static string BuildDescription(string subtitle, string lore, string effect)
            => $"<i>Derived · {subtitle}</i>\n<i>{lore}</i>\n\n<b>Effect:</b> {effect}\n\n<b>Restrictions:</b> None. This feat is independent and stacks normally with other feat families.";
    }
}
