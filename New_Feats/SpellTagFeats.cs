using System.Collections.Generic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Parts;

namespace AttributeFeats.New_Feats
{
    internal static class SpellTagFeats
    {
        private static readonly ModifierDescriptor Desc = ModifierDescriptor.None;
        private const SpellDescriptor PositiveEnergyDescriptor = SpellDescriptor.Cure
            | SpellDescriptor.RestoreHP
            | SpellDescriptor.ChannelPositiveHeal
            | SpellDescriptor.ChannelPositiveHarm;
        private const SpellDescriptor NegativeEnergyDescriptor = SpellDescriptor.ChannelNegativeHeal
            | SpellDescriptor.ChannelNegativeHarm
            | SpellDescriptor.NegativeLevel;
        private static readonly SpellSchool[] Schools =
        {
            SpellSchool.Abjuration,
            SpellSchool.Conjuration,
            SpellSchool.Divination,
            SpellSchool.Enchantment,
            SpellSchool.Evocation,
            SpellSchool.Illusion,
            SpellSchool.Necromancy,
            SpellSchool.Transmutation,
        };

        private static readonly SpellDescriptor[] Descriptors =
        {
            SpellDescriptor.Fire,
            SpellDescriptor.Cold,
            SpellDescriptor.Electricity,
            SpellDescriptor.Acid,
            SpellDescriptor.Sonic,
            SpellDescriptor.Force,
            PositiveEnergyDescriptor,
            NegativeEnergyDescriptor,
            SpellDescriptor.MindAffecting,
        };

        private static bool Initialized;

        private sealed class SchoolFeatDefinition
        {
            public SchoolFeatDefinition(
                string internalName,
                string nameEn,
                string nameZh,
                string guid,
                SpellSchool school,
                StatType attribute,
                string loreEn,
                string loreZh)
            {
                InternalName = internalName;
                NameEn = nameEn;
                NameZh = nameZh;
                Guid = guid;
                School = school;
                Attribute = attribute;
                LoreEn = loreEn;
                LoreZh = loreZh;
            }

            public string InternalName { get; }
            public string NameEn { get; }
            public string NameZh { get; }
            public string Guid { get; }
            public SpellSchool School { get; }
            public StatType Attribute { get; }
            public string LoreEn { get; }
            public string LoreZh { get; }
        }

        private sealed class DescriptorFeatDefinition
        {
            public DescriptorFeatDefinition(
                string internalName,
                string nameEn,
                string nameZh,
                string guid,
                SpellDescriptor descriptor,
                StatType attribute,
                string loreEn,
                string loreZh)
            {
                InternalName = internalName;
                NameEn = nameEn;
                NameZh = nameZh;
                Guid = guid;
                Descriptor = descriptor;
                Attribute = attribute;
                LoreEn = loreEn;
                LoreZh = loreZh;
            }

            public string InternalName { get; }
            public string NameEn { get; }
            public string NameZh { get; }
            public string Guid { get; }
            public SpellDescriptor Descriptor { get; }
            public StatType Attribute { get; }
            public string LoreEn { get; }
            public string LoreZh { get; }
        }

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var schools = new[]
            {
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "PureWarder",
                    nameEn: "Aegis of the Pure Warder",
                    nameZh: "绝界镇魔使",
                    guid: Guids.SpellTag.School.PureWarder,
                    school: SpellSchool.Abjuration,
                    attribute: StatType.Wisdom,
                    loreEn: "<i>Wards Seen Before They Rise.</i> Serene spiritual insight lets you feel the subtle fault lines in hostile magic before it forms. Every abjuration you weave becomes an unyielding dimensional seal, turning hostile sorcery to ash against your wards.",
                    loreZh: "<i>预见先机。</i>超然神识洞悉虚空法力之脉络，先于敌咒成形前截断其施法回路。所布防护结界如万载铁幕，令狂暴敌法触之即溃、封镇虚空。")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "MasterCaller",
                    nameEn: "Sovereign Gatekeeper",
                    nameZh: "统界辟门者",
                    guid: Guids.SpellTag.School.MasterCaller,
                    school: SpellSchool.Conjuration,
                    attribute: StatType.Charisma,
                    loreEn: "<i>Command the Threshold.</i> The boundaries between the Great Beyond and the mortal world buckle before your imperial presence. When you summon beings or open spatial rifts, planar entities answer with unquestioning allegiance.",
                    loreZh: "<i>界门独尊。</i>多元宇宙与主位面间的界膜在你霸道威仪下屈服洞开。凡你所敕召之异界军团、所撕裂之空间裂隙，皆奉汝意志为至高法度。")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "SeersEdge",
                    nameEn: "Eye of the Chronomancer",
                    nameZh: "溯时先知",
                    guid: Guids.SpellTag.School.SeersEdge,
                    school: SpellSchool.Divination,
                    attribute: StatType.Intelligence,
                    loreEn: "<i>Knowledge Drawn First.</i> Rigorous chronological calculus turns prophecy into a lethal tactical weapon. Every divination you cast is the inevitable conclusion of causal sequences that lesser minds could never anticipate.",
                    loreZh: "<i>洞悉天机。</i>以无上灵台推演因果命运之严密算式。所有占卜预言皆如落子定局，将未来千万重变数化为洞若观火的必胜兵法。")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "HeartsTyrant",
                    nameEn: "Sovereign of the Heart",
                    nameZh: "倾心国主",
                    guid: Guids.SpellTag.School.HeartsTyrant,
                    school: SpellSchool.Enchantment,
                    attribute: StatType.Charisma,
                    loreEn: "<i>Will as Sovereignty.</i> You do not coax or charm; you simply declare reality, and the minds of others willingly conform. Your psychic magnetism is so absolute that submitting to your enchantment feels like natural devotion.",
                    loreZh: "<i>魅惑众生。</i>无需曲意逢迎，亦不必谄媚欺瞒；你的每一道敕令皆化为直击灵魂的崇高魅力，令受术者如饮甘饴、心悦诚服归顺座下。")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "Spellforge",
                    nameEn: "Pyre of the Architect",
                    nameZh: "灾变筑城师",
                    guid: Guids.SpellTag.School.Spellforge,
                    school: SpellSchool.Evocation,
                    attribute: StatType.Intelligence,
                    loreEn: "<i>Calculated Conflagration.</i> Destructive magic is not wild fury, but precision engineering. You refine every blast and conflagration through mathematical analysis until thermal yield and concussive shock arrive with devastating economy.",
                    loreZh: "<i>析构破灭。</i>狂暴的塑能毁灭非是蛮力渲泄，而是精密的法理重构。精准计算每一丝元素聚变与爆轰角度，以最纯粹的毁灭算式夷平阵列。")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "Veilweaver",
                    nameEn: "Phantasmagoria Maestro",
                    nameZh: "织影幻圣",
                    guid: Guids.SpellTag.School.Veilweaver,
                    school: SpellSchool.Illusion,
                    attribute: StatType.Charisma,
                    loreEn: "<i>Reality by Performance.</i> Your dramatic charisma gives falsehood the unyielding weight of truth. Phantasms and sensory tapestries spun from your imagination are so intoxicating that the universe itself confuses them for genuine reality.",
                    loreZh: "<i>虚实莫辨。</i>绝代风华赋予镜花水月以颠扑不破的现世重量。指尖勾勒的千般幻象与光影迷宫如此逼真动魄，纵使天地规则亦为之淆乱。")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "DeathSpeaker",
                    nameEn: "Harvester of the Boneyard",
                    nameZh: "冥河渡魂人",
                    guid: Guids.SpellTag.School.DeathSpeaker,
                    school: SpellSchool.Necromancy,
                    attribute: StatType.Wisdom,
                    loreEn: "<i>Hear the Last Breath.</i> Deep, unblinking insight into Pharasma's river of souls grants you authority over death's threshold. Necromancy is no profane violation to you, but a quiet, chilling dialogue with mortality.",
                    loreZh: "<i>冥渊引渡。</i>超然静观骨园与冥河滔滔逝水，彻悟死生大限之终极奥秘。死灵术非是亵渎之艺，而是于残躯碎魂间聆听死寂真理之权能。")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "ShapeShifter",
                    nameEn: "Sculptor of Prime Matter",
                    nameZh: "塑质造化使",
                    guid: Guids.SpellTag.School.ShapeShifter,
                    school: SpellSchool.Transmutation,
                    attribute: StatType.Intelligence,
                    loreEn: "<i>Form as Formula.</i> To you, physical matter, bone, and flesh are mere variable constants waiting for rebalancing. Transmutation becomes an exact architectural art once you comprehend where reality's bonds intersect.",
                    loreZh: "<i>万化由心。</i>在明睿智力之下，万物质性与生灵躯干皆为可重构的几何方程式。参透构象基石之枢纽，挥手间点石成金、化凡为圣。")),
            };

            var descriptors = new[]
            {
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "InnerFlame",
                    nameEn: "Pyre of the Phoenix",
                    nameZh: "炽皇凤涅",
                    guid: Guids.SpellTag.Descriptor.InnerFlame,
                    descriptor: SpellDescriptor.Fire,
                    attribute: StatType.Charisma,
                    loreEn: "<i>Authority of Embers.</i> Primeval flame recognizes the blaze in your spirit. Raging conflagrations surge with sovereign fury when your commanding aura bids them burn with incandescent heat.",
                    loreZh: "<i>真火耀世。</i>太古烈焰呼应灵魂深处的炽烈威仪。赤炎如凤皇展翼遮天蔽日，随你的皇者令咒将世间一切污秽化为漫天熔灰。")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "FrozenHeart",
                    nameEn: "Stillness of the Glacial Void",
                    nameZh: "极渊玄冰",
                    guid: Guids.SpellTag.Descriptor.FrozenHeart,
                    descriptor: SpellDescriptor.Cold,
                    attribute: StatType.Wisdom,
                    loreEn: "<i>Winter Without Tremor.</i> Meditative serenity cools the blood to absolute stillness. Your frost magic carries the quiet finality of ancient glaciers, extinguishing heat and life with sublime detachment.",
                    loreZh: "<i>玄渊绝灭。</i>超然古拙的入定心境令万物生机归于沉寂。玄冰寒潮如万古不化之冰川冷酷蔓延，以绝对静滞冰封一切喧嚣。")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "StormChannel",
                    nameEn: "Tempest-Dancer",
                    nameZh: "御雷疾影",
                    guid: Guids.SpellTag.Descriptor.StormChannel,
                    descriptor: SpellDescriptor.Electricity,
                    attribute: StatType.Dexterity,
                    loreEn: "<i>Lightning in Motion.</i> Like a lightning rod moving across a storm front, your blinding agility guides electrical arcs with the precision of a rapier thrust, discharging thunderbolts into vital vulnerabilities.",
                    loreZh: "<i>乘雷御电。</i>如疾电般灵动的步法穿梭于雷暴前沿。指尖引导的狂暴雷霆如决斗者的绝杀刺击，循着破绽顷刻灌注崩解之能。")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "EtchingMind",
                    nameEn: "Vitriolic Equation",
                    nameZh: "腐解算律",
                    guid: Guids.SpellTag.Descriptor.EtchingMind,
                    descriptor: SpellDescriptor.Acid,
                    attribute: StatType.Intelligence,
                    loreEn: "<i>Corrosion by Design.</i> Acid is molecular entropy made manifest. Your intellect calculates the precise chemical weaknesses of armor, flesh, and stone, unleashing caustic liquefaction that dissolves all matter.",
                    loreZh: "<i>蚀质天平。</i>强酸消融乃是分子结构崩解的明证。以精密的炼金数律解析护甲与骨肉之薄弱点，倾泻出将金石化为泥浆的剧毒蚀流。")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "ResonantVoice",
                    nameEn: "Herald of the Shattered Sky",
                    nameZh: "破霄神音",
                    guid: Guids.SpellTag.Descriptor.ResonantVoice,
                    descriptor: SpellDescriptor.Sonic,
                    attribute: StatType.Charisma,
                    loreEn: "<i>Sound Given Command.</i> Sonic vibration resonates in harmony with your imperious vocal timbre. A single uttered word sends shockwaves shattering crystal, steel, and eardrums like thunderclaps at sea.",
                    loreZh: "<i>破晓神啸。</i>声波震颤与君王之音完美谐振。真言一出，音浪化为实质的摧城狂涛，震碎金铁重甲，断敌筋骨神识。")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "EthericMind",
                    nameEn: "Axiom of Unseen Force",
                    nameZh: "虚空定则",
                    guid: Guids.SpellTagDescriptor2.EthericMind,
                    descriptor: SpellDescriptor.Force,
                    attribute: StatType.Intelligence,
                    loreEn: "<i>Thought Given Impact.</i> Pure arcane kinetic vectors materialize at your intellectual command. Unseen force solidifies into mathematical theorems that smash through shields and spatial barriers alike.",
                    loreZh: "<i>灵能定界。</i>将纯粹的思维修为固化为不可动摇的力场矢线。无形力场化作坚不可摧的定理重击，轰碎一切凡俗护盾与位面阻隔。")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "RadiantSoul",
                    nameEn: "Fountain of Solar Dawn",
                    nameZh: "金阳圣晖",
                    guid: Guids.SpellTagDescriptor2.RadiantSoul,
                    descriptor: PositiveEnergyDescriptor,
                    attribute: StatType.Charisma,
                    loreEn: "<i>Grace That Rekindles.</i> Boundless celestial radiance pours through your transcendent presence. Positive energy floods your allies with solar vitality while searing the undead with blazing holy fury.",
                    loreZh: "<i>昭昭如日。</i>宛若太阳晨曦般的圣洁光芒自你的灵魂奔涌而出。蓬勃生机抚平盟友伤痕，同时化作焚灭不死邪祟的炽烈金焰。")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "HollowHeart",
                    nameEn: "Vigil of the Gloom",
                    nameZh: "死寂枯荣",
                    guid: Guids.SpellTagDescriptor2.HollowHeart,
                    descriptor: NegativeEnergyDescriptor,
                    attribute: StatType.Wisdom,
                    loreEn: "<i>Stillness Beyond Breath.</i> You observe the entropic stillness left behind when life recedes. Negative energy obeys your quiet discernment with chilling fidelity, withering life into ash.",
                    loreZh: "<i>寂灭归墟。</i>深体生死循环、万象归寂之大道。阴邪死能如冥河幽流般如臂使指，无声无息间剥离血肉精气，令生者枯槁。")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "SubtleTyrant",
                    nameEn: "Puppeteer of the Mind",
                    nameZh: "惑魂主宰",
                    guid: Guids.SpellTagDescriptor2.SubtleTyrant,
                    descriptor: SpellDescriptor.MindAffecting,
                    attribute: StatType.Charisma,
                    loreEn: "<i>Will Behind the Smile.</i> Your hypnotic charisma entwines itself with hostile psyches like invisible silk. Long before the target realizes their thoughts are no longer their own, your mental dominion is already complete.",
                    loreZh: "<i>提线惑魂。</i>无孔不入的摄魂魅力如蛛丝般深植于敌手识海。敌方尚不自知心意已动，其神魂早已沦为受你肆意摆弄的提线木偶。")),
            };

            AddFamilyMutex(schools);
            AddFamilyMutex(descriptors);
        }

        private static BlueprintFeature CreateSchoolFeat(SchoolFeatDefinition definition)
        {
            var desc = BuildSchoolDescription(definition);
            var settings = Main.Settings ?? new ModSettings();
            var cfg = FeatureConfigurator.New(definition.InternalName, definition.Guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"SpellTag_School_{definition.InternalName}.Name", definition.NameEn, definition.NameZh))
                .SetDescription(Common.L(
                    $"SpellTag_School_{definition.InternalName}.Desc",
                    desc.en,
                    desc.zh,
                    tagEncyclopediaEntries: true));

            Common.AddRank(cfg, definition.Attribute, AbilityRankType.Default, ContextRankProgression.Div2);

            if (settings.EnableCasterDC)
            {
                cfg.AddComponent<ContextIncreaseSpellSchoolDC>(c =>
                {
                    c.School = definition.School;
                    c.BonusDC = Common.Rank();
                    c.Descriptor = Desc;
                });

                foreach (var otherSchool in Schools)
                {
                    if (otherSchool == definition.School) continue;
                    cfg.AddComponent<ContextIncreaseSpellSchoolDC>(c =>
                    {
                        c.School = otherSchool;
                        c.BonusDC = Common.Rank();
                        c.Multiplier = -1;
                        c.Descriptor = Desc;
                    });
                }
            }

            if (settings.EnableCasterLevel)
            {
                cfg.AddComponent<ContextIncreaseSpellSchoolCasterLevel>(c =>
                {
                    c.School = definition.School;
                    c.BonusCasterLevel = Common.Rank();
                    c.Descriptor = Desc;
                });

                foreach (var otherSchool in Schools)
                {
                    if (otherSchool == definition.School) continue;
                    cfg.AddComponent<ContextIncreaseSpellSchoolCasterLevel>(c =>
                    {
                        c.School = otherSchool;
                        c.BonusCasterLevel = Common.Rank();
                        c.Multiplier = -1;
                        c.Descriptor = Desc;
                    });
                }
            }

            cfg.AddRecalculateOnStatChange(stat: definition.Attribute);
            return cfg.Configure();
        }

        private static BlueprintFeature CreateDescriptorFeat(DescriptorFeatDefinition definition)
        {
            var desc = BuildDescriptorDescription(definition);
            var settings = Main.Settings ?? new ModSettings();
            var cfg = FeatureConfigurator.New(definition.InternalName, definition.Guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"SpellTag_Descriptor_{definition.InternalName}.Name", definition.NameEn, definition.NameZh))
                .SetDescription(Common.L(
                    $"SpellTag_Descriptor_{definition.InternalName}.Desc",
                    desc.en,
                    desc.zh,
                    tagEncyclopediaEntries: true));

            Common.AddRank(cfg, definition.Attribute, AbilityRankType.Default, ContextRankProgression.Div2);

            if (settings.EnableCasterDC)
            {
                cfg.AddComponent<ContextIncreaseSpellDescriptorDC>(c =>
                {
                    c.Descriptor = definition.Descriptor;
                    c.BonusDC = Common.Rank();
                    c.ModifierDescriptor = Desc;
                });

                foreach (var otherDescriptor in Descriptors)
                {
                    if (otherDescriptor == definition.Descriptor) continue;
                    cfg.AddComponent<ContextIncreaseSpellDescriptorDC>(c =>
                    {
                        c.Descriptor = otherDescriptor;
                        c.BonusDC = Common.Rank();
                        c.Multiplier = -1;
                        c.ModifierDescriptor = Desc;
                    });
                }
            }

            if (settings.EnableCasterLevel)
            {
                cfg.AddComponent<ContextIncreaseSpellDescriptorCasterLevel>(c =>
                {
                    c.Descriptor = definition.Descriptor;
                    c.BonusCasterLevel = Common.Rank();
                    c.ModifierDescriptor = Desc;
                });

                foreach (var otherDescriptor in Descriptors)
                {
                    if (otherDescriptor == definition.Descriptor) continue;
                    cfg.AddComponent<ContextIncreaseSpellDescriptorCasterLevel>(c =>
                    {
                        c.Descriptor = otherDescriptor;
                        c.BonusCasterLevel = Common.Rank();
                        c.Multiplier = -1;
                        c.ModifierDescriptor = Desc;
                    });
                }
            }

            cfg.AddRecalculateOnStatChange(stat: definition.Attribute);
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

        private static (string en, string zh) BuildSchoolDescription(SchoolFeatDefinition def)
        {
            var schoolEn = GetSchoolName(def.School, false);
            var schoolZh = GetSchoolName(def.School, true);
            var attrEn = GetAttributeName(def.Attribute, false);
            var attrZh = GetAttributeName(def.Attribute, true);

            var en = $"<i>Spell Tag · School Specialist · {schoolEn} · {attrEn}</i>\n{def.LoreEn}\n\n<b>Effect:</b> Adds half your {attrEn} modifier (untyped) to {schoolEn} spell save DC and caster level, while all other schools take an equal penalty.\n\n<b>Restrictions:</b> Mutually exclusive with other Spell Tag School Specialist feats, but can stack with Descriptor Specialist feats. Your mastery comes at a price — DC and caster level of all other schools suffer an equal penalty.";
            var zh = $"<i>法术专精 · 学派特化 · {schoolZh} · {attrZh}</i>\n{def.LoreZh}\n\n<b>效果：</b>将你的{attrZh}调整值的一半作为无类型加值附加至{schoolZh}法术的豁免难度（DC）与施法者等级，但所有其他学派的法术承受等额减值。\n\n<b>限制：</b>与其他“学派特化”专长互斥，但可与“描述符特化”专长叠加。极致的专精伴随代价——其他学派的法术DC与施法者等级将承受等额减值惩罚。";
            return (en, zh);
        }

        private static (string en, string zh) BuildDescriptorDescription(DescriptorFeatDefinition def)
        {
            var descEn = GetDescriptorName(def.Descriptor, false);
            var descZh = GetDescriptorName(def.Descriptor, true);
            var attrEn = GetAttributeName(def.Attribute, false);
            var attrZh = GetAttributeName(def.Attribute, true);
            var otherEn = GetOtherDescriptorList(def.Descriptor, false);
            var otherZh = GetOtherDescriptorList(def.Descriptor, true);

            var en = $"<i>Spell Tag · Descriptor Specialist · {descEn} · {attrEn}</i>\n{def.LoreEn}\n\n<b>Effect:</b> Adds half your {attrEn} modifier (untyped) to {descEn} spell save DC and caster level, while {otherEn} spells take an equal penalty.\n\n<b>Restrictions:</b> Mutually exclusive with other Spell Tag Descriptor Specialist feats, but can stack with School Specialist feats. Your mastery comes at a price — DC and caster level of {otherEn} spells suffer an equal penalty.";
            var zh = $"<i>法术专精 · 描述符特化 · {descZh} · {attrZh}</i>\n{def.LoreZh}\n\n<b>效果：</b>将你的{attrZh}调整值的一半作为无类型加值附加至{descZh}描述符法术的豁免难度（DC）与施法者等级，但{otherZh}法术承受等额减值。\n\n<b>限制：</b>与其他“描述符特化”专长互斥，但可与“学派特化”专长叠加。极致的专精伴随代价——{otherZh}法术的DC与施法者等级将承受等额减值惩罚。";
            return (en, zh);
        }

        private static string GetSchoolName(SpellSchool school, bool zh = false)
        {
            if (zh)
            {
                switch (school)
                {
                    case SpellSchool.Abjuration: return "防护系";
                    case SpellSchool.Conjuration: return "咒法系";
                    case SpellSchool.Divination: return "预言系";
                    case SpellSchool.Enchantment: return "惑控系";
                    case SpellSchool.Evocation: return "塑能系";
                    case SpellSchool.Illusion: return "幻术系";
                    case SpellSchool.Necromancy: return "死灵系";
                    case SpellSchool.Transmutation: return "变化系";
                    default: return school.ToString();
                }
            }
            switch (school)
            {
                case SpellSchool.Abjuration: return "Abjuration";
                case SpellSchool.Conjuration: return "Conjuration";
                case SpellSchool.Divination: return "Divination";
                case SpellSchool.Enchantment: return "Enchantment";
                case SpellSchool.Evocation: return "Evocation";
                case SpellSchool.Illusion: return "Illusion";
                case SpellSchool.Necromancy: return "Necromancy";
                case SpellSchool.Transmutation: return "Transmutation";
                default: return school.ToString();
            }
        }

        private static string GetDescriptorName(SpellDescriptor descriptor, bool zh = false)
        {
            if (zh)
            {
                switch (descriptor)
                {
                    case SpellDescriptor.Fire: return "火焰";
                    case SpellDescriptor.Cold: return "寒冷";
                    case SpellDescriptor.Electricity: return "电击";
                    case SpellDescriptor.Acid: return "强酸";
                    case SpellDescriptor.Sonic: return "音波";
                    case SpellDescriptor.Force: return "力场";
                    case PositiveEnergyDescriptor: return "正能量";
                    case NegativeEnergyDescriptor: return "负能量";
                    case SpellDescriptor.MindAffecting: return "影响心灵";
                    default: return descriptor.ToString();
                }
            }
            switch (descriptor)
            {
                case SpellDescriptor.Fire: return "Fire";
                case SpellDescriptor.Cold: return "Cold";
                case SpellDescriptor.Electricity: return "Electricity";
                case SpellDescriptor.Acid: return "Acid";
                case SpellDescriptor.Sonic: return "Sonic";
                case SpellDescriptor.Force: return "Force";
                case PositiveEnergyDescriptor: return "Positive Energy";
                case NegativeEnergyDescriptor: return "Negative Energy";
                case SpellDescriptor.MindAffecting: return "Mind-Affecting";
                default: return descriptor.ToString();
            }
        }

        private static string GetOtherDescriptorList(SpellDescriptor chosenDescriptor, bool zh = false)
        {
            var names = new List<string>();
            foreach (var descriptor in Descriptors)
            {
                if (descriptor == chosenDescriptor) continue;
                names.Add(GetDescriptorName(descriptor, zh));
            }

            return JoinWithAnd(names, zh);
        }

        private static string JoinWithAnd(IReadOnlyList<string> values, bool zh = false)
        {
            if (values.Count == 0) return string.Empty;
            if (zh)
            {
                return string.Join("、", values);
            }
            if (values.Count == 1) return values[0];
            if (values.Count == 2) return $"{values[0]} and {values[1]}";

            var text = values[0];
            for (var i = 1; i < values.Count - 1; i++)
            {
                text += $", {values[i]}";
            }

            return $"{text}, and {values[values.Count - 1]}";
        }

        private static string GetAttributeName(StatType baseStat, bool zh = false)
        {
            if (zh)
            {
                switch (baseStat)
                {
                    case StatType.Strength: return "力量";
                    case StatType.Dexterity: return "敏捷";
                    case StatType.Constitution: return "体质";
                    case StatType.Intelligence: return "智力";
                    case StatType.Wisdom: return "感知";
                    case StatType.Charisma: return "魅力";
                    default: return baseStat.ToString();
                }
            }
            switch (baseStat)
            {
                case StatType.Strength: return "Strength";
                case StatType.Dexterity: return "Dexterity";
                case StatType.Constitution: return "Constitution";
                case StatType.Intelligence: return "Intelligence";
                case StatType.Wisdom: return "Wisdom";
                case StatType.Charisma: return "Charisma";
                default: return baseStat.ToString();
            }
        }
    }

    [TypeId("4de42b15f8964e32935e82f193b5ce97")]
    internal class ContextIncreaseSpellSchoolDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public SpellSchool School;
        public ContextValue BonusDC;
        public int Multiplier = 1;
        public ModifierDescriptor Descriptor = ModifierDescriptor.None;

        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (!SpellTagMechanics.MatchesSchool(evt, School)) return;
            evt.AddBonusDC(BonusDC.Calculate(Context) * Multiplier, Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }
    }

    [TypeId("9ab3dc9b7ec340f7bf9863ffbbca2cb8")]
    internal class ContextIncreaseSpellSchoolCasterLevel : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public SpellSchool School;
        public ContextValue BonusCasterLevel;
        public int Multiplier = 1;
        public ModifierDescriptor Descriptor = ModifierDescriptor.None;

        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (!SpellTagMechanics.MatchesSchool(evt, School)) return;
            evt.AddBonusCasterLevel(BonusCasterLevel.Calculate(Context) * Multiplier, Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }
    }

    [TypeId("55318f5b9ee3432682d70a86ae58f0da")]
    internal class ContextIncreaseSpellDescriptorDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public SpellDescriptorWrapper Descriptor;
        public ContextValue BonusDC;
        public int Multiplier = 1;
        public ModifierDescriptor ModifierDescriptor = ModifierDescriptor.None;
        public bool SpellsOnly = false;

        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (SpellsOnly && evt.Spellbook == null) return;
            if (!SpellTagMechanics.MatchesDescriptor(evt, Owner, Descriptor)) return;
            evt.AddBonusDC(BonusDC.Calculate(Context) * Multiplier, ModifierDescriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }
    }

    [TypeId("13ee1911c67b4ddf8e7314f524968312")]
    internal class ContextIncreaseSpellDescriptorCasterLevel : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public SpellDescriptorWrapper Descriptor;
        public ContextValue BonusCasterLevel;
        public int Multiplier = 1;
        public ModifierDescriptor ModifierDescriptor = ModifierDescriptor.None;
        public bool SpellsOnly = false;

        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (SpellsOnly && evt.Spellbook == null) return;
            if (!SpellTagMechanics.MatchesDescriptor(evt, Owner, Descriptor)) return;
            evt.AddBonusCasterLevel(BonusCasterLevel.Calculate(Context) * Multiplier, ModifierDescriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }
    }

    internal static class SpellTagMechanics
    {
        public static bool MatchesSchool(RuleCalculateAbilityParams evt, SpellSchool school)
        {
            var spell = GetEffectiveSpell(evt);
            if (spell == null) return false;
            if (school == SpellSchool.None) return true;
            return spell.School == school || spell.SpellComponent != null && spell.SpellComponent.School == school;
        }

        public static bool MatchesDescriptor(RuleCalculateAbilityParams evt, UnitEntityData owner, SpellDescriptorWrapper descriptor)
        {
            if ((SpellDescriptor)descriptor == SpellDescriptor.None) return true;
            var effectiveDescriptor = GetEffectiveDescriptor(evt, owner);
            return effectiveDescriptor.HasAnyFlag((SpellDescriptor)descriptor);
        }

        public static SpellDescriptor GetEffectiveDescriptor(RuleCalculateAbilityParams evt, UnitEntityData owner)
        {
            var spell = GetEffectiveSpell(evt);
            if (spell == null) return SpellDescriptor.None;
            return UnitPartChangeSpellElementalDamage.ReplaceSpellDescriptorIfCan<UnitEntityData>(owner, spell.SpellDescriptor);
        }

        private static BlueprintAbility GetEffectiveSpell(RuleCalculateAbilityParams evt)
        {
            var convertedFrom = evt.AbilityData?.ConvertedFrom;
            if (convertedFrom?.Blueprint?.AbilityShadowSpell != null)
            {
                return convertedFrom.Blueprint;
            }

            return evt.Spell;
        }
    }
}
