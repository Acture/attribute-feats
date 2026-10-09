using System;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

namespace ACHomebrew.Feats
{
    /// <summary>Small rule-triggering helpers shared by feat components.</summary>
    internal static class CombatActions
    {
        public static void DealDirectDamage(UnitEntityData source, UnitEntityData target, int amount)
        {
            if (amount <= 0 || target == null) return;
            Rulebook.Trigger(new RuleDealDamage(source, target, new DirectDamage(new DiceFormula(0, DiceType.Zero), amount)));
        }

        public static void Heal(UnitEntityData source, UnitEntityData target, int amount)
        {
            if (amount <= 0 || target == null) return;
            Rulebook.Trigger(new RuleHealDamage(source, target, amount));
        }

        /// <summary>Turn-based round number, or six-second real-time rounds.</summary>
        public static int CurrentRound()
            => Game.Instance.TurnBasedCombatController?.RoundNumber ?? (int)(Game.Instance.TimeController.GameTime.TotalSeconds / 6);
    }
}
