using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;

namespace AttributeFeats.New_Feats
{
    /// <summary>
    /// Batch 2: mechanics inspired by Dota heroes, with overlapping ideas merged
    /// (Viper/Phoenix, Lifestealer/Zeus, Phantom Assassin/Storm Spirit, Antimage/Nyx,
    /// Slardar/Juggernaut). See notes/2026-10-09-dota-inspired-backlog.md.
    /// </summary>
    internal static class DotaFeats
    {
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            ConfigureExecution();
            ConfigureArcana();
            ConfigureMomentum();
            ConfigureRetaliation();
            ConfigureGrowth();
            ConfigureSummoner();
        }

        private static (string en, string zh) Desc(string familyEn, string familyZh, string effectEn, string effectZh)
            => ($"<i>{familyEn}</i>\n\n<b>Effect:</b> {effectEn}", $"<i>{familyZh}</i>\n\n<b>效果：</b>{effectZh}");

        private static FeatureConfigurator Feat(FeatSelection family, string name, string guid, string nameEn, string nameZh, (string en, string zh) desc)
            => family.NewFeat(name, guid)
                .SetDisplayName(Common.L($"{name}.Name", nameEn, nameZh))
                .SetDescription(Common.L($"{name}.Desc", desc.en, desc.zh, tagEncyclopediaEntries: true));

        private static void ConfigureExecution()
        {
            Feat(FeatSelection.Execution, "CorrosiveFinish", Guids.Dota.CorrosiveFinish, "Corrosive Finish", "腐蚀处决",
                    Desc("Execution", "处决",
                        "For every 10% of hit points the target is missing (up to 90%), your weapon attacks deal +1 damage plus 1 per 10 character levels.",
                        "目标每损失10%生命值（最多计90%），你的武器攻击对其伤害+1，每10角色等级再+1。"))
                .AddComponent<MissingHealthDamage>(c => c.UseTarget = true)
                .Configure();

            Feat(FeatSelection.Execution, "PhoenixFury", Guids.Dota.PhoenixFury, "Burning Desperation", "涅槃之怒",
                    Desc("Execution", "处决",
                        "At the start of each round in combat, enemies within 15 feet take fire damage equal to half your character level (minimum 1), " +
                        "increased by the percentage of hit points you are missing (for example, +50% when you are at half health).",
                        "战斗中每轮开始时，15尺内的敌人受到等于你角色等级一半（最低1）的火焰伤害，并按你已损失生命值的百分比提高（例如半血时+50%）。"))
                .AddComponent<BurningAura>()
                .Configure();

            Feat(FeatSelection.Execution, "Feast", Guids.Dota.Feast, "Feast", "盛宴",
                    Desc("Execution", "处决",
                        "Your weapon hits deal extra damage equal to 2% of the target's maximum hit points (minimum 1, at most three times your character level).",
                        "你的武器命中额外造成目标最大生命值2%的伤害（最低1，最多为角色等级的3倍）。"))
                .AddComponent<PercentHealthDamage>(c => c.FromCurrent = false)
                .Configure();

            Feat(FeatSelection.Execution, "FeastCurrent", Guids.Dota.FeastCurrent, "Opening Bite", "先手撕咬",
                    Desc("Execution", "处决",
                        "Your weapon hits deal extra damage equal to 3% of the target's current hit points (minimum 1, at most three times your character level).",
                        "你的武器命中额外造成目标当前生命值3%的伤害（最低1，最多为角色等级的3倍）。"))
                .AddComponent<PercentHealthDamage>(c => c.FromCurrent = true)
                .Configure();

            Feat(FeatSelection.Execution, "FeastSpell", Guids.Dota.FeastSpell, "Arcane Feast", "法术盛宴",
                    Desc("Execution", "处决",
                        "When your spell damages an enemy, it takes extra damage equal to 2% of its maximum hit points " +
                        "(minimum 1, at most three times your character level). Once per target for each spell you cast.",
                        "你的法术伤害敌人时，其额外受到最大生命值2%的伤害（最低1，最多为角色等级的3倍）。你每施放一次法术，每个目标只触发一次。"))
                .AddComponent<SpellFeastDamage>()
                .Configure();

            Feat(FeatSelection.Execution, "FeastNatural", Guids.Dota.FeastNatural, "Savage Feast", "天武盛宴",
                    Desc("Execution", "处决",
                        "Your natural attacks deal extra damage equal to 2% of the target's maximum hit points " +
                        "(minimum 1, at most three times your character level), and you heal the same amount.",
                        "你的天生武器攻击额外造成目标最大生命值2%的伤害（最低1，最多为角色等级的3倍），并回复等量生命。"))
                .AddComponent<PercentHealthDamage>(c =>
                {
                    c.FromCurrent = false;
                    c.NaturalOnly = true;
                    c.HealSelf = true;
                })
                .Configure();

            Feat(FeatSelection.Execution, "Lifesteal", Guids.Dota.Lifesteal, "Lifesteal", "吸血",
                    Desc("Execution", "处决",
                        "You heal 15% of the damage your weapon attacks deal.",
                        "你回复武器攻击所造成伤害的15%。"))
                .AddComponent<WeaponLifesteal>()
                .Configure();
        }

        private static void ConfigureArcana()
        {
            Feat(FeatSelection.Arcana, "SlotHarvest", Guids.Dota.SlotHarvest, "Harvest of Power", "夺能",
                    Desc("Arcana", "法力",
                        "Once per round, when an enemy you damaged dies, regain one spent spell slot of a level no higher than half that enemy's Hit Dice (minimum 1).",
                        "每轮一次，被你伤害的敌人死亡时，恢复一个已消耗的法术位，其环级不高于该敌人生命骰的一半（最低1环）。"))
                .AddComponent<SlotHarvestOnKill>()
                .Configure();

            Feat(FeatSelection.Arcana, "ArcaneOrb", Guids.Dota.ArcaneOrb, "Arcane Orb", "奥术天球",
                    Desc("Arcana", "法力",
                        "Your cantrips deal extra damage equal to the total spell levels of your unspent spell slots divided by 5.",
                        "你的戏法额外造成伤害，数值等于你未消耗法术位的环级总和除以5。"))
                .AddComponent<ArcaneOrbDamage>()
                .Configure();

            Feat(FeatSelection.Arcana, "EssenceFlux", Guids.Dota.EssenceFlux, "Essence Flux", "精华流转",
                    Desc("Arcana", "法力",
                        "When you cast a spell of 1st level or higher from a spellbook, there is a 15% chance the spell slot is restored.",
                        "你从法术书施放1环或以上的法术时，有15%的几率恢复该法术位。"))
                .AddComponent<EssenceFluxRefund>()
                .Configure();

            Feat(FeatSelection.Arcana, "ManaBreak", Guids.Dota.ManaBreak, "Mana Break", "法力损毁",
                    Desc("Arcana", "法力",
                        "When your weapon hits a spellcaster, it loses its highest unspent spell slot and takes extra damage equal to twice that slot's level. " +
                        "Once per round.",
                        "你的武器命中施法者时，使其失去最高环的一个未消耗法术位，并额外受到等于该环级两倍的伤害。每轮一次。"))
                .AddComponent<ManaBreakOnHit>()
                .Configure();

            Feat(FeatSelection.Arcana, "ManaBreakSpell", Guids.Dota.ManaBreakSpell, "Mana Void", "法力虚空",
                    Desc("Arcana", "法力",
                        "When your spell damages a spellcaster, it loses its highest unspent spell slot and takes extra damage equal to twice that slot's level. " +
                        "Once per round.",
                        "你的法术伤害施法者时，使其失去最高环的一个未消耗法术位，并额外受到等于该环级两倍的伤害。每轮一次。"))
                .AddComponent<ManaBreakOnSpell>()
                .Configure();
        }

        private static void ConfigureMomentum()
        {
            var fervor = BuffConfigurator.New("FervorBuff", Guids.Dota.FervorBuff)
                .SetDisplayName(Common.L("FervorBuff.Name", "Fervor", "狂热"))
                .SetDescription(Common.L("FervorBuff.Desc", "+1 dodge AC per stack; at 3 stacks, one extra attack in a full attack.",
                    "每层闪避AC+1；3层时全回合攻击额外攻击一次。"))
                .SetStacking(StackingType.Rank)
                .SetRanks(3)
                .AddContextRankConfig(ContextRankConfigs.BuffRank(Guids.Dota.FervorBuff))
                .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: ModifierDescriptor.Dodge)
                .Configure();
            var fervorAttack = BuffConfigurator.New("FervorAttackBuff", Guids.Dota.FervorAttackBuff)
                .SetDisplayName(Common.L("FervorAttackBuff.Name", "Fervor: extra attack", "狂热：额外攻击"))
                .SetDescription(Common.L("FervorAttackBuff.Desc", "One extra attack in a full attack.", "全回合攻击额外攻击一次。"))
                .AddBuffExtraAttack(number: 1)
                .Configure();
            Feat(FeatSelection.Momentum, "Fervor", Guids.Dota.Fervor, "Fervor", "狂热",
                    Desc("Momentum", "动能",
                        "Each consecutive weapon attack against the same target, hit or miss, adds a Fervor stack (up to 3): +1 dodge AC per stack, " +
                        "and at 3 stacks you make one extra attack in a full attack. Attacking a different target resets the stacks.",
                        "连续攻击同一目标时（无论是否命中）每次获得一层狂热（最多3层）：每层闪避AC+1，3层时全回合攻击额外攻击一次。攻击其他目标时层数重置。"))
                .AddComponent<FervorTracker>(c =>
                {
                    c.Buff = fervor.ToReference<BlueprintBuffReference>();
                    c.AttackBuff = fervorAttack.ToReference<BlueprintBuffReference>();
                })
                .Configure();

            Feat(FeatSelection.Momentum, "CrushingRhythm", Guids.Dota.CrushingRhythm, "Crushing Rhythm", "重击节律",
                    Desc("Momentum", "动能",
                        "Every fourth weapon attack you make, hit or miss, is an automatic critical threat that is automatically confirmed if it hits.",
                        "你每第4次武器攻击（无论前几次是否命中）自动成为重击威胁，命中时自动确认为重击。"))
                .AddComponent<EveryNthHitCritical>(c => c.Every = 4)
                .Configure();
        }

        private static void ConfigureRetaliation()
        {
            Feat(FeatSelection.Retaliation, "ReturnBlow", Guids.Dota.ReturnBlow, "Return Blow", "反伤",
                    Desc("Retaliation", "反击",
                        "Whenever an enemy damages you with a melee or ranged attack or a spell, it takes damage equal to half your character level plus your Strength modifier (minimum 1).",
                        "敌人以近战、远程攻击或法术对你造成伤害时，受到等于你角色等级一半加力量调整值的伤害（最低1）。"))
                .AddComponent<ReturnBlowDamage>()
                .Configure();

            Feat(FeatSelection.Retaliation, "KrakenShell", Guids.Dota.KrakenShell, "Kraken Shell", "海妖外壳",
                    Desc("Retaliation", "反击",
                        "You gain DR 1/— plus 1 per 4 character levels. After you take damage totalling a tenth of your maximum hit points, " +
                        "all harmful effects on you are removed and the count resets.",
                        "你获得1/—的伤害减免，每4角色等级再+1。累计受到相当于最大生命值十分之一的伤害后，移除你身上所有有害效果，并重新计数。"))
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel().WithStartPlusDivStepProgression(4, 0))
                .AddDamageResistancePhysical(isStackable: true, value: Common.Rank())
                .AddComponent<KrakenShellPurge>()
                .Configure();
        }

        private static void ConfigureGrowth()
        {
            var souls = BuffConfigurator.New("NecromasteryBuff", Guids.Dota.NecromasteryBuff)
                .SetDisplayName(Common.L("NecromasteryBuff.Name", "Souls", "灵魂"))
                .SetDescription(Common.L("NecromasteryBuff.Desc", "+1 damage per 2 souls.", "每2个灵魂伤害+1。"))
                .SetStacking(StackingType.Rank)
                .SetRanks(40)
                .AddContextRankConfig(ContextRankConfigs.BuffRank(Guids.Dota.NecromasteryBuff).WithDivStepProgression(2))
                .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
                .Configure();
            Feat(FeatSelection.Growth, "Necromastery", Guids.Dota.Necromastery, "Necromastery", "魂之挽歌",
                    Desc("Growth", "成长",
                        "Each enemy you kill grants a soul, up to twice your character level (and never more than 40). You deal +1 damage per 2 souls. Souls persist until you die.",
                        "你每击杀一名敌人获得一个灵魂，上限为角色等级的2倍（不超过40）。每2个灵魂伤害+1。灵魂保留到你死亡为止。"))
                .AddComponent<NecromasterySouls>(c => c.Buff = souls.ToReference<BlueprintBuffReference>())
                .Configure();
        }

        private static void ConfigureSummoner()
        {
            Feat(FeatSelection.Summoner, "SpiritLink", Guids.Dota.SpiritLink, "Spirit Link", "灵魂链接",
                    Desc("Summoner", "召唤师",
                        "Whenever you are healed, your summoned creatures within 30 feet heal the same amount. Whenever one of your summoned creatures is healed, you heal half that amount. " +
                        "Healing shared by this link is never shared again, so it does not bounce back and forth.",
                        "你受到治疗时，你30尺内的召唤物回复等量生命；你的召唤物受到治疗时，你回复其一半的生命。经由本链接传递的治疗不会再次传递，不会来回反弹。"))
                .AddComponent<SpiritLinkHealing>()
                .Configure();
        }
    }

    /// <summary>Spell slot helpers shared by the Arcana feats.</summary>
    internal static class SpellSlots
    {
        private static readonly AccessTools.FieldRef<Spellbook, int[]> Spontaneous = AccessTools.FieldRefAccess<Spellbook, int[]>("m_SpontaneousSlots");

        public static bool RestoreOne(UnitEntityData unit, int maxLevel)
        {
            foreach (var book in unit.Descriptor.Spellbooks)
            {
                for (var level = Math.Min(maxLevel, book.MaxSpellLevel); level >= 1; level--)
                {
                    if (book.Blueprint.Spontaneous)
                    {
                        if (book.GetSpontaneousSlots(level) >= book.GetSpellsPerDay(level)) continue;
                        book.RestoreSpontaneousSlots(level, 1);
                        return true;
                    }
                    var slot = book.GetMemorizedSpellSlots(level).FirstOrDefault(s => !s.Available && s.SpellShell != null);
                    if (slot == null) continue;
                    slot.Available = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>Removes the highest unspent slot; returns its level, or 0 if none.</summary>
        public static int BurnHighest(UnitEntityData unit)
        {
            foreach (var book in unit.Descriptor.Spellbooks.OrderByDescending(b => b.MaxSpellLevel))
            {
                for (var level = book.MaxSpellLevel; level >= 1; level--)
                {
                    if (book.Blueprint.Spontaneous)
                    {
                        var slots = Spontaneous(book);
                        if (slots == null || slots[level] <= 0) continue;
                        slots[level]--;
                        return level;
                    }
                    var slot = book.GetMemorizedSpellSlots(level).FirstOrDefault(s => s.Available && s.SpellShell != null);
                    if (slot == null) continue;
                    slot.Available = false;
                    return level;
                }
            }
            return 0;
        }

        public static int RemainingSpellLevels(UnitEntityData unit)
        {
            var total = 0;
            foreach (var book in unit.Descriptor.Spellbooks)
            {
                for (var level = 1; level <= book.MaxSpellLevel; level++)
                {
                    total += level * (book.Blueprint.Spontaneous
                        ? book.GetSpontaneousSlots(level)
                        : book.GetMemorizedSpellSlots(level).Count(s => s.Available && s.SpellShell != null));
                }
            }
            return total;
        }

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

        public static int CurrentRound()
            => Game.Instance.TurnBasedCombatController?.RoundNumber ?? (int)(Game.Instance.TimeController.GameTime.TotalSeconds / 6);
    }

    [TypeId("a1b2c3d4e5f60718293a4b5c6d7e8f90")]
    public class MissingHealthDamage : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IInitiatorRulebookSubscriber
    {
        public bool UseTarget;

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            var unit = UseTarget ? evt.AttackWithWeapon?.Target : Owner;
            if (unit == null || unit.MaxHP <= 0) return;
            var missingTenths = Math.Min(9, (int)(10 * (1 - (float)Math.Max(0, unit.HPLeft) / unit.MaxHP)));
            var bonus = missingTenths * (1 + Owner.Descriptor.Progression.CharacterLevel / 10);
            if (bonus > 0) evt.AddDamageModifier(bonus, Fact);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) { }
    }

    [TypeId("b2c3d4e5f60718293a4b5c6d7e8f90a1")]
    public class PercentHealthDamage : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookSubscriber
    {
        public bool FromCurrent;
        public bool NaturalOnly;
        public bool HealSelf;

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (!evt.AttackRoll.IsHit || evt.Target == null) return;
            if (NaturalOnly && evt.Weapon?.Blueprint.IsNatural != true) return;
            var amount = Amount(Owner, evt.Target, FromCurrent);
            SpellSlots.DealDirectDamage(Owner, evt.Target, amount);
            if (HealSelf) SpellSlots.Heal(Owner, Owner, amount);
        }

        internal static int Amount(UnitEntityData source, UnitEntityData target, bool fromCurrent)
        {
            var basis = fromCurrent ? Math.Max(0, target.HPLeft) * 3 / 100 : target.MaxHP * 2 / 100;
            return Math.Max(1, Math.Min(basis, 3 * source.Descriptor.Progression.CharacterLevel));
        }
    }

    [TypeId("2b7200e358a04b10833f725c9f490c39")]
    public class SpellFeastDamage : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IInitiatorRulebookSubscriber
    {
        [ThreadStatic] private static bool Feasting;
        private readonly HashSet<(object cast, UnitEntityData target)> Fed = new();

        public void OnEventAboutToTrigger(RuleDealDamage evt) { }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            var context = evt.Reason?.Context;
            if (Feasting || evt.Result <= 0 || evt.Target == null || evt.Reason?.Ability?.Spellbook == null || context == null) return;
            if (Fed.Count > 256) Fed.Clear();
            if (!Fed.Add((context, evt.Target))) return;
            Feasting = true;
            try
            {
                SpellSlots.DealDirectDamage(Owner, evt.Target, PercentHealthDamage.Amount(Owner, evt.Target, fromCurrent: false));
            }
            finally
            {
                Feasting = false;
            }
        }
    }

    [TypeId("ae9f467f31b949eab006ea1c3deea277")]
    public class WeaponLifesteal : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleDealDamage evt) { }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            if (evt.Result <= 0 || evt.DamageBundle.Weapon == null) return;
            SpellSlots.Heal(Owner, Owner, evt.Result * 15 / 100);
        }
    }

    [TypeId("a9344ef73caf4372aaf027e4644d0ce1")]
    public class BurningAura : UnitFactComponentDelegate, Kingmaker.Controllers.Units.ITickEachRound
    {
        public void OnNewRound()
        {
            if (!Owner.IsInCombat || Owner.State.IsDead || Owner.MaxHP <= 0) return;
            var level = Owner.Descriptor.Progression.CharacterLevel;
            var missing = 1 - (float)Math.Max(0, Owner.HPLeft) / Owner.MaxHP;
            var damage = (int)(Math.Max(1, level / 2) * (1 + missing));
            foreach (var enemy in GameHelper.GetTargetsAround(Owner.Position, 15.Feet(), checkLOS: false)
                .Where(unit => unit.IsEnemy(Owner) && !unit.State.IsDead).ToList())
            {
                Rulebook.Trigger(new RuleDealDamage(Owner, enemy,
                    new EnergyDamage(new DiceFormula(0, DiceType.Zero), damage, Kingmaker.Enums.Damage.DamageEnergyType.Fire)));
            }
        }
    }

    [TypeId("c3d4e5f60718293a4b5c6d7e8f90a1b2")]
    public class SlotHarvestOnKill : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IInitiatorRulebookSubscriber
    {
        private int LastRound = -1;

        public void OnEventAboutToTrigger(RuleDealDamage evt) { }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            var target = evt.Target;
            if (target == null || evt.Result <= 0 || !target.IsEnemy(Owner)) return;
            if (target.HPLeft > 0 || target.HPLeft + evt.Result <= 0) return;
            var round = SpellSlots.CurrentRound();
            if (round == LastRound) return;
            if (SpellSlots.RestoreOne(Owner, Math.Max(1, target.Descriptor.Progression.CharacterLevel / 2))) LastRound = round;
        }
    }

    [TypeId("d4e5f60718293a4b5c6d7e8f90a1b2c3")]
    public class ArcaneOrbDamage : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleDealDamage evt)
        {
            var ability = evt.Reason?.Ability;
            if (ability?.Spellbook == null || ability.SpellLevel != 0) return;
            var first = evt.DamageBundle.FirstOrDefault();
            var bonus = SpellSlots.RemainingSpellLevels(Owner) / 5;
            if (first != null && bonus > 0) first.AddModifier(bonus, Fact);
        }

        public void OnEventDidTrigger(RuleDealDamage evt) { }
    }

    [TypeId("e5f60718293a4b5c6d7e8f90a1b2c3d4")]
    public class EssenceFluxRefund : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCastSpell>, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCastSpell evt) { }

        public void OnEventDidTrigger(RuleCastSpell evt)
        {
            var spell = evt.Spell;
            if (!evt.Success || spell?.Spellbook == null || spell.SpellLevel < 1) return;
            if (UnityEngine.Random.value >= 0.15f) return;
            var book = spell.Spellbook;
            if (book.Blueprint.Spontaneous) book.RestoreSpontaneousSlots(spell.SpellLevel, 1);
            else if (spell.SpellSlot != null) spell.SpellSlot.Available = true;
        }
    }

    /// <summary>Burns one spell slot once per round and deals twice its level as damage.</summary>
    internal sealed class ManaBurner
    {
        private int LastRound = -1;

        public void TryBurn(UnitEntityData source, UnitEntityData target)
        {
            if (target == null || target == source) return;
            var round = SpellSlots.CurrentRound();
            if (LastRound == round) return;
            var level = SpellSlots.BurnHighest(target);
            if (level <= 0) return;
            LastRound = round;
            SpellSlots.DealDirectDamage(source, target, 2 * level);
        }
    }

    [TypeId("f60718293a4b5c6d7e8f90a1b2c3d4e5")]
    public class ManaBreakOnHit : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookSubscriber
    {
        private readonly ManaBurner Burner = new();

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.AttackRoll.IsHit) Burner.TryBurn(Owner, evt.Target);
        }
    }

    [TypeId("0269a2e308aa441fa6d157db1b6debb8")]
    public class ManaBreakOnSpell : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IInitiatorRulebookSubscriber
    {
        private readonly ManaBurner Burner = new();

        public void OnEventAboutToTrigger(RuleDealDamage evt) { }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            if (evt.Result > 0 && evt.Reason?.Ability?.Spellbook != null) Burner.TryBurn(Owner, evt.Target);
        }
    }

    [TypeId("0718293a4b5c6d7e8f90a1b2c3d4e5f6")]
    public class FervorTracker : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookSubscriber
    {
        public BlueprintBuffReference Buff;
        public BlueprintBuffReference AttackBuff;
        private UnitEntityData LastTarget;

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.Target == null) return;
            var buff = Buff.Get();
            if (evt.Target != LastTarget)
            {
                Owner.Buffs.RemoveFact(buff);
                Owner.Buffs.RemoveFact(AttackBuff.Get());
                LastTarget = evt.Target;
            }
            Owner.AddBuff(buff, Context, 2.Rounds().Seconds);
            if (Owner.Buffs.GetBuff(buff)?.Rank >= 3) Owner.AddBuff(AttackBuff.Get(), Context, 2.Rounds().Seconds);
        }
    }

    [TypeId("18293a4b5c6d7e8f90a1b2c3d4e5f607")]
    public class EveryNthHitCritical : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IInitiatorRulebookSubscriber
    {
        public int Every = 4;
        private int Hits;

        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (Hits == Every - 1)
            {
                evt.AutoCriticalThreat = true;
                evt.AutoCriticalConfirmation = true;
            }
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.Weapon != null) Hits = (Hits + 1) % Every;
        }
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
            Retaliating = true;
            try
            {
                var strength = Owner.Stats.Strength.Bonus;
                SpellSlots.DealDirectDamage(Owner, attacker, Math.Max(1, Owner.Descriptor.Progression.CharacterLevel / 2 + strength));
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

    [TypeId("4b5c6d7e8f90a1b2c3d4e5f60718293a")]
    public class NecromasterySouls : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IInitiatorRulebookSubscriber
    {
        public BlueprintBuffReference Buff;

        public void OnEventAboutToTrigger(RuleDealDamage evt) { }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            var target = evt.Target;
            if (target == null || evt.Result <= 0 || !target.IsEnemy(Owner)) return;
            if (target.HPLeft > 0 || target.HPLeft + evt.Result <= 0) return;
            var buff = Buff.Get();
            var cap = Math.Min(40, 2 * Owner.Descriptor.Progression.CharacterLevel);
            if ((Owner.Buffs.GetBuff(buff)?.Rank ?? 0) < cap) Owner.AddBuff(buff, Context);
        }
    }

    [TypeId("5c6d7e8f90a1b2c3d4e5f60718293a4b")]
    public class SpiritLinkHealing : UnitFactComponentDelegate, IGlobalRulebookHandler<RuleHealDamage>, IGlobalRulebookSubscriber
    {
        [ThreadStatic] private static bool Sharing;

        public void OnEventAboutToTrigger(RuleHealDamage evt) { }

        public void OnEventDidTrigger(RuleHealDamage evt)
        {
            if (Sharing || evt.Value <= 0 || evt.Target == null) return;
            Sharing = true;
            try
            {
                if (evt.Target == Owner)
                {
                    foreach (var summon in GameHelper.GetTargetsAround(Owner.Position, 30.Feet(), checkLOS: false)
                        .Where(unit => unit.Get<UnitPartSummonedMonster>()?.Summoner == Owner))
                        SpellSlots.Heal(Owner, summon, evt.Value);
                }
                else if (evt.Target.Get<UnitPartSummonedMonster>()?.Summoner == Owner)
                {
                    SpellSlots.Heal(Owner, Owner, evt.Value / 2);
                }
            }
            finally
            {
                Sharing = false;
            }
        }
    }
}
