using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace WotRHomebrew.Feats
{
    internal static class WeaponDamageFeats
    {
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var feats = new[]
            {
                Create(StatType.Strength, Guids.WeaponDamage.Str),
                Create(StatType.Dexterity, Guids.WeaponDamage.Dex),
                Create(StatType.Constitution, Guids.WeaponDamage.Con),
                Create(StatType.Intelligence, Guids.WeaponDamage.Int),
                Create(StatType.Wisdom, Guids.WeaponDamage.Wis),
                Create(StatType.Charisma, Guids.WeaponDamage.Cha),
            };
        }

        private static BlueprintFeature Create(StatType attribute, string guid)
        {
            return FeatSelection.WeaponDamage.NewParametrizedFeat($"WeaponDamage_{attribute}", guid)
                .SetDisplayName(Common.L($"WeaponDamage_{attribute}.Name", $"{attribute} to Weapon Damage"))
                .SetDescription(Common.L($"WeaponDamage_{attribute}.Description",
                    "<b>Choose a weapon category in which you are proficient.</b>\n\n" +
                    $"<b>Replace mode (default):</b> Use your {attribute} modifier instead of the weapon's existing damage attribute when this improves damage. " +
                    "The normal weapon damage multiplier is preserved, including two-handed and off-hand use.\n\n" +
                    $"<b>Add mode:</b> Add your positive {attribute} modifier once to normal weapon damage. " +
                    "This extra bonus is not multiplied or halved for two-handed or off-hand use.\n\n" +
                    "Only applies to attacks with the chosen category that already use an attribute modifier for damage. " +
                    "Weapons without a damage attribute (such as ordinary crossbows) gain no benefit. " +
                    "Does not change attack rolls, spell damage, or other statistics. Does not require Power Mode.\n\n" +
                    "You can take this feat again for a different weapon category. When mutual exclusivity is enabled, " +
                    "you can select only one attribute within this family. " +
                    "Choose the active mode in the Weapon Damage setting. Changes apply on the next damage calculation, without restarting or reselecting the feat."))
                .SetParameterType(FeatureParameterType.WeaponCategory)
                .SetWeaponSubCategory(WeaponSubCategory.None)
                .SetRequireProficiency(true)
                .AddComponent<AttributeWeaponDamage>(c => c.Attribute = attribute)
                .Configure();
        }
    }

    [TypeId("f91c7a3d250a4a9a8cd12c6882f9381e")]
    public class AttributeWeaponDamage : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IInitiatorRulebookSubscriber
    {
        public StatType Attribute;
        public WeaponDamageMode Mode => Mod.Settings?.WeaponDamage ?? WeaponDamageMode.Replace;

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) { }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon == null || evt.DamageDescription.Count == 0) return;
            var matchesWeapon = Param.WeaponCategory == evt.Weapon.Blueprint.Category;
            if (!matchesWeapon || !evt.DamageBonusStat.HasValue) return;

            var current = Owner.Stats.GetStat(evt.DamageBonusStat.Value) as ModifiableValueAttributeStat;
            var selected = Owner.Stats.GetStat(Attribute) as ModifiableValueAttributeStat;
            if (current == null || selected == null) return;

            // Resolve after Wrath computes hand/natural-weapon multipliers and other
            // attribute replacements. Early replacement with a mental stat loses 1.5x.
            // Read once per calculation so existing feats follow live settings.
            var mode = Mode;
            var adjustment = WeaponDamageRules.CalculateAdjustment(mode, matchesWeapon, true,
                current.Bonus, selected.Bonus,
                evt.DamageBonusStatMultiplier + evt.AdditionalDamageBonusStatMultiplier);
            if (adjustment == 0) return;

            evt.DamageDescription[0].AddModifier(new Modifier(adjustment, Fact, ModifierDescriptor.UntypedStackable));
            if (mode == WeaponDamageMode.Replace)
            {
                // Subsequent replacement feats compare against the new attribute,
                // so disabling mutex still chooses the best instead of adding each delta.
                evt.OverrideDamageBonusStat(Attribute);
            }
        }
    }
}
