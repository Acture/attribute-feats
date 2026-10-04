using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace AttributeFeats.New_Feats
{
    // Shared names/lore plus menu and weapon-damage descriptions. Other rule
    // templates retain their compiled bilingual fallbacks in the feat builders.
    internal static class FeatTextCatalog
    {
        private sealed class Entry
        {
            public string Key { get; set; }
            public string enGB { get; set; }
            public string zhCN { get; set; }
        }

        private static readonly Dictionary<string, Entry> Entries = Load();

        private static Dictionary<string, Entry> Load()
        {
            var entries = new Dictionary<string, Entry>(StringComparer.Ordinal);
            try
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AttributeFeats.Localization.FeatText.json");
                if (stream == null) throw new FileNotFoundException("Embedded FeatText.json was not found.");
                using var reader = new StreamReader(stream);
                foreach (var entry in JsonConvert.DeserializeObject<List<Entry>>(reader.ReadToEnd()))
                {
                    if (string.IsNullOrWhiteSpace(entry.Key) || string.IsNullOrWhiteSpace(entry.enGB))
                        throw new InvalidDataException("Feat text requires a stable key and English fallback.");
                    entries.Add(entry.Key, entry);
                }
            }
            catch (Exception ex)
            {
                Main.Log?.Log("AttributeFeats: using compiled text fallbacks: " + ex.Message);
            }
            return entries;
        }

        public static string Get(string key, string fallback, bool chinese)
            => Entries.TryGetValue(key, out var entry)
                ? SelectText(entry.enGB, entry.zhCN, fallback, chinese)
                : fallback;

        internal static string SelectText(string english, string chinese, string fallback, bool useChinese)
            => useChinese && !string.IsNullOrWhiteSpace(chinese)
                ? chinese
                : !string.IsNullOrWhiteSpace(english) ? english : fallback;
    }
}
