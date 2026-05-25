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
            public SchoolFeatDefinition(string internalName, string displayName, string guid, SpellSchool school, StatType attribute, string loreText)
            {
                InternalName = internalName;
                DisplayName = displayName;
                Guid = guid;
                School = school;
                Attribute = attribute;
                LoreText = loreText;
            }

            public string InternalName { get; }
            public string DisplayName { get; }
            public string Guid { get; }
            public SpellSchool School { get; }
            public StatType Attribute { get; }
            public string LoreText { get; }
        }

        private sealed class DescriptorFeatDefinition
        {
            public DescriptorFeatDefinition(string internalName, string displayName, string guid, SpellDescriptor descriptor, StatType attribute, string loreText)
            {
                InternalName = internalName;
                DisplayName = displayName;
                Guid = guid;
                Descriptor = descriptor;
                Attribute = attribute;
                LoreText = loreText;
            }

            public string InternalName { get; }
            public string DisplayName { get; }
            public string Guid { get; }
            public SpellDescriptor Descriptor { get; }
            public StatType Attribute { get; }
            public string LoreText { get; }
        }

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            var schools = new[]
            {
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "PureWarder",
                    displayName: "Pure Warder",
                    guid: Guids.SpellTag.School.PureWarder,
                    school: SpellSchool.Abjuration,
                    attribute: StatType.Wisdom,
                    loreText: "<i>Wards Seen Before They Rise.</i> Insight lets you feel the fault lines in hostile magic before it forms, and every abjuration you weave lands with the certainty of a perfect seal.")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "MasterCaller",
                    displayName: "Master Caller",
                    guid: Guids.SpellTag.School.MasterCaller,
                    school: SpellSchool.Conjuration,
                    attribute: StatType.Charisma,
                    loreText: "<i>Command the Threshold.</i> Your presence bends the spaces between worlds, letting summoned allies and called powers answer as if your voice were law.")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "SeersEdge",
                    displayName: "Seer's Edge",
                    guid: Guids.SpellTag.School.SeersEdge,
                    school: SpellSchool.Divination,
                    attribute: StatType.Intelligence,
                    loreText: "<i>Knowledge Drawn First.</i> Analysis turns prophecy into a weapon; every divination you cast feels like the natural conclusion of facts others failed to read.")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "HeartsTyrant",
                    displayName: "Heart's Tyrant",
                    guid: Guids.SpellTag.School.HeartsTyrant,
                    school: SpellSchool.Enchantment,
                    attribute: StatType.Charisma,
                    loreText: "<i>Will as Sovereignty.</i> You do not persuade so much as define the mood of the room, pressing your personality into magic until resistance feels like disobedience.")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "Spellforge",
                    displayName: "Spellforge",
                    guid: Guids.SpellTag.School.Spellforge,
                    school: SpellSchool.Evocation,
                    attribute: StatType.Intelligence,
                    loreText: "<i>Calculated Conflagration.</i> You treat destructive magic as an engineered art, refining every blast through analysis until ruin arrives with mathematical certainty.")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "Veilweaver",
                    displayName: "Veilweaver",
                    guid: Guids.SpellTag.School.Veilweaver,
                    school: SpellSchool.Illusion,
                    attribute: StatType.Charisma,
                    loreText: "<i>Reality by Performance.</i> Your charm gives falsehood the weight of truth, making illusions linger like memories the world is afraid to question.")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "DeathSpeaker",
                    displayName: "Death Speaker",
                    guid: Guids.SpellTag.School.DeathSpeaker,
                    school: SpellSchool.Necromancy,
                    attribute: StatType.Wisdom,
                    loreText: "<i>Hear the Last Breath.</i> You understand the boundary between life and death with unsettling clarity, and necromancy obeys that insight like a whispered confession.")),
                CreateSchoolFeat(new SchoolFeatDefinition(
                    internalName: "ShapeShifter",
                    displayName: "Shape-Shifter",
                    guid: Guids.SpellTag.School.ShapeShifter,
                    school: SpellSchool.Transmutation,
                    attribute: StatType.Intelligence,
                    loreText: "<i>Form as Formula.</i> To you, flesh and matter are problems of structure; transmutation becomes simple once you know where reality's joints are hidden.")),
            };

            var descriptors = new[]
            {
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "InnerFlame",
                    displayName: "Inner Flame",
                    guid: Guids.SpellTag.Descriptor.InnerFlame,
                    descriptor: SpellDescriptor.Fire,
                    attribute: StatType.Charisma,
                    loreText: "<i>Authority of Embers.</i> Fire answers the certainty in your voice, surging hotter when your presence gives it permission to burn.")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "FrozenHeart",
                    displayName: "Frozen Heart",
                    guid: Guids.SpellTag.Descriptor.FrozenHeart,
                    descriptor: SpellDescriptor.Cold,
                    attribute: StatType.Wisdom,
                    loreText: "<i>Winter Without Tremor.</i> Calm insight turns frost into discipline, and every cold spell strikes with the stillness of perfect control.")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "StormChannel",
                    displayName: "Storm Channel",
                    guid: Guids.SpellTag.Descriptor.StormChannel,
                    descriptor: SpellDescriptor.Electricity,
                    attribute: StatType.Dexterity,
                    loreText: "<i>Lightning in Motion.</i> Agile instinct lets you catch the rhythm of the storm itself, guiding every arc with a duelist's precision.")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "EtchingMind",
                    displayName: "Etching Mind",
                    guid: Guids.SpellTag.Descriptor.EtchingMind,
                    descriptor: SpellDescriptor.Acid,
                    attribute: StatType.Intelligence,
                    loreText: "<i>Corrosion by Design.</i> Dissolution is simply logic made visible to you, and acid obeys the exact lines your scholarship says must fail first.")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "ResonantVoice",
                    displayName: "Resonant Voice",
                    guid: Guids.SpellTag.Descriptor.ResonantVoice,
                    descriptor: SpellDescriptor.Sonic,
                    attribute: StatType.Charisma,
                    loreText: "<i>Sound Given Command.</i> Tone bends around your presence until every word, note, and shockwave lands with irresistible force.")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "EthericMind",
                    displayName: "Etheric Mind",
                    guid: Guids.SpellTagDescriptor2.EthericMind,
                    descriptor: SpellDescriptor.Force,
                    attribute: StatType.Intelligence,
                    loreText: "<i>Thought Given Impact.</i> Your intellect gives raw force a precise vector, turning invisible pressure into a theorem the world is forced to obey.")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "RadiantSoul",
                    displayName: "Radiant Soul",
                    guid: Guids.SpellTagDescriptor2.RadiantSoul,
                    descriptor: PositiveEnergyDescriptor,
                    attribute: StatType.Charisma,
                    loreText: "<i>Grace That Rekindles.</i> Your presence makes renewal feel inevitable, and positive energy answers with a brilliance that floods every healing surge and sacred flare.")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "HollowHeart",
                    displayName: "Hollow Heart",
                    guid: Guids.SpellTagDescriptor2.HollowHeart,
                    descriptor: NegativeEnergyDescriptor,
                    attribute: StatType.Wisdom,
                    loreText: "<i>Stillness Beyond Breath.</i> You know the quiet shape left when life recedes, and negative energy follows that insight with chilling obedience.")),
                CreateDescriptorFeat(new DescriptorFeatDefinition(
                    internalName: "SubtleTyrant",
                    displayName: "Subtle Tyrant",
                    guid: Guids.SpellTagDescriptor2.SubtleTyrant,
                    descriptor: SpellDescriptor.MindAffecting,
                    attribute: StatType.Charisma,
                    loreText: "<i>Will Behind the Smile.</i> You lace every charm and compulsion with undeniable authority, making minds yield before they realize they were ever under siege.")),
            };

            AddFamilyMutex(schools);
            AddFamilyMutex(descriptors);
        }

        private static BlueprintFeature CreateSchoolFeat(SchoolFeatDefinition definition)
        {
            var schoolName = GetSchoolName(definition.School);
            var attributeName = GetAttributeName(definition.Attribute);
            var settings = Main.Settings ?? new ModSettings();
            var cfg = FeatureConfigurator.New(definition.InternalName, definition.Guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"SpellTag_School_{definition.InternalName}.Name", definition.DisplayName))
                .SetDescription(Common.L(
                    $"SpellTag_School_{definition.InternalName}.Desc",
                    BuildDescription(
                        schoolName,
                        attributeName,
                        definition.LoreText,
                        $"Adds half your {attributeName} modifier (untyped) to {schoolName} spell save DC and caster level, while all other schools take an equal penalty.",
                        "Mutually exclusive with other Spell Tag School Specialist feats, but can stack with Descriptor Specialist feats. Your mastery comes at a price — DC and caster level of all other schools suffer an equal penalty."),
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
            var descriptorName = GetDescriptorName(definition.Descriptor);
            var attributeName = GetAttributeName(definition.Attribute);
            var otherDescriptors = GetOtherDescriptorList(definition.Descriptor);
            var settings = Main.Settings ?? new ModSettings();
            var cfg = FeatureConfigurator.New(definition.InternalName, definition.Guid, FeatureGroup.Feat)
                .SetDisplayName(Common.L($"SpellTag_Descriptor_{definition.InternalName}.Name", definition.DisplayName))
                .SetDescription(Common.L(
                    $"SpellTag_Descriptor_{definition.InternalName}.Desc",
                    BuildDescription(
                        descriptorName,
                        attributeName,
                        definition.LoreText,
                        $"Adds half your {attributeName} modifier (untyped) to {descriptorName} spell save DC and caster level, while {otherDescriptors} spells take an equal penalty.",
                        $"Mutually exclusive with other Spell Tag Descriptor Specialist feats, but can stack with School Specialist feats. Your mastery comes at a price — DC and caster level of {otherDescriptors} spells suffer an equal penalty."),
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

        private static string BuildDescription(string tagName, string attributeName, string loreText, string effectText, string restrictionText)
            => $"<i>Spell Tag Specialist · {tagName} · {attributeName}</i>\n{loreText}\n\n<b>Effect:</b> {effectText}\n\n<b>Restrictions:</b> {restrictionText}";

        private static string GetSchoolName(SpellSchool school)
        {
            switch (school)
            {
                case SpellSchool.Abjuration:
                    return "Abjuration";
                case SpellSchool.Conjuration:
                    return "Conjuration";
                case SpellSchool.Divination:
                    return "Divination";
                case SpellSchool.Enchantment:
                    return "Enchantment";
                case SpellSchool.Evocation:
                    return "Evocation";
                case SpellSchool.Illusion:
                    return "Illusion";
                case SpellSchool.Necromancy:
                    return "Necromancy";
                case SpellSchool.Transmutation:
                    return "Transmutation";
                default:
                    return school.ToString();
            }
        }

        private static string GetDescriptorName(SpellDescriptor descriptor)
        {
            switch (descriptor)
            {
                case SpellDescriptor.Fire:
                    return "Fire";
                case SpellDescriptor.Cold:
                    return "Cold";
                case SpellDescriptor.Electricity:
                    return "Electricity";
                case SpellDescriptor.Acid:
                    return "Acid";
                case SpellDescriptor.Sonic:
                    return "Sonic";
                case SpellDescriptor.Force:
                    return "Force";
                case PositiveEnergyDescriptor:
                    return "Positive Energy";
                case NegativeEnergyDescriptor:
                    return "Negative Energy";
                case SpellDescriptor.MindAffecting:
                    return "Mind-Affecting";
                default:
                    return descriptor.ToString();
            }
        }

        private static string GetOtherDescriptorList(SpellDescriptor chosenDescriptor)
        {
            var names = new List<string>();
            foreach (var descriptor in Descriptors)
            {
                if (descriptor == chosenDescriptor) continue;
                names.Add(GetDescriptorName(descriptor));
            }

            return JoinWithAnd(names);
        }

        private static string JoinWithAnd(IReadOnlyList<string> values)
        {
            if (values.Count == 0) return string.Empty;
            if (values.Count == 1) return values[0];
            if (values.Count == 2) return $"{values[0]} and {values[1]}";

            var text = values[0];
            for (var i = 1; i < values.Count - 1; i++)
            {
                text += $", {values[i]}";
            }

            return $"{text}, and {values[values.Count - 1]}";
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
