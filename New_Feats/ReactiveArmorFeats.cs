using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;

namespace AttributeFeats.New_Feats
{
    internal static class ReactiveArmorFeats
    {
        private static readonly ArmorProficiencyGroup[] AnyArmor =
        {
            ArmorProficiencyGroup.Light,
            ArmorProficiencyGroup.Medium,
            ArmorProficiencyGroup.Heavy,
        };

        private static readonly ArmorProficiencyGroup[] MediumHeavyArmor =
        {
            ArmorProficiencyGroup.Medium,
            ArmorProficiencyGroup.Heavy,
        };

        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            ConfigureBulwarkBuff();
            ConfigureSpikedDefense();
            ConfigureBulwarkOfSteel();
        }

        private static void ConfigureSpikedDefense()
        {
            var cfg = FeatureConfigurator.New("SpikedDefense", Guids.ReactiveArmor.SpikedDefense, FeatureGroup.Feat)
                .SetDisplayName(Common.L("ReactiveArmor_SpikedDefense.Name", "Barbed Carapace", "逆鳞铁刺"))
                .SetDescription(Common.L(
                    "ReactiveArmor_SpikedDefense.Desc",
                    BuildSpikedDefenseDescriptionEn(),
                    BuildSpikedDefenseDescriptionZh(),
                    tagEncyclopediaEntries: true));

            cfg.AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.AC, ModifierDescriptor.Armor, min: 0));
            cfg.AddTargetAttackWithWeaponTrigger(
                actionsOnAttacker: ActionsBuilder.New()
                    .Conditional(
                        conditions: ConditionsBuilder.New()
                            .UnitArmor(includeArmorCategories: AnyArmor)
                            .Build(),
                        ifTrue: ActionsBuilder.New().DealDamage(
                            DamageTypes.Untyped(),
                            ContextDice.Value(DiceType.D6, bonus: Common.Rank()))
                            .Build(),
                        ifFalse: ActionsBuilder.New().Build())
                    .Build(),
                onlyHit: true,
                onlyMelee: true,
                waitForAttackResolve: true);
            cfg.Configure();
        }

        private static void ConfigureBulwarkOfSteel()
        {
            var refreshBuff = ActionsBuilder.New()
                .RemoveBuff(Guids.ReactiveArmor.BulwarkOfSteelBuff)
                .Conditional(
                    conditions: ConditionsBuilder.New()
                        .UnitArmor(includeArmorCategories: MediumHeavyArmor)
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Guids.ReactiveArmor.BulwarkOfSteelBuff, ContextDuration.Fixed(1), toCaster: true)
                        .Build(),
                    ifFalse: ActionsBuilder.New().Build());

            FeatureConfigurator.New("BulwarkOfSteel", Guids.ReactiveArmor.BulwarkOfSteel, FeatureGroup.Feat)
                .SetDisplayName(Common.L("ReactiveArmor_BulwarkOfSteel.Name", "Citadel of Steel", "铸铁城阙"))
                .SetDescription(Common.L(
                    "ReactiveArmor_BulwarkOfSteel.Desc",
                    BuildBulwarkOfSteelDescriptionEn(),
                    BuildBulwarkOfSteelDescriptionZh(),
                    tagEncyclopediaEntries: true))
                .AddFactContextActions(
                    activated: refreshBuff,
                    deactivated: ActionsBuilder.New().RemoveBuff(Guids.ReactiveArmor.BulwarkOfSteelBuff),
                    dispose: ActionsBuilder.New().RemoveBuff(Guids.ReactiveArmor.BulwarkOfSteelBuff),
                    newRound: refreshBuff)
                .Configure();
        }

        private static void ConfigureBulwarkBuff()
        {
            BuffConfigurator.New("BulwarkOfSteelBuff", Guids.ReactiveArmor.BulwarkOfSteelBuff)
                .SetDisplayName(Common.L("ReactiveArmor_BulwarkOfSteelBuff.Name", "Citadel of Steel", "铸铁城阙"))
                .SetDescription(Common.L(
                    "ReactiveArmor_BulwarkOfSteelBuff.Desc",
                    "<i>Reactive Armor · Living Fortress</i>\nCitadel of Steel is active, granting temporary hit points each round equal to your current armor bonus.",
                    "<i>活性护甲 · 身化铁城</i>\n铸铁城阙已激活，每轮获得等同于你的护甲加值的临时生命值。"))
                .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.AC, ModifierDescriptor.Armor, min: 0))
                .AddComponent<TemporaryHitPointsFromAbilityValue>(c =>
                {
                    c.Descriptor = ModifierDescriptor.None;
                    c.RemoveWhenHitPointsEnd = true;
                    c.Value = Common.Rank();
                })
                .Configure();
        }

        private static string BuildSpikedDefenseDescriptionEn()
            => "<i>Reactive Armor · Retaliation</i>\n<i>Retaliation in Iron.</i> Your armor is forged not merely for passive deflection, but bristling with predatory counter-spikes and interlocking serrated plates. Every close strike bites into your guard only to tear the attacker's own flesh to ribbons.\n\n<b>Effect:</b> While wearing armor, whenever a foe hits you with a melee weapon attack, that attacker takes 1d6 + your armor bonus as untyped damage.\n\n<b>Restrictions:</b> Requires armor to function. This feat has no Reactive Armor mutex.";

        private static string BuildSpikedDefenseDescriptionZh()
            => "<i>活性护甲 · 反伤</i>\n<i>荆棘反噬。</i>披挂之甲胄绝非被动格挡之死物，其上密布倒钩棘刺与机括咬合的利齿铁鳞。敌刃犯我中门之时，必遭荆甲逆鳞凶狠绞杀，反噬其血肉筋骨。\n\n<b>效果：</b>穿着护甲时，每当有敌人以近战武器攻击命中你，该攻击者将受到1d6+你的护甲加值的无类型伤害。\n\n<b>限制：</b>需穿着护甲方可生效。此专长无同类互斥限制。";

        private static string BuildBulwarkOfSteelDescriptionEn()
            => "<i>Reactive Armor · Sustained Guard</i>\n<i>Living Fortress.</i> Dwarven fortress smiths know that heavy mail and folded plate can become an impenetrable bulwark. With each rhythmically measured breath and foot plant, your layered harness absorbs incoming momentum, continuously reinforcing an impregnable shield of temporary vitality.\n\n<b>Effect:</b> While wearing medium or heavy armor, you refresh temporary hit points each round equal to your current armor bonus.\n\n<b>Restrictions:</b> Requires medium or heavy armor to function. This feat has no Reactive Armor mutex.";

        private static string BuildBulwarkOfSteelDescriptionZh()
            => "<i>活性护甲 · 坚守</i>\n<i>身化铁城。</i>矮人要塞匠师的秘传技艺：重甲与折叠钢板在百战之躯上融为不可逾越的移动要塞。每随沉稳呼吸起伏吐纳，层叠重甲化解冲击余波，每轮重聚护体罡气。\n\n<b>效果：</b>穿着中甲或重甲时，每轮获得等同于你的护甲加值的临时生命值（持续刷新）。\n\n<b>限制：</b>需穿着中甲或重甲方可生效。此专长无同类互斥限制。";
    }
}
