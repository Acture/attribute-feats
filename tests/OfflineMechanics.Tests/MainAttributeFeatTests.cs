using System;
using System.Collections.Generic;
using System.Linq;
using WotR.Testing.Offline;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Xunit;
using Xunit.Sdk;

namespace AttributeFeats.OfflineTests
{
    /// <summary>
    /// Titan's Apotheosis (Main Attribute Mastery, Strength) on a real vanilla unit:
    /// add → game stat system shows the bonus → remove → every stat returns.
    /// </summary>
    [Collection(AttributeFeatsGameCollection.Name)]
    public sealed class MainAttributeFeatTests
    {
        // MC_Human_M_Cavalier_Base: a vanilla human pregen; its class levels are applied by the game's own AddClassLevels.
        internal const string VanillaUnit = "5def1061f2e84909a1235ac8bb42578f";

        // Documented effect (feat description): the Strength modifier, minimum 0, as an inherent bonus to the other
        // five attributes, AC, CMD, saves, initiative, CMB and skills/checks with default settings.
        private static readonly StatType[] OtherAttributes = { StatType.Dexterity, StatType.Constitution, StatType.Intelligence, StatType.Wisdom, StatType.Charisma };
        private static readonly StatType[] DirectlyModified =
        {
            StatType.AC, StatType.AdditionalCMD, StatType.SaveFortitude, StatType.SaveReflex, StatType.SaveWill, StatType.Initiative,
            StatType.AdditionalCMB, StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillPerception, StatType.SkillPersuasion,
            StatType.CheckBluff, StatType.CheckDiplomacy, StatType.CheckIntimidate,
        };

        private static BlueprintFeature Feat => OfflineGame.Blueprint<BlueprintFeature>(ModGuids.Get("str_main_to_everything"));

        private readonly AttributeFeatsGame fixture;

        public MainAttributeFeatTests(AttributeFeatsGame fixture) => this.fixture = fixture;

        [Fact]
        public void AddingAppliesStrengthModifierAndRemovingRestoresEveryStat()
        {
            fixture.RequireGame();
            var unit = OfflineGame.CreateUnit(VanillaUnit);
            var before = Snapshot(unit);
            var expected = StrengthModifier(unit);
            AssertLivingWithStrength(unit, expected);

            var fact = unit.Progression.Features.AddFeature(Feat);
            Assert.True(fact.IsActive && fact.IsTurnedOn, "The game did not activate the feat.");
            AssertMainAttributeEffect(unit, Feat, before, expected);
            fixture.Observations["mainAttribute.added"] = Describe(unit, before, expected);

            UnitHelper.RemoveFact(unit, fact);
            Assert.False(unit.Progression.Features.HasFact(Feat));
            Assert.Equal(before, Snapshot(unit));
            Assert.Empty(ModifiersFrom(unit, Feat));
        }

        [Fact]
        public void FailureControlWithoutStatComponentsFailsTheSameAssertion()
        {
            fixture.RequireGame();
            var feat = Feat;
            var original = feat.ComponentsArray;
            var removed = original.Count(c => c is AddContextStatBonus);
            Assert.True(removed > 0);
            try
            {
                // Deliberately remove the effect from the real blueprint; everything else stays the same.
                feat.ComponentsArray = original.Where(c => c is not AddContextStatBonus).ToArray();
                var unit = OfflineGame.CreateUnit(VanillaUnit);
                var before = Snapshot(unit);
                var expected = StrengthModifier(unit);
                var fact = unit.Progression.Features.AddFeature(feat);
                Assert.True(fact.IsActive, "The control feat must still be added; only its effect is missing.");

                var failure = Assert.ThrowsAny<XunitException>(() => AssertMainAttributeEffect(unit, feat, before, expected));
                fixture.Observations["mainAttribute.failureControl"] = new { removedComponents = removed, assertionMessage = failure.Message };
                UnitHelper.RemoveFact(unit, fact);
            }
            finally
            {
                feat.ComponentsArray = original;
            }
        }

        /// <summary>The single effect assertion shared by the normal case and the failure control.</summary>
        internal static void AssertMainAttributeEffect(UnitEntityData unit, BlueprintFeature feat, IReadOnlyDictionary<StatType, int> before, int expected)
        {
            foreach (var stat in OtherAttributes)
            {
                Assert.True(unit.Stats.GetStat(stat).ModifiedValue - before[stat] == expected,
                    $"{stat}: expected +{expected}, got {unit.Stats.GetStat(stat).ModifiedValue - before[stat]:+0;-0;0} ({DescribeStat(unit.Stats.GetStat(stat))})");
            }
            foreach (var stat in OtherAttributes.Concat(DirectlyModified))
            {
                var modifiers = unit.Stats.GetStat(stat).Modifiers.Where(m => m.Source?.Blueprint == feat).ToArray();
                Assert.True(modifiers.Length == 1, $"{stat}: expected one modifier from {feat.name}, found {modifiers.Length}");
                Assert.Equal(ModifierDescriptor.Inherent, modifiers[0].ModDescriptor);
                Assert.Equal(expected, modifiers[0].ModValue);
            }
            Assert.DoesNotContain(unit.Stats.Strength.Modifiers, m => m.Source?.Blueprint == feat);
            Assert.Equal(before[StatType.Strength], unit.Stats.Strength.ModifiedValue);
        }

        /// <summary>Undead and constructs have no Constitution in the game rules, so the documented Con bonus cannot apply to them.</summary>
        internal static void AssertLivingWithStrength(UnitEntityData unit, int expected)
        {
            var types = unit.Facts.List.Select(f => f.Blueprint.name).Where(n => n is "UndeadType" or "ConstructType").ToArray();
            Assert.True(types.Length == 0, $"Precondition: {unit.Blueprint.name} must be a living creature, has {string.Join(", ", types)}.");
            Assert.True(expected > 0, $"Precondition: {unit.Blueprint.name} needs a positive Strength modifier, has Strength {unit.Stats.Strength.ModifiedValue}.");
        }

        private static string DescribeStat(ModifiableValue stat)
            => $"base {stat.BaseValue}, modified {stat.ModifiedValue}, type {stat.GetType().Name}, modifiers: "
               + string.Join(", ", stat.Modifiers.Select(m => $"{m.ModDescriptor} {m.ModValue:+0;-0;0} from {m.Source?.Blueprint?.name ?? m.SourceComponent ?? "?"}"));

        /// <summary>Pathfinder ability modifier, independent of the mod: floor((score - 10) / 2), minimum 0 for this feat.</summary>
        internal static int StrengthModifier(UnitEntityData unit)
            => Math.Max(0, (int)Math.Floor((unit.Stats.Strength.ModifiedValue - 10) / 2.0));

        internal static Dictionary<StatType, int> Snapshot(UnitEntityData unit) => OfflineGame.StatSnapshot(unit);

        private static IEnumerable<ModifiableValue.Modifier> ModifiersFrom(UnitEntityData unit, BlueprintFeature feat)
            => Enum.GetValues(typeof(StatType)).Cast<StatType>().Distinct()
                .Select(unit.Stats.GetStat).Where(s => s != null)
                .SelectMany(s => s.Modifiers).Where(m => m.Source?.Blueprint == feat);

        private static object Describe(UnitEntityData unit, IReadOnlyDictionary<StatType, int> before, int expected)
            => new
            {
                unit = unit.Blueprint.name,
                strength = unit.Stats.Strength.ModifiedValue,
                expectedBonus = expected,
                changes = Snapshot(unit).Where(kv => before.TryGetValue(kv.Key, out var old) && old != kv.Value)
                    .ToDictionary(kv => kv.Key.ToString(), kv => $"{before[kv.Key]} -> {kv.Value}"),
            };
    }

    /// <summary>Reads blueprint GUIDs from the mod's own Guids class so the tests share its single source of truth.</summary>
    internal static class ModGuids
    {
        public static string Get(string path)
        {
            var type = typeof(AttributeFeats.Main).Assembly.GetType("AttributeFeats.New_Feats.Guids", throwOnError: true);
            var parts = path.Split('.');
            foreach (var nested in parts.Take(parts.Length - 1)) type = type.GetNestedType(nested, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            return (string)type.GetField(parts.Last()).GetRawConstantValue();
        }
    }
}
