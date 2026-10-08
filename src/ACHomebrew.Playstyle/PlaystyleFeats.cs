using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace ACHomebrew.Feats
{
    /// <summary>
    /// Batch 1 playstyle feats agreed in OSS-318: retaliation, momentum, stealth,
    /// lone wolf and permanent kill-based growth. Each feat covers one mechanic.
    /// </summary>
    internal static class PlaystyleFeats
    {
        private const string VanillaGreaterInvisibilityBuff = "e6b35473a237a6045969253beb09777c";
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            ConfigureRetaliation();
            ConfigureMomentum();
            ConfigureStealth();
            ConfigureSolo();
            ConfigureGrowth();
        }

        private static (string en, string zh) Desc(string familyEn, string familyZh, string effectEn, string effectZh)
            => ($"<i>{familyEn}</i>\n\n<b>Effect:</b> {effectEn}", $"<i>{familyZh}</i>\n\n<b>效果：</b>{effectZh}");

        private static FeatureConfigurator Feat(FeatSelection family, string name, string guid, string nameEn, string nameZh, (string en, string zh) desc)
            => family.NewFeat(name, guid)
                .SetDisplayName(Common.L($"{name}.Name", nameEn, nameZh))
                .SetDescription(Common.L($"{name}.Desc", desc.en, desc.zh, tagEncyclopediaEntries: true));

        private static BuffConfigurator Buff(string name, string guid, string nameEn, string nameZh, string descEn, string descZh)
            => BuffConfigurator.New(name, guid)
                .SetDisplayName(Common.L($"{name}.Name", nameEn, nameZh))
                .SetDescription(Common.L($"{name}.Desc", descEn, descZh));

        private static ContextDurationValue Rounds(int rounds) => ContextDuration.Fixed(rounds);

        private static void ConfigureRetaliation()
        {
            Feat(FeatSelection.Retaliation, "CounterAttack", Guids.Playstyle.CounterAttack, "Riposte Reflex", "还手本能",
                    Desc("Retaliation", "反击",
                        "When an enemy makes a melee attack against you, you make an attack of opportunity against it if it is within your reach. " +
                        "This uses one of your attacks of opportunity for the round, so Combat Reflexes increases how often you can counter.",
                        "敌人对你发动近战攻击时，若其在你的触及范围内，你对其发动一次借机攻击。这会消耗你本轮的借机攻击次数，因此战斗反射等能增加反击次数。"))
                .AddComponent<CounterAttackOnMelee>(c => c.Mode = CounterAttackMode.AnyAttack)
                .Configure();

            Feat(FeatSelection.Retaliation, "MissCounter", Guids.Playstyle.MissCounter, "Punish the Opening", "破绽反击",
                    Desc("Retaliation", "反击",
                        "Once per round, when an enemy's melee attack misses you, you make an attack against it if it is within your reach. " +
                        "This counterattack does not use your attacks of opportunity.",
                        "每轮一次，敌人对你的近战攻击未命中时，若其在你的触及范围内，你对其发动一次攻击。这次反击不消耗借机攻击次数。"))
                .AddComponent<CounterAttackOnMelee>(c => c.Mode = CounterAttackMode.FreeOnMiss)
                .Configure();

            var mark = Buff("PunisherMarkBuff", Guids.Playstyle.PunisherMarkBuff, "Marked for Punishment", "惩戒标记",
                    "This creature attacked someone with the Punisher feat this round.", "该生物本轮攻击过拥有惩戒者专长的角色。")
                .Configure();
            Feat(FeatSelection.Retaliation, "Punisher", Guids.Playstyle.Punisher, "Punisher", "惩戒者",
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
            Feat(FeatSelection.Retaliation, "ParryCrit", Guids.Playstyle.ParryCrit, "Read the Blade", "看破招架",
                    Desc("Retaliation", "反击",
                        "After you successfully parry an attack, your next attack within 2 rounds is an automatically confirmed critical hit.",
                        "成功招架一次攻击后，你在2轮内的下一次攻击自动成为确认的重击。"))
                .AddComponent<ApplyBuffOnParry>(c => c.Buff = crit.ToReference<BlueprintBuffReference>())
                .Configure();

            Feat(FeatSelection.Retaliation, "Shove", Guids.Playstyle.Shove, "Shoving Blows", "推搡",
                    Desc("Retaliation", "反击",
                        "After each of your weapon attacks and after each weapon attack against you, hit or miss, you attempt a bull rush against that enemy. " +
                        "This bull rush does not provoke attacks of opportunity.",
                        "你的每次武器攻击结算后，以及敌人对你的每次武器攻击结算后（无论是否命中），你对该敌人尝试一次冲撞。该冲撞不引发借机攻击。"))
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().CombatManeuver(ActionsBuilder.New(), CombatManeuver.BullRush))
                .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: ActionsBuilder.New().CombatManeuver(ActionsBuilder.New(), CombatManeuver.BullRush))
                .Configure();
        }

        private static void ConfigureMomentum()
        {
            var spree = Buff("KillingSpreeBuff", Guids.Playstyle.KillingSpreeBuff, "Killing Spree", "连杀",
                    "+2 on attack rolls and +10 feet speed per stack (up to 3 stacks).", "每层攻击检定+2、速度+10尺（最多3层）。")
                .SetStacking(StackingType.Rank)
                .SetRanks(3)
                .AddContextRankConfig(ContextRankConfigs.BuffRank(Guids.Playstyle.KillingSpreeBuff))
                .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable, multiplier: 2)
                .AddContextStatBonus(StatType.Speed, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable, multiplier: 10)
                .Configure();
            Feat(FeatSelection.Momentum, "KillingSpree", Guids.Playstyle.KillingSpree, "Killing Spree", "连杀",
                    Desc("Momentum", "动能",
                        "When your weapon attack drops an enemy, gain a Killing Spree stack until the end of your next turn: +2 on attack rolls and +10 feet speed per stack, up to 3 stacks.",
                        "你的武器攻击使敌人倒下时，获得一层连杀直到你下回合结束：每层攻击检定+2、速度+10尺，最多3层。"))
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().ApplyBuff(spree, Rounds(2)), actionsOnInitiator: true,
                    onlyHit: true, reduceHPToZero: true)
                .Configure();

            var rhythm = Buff("BattleRhythmBuff", Guids.Playstyle.BattleRhythmBuff, "Battle Rhythm", "战斗节奏",
                    "+1 on attack rolls and +2 damage per stack (up to 3 stacks).", "每层攻击检定+1、伤害+2（最多3层）。")
                .SetStacking(StackingType.Rank)
                .SetRanks(3)
                .AddContextRankConfig(ContextRankConfigs.BuffRank(Guids.Playstyle.BattleRhythmBuff))
                .AddContextStatBonus(StatType.AdditionalAttackBonus, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
                .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable, multiplier: 2)
                .Configure();
            Feat(FeatSelection.Momentum, "BattleRhythm", Guids.Playstyle.BattleRhythm, "Battle Rhythm", "战斗节奏",
                    Desc("Momentum", "动能",
                        "Each weapon hit you land adds a Battle Rhythm stack for 3 rounds: +1 on attack rolls and +2 damage per stack, up to 3 stacks. " +
                        "Each weapon hit you take removes one stack.",
                        "你每次武器命中获得一层战斗节奏，持续3轮：每层攻击检定+1、伤害+2，最多3层。你每被武器命中一次失去一层。"))
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().ApplyBuff(rhythm, Rounds(3)), actionsOnInitiator: true, onlyHit: true)
                .AddTargetAttackWithWeaponTrigger(actionOnSelf: ActionsBuilder.New().RemoveBuffSingleStack(rhythm), onlyHit: true)
                .Configure();
        }

        private static void ConfigureStealth()
        {
            var cooldown = Buff("ReStealthCooldownBuff", Guids.Playstyle.ReStealthCooldownBuff, "Fading Step (used)", "隐退（已触发）",
                    "Fading Step has triggered this round.", "隐退本轮已触发。")
                .Configure();
            var vanish = ActionsBuilder.New().Conditional(
                ConditionsBuilder.New().HasBuff(cooldown, negate: true),
                ifTrue: ActionsBuilder.New()
                    .ApplyBuff(VanillaGreaterInvisibilityBuff, Rounds(1))
                    .ApplyBuff(cooldown, Rounds(1)));
            Feat(FeatSelection.Stealth, "ReStealth", Guids.Playstyle.ReStealth, "Fading Step", "隐退",
                    Desc("Stealth", "潜行",
                        "Once per round, when your weapon attack drops an enemy, scores a critical hit, or deals sneak attack damage, you become greatly invisible for 1 round.",
                        "每轮一次，你的武器攻击使敌人倒下、造成重击或造成偷袭伤害时，你获得1轮高等隐身。"))
                .AddInitiatorAttackWithWeaponTrigger(action: vanish, actionsOnInitiator: true, onlyHit: true, reduceHPToZero: true)
                .AddInitiatorAttackWithWeaponTrigger(action: vanish, actionsOnInitiator: true, onlyHit: true, criticalHit: true)
                .AddInitiatorAttackWithWeaponTrigger(action: vanish, actionsOnInitiator: true, onlyHit: true, onlySneakAttack: true)
                .Configure();
        }

        private static void ConfigureSolo()
        {
            var buff = Buff("LoneWolfBuff", Guids.Playstyle.LoneWolfBuff, "Lone Wolf", "独狼",
                    "No ally is within 30 feet: dodge bonus to AC, bonus on saving throws and damage of +1 per 4 character levels (minimum +1).",
                    "30尺内没有队友：防御等级获得闪避加值，豁免与伤害获得加值，每4角色等级+1（最低+1）。")
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel(min: 1).WithDivStepProgression(4))
                .AddContextStatBonus(StatType.AC, Common.Rank(), descriptor: ModifierDescriptor.Dodge)
                .AddContextStatBonus(StatType.SaveFortitude, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
                .AddContextStatBonus(StatType.SaveReflex, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
                .AddContextStatBonus(StatType.SaveWill, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
                .AddContextStatBonus(StatType.AdditionalDamage, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
                .Configure();
            Feat(FeatSelection.Solo, "LoneWolf", Guids.Playstyle.LoneWolf, "Lone Wolf", "独狼",
                    Desc("Solo", "独行",
                        "While no conscious ally is within 30 feet of you, you gain a dodge bonus to AC and a bonus on saving throws and damage rolls " +
                        "equal to +1 per 4 character levels (minimum +1). Summoned creatures and pets count as allies. Checked each round.",
                        "30尺内没有清醒的队友时，你的防御等级获得闪避加值，豁免与伤害骰获得加值，每4角色等级+1（最低+1）。召唤物与宠物算作队友。每轮检查一次。"))
                .AddComponent<LoneWolfCheck>(c => c.Buff = buff.ToReference<BlueprintBuffReference>())
                .Configure();
        }

        private static void ConfigureGrowth()
        {
            Growth("EssenceShift", Guids.Playstyle.EssenceShift, Guids.Playstyle.EssenceShiftCounter, "Essence Thief", "精华窃取",
                StatType.Dexterity, 100, casterKillsOnly: false, partyKills: false,
                "Every 100 enemies you kill permanently increase your Dexterity by 1. There is no limit.",
                "你每击杀100名敌人，永久提升1点敏捷，没有上限。");
            Growth("FleshHeap", Guids.Playstyle.FleshHeap, Guids.Playstyle.FleshHeapCounter, "Flesh Heap", "腐肉堆积",
                StatType.Strength, 100, casterKillsOnly: false, partyKills: true,
                "Every 100 enemies killed by your party (including pets) permanently increase your Strength by 1. There is no limit.",
                "你的队伍（含宠物）每击杀100名敌人，你永久提升1点力量，没有上限。");
            Growth("ArcaneSiphon", Guids.Playstyle.ArcaneSiphon, Guids.Playstyle.ArcaneSiphonCounter, "Arcane Siphon", "智慧之刃",
                StatType.Intelligence, 10, casterKillsOnly: true, partyKills: false,
                "Every 10 spellcasting enemies you kill permanently increase your Intelligence by 1. There is no limit.",
                "你每击杀10名施法者敌人，永久提升1点智力，没有上限。");
            Growth("DevouredVigor", Guids.Playstyle.DevouredVigor, Guids.Playstyle.DevouredVigorCounter, "Devoured Vigor", "噬魂",
                StatType.HitPoints, 10, casterKillsOnly: false, partyKills: false,
                "Every 10 enemies you kill permanently increase your maximum hit points by 1. There is no limit.",
                "你每击杀10名敌人，永久提升1点生命上限，没有上限。");
        }

        private static void Growth(string name, string guid, string counterGuid, string nameEn, string nameZh, StatType stat, int killsPerPoint,
            bool casterKillsOnly, bool partyKills, string effectEn, string effectZh)
        {
            // The counter's rank is the kill count; it is saved with the character like any feature.
            var counter = FeatureConfigurator.New($"{name}Counter", counterGuid)
                .SetDisplayName(Common.L($"{name}Counter.Name", $"{nameEn} (kills)", $"{nameZh}（击杀数）"))
                .SetDescription(Common.L($"{name}Counter.Desc", effectEn, effectZh))
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetRanks(1000000)
                .AddContextRankConfig(ContextRankConfigs.FeatureRank(counterGuid).WithDivStepProgression(killsPerPoint))
                .AddContextStatBonus(stat, Common.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
                .Configure();
            Feat(FeatSelection.Growth, name, guid, nameEn, nameZh, Desc("Growth", "成长", effectEn, effectZh))
                .AddComponent<KillGrowth>(c =>
                {
                    c.Counter = counter.ToReference<BlueprintFeatureReference>();
                    c.CasterKillsOnly = casterKillsOnly;
                    c.PartyKills = partyKills;
                })
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

    /// <summary>Each round, applies the lone-wolf buff while no conscious ally is within 30 feet.</summary>
    [TypeId("7c9e1a3b5d7f4c2e8a0b6d4f2e1c3a5b")]
    public class LoneWolfCheck : UnitFactComponentDelegate, ITickEachRound
    {
        public BlueprintBuffReference Buff;

        protected override void OnActivate() => OnNewRound();

        protected override void OnDeactivate() => Owner.Buffs.RemoveFact(Buff.Get());

        public void OnNewRound()
        {
            var unit = Owner;
            var alone = !GameHelper.GetTargetsAround(unit.Position, 30.Feet(), checkLOS: false)
                .Any(other => other != unit && other.IsAlly(unit) && !other.State.IsUnconscious && !other.State.IsDead);
            var buff = Buff.Get();
            var has = Owner.Buffs.GetBuff(buff) != null;
            if (alone && !has) Owner.AddBuff(buff, Context);
            else if (!alone && has) Owner.Buffs.RemoveFact(buff);
        }
    }

    /// <summary>
    /// Counts kills by the owner (or, with PartyKills, by any party member or pet) in a hidden
    /// ranked feature that grants permanent bonuses. Listens globally to see party kills.
    /// </summary>
    [TypeId("5e3c1a9f7d5b4e3c1a9f7d5b3e1c9a7f")]
    public class KillGrowth : UnitFactComponentDelegate, IGlobalRulebookHandler<RuleDealDamage>, IGlobalRulebookSubscriber
    {
        public BlueprintFeatureReference Counter;
        public bool CasterKillsOnly;
        public bool PartyKills;

        public void OnEventAboutToTrigger(RuleDealDamage evt) { }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            var target = evt.Target;
            var killer = evt.Initiator;
            if (killer == null || (PartyKills ? !Kingmaker.Game.Instance.Player.PartyAndPets.Contains(killer) : killer != Owner)) return;
            if (target == null || evt.Result <= 0 || !target.IsEnemy(Owner)) return;
            // Count the hit that took the target from positive to zero or below only once.
            if (target.HPLeft > 0 || target.HPLeft + evt.Result <= 0) return;
            if (CasterKillsOnly && !target.Descriptor.Spellbooks.Any()) return;

            var counter = Counter.Get();
            var fact = Owner.Progression.Features.GetFact(counter);
            if (fact == null) Owner.Progression.Features.AddFeature(counter);
            else fact.AddRank();
        }
    }
}
