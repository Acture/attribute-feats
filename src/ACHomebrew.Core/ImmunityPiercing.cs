using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;

namespace ACHomebrew.Feats
{
    /// <summary>Kinds of immunity a Penetration feat lets its owner pierce.</summary>
    [System.Flags]
    public enum PiercedImmunity
    {
        None = 0,
        Precision = 1,
        Energy = 2,
    }

    /// <summary>Registers the immunities its owner pierces while the feat is active.</summary>
    [TypeId("d705678dbf654cd3a8f75e48f95add48")]
    public class ImmunityPiercer : UnitFactComponentDelegate
    {
        public SpellDescriptor Descriptors;
        public PiercedImmunity Other;

        protected override void OnTurnOn() => ImmunityPiercing.Register(Owner, Fact, Descriptors, Other);

        protected override void OnTurnOff() => ImmunityPiercing.Unregister(Owner, Fact);
    }

    internal static class ImmunityPiercing
    {
        private static readonly ConditionalWeakTable<UnitEntityData, Dictionary<EntityFact, (SpellDescriptor descriptors, PiercedImmunity other)>> Pierced = new();

        public static void Register(UnitEntityData unit, EntityFact fact, SpellDescriptor descriptors, PiercedImmunity other)
            => Pierced.GetOrCreateValue(unit)[fact] = (descriptors, other);

        public static void Unregister(UnitEntityData unit, EntityFact fact)
        {
            if (Pierced.TryGetValue(unit, out var entries)) entries.Remove(fact);
        }

        public static bool Pierces(UnitEntityData unit, SpellDescriptor descriptors)
        {
            if (unit == null || descriptors == SpellDescriptor.None || !Pierced.TryGetValue(unit, out var entries)) return false;
            foreach (var entry in entries.Values)
                if ((entry.descriptors & descriptors) != 0) return true;
            return false;
        }

        public static bool Pierces(UnitEntityData unit, PiercedImmunity kind)
        {
            if (unit == null || !Pierced.TryGetValue(unit, out var entries)) return false;
            foreach (var entry in entries.Values)
                if ((entry.other & kind) != 0) return true;
            return false;
        }

        // Spell immunities by descriptor (e.g. undead immunity to mind-affecting spells).
        [HarmonyPatch(typeof(UnitPartSpellResistance), "CanApply",
            new[] { typeof(UnitPartSpellResistance.SpellImmunity), typeof(Kingmaker.UnitLogic.Abilities.AbilityData), typeof(UnitEntityData),
                typeof(SpellDescriptor?), typeof(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbility) })]
        private static class SpellImmunity_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(UnitPartSpellResistance.SpellImmunity immunity, UnitEntityData caster, ref bool __result)
            {
                if (__result && immunity.Type == SpellImmunityType.SpellDescriptor && Pierces(caster, immunity.SpellDescriptor))
                    __result = false;
            }
        }

        // Buff immunities by descriptor (e.g. immunity to fear or paralysis effects).
        [HarmonyPatch(typeof(BuffDescriptorImmunity), "IsImmune")]
        private static class BuffImmunity_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(BuffDescriptorImmunity __instance, MechanicsContext context, ref bool __result)
            {
                if (__result && context != null && Pierces(context.MaybeCaster, __instance.Descriptor & context.SpellDescriptor))
                    __result = false;
            }
        }

        // Precision damage immunity (sneak attack and similar).
        [HarmonyPatch(typeof(AddImmunityToPrecisionDamage), nameof(AddImmunityToPrecisionDamage.OnEventAboutToTrigger), new[] { typeof(RuleCalculateDamage) })]
        private static class PrecisionDamage_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(RuleCalculateDamage evt) => !Pierces(evt.Initiator, PiercedImmunity.Precision);
        }

        [HarmonyPatch(typeof(AddImmunityToPrecisionDamage), nameof(AddImmunityToPrecisionDamage.OnEventAboutToTrigger), new[] { typeof(RuleAttackRoll) })]
        private static class PrecisionAttack_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(RuleAttackRoll evt) => !Pierces(evt.Initiator, PiercedImmunity.Precision);
        }

        // Energy immunity becomes resistance: the damage is halved instead of negated, and never heals.
        [HarmonyPatch(typeof(AddEnergyDamageImmunity), nameof(AddEnergyDamageImmunity.OnEventAboutToTrigger), new[] { typeof(RuleCalculateDamage) })]
        private static class EnergyDamage_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(AddEnergyDamageImmunity __instance, RuleCalculateDamage evt)
            {
                if (!Pierces(evt.Initiator, PiercedImmunity.Energy)) return true;
                foreach (var damage in evt.DamageBundle)
                    if ((damage as EnergyDamage)?.EnergyType == __instance.EnergyType)
                        damage.AddDecline(new DamageDecline(DamageDeclineType.ByHalf, Traverse.Create(__instance).Property("Fact").GetValue<EntityFact>()));
                return false;
            }
        }

        [HarmonyPatch(typeof(AddEnergyDamageImmunity), nameof(AddEnergyDamageImmunity.OnEventAboutToTrigger), new[] { typeof(RuleDealDamage) })]
        private static class EnergyHeal_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(RuleDealDamage evt) => !Pierces(evt.Initiator, PiercedImmunity.Energy);
        }
    }
}
