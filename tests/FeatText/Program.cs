using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ACHomebrew.Feats;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Newtonsoft.Json.Linq;

namespace ACHomebrew
{
    internal static class Mod { internal static Logger Log = new Logger(); }
    internal sealed class Logger { public void Log(string message) => throw new Exception(message); }
}

internal static class Program
{
    private static int checks;
    private static void Equal(string expected, string actual)
    {
        if (expected != actual) throw new Exception($"Expected '{expected}', got '{actual}'");
        checks++;
    }

    private static int Main()
    {
        try
        {
            RunChecks();
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
    }

    private static void RunChecks()
    {
        Equal("Titan's Apotheosis", FeatTextCatalog.Get("MainAttr_Str.Name", "fallback", false));
        Equal("泰坦登阶", FeatTextCatalog.Get("MainAttr_Str.Name", "fallback", true));
        Equal("fallback", FeatTextCatalog.Get("Missing.Name", "fallback", true));
        Equal("English", FeatTextCatalog.SelectText("English", null, "fallback", true));
        Equal("English", FeatTextCatalog.SelectText("English", "  ", "fallback", true));
        Equal("English", FeatTextCatalog.SelectText("English", "中文", "fallback", false));
        Equal("fallback", FeatTextCatalog.SelectText(null, null, "fallback", false));
        Equal("<i>文本</i>\n{0}", FeatTextCatalog.SelectText("<i>text</i>\n{0}", "<i>文本</i>\n{0}", "fallback", true));
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AttributeFeats.Localization.FeatText.json");
        using var reader = new StreamReader(stream);
        var resources = JArray.Parse(reader.ReadToEnd());
        var entries = resources.ToDictionary(e => (string)e["Key"]);
        var menus = new[] { "Root", "MainAttribute", "Defensive", "Maneuver", "Skilled", "Arcane", "Stance", "Conditional", "WeaponInsight", "ExtendedReplacement", "GreaterSummoning", "SummonerSacrifice", "ReactiveArmor", "DerivedStat", "SpellSchool", "SpellDescriptor", "DistanceDamage", "WeaponDamage", "MainAttribute_Str", "MainAttribute_Dex", "MainAttribute_Con", "MainAttribute_Int", "MainAttribute_Wis", "MainAttribute_Cha", "CastingStat", "ResourceStat", "Retaliation", "Momentum", "Stealth", "Solo", "Growth", "Execution", "Arcana", "Summoner", "Survival", "Meme" };
        var attributes = new[] { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };
        var newKeys = menus.Select(m => "AttributeFeatsSelection_" + m)
            .Concat(attributes.Select(a => "WeaponDamage_" + a))
            .SelectMany(prefix => new[] { prefix + ".Name", prefix + ".Description" })
            .Concat(new[] { "Count", "Points", "CountFull", "PointsShort", "OverBudget" }.Select(k => "FeatBudget." + k).Append("FeatGroups.Member")).ToArray();
        foreach (var key in newKeys)
        {
            if (!entries.TryGetValue(key, out var entry)
                || string.IsNullOrWhiteSpace((string)entry["zhCN"])
                || (string)entry["zhCN"] == (string)entry["enGB"])
                throw new Exception("Missing bilingual menu/weapon/budget text: " + key);
            checks++;
        }

        // Run the production registration and refresh methods against an in-memory
        // localization pack. BlueprintCore/Unity boundaries are doubles, not a game.
        LocalizationManager.CurrentLocale = Locale.enGB;
        foreach (var entry in entries)
        {
            Common.L(entry.Key, "compiled fallback");
            Equal((string)entry.Value["enGB"], LocalizationManager.CurrentPack.GetString(entry.Key));
        }
        Common.L("Missing.Bilingual", "English fallback", "中文回退");
        Common.L("Missing.EnglishOnly", "English only");
        Common.L("Missing.Tagged", "<b>English</b> {0}", "<b>中文</b> {0}", true);
        Equal("tag:<b>English</b> {0}", LocalizationManager.CurrentPack.GetString("Missing.Tagged"));

        // Locale changes replace the entire game pack. Previously returned keys
        // must be repopulated, including ones registered while English was active.
        LocalizationManager.CurrentPack = new LocalizationPack();
        LocalizationManager.CurrentLocale = Locale.zhCN;
        Common.RefreshLocale();
        foreach (var entry in entries)
            Equal((string)entry.Value["zhCN"], LocalizationManager.CurrentPack.GetString(entry.Key));
        Equal("中文回退", LocalizationManager.CurrentPack.GetString("Missing.Bilingual"));
        Equal("English only", LocalizationManager.CurrentPack.GetString("Missing.EnglishOnly"));
        Equal("tag:<b>中文</b> {0}", LocalizationManager.CurrentPack.GetString("Missing.Tagged"));
        Common.L("WeaponDamage_Wisdom.Name", "compiled fallback");
        Equal("以感知计算武器伤害", LocalizationManager.CurrentPack.GetString("WeaponDamage_Wisdom.Name"));

        LocalizationManager.CurrentPack = new LocalizationPack();
        LocalizationManager.CurrentLocale = Locale.enGB;
        Common.RefreshLocale();
        foreach (var entry in entries)
            Equal((string)entry.Value["enGB"], LocalizationManager.CurrentPack.GetString(entry.Key));
        Equal("English fallback", LocalizationManager.CurrentPack.GetString("Missing.Bilingual"));
        Equal("tag:<b>English</b> {0}", LocalizationManager.CurrentPack.GetString("Missing.Tagged"));

        // Runtime-built budget text follows the active locale without registration.
        LocalizationManager.CurrentLocale = Locale.zhCN;
        Equal("还差 {0} 点", Common.CurrentText("FeatBudget.PointsShort", "fallback"));
        LocalizationManager.CurrentLocale = Locale.enGB;
        Equal("Requires {0} more points", Common.CurrentText("FeatBudget.PointsShort", "fallback"));
        Equal("fallback", Common.CurrentText("FeatBudget.Missing", "fallback"));

        LocalizationManager.CurrentLocale = Locale.Other;
        Common.RefreshLocale();
        Equal("Wisdom to Weapon Damage", LocalizationManager.CurrentPack.GetString("WeaponDamage_Wisdom.Name"));
        LocalizationManager.CurrentPack = null;
        Common.RefreshLocale(); // Startup without a pack is deliberately harmless.
        checks++;
        Console.WriteLine($"{checks} localization assertions passed: {entries.Count} resources, {newKeys.Length} menu/weapon/budget keys, EN -> zhCN -> EN, fallbacks and tagging (outside the game).");
    }
}
