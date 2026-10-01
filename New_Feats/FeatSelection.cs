using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;

namespace AttributeFeats.New_Feats
{
    internal sealed class FeatSelection
    {
        public static readonly FeatSelection Root = new("Root", Guids.AttributeFeatsSelection, "Attribute Feats");
        public static readonly FeatSelection MainAttribute = new("MainAttribute", Guids.FeatSelections.MainAttribute, "Main Attribute Mastery");
        public static readonly FeatSelection Defensive = new("Defensive", Guids.FeatSelections.Defensive, "Defensive Adept");
        public static readonly FeatSelection Maneuver = new("Maneuver", Guids.FeatSelections.Maneuver, "Maneuver Adept");
        public static readonly FeatSelection Skilled = new("Skilled", Guids.FeatSelections.Skilled, "Skilled Adept");
        public static readonly FeatSelection Arcane = new("Arcane", Guids.FeatSelections.Arcane, "Arcane Adept");
        public static readonly FeatSelection Stance = new("Stance", Guids.FeatSelections.Stance, "Stance");
        public static readonly FeatSelection Conditional = new("Conditional", Guids.FeatSelections.Conditional, "Conditional Trigger");
        public static readonly FeatSelection WeaponInsight = new("WeaponInsight", Guids.FeatSelections.WeaponInsight, "Weapon Insight");
        public static readonly FeatSelection ExtendedReplacement = new("ExtendedReplacement", Guids.FeatSelections.ExtendedReplacement, "Extended Replacement");
        public static readonly FeatSelection GreaterSummoning = new("GreaterSummoning", Guids.FeatSelections.GreaterSummoning, "Greater Summoning");
        public static readonly FeatSelection SummonerSacrifice = new("SummonerSacrifice", Guids.FeatSelections.SummonerSacrifice, "Summoner Sacrifice");
        public static readonly FeatSelection ReactiveArmor = new("ReactiveArmor", Guids.FeatSelections.ReactiveArmor, "Reactive Armor");
        public static readonly FeatSelection DerivedStat = new("DerivedStat", Guids.FeatSelections.DerivedStat, "Derived Stat Conversion");
        public static readonly FeatSelection SpellSchool = new("SpellSchool", Guids.FeatSelections.SpellSchool, "Spell School Specialist");
        public static readonly FeatSelection SpellDescriptor = new("SpellDescriptor", Guids.FeatSelections.SpellDescriptor, "Spell Descriptor Specialist");
        public static readonly FeatSelection DistanceDamage = new("DistanceDamage", Guids.FeatSelections.DistanceDamage, "Distance Damage");
        public static readonly FeatSelection WeaponDamage = new("WeaponDamage", Guids.FeatSelections.WeaponDamage, "Weapon Damage");

        private static readonly FeatSelection[] Families =
        {
            MainAttribute, Defensive, Maneuver, Skilled, Arcane, Stance, Conditional,
            WeaponInsight, ExtendedReplacement, GreaterSummoning, SummonerSacrifice,
            ReactiveArmor, DerivedStat, SpellSchool, SpellDescriptor, DistanceDamage, WeaponDamage,
        };

        private readonly string Name;
        private readonly string Guid;
        private readonly string DisplayName;
        private readonly Dictionary<BlueprintGuid, BlueprintFeature> Feats = new();
        private bool Configured;

        private FeatSelection(string name, string guid, string displayName)
        {
            Name = $"AttributeFeatsSelection_{name}";
            Guid = guid;
            DisplayName = displayName;
        }

        // Keep the Feat group for game mechanics, but expose each feat only inside its family.
        public FeatureConfigurator NewFeat(string name, string guid)
            => FeatureConfigurator.New(name, guid, FeatureGroup.Feat)
                .SkipAddToSelections()
                .OnConfigure(feat => Feats[feat.AssetGuid] = feat);

        // Parametrized configurators do not auto-register in outer selections.
        public ParametrizedFeatureConfigurator NewParametrizedFeat(string name, string guid)
            => ParametrizedFeatureConfigurator.New(name, guid)
                .SetGroups(FeatureGroup.Feat)
                .OnConfigure(feat => Feats[feat.AssetGuid] = feat);

        public static void ConfigureAll()
        {
            foreach (var family in Families)
            {
                family.Configure();
            }
            Root.Configure();
        }

        private void Configure()
        {
            if (Configured || Feats.Count == 0) return;

            var selection = FeatureSelectionConfigurator.New(Name, Guid, FeatureGroup.Feat)
                .SkipAddToSelections()
                .SetDisplayName(Common.L($"{Name}.Name", DisplayName))
                .SetDescription(Common.L(
                    $"{Name}.Description",
                    "Choose one feat. Each choice grants only the selected feat. " +
                    "You can return to this selection whenever you gain another feat; " +
                    "each feat's normal prerequisites and mutual-exclusion rules still apply."))
                .SetGroup(FeatureGroup.Feat)
                // Family selections can be revisited; only the final feats must be new.
                .SetMode(this == Root || this == WeaponDamage ? SelectionMode.Default : SelectionMode.OnlyNew)
                .SetIgnorePrerequisites(false)
                .SetAllFeatures(Feats.Values.Select(feat => (Blueprint<BlueprintFeatureReference>)feat).ToArray())
                .Configure();

            if (this == Root)
            {
                // Register only the root in the same outer lists as the original feats.
                FeatureSelectionConfigurator.For(selection, updateSelections: true).Configure();
            }
            else
            {
                Root.Feats[selection.AssetGuid] = selection;
            }
            Configured = true;
            Main.Log?.Log($"AttributeFeats: grouped {Feats.Count} options under {DisplayName}.");
        }
    }
}
