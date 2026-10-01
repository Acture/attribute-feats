using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace AttributeFeats.New_Feats
{
    internal static class MainAbilityToEverything_Feats
    {
        private static readonly ModifierDescriptor Desc = ModifierDescriptor.Inherent;

        private static readonly StatType[] AttributeStats =
        {
            StatType.Strength,
            StatType.Dexterity,
            StatType.Constitution,
            StatType.Intelligence,
            StatType.Wisdom,
            StatType.Charisma,
        };

        private static readonly StatType[] DefenseStats =
        {
            StatType.AC,
            StatType.AdditionalCMD,
            StatType.SaveFortitude,
            StatType.SaveReflex,
            StatType.SaveWill,
            StatType.Initiative,
        };

        private static readonly StatType[] ManeuverStats =
        {
            StatType.AdditionalCMB,
        };

        private static readonly StatType[] BABStats =
        {
            StatType.BaseAttackBonus,
        };

        private static readonly StatType[] PowerStats =
        {
            StatType.AdditionalAttackBonus,
            StatType.AdditionalDamage,
            StatType.AttackOfOpportunityCount,
            StatType.SneakAttack,
            StatType.HitPoints,
            StatType.Speed,
        };

        private static readonly StatType[] CheckStats =
        {
            StatType.CheckBluff,
            StatType.CheckDiplomacy,
            StatType.CheckIntimidate,
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

        private static readonly StatType[] SkilledStats = SkillStats.Concat(CheckStats).ToArray();
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var feats = new List<BlueprintFeature>
            {
                CreateOne(
                    StatType.Strength,
                    "ApexPredator",
                    Guids.str_main_to_everything,
                    "MainAttr_Str.Name",
                    "Titan's Apotheosis",
                    "泰坦登阶",
                    "MainAttr_Str.Desc",
                    BuildDescription(
                        "Strength",
                        "力量",
                        "Primal Gravity.",
                        "To you, strength is not merely physical mass, but the foundational gravity by which reality holds together. Like the primordial titans who reshaped Golarion before the gods raised walls against the dark, every contest of power bows to your crushing sovereignty.",
                        "原初重力。",
                        "力之至极，并非筋肉之凡力，而是支撑现世运转的原初重力。正如神明筑墙封绝暗渊之前、挥手重塑山河的亘古泰坦，万象诸界的一切权柄皆在汝之霸力前俯首称臣。")),
                CreateOne(
                    StatType.Dexterity,
                    "EmbodiedGrace",
                    Guids.dex_main_to_everything,
                    "MainAttr_Dex.Name",
                    "Quicksilver Incarnate",
                    "水银具现",
                    "MainAttr_Dex.Desc",
                    BuildDescription(
                        "Dexterity",
                        "敏捷",
                        "Unanchored Motion.",
                        "You move with the frictionless purity of living quicksilver, slipping through the grasp of fate itself. Motion and stillness become choices you author, leaving the world to desperately strike at where you were a heartbeat ago.",
                        "不羁灵动。",
                        "身形宛若具现之水银，穿行于因果与杀伐的缝隙之间。动静起落皆随心生意动，任凭天地间刀光如幕，所及者唯有汝已逝去一刹之残影。")),
                CreateOne(
                    StatType.Constitution,
                    "LivingBulwark",
                    Guids.con_main_to_everything,
                    "MainAttr_Con.Name",
                    "Adamantine Vessel",
                    "万劫金身",
                    "MainAttr_Con.Desc",
                    BuildDescription(
                        "Constitution",
                        "体质",
                        "Crucible of the Mountain.",
                        "Your body is forged in the deep veins of the earth where adamantine forms under the crushing weight of continents. What mortals call agony or poison washes over you like rain upon granite, feeding a vitality that cannot be extinguished.",
                        "崇山熔炉。",
                        "此躯如在大陆重压下的地脉深处百炼而成的精金。凡人所谓之裂体剧痛与穿肠剧毒，落于汝身不过如山雨淋漓，反化作深不见底、万劫不灭之浩荡生机。")),
                CreateOne(
                    StatType.Intelligence,
                    "ArchitectOfSelf",
                    Guids.int_main_to_everything,
                    "MainAttr_Int.Name",
                    "Architect of Self",
                    "灵枢架构师",
                    "MainAttr_Int.Desc",
                    BuildDescription(
                        "Intelligence",
                        "智力",
                        "Anatomy of the Theorem.",
                        "In the grand tradition of Nexian arcane schematics, the mortal form is merely a rough theorem awaiting proof and optimization. Through rigorous calculation and sacred geometry, you re-inscribe your own physical laws to transcend mortal boundaries.",
                        "定理形构。",
                        "秉承内克斯奥术学派之玄思，凡胎肉身不过是待以理性规正之粗糙算式。借由严密数学与神圣几何之构析，汝重编身骨律法，令智慧之辉凌驾于血肉樊笼之上。")),
                CreateOne(
                    StatType.Wisdom,
                    "WellspringOfInsight",
                    Guids.wis_main_to_everything,
                    "MainAttr_Wis.Name",
                    "Ocular of the Cosmos",
                    "寰宇天心",
                    "MainAttr_Wis.Desc",
                    BuildDescription(
                        "Wisdom",
                        "感知",
                        "Still Waters of the Void.",
                        "Treading the contemplative discipline of Irori, you settle your consciousness into utter stillness. When the turbulence of ego subsides, the tides of causality and the whispers of the Great Beyond become as legible as ripples upon calm water.",
                        "灵渊止水。",
                        "步入兼爱明心之悟境，神思归于深渊古井般的终极寂静。当私欲波澜尽平，诸界因果的流转脉络与幽微隐兆皆洞若观火，如照平湖清漪，无所遁形。")),
                CreateOne(
                    StatType.Charisma,
                    "CrownOfWill",
                    Guids.cha_main_to_everything,
                    "MainAttr_Cha.Name",
                    "Sovereign of Wills",
                    "至高皇威",
                    "MainAttr_Cha.Desc",
                    BuildDescription(
                        "Charisma",
                        "魅力",
                        "The Royal Decree.",
                        "Reality bends not to physical lever or silent prayer, but to the audacity of imperial conviction. When you speak, the surrounding tapestry of fate yields to your proclamation, recognizing a soul whose sheer presence refuses to be denied.",
                        "王庭御令。",
                        "现世所屈从者，非蛮力之杠杆，亦非幽微之祷祝，乃傲岸不屈之皇道王权。言出法随，周遭命运之织锦皆遵汝之意志而易向，唯此真王不容置疑。")),
            };

            foreach (var feat in feats)
            {
                FeatureConfigurator.For(feat).Configure();
            }

            for (var i = 0; i < feats.Count; i++)
            {
                for (var j = i + 1; j < feats.Count; j++)
                {
                    Common.AddBidirectionalMutex(feats[i], feats[j]);
                }
            }
        }

        private static (string en, string zh) BuildDescription(
            string attrEn,
            string attrZh,
            string loreTitleEn,
            string loreBodyEn,
            string loreTitleZh,
            string loreBodyZh)
        {
            var en = $"<i>Main Attribute Mastery · {attrEn}</i>\n<i>{loreTitleEn}</i> {loreBodyEn}\n\n<b>Effect:</b> Adds your {attrEn} modifier as an inherent bonus to enabled attributes, defenses, maneuvers, skills/checks, caster bonuses, Base Attack Bonus, and Power Mode bonuses from the mod settings. If self-stacking is enabled it also adds to {attrEn}, and Reach becomes a fixed +1 when Power Mode is enabled.\n\n<b>Restrictions:</b> Mutually exclusive with the other Main Attribute Mastery feats.";
            var zh = $"<i>主属性专精 · {attrZh}</i>\n<i>{loreTitleZh}</i> {loreBodyZh}\n\n<b>效果：</b>将你的{attrZh}调整值作为固有加值，附加至模组设置中启用的属性、防御、战路、技能/检定、施法能力、基础攻击加值（BAB）及威力模式加成。若启用了自我属性叠加，则同样附加至{attrZh}；若启用了威力模式，触及范围固定增加1尺。\n\n<b>限制：</b>与其他主属性专精专长互相排斥。";
            return (en, zh);
        }

        private static BlueprintFeature CreateOne(
            StatType baseStat,
            string internalName,
            string guid,
            string nameKey,
            string nameEn,
            string nameZh,
            string descKey,
            (string en, string zh) desc)
        {
            var cfg = FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L(nameKey, nameEn, nameZh))
                .SetDescription(Common.L(descKey, desc.en, desc.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(internalName);

            var settings = Main.Settings ?? new ModSettings();
            Common.AddRank(cfg, baseStat, AbilityRankType.Default, Common.ResolveProgression(settings.powerLevel, ScalingIntent.Full));
            Common.AddRank(cfg, baseStat, AbilityRankType.StatBonus, Common.ResolveProgression(settings.powerLevel, ScalingIntent.Half));

            void AddContextBonuses(IEnumerable<StatType> stats, AbilityRankType rankType)
            {
                foreach (var stat in stats)
                {
                    cfg.AddComponent<AddContextStatBonus>(c =>
                    {
                        c.Stat = stat;
                        c.Descriptor = Desc;
                        c.Value = Common.Rank(rankType);
                    });
                }
            }

            if (settings.EnableAttributes)
            {
                var attributeStats = settings.IncludeSelfInAttributeStack
                    ? AttributeStats
                    : AttributeStats.Where(stat => stat != baseStat);
                AddContextBonuses(attributeStats, AbilityRankType.Default);
            }

            if (settings.EnableDefenses)
                AddContextBonuses(DefenseStats, AbilityRankType.Default);

            if (settings.EnableManeuvers)
                AddContextBonuses(ManeuverStats, AbilityRankType.Default);

            if (settings.EnableSkills && settings.EnableChecks)
            {
                AddContextBonuses(SkilledStats, AbilityRankType.Default);
            }
            else
            {
                if (settings.EnableSkills)
                    AddContextBonuses(SkillStats, AbilityRankType.Default);

                if (settings.EnableChecks)
                    AddContextBonuses(CheckStats, AbilityRankType.Default);
            }

            if (settings.EnableBAB)
                AddContextBonuses(BABStats, AbilityRankType.StatBonus);

            if (settings.EnablePowerMode)
            {
                AddContextBonuses(PowerStats, AbilityRankType.StatBonus);
                cfg.AddComponent<AddStatBonus>(c =>
                {
                    c.Stat = StatType.Reach;
                    c.Value = 1;
                    c.Descriptor = Desc;
                });
            }

            if (settings.EnableCasterDC)
            {
                cfg.AddComponent<IncreaseAllSpellsDC>(c =>
                {
                    c.Value = Common.Rank(AbilityRankType.StatBonus);
                    c.Descriptor = Desc;
                    c.SpellsOnly = false;
                });
            }

            if (settings.EnableCasterLevel)
            {
                cfg.AddComponent<IncreaseCasterLevel>(c =>
                {
                    c.Value = Common.Rank(AbilityRankType.Default);
                    c.Descriptor = Desc;
                });
            }

            if (settings.EnableSpellPenetration)
            {
                cfg.AddComponent<SpellPenetrationBonus>(c =>
                {
                    c.Value = Common.Rank(AbilityRankType.Default);
                    c.Descriptor = Desc;
                });
            }

            cfg.AddRecalculateOnStatChange(stat: baseStat);
            return cfg.Configure();
        }
    }
}
