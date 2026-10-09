using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Kingmaker;
using System.Collections.Generic;
using System.Linq;
using System;
using static ACHomebrew.Feats.FeatBuilders;

namespace ACHomebrew.Feats
{
    /// <summary>Retaliation: counterattacks, punishment, shoves and returned damage.</summary>
    internal static class RetaliationFeats
    {
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            ConfigureCounters();
            ConfigureReturnDamage();
        }

        private static void ConfigureCounters()
        {
            Feat(FeatSelection.Retaliation, "CounterAttack", Guids.Playstyle.CounterAttack, "Riposte Reflex", "截锋逆击",
                    Desc("Retaliation", "反击",
                        "When an enemy makes a melee attack against you, you make an attack of opportunity against it if it is within your reach. " +
                        "This uses one of your attacks of opportunity for the round, so Combat Reflexes increases how often you can counter.",
                        "敌人对你发动近战攻击时，若其在你的触及范围内，你对其发动一次借机攻击。这会消耗你本轮的借机攻击次数，因此战斗反射等能增加反击次数。"))
                .AddComponent<CounterAttackOnMelee>(c => c.Mode = CounterAttackMode.AnyAttack)
                .Configure();

            Feat(FeatSelection.Retaliation, "MissCounter", Guids.Playstyle.MissCounter, "Punish the Overreach", "乘虚落刃",
                    Desc("Retaliation", "反击",
                        "Once per round, when an enemy's melee attack misses you, you make an attack against it if it is within your reach. " +
                        "This counterattack does not use your attacks of opportunity.",
                        "每轮一次，敌人对你的近战攻击未命中时，若其在你的触及范围内，你对其发动一次攻击。这次反击不消耗借机攻击次数。"))
                .AddComponent<CounterAttackOnMelee>(c => c.Mode = CounterAttackMode.FreeOnMiss)
                .Configure();

            var mark = Buff("PunisherMarkBuff", Guids.Playstyle.PunisherMarkBuff, "Blood Debt", "血债",
                    "This creature attacked someone with the Blood-Debt Reckoning feat this round.", "该生物本轮攻击过拥有睚眦必报专长的角色。")
                .Configure();
            Feat(FeatSelection.Retaliation, "Punisher", Guids.Playstyle.Punisher, "Blood-Debt Reckoning", "睚眦必报",
                    Desc("Retaliation", "反击",
                        "Against enemies that attacked you this round, you gain +1 on attack and damage rolls per 4 character levels (minimum +1, maximum +5).",
                        "对本轮攻击过你的敌人，你的攻击检定与伤害骰每4角色等级获得+1（最低+1，最高+5）。"))
                .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: ActionsBuilder.New().ApplyBuff(mark, Rounds(1)), onlyMelee: false)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(min: 1, max: 5).WithDivStepProgression(4))
                .AddAttackBonusAgainstFactOwner(bonus: Common.Rank(), checkedFact: mark, descriptor: ModifierDescriptor.UntypedStackable)
                .AddDamageBonusAgainstFactOwner(bonus: Common.Rank(), checkedFact: mark, descriptor: ModifierDescriptor.UntypedStackable)
                .Configure();

            var crit = Buff("ParryCritBuff", Guids.Playstyle.ParryCritBuff, "Opening Read", "看破",
                    "Your next attack is a confirmed critical hit.", "你的下一次攻击必定为重击。")
                .AddComponent<NextAttackCritical>()
                .Configure();
            Feat(FeatSelection.Retaliation, "ParryCrit", Guids.Playstyle.ParryCrit, "Read the Blade", "明镜识锋",
                    Desc("Retaliation", "反击",
                        "After you successfully parry an attack, your next attack within 2 rounds is an automatically confirmed critical hit.",
                        "成功招架一次攻击后，你在2轮内的下一次攻击自动成为确认的重击。"))
                .AddComponent<ApplyBuffOnParry>(c => c.Buff = crit.ToReference<BlueprintBuffReference>())
                .Configure();

            Feat(FeatSelection.Retaliation, "Shove", Guids.Playstyle.Shove, "Batter and Displace", "撼步冲撞",
                    Desc("Retaliation", "反击",
                        "After each of your weapon attacks and after each weapon attack against you, hit or miss, you attempt a bull rush against that enemy. " +
                        "This bull rush does not provoke attacks of opportunity.",
                        "你的每次武器攻击结算后，以及敌人对你的每次武器攻击结算后（无论是否命中），你对该敌人尝试一次冲撞。该冲撞不引发借机攻击。"))
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().CombatManeuver(ActionsBuilder.New(), CombatManeuver.BullRush))
                .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: ActionsBuilder.New().CombatManeuver(ActionsBuilder.New(), CombatManeuver.BullRush))
                .Configure();
        }

        private static void ConfigureReturnDamage()
        {
            Feat(FeatSelection.Retaliation, "ReturnBlow", Guids.Dota.ReturnBlow, "Anvil's Retribution", "铁砧反震",
                    Desc("Retaliation", "反击",
                        "Whenever an enemy damages you with a melee or ranged attack or a spell, it takes damage equal to half your character level plus your Strength modifier (minimum 1).",
                        "敌人以近战、远程攻击或法术对你造成伤害时，受到等于你角色等级一半加力量调整值的伤害（最低1）。"))
                .AddComponent<ReturnBlowDamage>()
                .Configure();

            Feat(FeatSelection.Retaliation, "KrakenShell", Guids.Dota.KrakenShell, "Leviathan's Carapace", "渊海甲胄",
                    Desc("Retaliation", "反击",
                        "You gain DR 1/— plus 1 per 4 character levels. After you take damage totalling a tenth of your maximum hit points, " +
                        "all harmful effects on you are removed and the count resets.",
                        "你获得1/—的伤害减免，每4角色等级再+1。累计受到相当于最大生命值十分之一的伤害后，移除你身上所有有害效果，并重新计数。"))
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel().WithStartPlusDivStepProgression(4, 0))
                .AddDamageResistancePhysical(isStackable: true, value: Common.Rank())
                .AddComponent<KrakenShellPurge>()
                .Configure();
        }
    }

    public enum CounterAttackMode
    {
        AnyAttack,
        FreeOnMiss,
    }

    /// <summary>Counterattacks melee attackers after their attack resolves.</summary>
    [TypeId("4f1c2a9e7b3d4e5f8a6b0c1d2e3f4a5b")]
    public class CounterAttackOnMelee : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackWithWeapon>, ITargetRulebookSubscriber
    {
        public CounterAttackMode Mode;
        private int LastRound = -1;

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            var attacker = evt.Initiator;
            var unit = Owner;
            if (attacker == null || attacker == unit || evt.IsAttackOfOpportunity || evt.Weapon?.Blueprint.IsMelee != true) return;
            if (unit.State.IsDead || !unit.CombatState.IsEngage(attacker)) return;

            if (Mode == CounterAttackMode.AnyAttack)
            {
                unit.CombatState.AttackOfOpportunity(attacker);
                return;
            }

            if (evt.AttackRoll.IsHit) return;
            var round = Kingmaker.Game.Instance.TurnBasedCombatController?.RoundNumber ?? (int)(Kingmaker.Game.Instance.TimeController.GameTime.TotalSeconds / 6);
            if (round == LastRound) return;
            LastRound = round;
            unit.Commands.Run(new UnitAttackOfOpportunity(attacker));
        }
    }

    /// <summary>Applies a buff to the owner after it parries an attack.</summary>
    [TypeId("9a7e3b1c5d2f4a6b8c0e1f2a3b4c5d6e")]
    public class ApplyBuffOnParry : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        public BlueprintBuffReference Buff;

        public void OnEventAboutToTrigger(RuleAttackRoll evt) { }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.Parry != null && evt.Parry.IsTriggered && !evt.IsHit)
                Owner.AddBuff(Buff.Get(), Context, 2.Rounds().Seconds);
        }
    }

    /// <summary>Makes the owner's next attack roll a confirmed critical, then removes its buff.</summary>
    [TypeId("2b6d8f0a1c3e4b5d7f9a0b2c4d6e8f1a")]
    public class NextAttackCritical : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            evt.AutoCriticalThreat = true;
            evt.AutoCriticalConfirmation = true;
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) => Buff.Remove();
    }

    [TypeId("293a4b5c6d7e8f90a1b2c3d4e5f60718")]
    public class ReturnBlowDamage : UnitFactComponentDelegate, ITargetRulebookHandler<RuleDealDamage>, ITargetRulebookSubscriber
    {
        [ThreadStatic] private static bool Retaliating;

        public void OnEventAboutToTrigger(RuleDealDamage evt) { }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            var attacker = evt.Initiator;
            if (Retaliating || evt.Result <= 0 || attacker == null || attacker == Owner || !attacker.IsEnemy(Owner)) return;
            // Weapon attacks and spells only: no damage-over-time, aura ticks or other reflected damage.
            if (evt.DamageBundle.Weapon == null && evt.Reason?.Ability?.Spellbook == null) return;
            Retaliating = true;
            try
            {
                var strength = Owner.Stats.Strength.Bonus;
                CombatActions.DealDirectDamage(Owner, attacker, Math.Max(1, Owner.Descriptor.Progression.CharacterLevel / 2 + strength));
            }
            finally
            {
                Retaliating = false;
            }
        }
    }

    [TypeId("3a4b5c6d7e8f90a1b2c3d4e5f6071829")]
    public class KrakenShellPurge : UnitFactComponentDelegate, ITargetRulebookHandler<RuleDealDamage>, ITargetRulebookSubscriber
    {
        private int Taken;

        public void OnEventAboutToTrigger(RuleDealDamage evt) { }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            if (evt.Result <= 0) return;
            Taken += evt.Result;
            if (Taken < Math.Max(1, Owner.MaxHP / 10)) return;
            Taken = 0;
            foreach (var buff in Owner.Buffs.Enumerable.Where(b => b.Blueprint.Harmful).ToList())
                buff.Remove();
        }
    }
}
