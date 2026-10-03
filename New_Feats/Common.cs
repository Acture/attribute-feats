using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using HarmonyLib;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;

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

        private static readonly Dictionary<string, (string en, string zh, bool tag)> LocalizedStrings = new();

        public static string Text(string key, string fallback, bool chinese = false)
            => FeatTextCatalog.Get(key, fallback, chinese);

        public static LocalizedString L(string key, string enValue, string zhValue = null, bool tagEncyclopediaEntries = false)
        {
            LocalizedStrings[key] = (enValue, zhValue, tagEncyclopediaEntries);
            var isZh = LocalizationManager.CurrentLocale == Locale.zhCN;
            var text = (isZh && !string.IsNullOrEmpty(zhValue)) ? zhValue : enValue;
            return LocalizationTool.CreateString(key, text, tagEncyclopediaEntries);
        }

        public static LocalizedString L(string key, string value, bool tagEncyclopediaEntries)
            => L(key, value, null, tagEncyclopediaEntries);

        public static void RefreshLocale()
        {
            try
            {
                var pack = LocalizationManager.CurrentPack;
                if (pack == null) return;
                var isZh = LocalizationManager.CurrentLocale == Locale.zhCN;
                foreach (var entry in LocalizedStrings)
                {
                    var text = (isZh && !string.IsNullOrEmpty(entry.Value.zh)) ? entry.Value.zh : entry.Value.en;
                    pack.PutString(entry.Key, entry.Value.tag ? EncyclopediaTool.TagEncyclopediaEntries(text) : text);
                }
            }
            catch (System.Exception ex)
            {
                Main.Log?.Log("AttributeFeats: RefreshLocale error: " + ex);
            }
        }

        // OnLocaleChanged is a private method, not an event. BlueprintCore's own postfix there re-applies
        // the creation-time (single-locale) text, so ours must run after it to restore the zhCN strings.
        [HarmonyPatch(typeof(LocalizationManager), "OnLocaleChanged")]
        private static class LocalizationManager_OnLocaleChanged_Patch
        {
            [HarmonyPriority(Priority.Last)]
            [HarmonyPostfix]
            private static void Postfix() => RefreshLocale();
        }

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

        public static FeatureConfigurator SetIconIfPresent(this FeatureConfigurator cfg, string internalName)
        {
            var icon = IconLoader.Get(internalName);
            if (icon != null)
            {
                cfg.SetIcon(icon);
            }
            return cfg;
        }

        public static BuffConfigurator SetIconIfPresent(this BuffConfigurator cfg, string internalName)
        {
            var icon = IconLoader.Get(internalName);
            if (icon != null)
            {
                cfg.SetIcon(icon);
            }
            return cfg;
        }

        public static ActivatableAbilityConfigurator SetIconIfPresent(this ActivatableAbilityConfigurator cfg, string internalName)
        {
            var icon = IconLoader.Get(internalName);
            if (icon != null)
            {
                cfg.SetIcon(icon);
            }
            return cfg;
        }

        public static AbilityConfigurator SetIconIfPresent(this AbilityConfigurator cfg, string internalName)
        {
            var icon = IconLoader.Get(internalName);
            if (icon != null)
            {
                cfg.SetIcon(icon);
            }
            return cfg;
        }
    }
}
