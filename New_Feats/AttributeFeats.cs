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
                    Common.Text("MainAttr_Str.Name", "Titan's Apotheosis"),
                    Common.Text("MainAttr_Str.Name", "泰坦登阶", true),
                    "MainAttr_Str.Desc",
                    BuildDescription(
                        "Strength",
                        "力量",
                        Common.Text("MainAttr_Str.LoreTitle", "Primal Gravity."),
                        Common.Text("MainAttr_Str.Lore", "You train as though every task were a stone to be lifted, making strength the foundation of your whole bearing."),
                        Common.Text("MainAttr_Str.LoreTitle", "原初重力。", true),
                        Common.Text("MainAttr_Str.Lore", "你将每一次试炼都视作待举之石，以力量奠定身心的根基。", true))),
                CreateOne(
                    StatType.Dexterity,
                    "EmbodiedGrace",
                    Guids.dex_main_to_everything,
                    "MainAttr_Dex.Name",
                    Common.Text("MainAttr_Dex.Name", "Quicksilver Incarnate"),
                    Common.Text("MainAttr_Dex.Name", "水银具现", true),
                    "MainAttr_Dex.Desc",
                    BuildDescription(
                        "Dexterity",
                        "敏捷",
                        Common.Text("MainAttr_Dex.LoreTitle", "Unanchored Motion."),
                        Common.Text("MainAttr_Dex.Lore", "Balance, breath, and movement flow together like quicksilver, carrying your discipline beyond the blade."),
                        Common.Text("MainAttr_Dex.LoreTitle", "不羁灵动。", true),
                        Common.Text("MainAttr_Dex.Lore", "步法、呼吸与重心如水银般连贯，将敏捷的修习带到兵刃之外。", true))),
                CreateOne(
                    StatType.Constitution,
                    "LivingBulwark",
                    Guids.con_main_to_everything,
                    "MainAttr_Con.Name",
                    Common.Text("MainAttr_Con.Name", "Adamantine Vessel"),
                    Common.Text("MainAttr_Con.Name", "万劫金身", true),
                    "MainAttr_Con.Desc",
                    BuildDescription(
                        "Constitution",
                        "体质",
                        Common.Text("MainAttr_Con.LoreTitle", "Crucible of the Mountain."),
                        Common.Text("MainAttr_Con.Lore", "Years of hardship have taught your body to meet every ordeal with the patience of tempered metal."),
                        Common.Text("MainAttr_Con.LoreTitle", "崇山熔炉。", true),
                        Common.Text("MainAttr_Con.Lore", "多年的磨砺让你的身躯学会以百炼金属般的沉稳迎接试炼。", true))),
                CreateOne(
                    StatType.Intelligence,
                    "ArchitectOfSelf",
                    Guids.int_main_to_everything,
                    "MainAttr_Int.Name",
                    Common.Text("MainAttr_Int.Name", "Architect of Self"),
                    Common.Text("MainAttr_Int.Name", "灵枢架构师", true),
                    "MainAttr_Int.Desc",
                    BuildDescription(
                        "Intelligence",
                        "智力",
                        Common.Text("MainAttr_Int.LoreTitle", "Anatomy of the Theorem."),
                        Common.Text("MainAttr_Int.Lore", "You study your own habits like an arcane diagram, turning careful understanding into practiced control."),
                        Common.Text("MainAttr_Int.LoreTitle", "定理形构。", true),
                        Common.Text("MainAttr_Int.Lore", "你如研读奥术图谱般审视自身习惯，以理解换来娴熟的控制。", true))),
                CreateOne(
                    StatType.Wisdom,
                    "WellspringOfInsight",
                    Guids.wis_main_to_everything,
                    "MainAttr_Wis.Name",
                    Common.Text("MainAttr_Wis.Name", "Ocular of the Cosmos"),
                    Common.Text("MainAttr_Wis.Name", "寰宇天心", true),
                    "MainAttr_Wis.Desc",
                    BuildDescription(
                        "Wisdom",
                        "感知",
                        Common.Text("MainAttr_Wis.LoreTitle", "Still Waters of the Void."),
                        Common.Text("MainAttr_Wis.Lore", "Following the ideal of self-perfection associated with Irori, you make quiet attention the center of your practice."),
                        Common.Text("MainAttr_Wis.LoreTitle", "灵渊止水。", true),
                        Common.Text("MainAttr_Wis.Lore", "你借鉴伊洛里的自我完善之道，以宁静而专注的觉察统摄修习。", true))),
                CreateOne(
                    StatType.Charisma,
                    "CrownOfWill",
                    Guids.cha_main_to_everything,
                    "MainAttr_Cha.Name",
                    Common.Text("MainAttr_Cha.Name", "Sovereign of Wills"),
                    Common.Text("MainAttr_Cha.Name", "至高皇威", true),
                    "MainAttr_Cha.Desc",
                    BuildDescription(
                        "Charisma",
                        "魅力",
                        Common.Text("MainAttr_Cha.LoreTitle", "The Royal Decree."),
                        Common.Text("MainAttr_Cha.Lore", "Your confidence gathers scattered effort into a single purpose, as a sovereign gathers a wavering court."),
                        Common.Text("MainAttr_Cha.LoreTitle", "王庭御令。", true),
                        Common.Text("MainAttr_Cha.Lore", "你的自信将纷乱的行动凝成一个目标，宛如君主整肃摇摆的王庭。", true))),
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
            var en = $"<i>Main Attribute Mastery · {attrEn}</i>\n<i>{loreTitleEn}</i> {loreBodyEn}\n\n<b>Effect:</b> Uses your {attrEn} modifier, minimum 0, for the bonuses enabled in mod settings. Stat bonuses use the inherent type. Attributes, defenses, maneuvers, skills/checks, caster level, and spell penetration use the full modifier. Spell and ability save DC, Base Attack Bonus, and rank-based Power Mode bonuses use half the modifier (rounded down) in Balanced mode and the full modifier in Legacy_AllFull. If self-stacking is enabled it also adds to {attrEn}, and Reach becomes a fixed +1 foot when Power Mode is enabled.\n\n<b>Restrictions:</b> When EnableMutex is enabled, mutually exclusive with the other Main Attribute Mastery feats.";
            var zh = $"<i>主属性专精 · {attrZh}</i>\n<i>{loreTitleZh}</i> {loreBodyZh}\n\n<b>效果：</b>以你的{attrZh}调整值（最低0）计算模组设置启用的加成；属性类加值使用固有类型。属性、防御、战技、技能/检定、施法者等级与法术抗力穿透使用完整调整值；法术及能力的豁免DC、基础攻击加值（BAB）与按调整值成长的威力模式加成，在Balanced模式下使用半数调整值（向下取整），在Legacy_AllFull模式下使用完整调整值。若启用了自我属性叠加，则同样附加至{attrZh}；若启用了威力模式，触及范围固定增加1英尺。\n\n<b>限制：</b>启用EnableMutex时，与其他主属性专精专长互相排斥。";
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
