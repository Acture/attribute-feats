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
    /// <summary>Summoner: links between you and your summoned creatures.</summary>
    internal static class SummonerFeats
    {
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            ConfigureSummoner();
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
                        CombatActions.Heal(Owner, summon, evt.Value);
                }
                else if (evt.Target.Get<UnitPartSummonedMonster>()?.Summoner == Owner)
                {
                    CombatActions.Heal(Owner, Owner, evt.Value / 2);
                }
            }
            finally
            {
                Sharing = false;
            }
        }
    }
}
