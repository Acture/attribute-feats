#if MODMENU
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WotRHomebrew.Feats;
using Kingmaker.Localization;
using ModMenu.Settings;
using UnityModManagerNet;
using MenuApi = ModMenu.ModMenu;

namespace WotRHomebrew
{
    /// <summary>
    /// Optional ModMenu page that mirrors the UMM settings. ModMenu types are only touched
    /// when ModMenu is loaded; values are written back to the same settings file.
    /// </summary>
    internal static class ModMenuSettings
    {
        private const string Prefix = "achomebrew";
        private static readonly List<Action> PushCurrentValues = new();

        public static void TryRegister()
        {
            if (UnityModManager.FindMod("ModMenu")?.Active != true) return;
            try
            {
                Register();
                Main.Log?.Log("WotR Homebrew: settings added to ModMenu.");
            }
            catch (Exception error)
            {
                Main.Log?.Log("WotR Homebrew: ModMenu settings unavailable - " + error);
            }
        }

        private static string Key(string name) => $"{Prefix}.{name}";

        private static LocalizedString L(string name, string en, string zh) => Common.L($"WotRHomebrew.Settings.{name}", en, zh);

        private static void Apply(Action change)
        {
            change();
            Main.Settings.Save(Main.Entry);
            FeatBudget.SyncVisibility();
        }

        private static Toggle Toggle(string name, bool defaultValue, Func<bool> read, Action<bool> write, string en, string zh)
        {
            var key = Key(name);
            PushCurrentValues.Add(() => MenuApi.SetSetting(key, read()));
            return ModMenu.Settings.Toggle.New(key, defaultValue, L(name, en, zh)).OnValueChanged(value => Apply(() => write(value)));
        }

        private static SliderInt Slider(string name, int defaultValue, Func<int> read, Action<int> write, string en, string zh)
        {
            var key = Key(name);
            PushCurrentValues.Add(() => MenuApi.SetSetting(key, read()));
            return SliderInt.New(key, defaultValue, L(name, en, zh), 0, FeatBudgetRules.MaxLimit)
                .OnValueChanged(value => Apply(() => write(value)));
        }

        private static DropdownList Choice(string name, int defaultValue, Func<int> read, Action<int> write, string en, string zh,
            params (string en, string zh)[] values)
        {
            var key = Key(name);
            PushCurrentValues.Add(() => MenuApi.SetSetting(key, read()));
            var labels = new List<LocalizedString>();
            for (var i = 0; i < values.Length; i++) labels.Add(L($"{name}.{i}", values[i].en, values[i].zh));
            return DropdownList.New(key, defaultValue, L(name, en, zh), labels).OnValueChanged(value => Apply(() => write(value)));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Register()
        {
            var s = Main.Settings;
            var d = new ModSettings();
            var builder = SettingsBuilder.New(Prefix, L("Title", "WotR Homebrew", "WotR 房规手册"))
                .SetMod(Main.Entry, false)
                .AddSubHeader(L("Budget", "Feat budget (applies immediately)", "专长额度（即时生效）"), true)
                .AddToggle(Toggle("budget-count", d.EnableFeatCountLimit, () => s.EnableFeatCountLimit, v => s.EnableFeatCountLimit = v,
                    "Limit the number of Homebrew feats per character", "限制每个角色的房规专长数量"))
                .AddSliderInt(Slider("budget-count-max", d.MaxFeatCount, () => s.MaxFeatCount, v => s.MaxFeatCount = v,
                    "Maximum feats", "专长数量上限"))
                .AddToggle(Toggle("budget-points", d.EnableFeatPointLimit, () => s.EnableFeatPointLimit, v => s.EnableFeatPointLimit = v,
                    "Limit Homebrew feat points per character", "限制每个角色的房规专长点数"))
                .AddSliderInt(Slider("budget-points-max", d.MaxFeatPoints, () => s.MaxFeatPoints, v => s.MaxFeatPoints = v,
                    "Maximum points", "点数上限"))
                .AddToggle(Toggle("solo-pets", d.TrulySoloCountsPets, () => s.TrulySoloCountsPets, v => s.TrulySoloCountsPets = v,
                    "Truly Solo: pets in the party reduce the count", "真·独行：队伍中的宠物抵扣数量"))
                .AddSubHeader(L("Exclusion", "Exclusion groups (applies immediately)", "互斥组（即时生效）"))
                .AddToggle(Toggle("mutex", d.EnableMutex, () => s.EnableMutex, v => s.EnableMutex = v,
                    "Enable exclusion groups (master switch)", "启用互斥组（总开关）"));

            foreach (var group in FeatGroupRules.Settings)
            {
                var current = group;
                builder
                    .AddToggle(Toggle($"group-{current.SettingId.ToLowerInvariant()}", current.DefaultEnabled,
                        () => FeatBudget.GroupLimit(current).Enabled,
                        v => FeatBudget.SetGroupLimit(current, v, FeatBudget.GroupLimit(current).Max),
                        current.NameEn, current.NameZh))
                    .AddSliderInt(Slider($"group-{current.SettingId.ToLowerInvariant()}-max", current.DefaultMax,
                        () => FeatBudget.GroupLimit(current).Max,
                        v => FeatBudget.SetGroupLimit(current, FeatBudget.GroupLimit(current).Enabled, v),
                        $"{current.NameEn}: limit", $"{current.NameZh}：上限"));
            }

            builder
                .AddSubHeader(L("Modes", "Damage and casting modes (applies immediately)", "伤害与施法模式（即时生效）"))
                .AddDropdownList(Choice("weapon-damage", (int)d.WeaponDamage, () => (int)s.WeaponDamage, v => s.WeaponDamage = (WeaponDamageMode)v,
                    "Weapon Damage mode", "武器伤害模式", ("Replace", "替换"), ("Add", "附加")))
                .AddDropdownList(Choice("casting-scope", (int)d.CastingScope, () => (int)s.CastingScope, v => s.CastingScope = (CastingAttributeScope)v,
                    "Casting attribute scope", "施法属性范围", ("Selected spellbook", "选定的法术书"), ("All spellbooks", "全部法术书")))
                .AddDropdownList(Choice("casting-mode", (int)d.CastingMode, () => (int)s.CastingMode, v => s.CastingMode = (CastingAttributeMode)v,
                    "Casting attribute mode", "施法属性模式", ("Always", "总是"), ("Only when higher", "仅在更高时")))
                .AddSubHeader(L("Restart", "Requires restarting the game", "需要重启游戏"))
                .AddDropdownList(Choice("power-level", (int)d.powerLevel, () => (int)s.powerLevel, v => s.powerLevel = (PowerLevel)v,
                    "Power level", "强度等级", ("Balanced", "平衡"), ("Legacy all full", "旧版全额")))
                .AddToggle(Toggle("self-stack", d.IncludeSelfInAttributeStack, () => s.IncludeSelfInAttributeStack, v => s.IncludeSelfInAttributeStack = v,
                    "Include self in attribute stack", "属性自我叠加"))
                .AddToggle(Toggle("attributes", d.EnableAttributes, () => s.EnableAttributes, v => s.EnableAttributes = v, "Enable attribute bonuses", "启用属性加成"))
                .AddToggle(Toggle("defenses", d.EnableDefenses, () => s.EnableDefenses, v => s.EnableDefenses = v, "Enable defense bonuses", "启用防御加成"))
                .AddToggle(Toggle("maneuvers", d.EnableManeuvers, () => s.EnableManeuvers, v => s.EnableManeuvers = v, "Enable maneuver bonuses", "启用战技加成"))
                .AddToggle(Toggle("checks", d.EnableChecks, () => s.EnableChecks, v => s.EnableChecks = v, "Enable check bonuses", "启用检定加成"))
                .AddToggle(Toggle("skills", d.EnableSkills, () => s.EnableSkills, v => s.EnableSkills = v, "Enable skill bonuses", "启用技能加成"))
                .AddToggle(Toggle("caster-dc", d.EnableCasterDC, () => s.EnableCasterDC, v => s.EnableCasterDC = v, "Enable spell DC bonuses", "启用法术DC加成"))
                .AddToggle(Toggle("caster-level", d.EnableCasterLevel, () => s.EnableCasterLevel, v => s.EnableCasterLevel = v, "Enable caster level bonuses", "启用施法者等级加成"))
                .AddToggle(Toggle("spell-penetration", d.EnableSpellPenetration, () => s.EnableSpellPenetration, v => s.EnableSpellPenetration = v,
                    "Enable spell penetration bonuses", "启用法术穿透加成"))
                .AddToggle(Toggle("meme", d.EnableMemeFeats, () => s.EnableMemeFeats, v => s.EnableMemeFeats = v,
                    "Enable meme feats", "启用梗专长"))
                .AddToggle(Toggle("bab", d.EnableBAB, () => s.EnableBAB, v => s.EnableBAB = v, "Enable BAB bonuses (power option)", "启用BAB加成（强力选项）"))
                .AddToggle(Toggle("power-mode", d.EnablePowerMode, () => s.EnablePowerMode, v => s.EnablePowerMode = v,
                    "Enable Power Mode (power option)", "启用威力模式（强力选项）"));

            MenuApi.AddSettings(builder);
            // The settings file stays authoritative; show its values in the menu.
            foreach (var push in PushCurrentValues) push();
        }
    }
}
#else
namespace WotRHomebrew
{
    /// <summary>Built without ModMenu installed: the UMM settings panel is the only settings UI.</summary>
    internal static class ModMenuSettings
    {
        public static void TryRegister() { }
    }
}
#endif
