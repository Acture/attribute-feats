using System.Collections.Generic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace AttributeFeats.New_Feats
{
    /// <summary>
    /// Creates the 24 Specialized Adept feats.
    /// </summary>
    internal static class SpecializedFeats
    {
        private static readonly ModifierDescriptor Desc = ModifierDescriptor.None;
        private static readonly StatType[] NoStats = new StatType[0];

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

        private static readonly StatType[] CheckStats =
        {
            StatType.CheckBluff,
            StatType.CheckDiplomacy,
            StatType.CheckIntimidate,
        };

        private static bool Initialized;

        private enum SpecializedFamily
        {
            Defensive,
            Maneuver,
            Skilled,
            Arcane,
        }

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var defensive = new[]
            {
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Strength,
                    DefenseStats,
                    Guids.Specialized.Defensive.Str,
                    "TitansStance",
                    "Colossus Bastion",
                    "巨灵重障",
                    "<i>Kinetic Displacement.</i> Your massive muscular bulk and grounded posture create a localized barrier of kinetic displacement. Strikes that would cleave a normal warrior shudder off your braced sinews like hail against an ironclad fortress gate.",
                    "<i>力场偏折。</i>以万钧身魄与深厚桩步筑成偏折动量之重障。足以劈山断岳之凶险斩击，落于千锤百炼之筋肉间，皆若飞雪叩击精钢要塞之门。"),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Dexterity,
                    DefenseStats,
                    Guids.Specialized.Defensive.Dex,
                    "FlowingForm",
                    "Wind-Dancer's Shroud",
                    "风舞虚影",
                    "<i>Aldori Cadence.</i> Moving with the sublime cadence of an Aldori swordlord, you do not block steel—you vacate the very space it aims to claim. The air itself seems to twist around your wake, blinding enemy aim.",
                    "<i>阿尔多里韵律。</i>承袭阿尔多里剑圣流派之神髓。从不与顽铁硬碰，唯自锋芒所向之方寸虚空中飘然而逝，旋身引风，教敌兵每每刺空。"),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Constitution,
                    DefenseStats,
                    Guids.Specialized.Defensive.Con,
                    "IronBulwark",
                    "Inured Carapace",
                    "百炼金身",
                    "<i>Living Scar-Tissue.</i> What does not kill you crystallizes within your marrow into adamantine density. Blades deflect from ossified muscle, and toxins break against blood that has conquered every known plague of Golarion.",
                    "<i>金石道体。</i>历尽千创百孔，骨骼肌理早已淬炼为晶化精金。刀劈钝折，诸毒退散，体内流淌之血脉曾踏平葛拉利昂之万般灾厄，百劫不坏。"),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Intelligence,
                    DefenseStats,
                    Guids.Specialized.Defensive.Int,
                    "CalculatedDefense",
                    "Analytical Aegis",
                    "算律之盾",
                    "<i>Trajectory Matrix.</i> To an analytical mind, an attack is simply a parabolic vector with a predictable apex. By calculating velocity, weapon balance, and reach before a blow connects, you step outside the arc where the blade cannot follow.",
                    "<i>弹道矩阵。</i>于大智者眼中，战阵击杀无非是带有既定抛物线之运动矢量。推演其势能、重心与盲区，先敌一步立于刃锋所不能及之算理死角。"),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Wisdom,
                    DefenseStats,
                    Guids.Specialized.Defensive.Wis,
                    "StoicVigilance",
                    "Third Eye Vigil",
                    "天目清照",
                    "<i>Preternatural Awareness.</i> Long meditation upon the ethereal currents has awakened an instinctive awareness that transcends eyesight. Malice and ambush are felt as distinct ripples in the air, giving you time to answer danger before it strikes.",
                    "<i>超然感通。</i>长年静坐参禅，神念早已透视以太微光。杀意未现，虚空已荡漾微澜；未卜先知，无论暗袭毒计抑或幻蛊邪法，皆在天目清照下无所遁形。"),
                CreateSpecialized(
                    SpecializedFamily.Defensive,
                    StatType.Charisma,
                    DefenseStats,
                    Guids.Specialized.Defensive.Cha,
                    "IndomitablePresence",
                    "Majesty's Reproach",
                    "凛然天威",
                    "<i>Sovereign Stature.</i> Your bearing radiates such haughty, indomitable nobility that striking you feels like an act of blasphemy. Foes waver as self-doubt and primal awe sap the lethal purpose from their swings.",
                    "<i>帝胄皇威。</i>顾盼间自生帝胄君临之威仪，令拔刃相向者如犯亵渎重罪。强敌心旌神摇，傲气杀意尽为这股凌厉天威所摄，兵刃犹疑难发。"),
            };

            var maneuver = new[]
            {
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Strength,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Str,
                    "CrushingGrip",
                    "Grip of the Behemoth",
                    "比蒙扼击",
                    "<i>Primal Leverage.</i> Channeling the primeval strength of the great beasts, your hands clamp onto armor, horns, and limbs like siege machinery, snapping bone and pinning foes through pure brute superiority.",
                    "<i>荒蛮扼杀。</i>运转上古荒蛮巨兽之凶性，五指并拢如攻城机枢生生嵌合。扣其铁甲、折其筋骨，以绝对蛮力将一切抵抗碾碎为泥。"),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Dexterity,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Dex,
                    "DeftHand",
                    "Fulcrum of the Viper",
                    "灵蛇巧掣",
                    "<i>Dynamic Redirection.</i> You never resist an opponent's momentum; you hook a wrist or sweep an ankle at the precise pivot point, converting their charge into a violent face-first collision with the earth.",
                    "<i>借势倾敌。</i>从不以力相抗，唯于千钧一发之际挑其腕节、绊其脚踝。顺水推舟，借敌之奔袭冲力反制其身，令其轰然仆地。"),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Constitution,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Con,
                    "UnyieldingHold",
                    "Deep-Root Clinch",
                    "沉洋扼锁",
                    "<i>Suffocating Asphyxiation.</i> Once your hold is established, you outlast your quarry's stamina completely. With the cold patience of a constrictor, your unyielding lungs grind down their desperate thrashing until they submit.",
                    "<i>长息窒竭。</i>一旦错身合锁，生机之深沉便成定局之锁钥。如巨蟒绞杀猎物般冷酷持久，以绵绵不绝之气血磨尽猎物的最后一丝挣扎。"),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Intelligence,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Int,
                    "TacticalBind",
                    "Anatomical Pivot",
                    "机理断节",
                    "<i>Skeletal Engineering.</i> By mapping the mechanical weak points of the humanoid and monstrous skeletal structure, you twist joints against their natural articulation with effortless efficiency.",
                    "<i>骸骨机枢。</i>观万灵骸骨如发条转轴。准确洞悉关节与肌腱之天然逆向，轻施巧劲反折其轴，令敌身不由己溃不成军。"),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Wisdom,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Wis,
                    "PredictiveLock",
                    "Crane's Anticipation",
                    "玄鹤听劲",
                    "<i>Attuned Weight.</i> Sensing where an opponent plans to step before their weight even transfers, you intercept their limbs at the initiation of their stride, turning their own intentions against them.",
                    "<i>神意先占。</i>神意通明，听劲化劲。在敌重心尚未转移之前已洞察其发力意向，于虚实转换之刹那截其锋芒，先发而制人。"),
                CreateSpecialized(
                    SpecializedFamily.Maneuver,
                    StatType.Charisma,
                    ManeuverStats,
                    Guids.Specialized.Maneuver.Cha,
                    "DomineeringThrow",
                    "Audacious Overthrow",
                    "叱喝倾山",
                    "<i>Theatrical Humiliation.</i> With a commanding roar and flamboyant disdain, you fling your opponent to the floor as if discarding worthless garbage, inspiring allies and demoralizing the enemy lines.",
                    "<i>万夫莫当。</i>叱咤声如惊雷破阵，举手投足尽展豪雄霸气。将披甲强敌若弃敝履般掷翻于地，把角力博弈化作震撼全场的凯旋宣告。"),
            };

            var skilled = new[]
            {
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Strength,
                    SkillStats,
                    Guids.Specialized.Skilled.Str,
                    "PracticedHand",
                    "Herculean Craft",
                    "赫拉克勒斯之工",
                    "<i>Forge-Tempered Labor.</i> Endless years of wielding massive sledgehammers, felling ancient ironwoods, and forcing unyielding mechanisms grant your mighty hands an unmatched, steady precision.",
                    "<i>千钧之工。</i>历经挥动万钧巨锤、开山拓荒之淬砺。浑厚膂力化作最稳健之基底，纵使最为繁难沉重之工巧，亦在绝对力量操弄下轻巧如愿。"),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Dexterity,
                    SkillStats,
                    Guids.Specialized.Skilled.Dex,
                    "EffortlessSkill",
                    "Thief-King's Panache",
                    "妙手绝尘",
                    "<i>Invisible Prestidigitation.</i> Your fingers weave through delicate tumblers, pickpocketing, and tumbling maneuvers with the sublime grace of falling silk, turning the hardest thievery into fine art.",
                    "<i>穿花妙手。</i>十指翻飞若落樱拂水，轻灵身法如穿林飞燕。锁簧微鸣、暗度陈仓，诸般繁琐杂学在绝妙敏捷催动下，皆成神乎其技的优雅艺术。"),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Constitution,
                    SkillStats,
                    Guids.Specialized.Skilled.Con,
                    "TirelessPractice",
                    "Ascetic Diligence",
                    "苦行研磨",
                    "<i>Mettle of the Anchorite.</i> Where brilliant scholars collapse from exhaustion and nimble artisans cramp with fatigue, your iron constitution allows you to study, practice, and refine your craft through endless sleepless vigils.",
                    "<i>铁砚磨穿。</i>凡夫智穷于神疲，巧匠力竭于形惫。唯汝凭金石气血苦度寒暑，彻夜推敲，将寻常百艺千锤百炼至通神化境。"),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Intelligence,
                    SkillStats,
                    Guids.Specialized.Skilled.Int,
                    "PolymathsTouch",
                    "Encyclopedic Synthesis",
                    "格物万象",
                    "<i>Universal Blueprint.</i> To you, all branches of knowledge are interconnected facets of one grand cosmic design. Planar astronomy decodes lost ruins, and alchemy illuminates the biology of ancient aberrations.",
                    "<i>博洽古今。</i>天下万法，殊途同归。以穷理尽性之哲思融会百家，以星相几何照彻古墓暗阁，以炼金秘要剖解异界畸变，博洽冠绝当世。"),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Wisdom,
                    SkillStats,
                    Guids.Specialized.Skilled.Wis,
                    "QuietMastery",
                    "Wanderer's Lucidity",
                    "云水澄明",
                    "<i>Primal Empathy.</i> You master the world not by reading parchment, but by listening to its subtle heartbeat. Instinctive clarity whispers the hidden path across mountains, deciphering deceit without words.",
                    "<i>谛听万籁。</i>行万里路，体万物情。不滞于文牍断章，而听山川呼吸、察人心微澜。灵台通明，虽不言而洞晓万方机巧。"),
                CreateSpecialized(
                    SpecializedFamily.Skilled,
                    StatType.Charisma,
                    SkillStats,
                    Guids.Specialized.Skilled.Cha,
                    "InspiredVersatility",
                    "Silver-Tongued Virtuoso",
                    "锦绣天潢",
                    "<i>Audacious Charlatanism.</i> Impeccable charm and overwhelming bravado bridge any chasm in your formal training. When you lie, analyze lore, or charm a king, the sheer brilliance of your performance makes reality conform.",
                    "<i>风华绝代。</i>卓绝自信与旷世风华跨越经验之鸿沟。无论是舌战群儒、密探宫闱，抑或弄巧弄险，举首投足间皆教举世景仰、化假为真。"),
            };

            var arcane = new[]
            {
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Strength,
                    NoStats,
                    Guids.Specialized.Arcane.Str,
                    "SpellForgedWill",
                    "Mage-Hammer Inscription",
                    "铁骨铸咒",
                    "<i>Kinetic Sorcery.</i> You bend unstable arcane currents across your weapon and limbs through sheer crushing force, hammering spells into reality like incandescent iron forged upon an enchanted anvil.",
                    "<i>蛮霸注能。</i>以刚猛筋骨强锁狂暴魔能，如神匠抡锤砸击赤铁，将磅礴杀伐之势熔铸于符文咒令之中，法理威猛霸道，莫可撄其锋。"),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Dexterity,
                    NoStats,
                    Guids.Specialized.Arcane.Dex,
                    "QuickcastReflex",
                    "Somatic Velocity",
                    "疾影手印",
                    "<i>Flicker Gestures.</i> Your somatic incantations unfold in blurred flourishes between heartbeats, releasing complex metamagic and spell-forms faster than enemy counter-mages can formulate a response.",
                    "<i>流光结印。</i>施法手势快逾流光惊鸿，指尖印契于刹那明灭间结成。敌方反制之咒尚未启唇，毁天灭地之法印已然呼啸破空。"),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Constitution,
                    NoStats,
                    Guids.Specialized.Arcane.Con,
                    "SpellTemperedBody",
                    "Crucible of the Conduit",
                    "鼎炉承法",
                    "<i>Living Leyline.</i> Your flesh and blood serve as an insulated crucible for the most volatile planar magics. Arcane backlash that would vaporize frail wizards is absorbed harmlessly into your boundless vitality.",
                    "<i>血肉熔炉。</i>将一身血肉铸就为吞吐天地灵潮之活体鼎炉。足以将凡俗法师撕裂融化的狂暴反噬，尽数被深沉生机纳为薪柴，咒力沉凝雄浑。"),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Intelligence,
                    NoStats,
                    Guids.Specialized.Arcane.Int,
                    "ScholarOfTheWeave",
                    "Archmage's Codex",
                    "万法源流",
                    "<i>The Prime Paradigm.</i> Magic is neither gift nor miracle; it is the ultimate science. By cross-referencing planar laws and ancient treatises, you cast with the unerring mathematical perfection of a high archmage.",
                    "<i>大奥术师真典。</i>魔法既非恩赐，亦非奇迹，乃虚空至高之严密数理。研索三千法则，条分缕析，令每一道法术皆如神来之笔，穷极奥术造化。"),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Wisdom,
                    NoStats,
                    Guids.Specialized.Arcane.Wis,
                    "OraclesIntuition",
                    "Gnostic Channel",
                    "玄鉴通幽",
                    "<i>Resonance of the Void.</i> You draw upon magic not by memorizing ink on sheepskin, but by tuning your soul to the primeval song that reverberates through the planes, giving your spells irresistible spiritual weight.",
                    "<i>通灵契道。</i>不滞死理，唯契天机。将神魂校准于漫贯万界的原初天籁，法随心动，言合天道，令群魔难脱此玄妙法网。"),
                CreateSpecialized(
                    SpecializedFamily.Arcane,
                    StatType.Charisma,
                    NoStats,
                    Guids.Specialized.Arcane.Cha,
                    "SorcerousPresence",
                    "Sovereign Decrees",
                    "天宪法旨",
                    "<i>Mandate of the Monarch.</i> The magical weave submits because you do not ask—you command. Your incantations ring with the terrifying resonance of the First Kings, making spell resistance wither before your absolute authority.",
                    "<i>神皇玉律。</i>诸界灵潮因吾言而俯首。咒言响遏行云，蕴藏太古龙皇与创世王侯之无上律令，所过之处法抗崩散、万象顺从。"),
            };

            AddFamilyMutex(defensive);
            AddFamilyMutex(maneuver);
            AddFamilyMutex(skilled);
            AddFamilyMutex(arcane);
        }

        private static BlueprintFeature CreateSpecialized(
            SpecializedFamily family,
            StatType baseStat,
            IReadOnlyList<StatType> stats,
            string guid,
            string internalName,
            string flavorNameEn,
            string flavorNameZh,
            string loreTextEn,
            string loreTextZh)
        {
            var settings = Main.Settings ?? new ModSettings();
            var attributeKey = GetAttributeKey(baseStat);
            var attributeNameEn = GetAttributeName(baseStat);
            var attributeNameZh = GetAttributeNameZh(baseStat);
            var familyKey = GetFamilyKey(family);

            var desc = BuildDescription(
                family,
                attributeNameEn,
                attributeNameZh,
                loreTextEn,
                loreTextZh);

            var cfg = FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"{familyKey}_{attributeKey}.Name", flavorNameEn, flavorNameZh))
                .SetDescription(Common.L(
                    $"{familyKey}_{attributeKey}.Desc",
                    desc.en,
                    desc.zh,
                    tagEncyclopediaEntries: true));

            Common.AddRank(cfg, baseStat, AbilityRankType.Default, Common.ResolveProgression(settings.powerLevel, ScalingIntent.Full));
            if (family == SpecializedFamily.Arcane)
            {
                Common.AddRank(cfg, baseStat, AbilityRankType.StatBonus, Common.ResolveProgression(settings.powerLevel, ScalingIntent.Half));
            }

            switch (family)
            {
                case SpecializedFamily.Defensive:
                    if (settings.EnableDefenses)
                    {
                        AddContextBonuses(cfg, stats, AbilityRankType.Default);
                    }
                    break;
                case SpecializedFamily.Maneuver:
                    if (settings.EnableManeuvers)
                    {
                        AddContextBonuses(cfg, stats, AbilityRankType.Default);
                    }
                    break;
                case SpecializedFamily.Skilled:
                    if (settings.EnableSkills)
                    {
                        AddContextBonuses(cfg, SkillStats, AbilityRankType.Default);
                    }

                    if (settings.EnableChecks)
                    {
                        AddContextBonuses(cfg, CheckStats, AbilityRankType.Default);
                    }
                    break;
                case SpecializedFamily.Arcane:
                    if (settings.EnableCasterLevel)
                    {
                        cfg.AddComponent<IncreaseCasterLevel>(c =>
                        {
                            c.Value = Common.Rank(AbilityRankType.Default);
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

                    if (settings.EnableSpellPenetration)
                    {
                        cfg.AddComponent<SpellPenetrationBonus>(c =>
                        {
                            c.Value = Common.Rank(AbilityRankType.Default);
                            c.Descriptor = Desc;
                        });
                    }
                    break;
            }

            cfg.AddRecalculateOnStatChange(stat: baseStat);
            return cfg.Configure();
        }

        private static void AddFamilyMutex(IReadOnlyList<BlueprintFeature> feats)
        {
            for (var i = 0; i < feats.Count; i++)
            {
                for (var j = i + 1; j < feats.Count; j++)
                {
                    Common.AddBidirectionalMutex(feats[i], feats[j]);
                }
            }
        }

        private static void AddContextBonuses(FeatureConfigurator cfg, IReadOnlyList<StatType> stats, AbilityRankType rankType)
        {
            foreach (var stat in stats)
            {
                cfg.AddContextStatBonus(stat, Common.Rank(rankType), Desc);
            }
        }

        private static (string en, string zh) BuildDescription(
            SpecializedFamily family,
            string attributeNameEn,
            string attributeNameZh,
            string loreTextEn,
            string loreTextZh)
        {
            var familyNameEn = GetFamilyDisplayName(family);
            var familyNameZh = GetFamilyDisplayNameZh(family);
            var effectEn = GetEffectText(family, attributeNameEn);
            var effectZh = GetEffectTextZh(family, attributeNameZh);
            var restrictionEn = GetRestrictionText(family);
            var restrictionZh = GetRestrictionTextZh(family);

            var en = $"<i>{familyNameEn} · {attributeNameEn}</i>\n{loreTextEn}\n\n<b>Effect:</b> {effectEn}\n\n<b>Restrictions:</b> {restrictionEn}";
            var zh = $"<i>{familyNameZh} · {attributeNameZh}</i>\n{loreTextZh}\n\n<b>效果：</b>{effectZh}\n\n<b>限制：</b>{restrictionZh}";
            return (en, zh);
        }

        private static string GetEffectText(SpecializedFamily family, string attributeName)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return $"Adds your {attributeName} modifier (untyped) to AC, CMD, Initiative, and all saving throws.";
                case SpecializedFamily.Maneuver:
                    return $"Adds your {attributeName} modifier (untyped) to CMB.";
                case SpecializedFamily.Skilled:
                    return $"Adds your {attributeName} modifier (untyped) to all skills, plus Bluff, Diplomacy, and Intimidate checks.";
                case SpecializedFamily.Arcane:
                    return $"Adds your {attributeName} modifier (untyped) to caster level and spell penetration checks, plus half your {attributeName} modifier to spell save DCs.";
                default:
                    return string.Empty;
            }
        }

        private static string GetEffectTextZh(SpecializedFamily family, string attributeZh)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return $"将你的{attributeZh}调整值（无类型加值）附加至防御等级（AC）、战路防御（CMD）、先攻及所有豁免检定。";
                case SpecializedFamily.Maneuver:
                    return $"将你的{attributeZh}调整值（无类型加值）附加至战路加值（CMB）。";
                case SpecializedFamily.Skilled:
                    return $"将你的{attributeZh}调整值（无类型加值）附加至所有技能检定，以及欺诈、交涉、威吓检定。";
                case SpecializedFamily.Arcane:
                    return $"将你的{attributeZh}调整值（无类型加值）附加至施法者等级与法术抗力穿透检定，并将半数{attributeZh}调整值附加至法术豁免DC。";
                default:
                    return string.Empty;
            }
        }

        private static string GetRestrictionText(SpecializedFamily family)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return "Mutually exclusive with other Defensive Adept feats.";
                case SpecializedFamily.Maneuver:
                    return "Mutually exclusive with other Maneuver Adept feats.";
                case SpecializedFamily.Skilled:
                    return "Mutually exclusive with other Skilled feats.";
                case SpecializedFamily.Arcane:
                    return "Mutually exclusive with other Arcane Insight feats.";
                default:
                    return string.Empty;
            }
        }

        private static string GetRestrictionTextZh(SpecializedFamily family)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return "与其他防御行家专长互相排斥。";
                case SpecializedFamily.Maneuver:
                    return "与其他战路行家专长互相排斥。";
                case SpecializedFamily.Skilled:
                    return "与其他技能行家专长互相排斥。";
                case SpecializedFamily.Arcane:
                    return "与其他奥术洞察专长互相排斥。";
                default:
                    return string.Empty;
            }
        }

        private static string GetFamilyKey(SpecializedFamily family)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return "Defensive";
                case SpecializedFamily.Maneuver:
                    return "Maneuver";
                case SpecializedFamily.Skilled:
                    return "Skilled";
                case SpecializedFamily.Arcane:
                    return "Arcane";
                default:
                    return family.ToString();
            }
        }

        private static string GetFamilyDisplayName(SpecializedFamily family)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return "Defensive Adept";
                case SpecializedFamily.Maneuver:
                    return "Maneuver Adept";
                case SpecializedFamily.Skilled:
                    return "Skilled";
                case SpecializedFamily.Arcane:
                    return "Arcane Insight";
                default:
                    return family.ToString();
            }
        }

        private static string GetFamilyDisplayNameZh(SpecializedFamily family)
        {
            switch (family)
            {
                case SpecializedFamily.Defensive:
                    return "防御行家";
                case SpecializedFamily.Maneuver:
                    return "战路行家";
                case SpecializedFamily.Skilled:
                    return "技能行家";
                case SpecializedFamily.Arcane:
                    return "奥术洞察";
                default:
                    return family.ToString();
            }
        }

        private static string GetAttributeKey(StatType baseStat)
        {
            switch (baseStat)
            {
                case StatType.Strength:
                    return "Str";
                case StatType.Dexterity:
                    return "Dex";
                case StatType.Constitution:
                    return "Con";
                case StatType.Intelligence:
                    return "Int";
                case StatType.Wisdom:
                    return "Wis";
                case StatType.Charisma:
                    return "Cha";
                default:
                    return baseStat.ToString();
            }
        }

        private static string GetAttributeName(StatType baseStat)
        {
            switch (baseStat)
            {
                case StatType.Strength:
                    return "Strength";
                case StatType.Dexterity:
                    return "Dexterity";
                case StatType.Constitution:
                    return "Constitution";
                case StatType.Intelligence:
                    return "Intelligence";
                case StatType.Wisdom:
                    return "Wisdom";
                case StatType.Charisma:
                    return "Charisma";
                default:
                    return baseStat.ToString();
            }
        }

        private static string GetAttributeNameZh(StatType baseStat)
        {
            switch (baseStat)
            {
                case StatType.Strength:
                    return "力量";
                case StatType.Dexterity:
                    return "敏捷";
                case StatType.Constitution:
                    return "体质";
                case StatType.Intelligence:
                    return "智力";
                case StatType.Wisdom:
                    return "感知";
                case StatType.Charisma:
                    return "魅力";
                default:
                    return baseStat.ToString();
            }
        }
    }
}
