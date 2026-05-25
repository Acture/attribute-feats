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
                .SetDisplayName(Common.L("ReactiveArmor_SpikedDefense.Name", "Spiked Defense"))
                .SetDescription(Common.L("ReactiveArmor_SpikedDefense.Desc", BuildSpikedDefenseDescription(), tagEncyclopediaEntries: true));

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
                .SetDisplayName(Common.L("ReactiveArmor_BulwarkOfSteel.Name", "Bulwark of Steel"))
                .SetDescription(Common.L("ReactiveArmor_BulwarkOfSteel.Desc", BuildBulwarkOfSteelDescription(), tagEncyclopediaEntries: true))
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
                .SetDisplayName(Common.L("ReactiveArmor_BulwarkOfSteelBuff.Name", "Bulwark of Steel"))
                .SetDescription(Common.L("ReactiveArmor_BulwarkOfSteelBuff.Desc", "<i>Reactive Armor · Sustained Guard</i>\n<i>Steel Holds the Line.</i> Heavy plates and layered mail turn each measured breath into a fresh reserve of staying power.\n\n<b>Effect:</b> Grants temporary hit points equal to your current armor bonus.\n\n<b>Restrictions:</b> Applied by Bulwark of Steel while you wear medium or heavy armor."))
                .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.AC, ModifierDescriptor.Armor, min: 0))
                .AddComponent<TemporaryHitPointsFromAbilityValue>(c =>
                {
                    c.Descriptor = ModifierDescriptor.None;
                    c.RemoveWhenHitPointsEnd = true;
                    c.Value = Common.Rank();
                })
                .Configure();
        }

        private static string BuildSpikedDefenseDescription()
            => "<i>Reactive Armor · Retaliation</i>\n<i>Steel Bites Back.</i> The weight of your harness turns every close strike into a painful lesson, jagged edges and hardened plates punishing those who crowd your guard.\n\n<b>Effect:</b> While wearing armor, whenever a foe hits you with a melee weapon attack, that attacker takes 1d6 + your armor bonus as untyped damage.\n\n<b>Restrictions:</b> Requires armor to function. This feat has no Reactive Armor mutex.";

        private static string BuildBulwarkOfSteelDescription()
            => "<i>Reactive Armor · Sustained Guard</i>\n<i>Steel Holds the Line.</i> You settle into the rhythm of battle behind measured steps and layered plates, letting your armor renew your resolve each round.\n\n<b>Effect:</b> While wearing medium or heavy armor, you refresh temporary hit points each round equal to your current armor bonus.\n\n<b>Restrictions:</b> Requires medium or heavy armor to function. This feat has no Reactive Armor mutex.";
    }
}
