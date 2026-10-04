using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace AttributeFeats.New_Feats
{
    internal enum ScalingIntent
    {
        Full,
        Half,
    }

    internal static class Common
    {
        private static readonly ConditionalWeakTable<FeatureConfigurator, HashSet<string>> RankRegistrations = new();

        public static LocalizedString L(string key, string value, bool tagEncyclopediaEntries = false)
            => LocalizationTool.CreateString(key, value, tagEncyclopediaEntries);

        public static ContextValue Rank(AbilityRankType type = AbilityRankType.Default)
            => new()
            {
                ValueType = ContextValueType.Rank,
                ValueRank = type,
            };

        public static void AddRank(FeatureConfigurator cfg, StatType baseStat, AbilityRankType type, ContextRankProgression prog)
        {
            var registrations = RankRegistrations.GetValue(cfg, _ => new HashSet<string>());
            var key = $"{baseStat}:{type}";
            if (!registrations.Add(key))
            {
                Main.Log?.Log($"AttributeFeats: duplicate ContextRankConfig skipped for {baseStat}/{type}.");
                return;
            }

            var config = ContextRankConfigs.StatBonus(baseStat, ModifierDescriptor.None, type, min: 0);
            switch (prog)
            {
                case ContextRankProgression.AsIs:
                    break;
                case ContextRankProgression.Div2:
                    config = config.WithDiv2Progression();
                    break;
                case ContextRankProgression.HalfMore:
                    config = config.WithHalfMoreProgression();
                    break;
                default:
                    Main.Log?.Log($"AttributeFeats: unsupported progression {prog}, falling back to AsIs.");
                    break;
            }

            cfg.AddContextRankConfig(config);
        }

        public static ContextRankProgression ResolveProgression(PowerLevel powerLevel, ScalingIntent intent)
            => (powerLevel, intent) switch
            {
                (PowerLevel.Balanced, ScalingIntent.Full) => ContextRankProgression.AsIs,
                (PowerLevel.Balanced, ScalingIntent.Half) => ContextRankProgression.Div2,
                (PowerLevel.Legacy_AllFull, ScalingIntent.Full) => ContextRankProgression.AsIs,
                (PowerLevel.Legacy_AllFull, ScalingIntent.Half) => ContextRankProgression.AsIs,
                _ => ContextRankProgression.AsIs,
            };

        public static void AddBidirectionalMutex(BlueprintFeature a, BlueprintFeature b)
        {
            if (a == null || b == null)
            {
                Main.Log?.Log("AttributeFeats: AddBidirectionalMutex received a null feature reference.");
                return;
            }

            if (Main.Settings != null && !Main.Settings.EnableMutex)
            {
                return;
            }

            FeatureConfigurator.For(a)
                .AddPrerequisiteNoFeature(b)
                .Configure();
            FeatureConfigurator.For(b)
                .AddPrerequisiteNoFeature(a)
                .Configure();
        }
    }
}
