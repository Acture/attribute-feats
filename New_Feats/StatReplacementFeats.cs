using System;
using System.Collections.Generic;
using BlueprintCore.Blueprints.Components.Replacements;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts.Restrictions;
using Kingmaker.EntitySystem.Properties;
using Kingmaker.EntitySystem.Properties.BaseGetter;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace AttributeFeats.New_Feats
{
    internal static class StatReplacementFeats
    {
        private static readonly WeaponSubCategory[] WeaponInsightCategories =
        {
            WeaponSubCategory.Melee,
            WeaponSubCategory.Ranged,
            WeaponSubCategory.Thrown,
            WeaponSubCategory.Natural,
        };

        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var weaponInsights = new List<BlueprintFeature>
            {
                CreateWeaponInsight(
                    StatType.Strength,
                    "CrushingForm",
                    Guids.Replacement.WeaponInsight.Str,
                    "WeaponInsight_Str.Name",
                    "Titan's Momentum",
                    "巨灵摧破",
                    "WeaponInsight_Str.Desc",
                    BuildWeaponInsightDescription(
                        "Strength",
                        "力量",
                        "<i>Force Finds the Gap.</i> You do not seek delicate angles when sheer mass can shatter them. Momentum and crushing weight turn every swing into an avalanche that drives through shields and armor alike.",
                        "<i>势破万钧。</i>不假精微机巧，唯仗移山之伟力。动量与蛮劲倾泻如崩山裂石，哪怕坚甲重盾亦在其霸道轰击下荡然无存。")),
                CreateWeaponInsight(
                    StatType.Dexterity,
                    "DuelistsEye",
                    Guids.Replacement.WeaponInsight.Dex,
                    "WeaponInsight_Dex.Name",
                    "Aldori Finesse",
                    "阿尔多里绝诣",
                    "WeaponInsight_Dex.Desc",
                    BuildWeaponInsightDescription(
                        "Dexterity",
                        "敏捷",
                        "<i>The Flow of the Blade.</i> Rooted in the legendary dueling disciplines of Restov, your weapon dances on the razor edge of balance. Grace and split-second precision exploit the faintest opening before an enemy can react.",
                        "<i>剑随游丝。</i>源自雷斯托夫剑爵的不传秘技。刃尖悬于分毫之隙，以绝伦灵敏与无瑕准度切入破绽，敌未觉察而已受剑创。")),
                CreateWeaponInsight(
                    StatType.Constitution,
                    "IronStance",
                    Guids.Replacement.WeaponInsight.Con,
                    "WeaponInsight_Con.Name",
                    "Stout Grounding",
                    "磐固千钧",
                    "WeaponInsight_Con.Desc",
                    BuildWeaponInsightDescription(
                        "Constitution",
                        "体质",
                        "<i>Anchor of Iron.</i> Like an ancient megalith facing tempest winds, you anchor every swing into the ground beneath your feet. Unwavering stamina and sheer physical density guide your weapon's trajectory true.",
                        "<i>立地生根。</i>如千古顽石迎击暴风狂澜。将周身耐力与磐固身躯与大地相联，以雄浑底力贯通兵刃，招式沉稳如岳、无懈可击。")),
                CreateWeaponInsight(
                    StatType.Intelligence,
                    "TacticalStrike",
                    Guids.Replacement.WeaponInsight.Int,
                    "WeaponInsight_Int.Name",
                    "Geometer's Edge",
                    "规矩之锋",
                    "WeaponInsight_Int.Desc",
                    BuildWeaponInsightDescription(
                        "Intelligence",
                        "智力",
                        "<i>The Measured Vector.</i> Trajectories, fulcrums, and velocities resolve into crisp geometrical lines before your mind's eye. Every cut is calculated with mathematical certainty, landing with devastating economy.",
                        "<i>规矩方圆。</i>在敏锐心智中，弹道、支点与轨迹化作严整的几何图谱。每一次挥击皆经精微筹算，以最小能耗切中命门死线。")),
                CreateWeaponInsight(
                    StatType.Wisdom,
                    "PredictiveCut",
                    Guids.Replacement.WeaponInsight.Wis,
                    "WeaponInsight_Wis.Name",
                    "Karmic Interception",
                    "因果断隙",
                    "WeaponInsight_Wis.Desc",
                    BuildWeaponInsightDescription(
                        "Wisdom",
                        "感知",
                        "<i>Severing the Intention.</i> Before an enemy's muscles can twitch to unleash an attack, your serene awareness has already perceived its karmic trajectory. You strike not where they are, but where their fate arrives.",
                        "<i>见微知著。</i>敌机方萌，心识已照。不待其筋肉发劲，超然直觉已先一步断定因果去向，截击于必经之隙。")),
                CreateWeaponInsight(
                    StatType.Charisma,
                    "TheatricalCombat",
                    Guids.Replacement.WeaponInsight.Cha,
                    "WeaponInsight_Cha.Name",
                    "Swashbuckler's Flourish",
                    "游侠华彩",
                    "WeaponInsight_Cha.Desc",
                    BuildWeaponInsightDescription(
                        "Charisma",
                        "魅力",
                        "<i>Dazzling Bravura.</i> Combat is a grand stage, and your audacious spectacle commands every eye. With hypnotic feints and sheer theatrical swagger, your weapon finds openings created by your irresistible presence.",
                        "<i>纵横惊鸿。</i>沙场如氍毹，气场压万夫。以华丽炫目的假动作与傲世豪情摄人心魄，使敌心神失据，兵刃顺势夺命。")),
            };

            for (var i = 0; i < weaponInsights.Count; i++)
            {
                for (var j = i + 1; j < weaponInsights.Count; j++)
                {
                    Common.AddBidirectionalMutex(weaponInsights[i], weaponInsights[j]);
                }
            }

            CreateExtendedFeat(
                StatType.Wisdom,
                StatType.AC,
                "InnerSentinel",
                Guids.Replacement.Extended.InnerSentinel,
                "Extended_InnerSentinel.Name",
                "Ascetic's Ward",
                "云水自真",
                "Extended_InnerSentinel.Desc",
                BuildExtendedDescription(
                    "Wisdom",
                    "感知",
                    "<i>Still at the Center.</i> In the tradition of ascetic hermits and monastery masters, true defense is not found in iron plates, but in absolute stillness of mind. The danger you perceive before it manifests is the danger that fails to touch you.",
                    "<i>身如止水。</i>承袭苦行宗师与山岳隐士的心法，真正的守御不在顽铁重甲，而在于明澈澄空的心意。见机于微者，刃锋莫能侵其分毫。",
                    "Adds your Wisdom modifier as an untyped bonus to AC while wearing no armor or light armor.",
                    "在未着甲或穿着轻甲时，将你的感知调整值作为无类型加值附加至防御等级（AC）。",
                    "Applies only while wearing no armor or light armor. This feat can be combined with other Extended Replacement feats.",
                    "仅在未着甲或穿着轻甲时生效。此专长可与其他“属性延展”专长正常叠加。"),
                CreateLightOrNoArmorRestriction());

            CreateExtendedFeat(
                StatType.Intelligence,
                StatType.AdditionalCMB,
                "CalculatedGrip",
                Guids.Replacement.Extended.CalculatedGrip,
                "Extended_CalculatedGrip.Name",
                "Anatomical Leverage",
                "筋络推演",
                "Extended_CalculatedGrip.Desc",
                BuildExtendedDescription(
                    "Intelligence",
                    "智力",
                    "<i>Leverage by Design.</i> Grappling, tripping, and disarming are mere physics applied to living anatomy. By calculating fulcrums, joint limits, and weight distribution, you topple giants with calculated efficiency.",
                    "<i>骨骼力学。</i>擒拿、摔跌与卸武，无非是作用于血肉机巧的力学解析。洞悉敌之关节支点与重心位移，便能以精微杠杆之力掀翻巍峨巨怪。",
                    "Adds your Intelligence modifier as an untyped bonus to CMB.",
                    "将你的智力调整值作为无类型加值附加至战技加值（CMB）。",
                    "This feat can be combined with other Extended Replacement feats.",
                    "此专长可与其他“属性延展”专长正常叠加。"));

            CreateExtendedFeat(
                StatType.Charisma,
                StatType.AdditionalCMD,
                "UnyieldingWill",
                Guids.Replacement.Extended.UnyieldingWill,
                "Extended_UnyieldingWill.Name",
                "Monarch's Stature",
                "帝胄岳立",
                "Extended_UnyieldingWill.Desc",
                BuildExtendedDescription(
                    "Charisma",
                    "魅力",
                    "<i>The Emperor's Gravity.</i> Sovereign presence radiates outward, imposing your will upon reality. Opponents who attempt to sweep, grapple, or displace you find themselves rebuffed by an insurmountable aura of authority.",
                    "<i>帝胄皇威。</i>至尊霸气油然而生，将无形威严化为立足乾坤的渊渟岳峙。妄图摔跌、纠缠或击退你的敌手，皆在其无上威压前铩羽受制。",
                    "Adds your Charisma modifier as an untyped bonus to CMD.",
                    "将你的魅力调整值作为无类型加值附加至战技防御（CMD）。",
                    "This feat can be combined with other Extended Replacement feats.",
                    "此专长可与其他“属性延展”专长正常叠加。"));

            CreateExtendedFeat(
                StatType.Strength,
                StatType.AdditionalCMD,
                "BrutalDefender",
                Guids.ExtendedReplacement2.BrutalDefender,
                "Extended_BrutalDefender.Name",
                "Titan's Footing",
                "巨灵固步",
                "Extended_BrutalDefender.Desc",
                BuildExtendedDescription(
                    "Strength",
                    "力量",
                    "<i>Rooted Mountain.</i> Like a rooted ironwood tree or a colossal titan, your colossal weight and sinew anchor directly into the stone. Foes attempting to leverage you off your feet succeed only in breaking their own leverage.",
                    "<i>生根盘石。</i>如古老铁木深扎重岩，如巨灵大君拔地参天。无可撼动的千钧骨肉锁固地脉，任何撼动你平衡的妄图，终将自行折断其杠杆。",
                    "Adds your Strength modifier as an untyped bonus to CMD.",
                    "将你的力量调整值作为无类型加值附加至战技防御（CMD）。",
                    "This feat can be combined with other Extended Replacement feats.",
                    "此专长可与其他“属性延展”专长正常叠加。"));

            CreateExtendedFeat(
                StatType.Dexterity,
                StatType.AC,
                "LightfootDefense",
                Guids.ExtendedReplacement2.LightfootDefense,
                "Extended_LightfootDefense.Name",
                "Zephyr's Grace",
                "穿风灵步",
                "Extended_LightfootDefense.Desc",
                BuildExtendedDescription(
                    "Dexterity",
                    "敏捷",
                    "<i>Untouched in Motion.</i> Flowing like the wind across a blade's edge, your evasive footwork never remains in the space a weapon falls. Steel and claws taste only empty air left in your wake.",
                    "<i>踏风无痕。</i>身如长风绕刃而转，步似流云游走无定。任何落向身侧的利刃与利爪，撕裂的不过是你留在原地的残影微尘。",
                    "Adds your Dexterity modifier as an untyped bonus to AC while wearing no armor or light armor.",
                    "在未着甲或穿着轻甲时，将你的敏捷调整值作为无类型加值附加至防御等级（AC）。",
                    "Applies only while wearing no armor or light armor. This feat can be combined with other Extended Replacement feats.",
                    "仅在未着甲或穿着轻甲时生效。此专长可与其他“属性延展”专长正常叠加。"),
                CreateLightOrNoArmorRestriction());

            CreateExtendedFeat(
                StatType.Constitution,
                StatType.HitPoints,
                "IronEndurance",
                Guids.ExtendedReplacement2.IronEndurance,
                "Extended_IronEndurance.Name",
                "Adamantine Mettle",
                "生机洪炉",
                "Extended_IronEndurance.Desc",
                BuildExtendedDescription(
                    "Constitution",
                    "体质",
                    "<i>Crucible of Vitality.</i> Your heart beats like a great forge bellows, filling veins with primeval resilience. Wounds that would fell lesser warriors merely stoke the inextinguishable furnace of your physical resolve.",
                    "<i>气血烘炉。</i>强韧的心脏如熔炉风箱轰鸣运转，将磅礴血气倾注周身筋脉。足以致寻常武者横死的重创，反更淬炼激荡出你体内源源不绝的不灭生机。",
                    "Adds your Constitution modifier as an untyped bonus to Hit Points.",
                    "将你的体质调整值作为无类型加值附加至生命值上限（HP）。",
                    "This feat can be combined with other Extended Replacement feats.",
                    "此专长可与其他“属性延展”专长正常叠加。"));
        }

        private static BlueprintFeature CreateWeaponInsight(
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

            AddWeaponInsightReplacements(cfg, baseStat);
            return cfg.Configure();
        }

        private static BlueprintFeature CreateExtendedFeat(
            StatType baseStat,
            StatType targetStat,
            string internalName,
            string guid,
            string nameKey,
            string nameEn,
            string nameZh,
            string descKey,
            (string en, string zh) desc,
            RestrictionCalculator restriction = null)
        {
            var cfg = FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L(nameKey, nameEn, nameZh))
                .SetDescription(Common.L(descKey, desc.en, desc.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(internalName);

            Common.AddRank(cfg, baseStat, AbilityRankType.Default, ContextRankProgression.AsIs);
            cfg.AddContextStatBonus(
                stat: targetStat,
                value: Common.Rank(),
                descriptor: ModifierDescriptor.None,
                restrictions: restriction);
            return cfg.Configure();
        }

        private static void AddWeaponInsightReplacements(FeatureConfigurator cfg, StatType baseStat)
        {
            foreach (var subCategory in WeaponInsightCategories)
            {
                cfg.AddAttackStatReplacementFixed(new AttackStatReplacementFixed(baseStat, subCategory));
            }
        }

        private static RestrictionCalculator CreateLightOrNoArmorRestriction()
            => new()
            {
                Property = new PropertyCalculator
                {
                    Operation = PropertyCalculator.OperationType.Sum,
                    TargetType = PropertyTargetType.CurrentEntity,
                    Getters = new PropertyGetter[]
                    {
                        new LightOrNoArmorPropertyGetter(),
                    },
                },
            };

        private static (string en, string zh) BuildWeaponInsightDescription(
            string statEn,
            string statZh,
            string loreEn,
            string loreZh)
        {
            var en = $"<i>Weapon Insight · {statEn}</i>\n{loreEn}\n\n<b>Effect:</b> Your weapon attack rolls use your {statEn} modifier instead of Strength or Dexterity whenever {statEn} would be better.\n\n<b>Restrictions:</b> Mutually exclusive with other Weapon Insight feats.";
            var zh = $"<i>武器洞察 · {statZh}</i>\n{loreZh}\n\n<b>效果：</b>当你的{statZh}调整值更高时，所有武器攻击检定均使用{statZh}调整值替代力量或敏捷调整值。\n\n<b>限制：</b>与其他“武器洞察”专长互斥。";
            return (en, zh);
        }

        private static (string en, string zh) BuildExtendedDescription(
            string statEn,
            string statZh,
            string loreEn,
            string loreZh,
            string effectEn,
            string effectZh,
            string restrictionEn,
            string restrictionZh)
        {
            var en = $"<i>Extended Replacement · {statEn}</i>\n{loreEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Restrictions:</b> {restrictionEn}";
            var zh = $"<i>属性延展 · {statZh}</i>\n{loreZh}\n\n<b>效果：</b>{effectZh}\n\n<b>限制：</b>{restrictionZh}";
            return (en, zh);
        }
    }

    [Serializable]
    internal sealed class LightOrNoArmorPropertyGetter : UnitPropertyGetter
    {
        protected override int GetBaseValue()
        {
            var unit = CurrentEntity;
            if (unit?.Body == null)
            {
                return 1;
            }

            foreach (var slot in unit.Body.CurrentEquipmentSlots)
            {
                if (slot is ArmorSlot armorSlot)
                {
                    if (!armorSlot.HasArmor)
                    {
                        return 1;
                    }

                    var proficiencyGroup = armorSlot.Armor?.Blueprint?.ProficiencyGroup ?? ArmorProficiencyGroup.None;
                    return proficiencyGroup == ArmorProficiencyGroup.None || proficiencyGroup == ArmorProficiencyGroup.Light ? 1 : 0;
                }
            }

            return 1;
        }

        protected override string GetInnerCaption() => "No or light armor";
    }
}
