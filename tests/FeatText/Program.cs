using System;
using AttributeFeats.New_Feats;

namespace AttributeFeats
{
    internal static class Main { internal static Logger Log = new Logger(); }
    internal sealed class Logger { public void Log(string message) => throw new Exception(message); }
}

internal static class Program
{
    private static void Equal(string expected, string actual)
    {
        if (expected != actual) throw new Exception($"Expected '{expected}', got '{actual}'");
    }

    private static void Main()
    {
        Equal("Titan's Apotheosis", FeatTextCatalog.Get("MainAttr_Str.Name", "fallback", false));
        Equal("泰坦登阶", FeatTextCatalog.Get("MainAttr_Str.Name", "fallback", true));
        Equal("fallback", FeatTextCatalog.Get("Missing.Name", "fallback", true));
        Equal("English", FeatTextCatalog.SelectText("English", null, "fallback", true));
        Equal("English", FeatTextCatalog.SelectText("English", "  ", "fallback", true));
        Equal("English", FeatTextCatalog.SelectText("English", "中文", "fallback", false));
        Equal("fallback", FeatTextCatalog.SelectText(null, null, "fallback", false));
        Equal("<i>文本</i>\n{0}", FeatTextCatalog.SelectText("<i>text</i>\n{0}", "<i>文本</i>\n{0}", "fallback", true));
        Console.WriteLine("8 text resource and fallback checks passed (outside the game).");
    }
}
