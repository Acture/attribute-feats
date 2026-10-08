using System.Collections.Generic;
using System.Linq;

namespace AttributeFeats.New_Feats
{
    /// <summary>
    /// A configurable exclusion group: a character may own at most <see cref="DefaultMax"/>
    /// distinct member feats. Members are explicit feats and/or every menu family whose key
    /// starts with <see cref="Family"/> (so "MainAttribute" covers its source menus and legacy feats).
    /// Groups sharing a <see cref="SettingId"/> share one on/off switch and limit.
    /// </summary>
    internal sealed class FeatGroup
    {
        public string Id;
        public string SettingId;
        public string NameEn;
        public string NameZh;
        public bool DefaultEnabled = true;
        public int DefaultMax = 1;
        public string Family;
        public string[] Members = new string[0];
    }

    internal readonly struct FeatGroupLimit
    {
        public readonly bool Enabled;
        public readonly int Max;

        public FeatGroupLimit(bool enabled, int max)
        {
            Enabled = enabled;
            Max = FeatBudgetRules.ClampLimit(max);
        }
    }

    internal static class FeatGroupRules
    {
        private static FeatGroup Family(string family, string en, string zh, int max = 1)
            => new() { Id = family, SettingId = family, Family = family, NameEn = en, NameZh = zh, DefaultMax = max };

        private static FeatGroup Explicit(string id, string en, string zh, int max, bool enabled, params string[] members)
            => new() { Id = id, SettingId = id, NameEn = en, NameZh = zh, DefaultMax = max, DefaultEnabled = enabled, Members = members };

        // Every Main feat whose source is this attribute, plus the retired broad Main feat.
        private static IEnumerable<string> MainFrom(string attribute)
            => typeof(Guids.MainAttribute).GetNestedType(attribute).GetFields()
                .Select(field => (string)field.GetValue(null));

        private static FeatGroup SameAttribute(string attribute, params string[] members)
            => new()
            {
                Id = "SameAttribute_" + attribute,
                SettingId = "SameAttribute",
                NameEn = "Same attribute across Main, Adept and Stance",
                NameZh = "主属性、专精与架势同属性互斥",
                DefaultEnabled = false,
                Members = members.Concat(MainFrom(attribute)).ToArray(),
            };

        // Feats within one distance style stack; feats from different styles exclude each other.
        private static readonly string[][] DistanceStyleSets =
        {
            new[] { Guids.DistanceDamage.AggressorsEdge, Guids.DistanceDamage.ShortBlade, Guids.DistanceDamage.CloseQuarters },
            new[] { Guids.DistanceDamage.OptimalRange },
            new[] { Guids.DistanceDamage.MarksmansFocus },
        };

        private static IEnumerable<FeatGroup> DistanceStyles()
        {
            var index = 0;
            for (var a = 0; a < DistanceStyleSets.Length; a++)
                for (var b = a + 1; b < DistanceStyleSets.Length; b++)
                    foreach (var first in DistanceStyleSets[a])
                        foreach (var second in DistanceStyleSets[b])
                            yield return new FeatGroup
                            {
                                Id = "DistanceStyle_" + index++,
                                SettingId = "DistanceStyle",
                                NameEn = "One distance style (close, mid or long range)",
                                NameZh = "只选一种距离风格（近身、中距、远距）",
                                Members = new[] { first, second },
                            };
        }

        public static readonly FeatGroup[] All = new[]
        {
            Family("MainAttribute", "Main Attribute Mastery (one per character)", "主属性专精（每角色一个）"),
            Family("Defensive", "Defensive Adept", "防御专精"),
            Family("Maneuver", "Maneuver Adept", "战技专精"),
            Family("Skilled", "Skilled Adept", "技能专精"),
            Family("Arcane", "Arcane Adept", "奥术专精"),
            Family("Stance", "Stance", "架势"),
            Family("WeaponInsight", "Weapon Insight", "武器洞察"),
            Family("WeaponDamage", "Weapon Damage attribute", "武器伤害属性"),
            Family("GreaterSummoning", "Greater Summoning", "高等召唤"),
            Family("SummonerSacrifice", "Summoner Sacrifice", "召唤者献祭"),
            Family("SpellSchool", "Spell School Specialist", "法术学派专精"),
            Family("SpellDescriptor", "Spell Descriptor Specialist", "法术描述符专精"),
            Family("ReactiveArmor", "Reactive Armor", "反应护甲"),
            Family("Conditional", "Conditional Trigger", "条件触发", max: 2),
            Explicit("AcReplacement", "Attribute replaces armor class", "属性替代护甲等级", 1, true,
                Guids.Replacement.Extended.InnerSentinel, Guids.ExtendedReplacement2.LightfootDefense),
            Explicit("CmdReplacement", "Attribute replaces CMD", "属性替代战技防御", 1, true,
                Guids.Replacement.Extended.UnyieldingWill, Guids.ExtendedReplacement2.BrutalDefender),
            Explicit("DerivedAc", "Derived armor class", "衍生护甲等级", 1, true,
                Guids.Derived.ArcaneAegis, Guids.Derived.SkilledDefender),
            Explicit("CasterLevelVitality", "Caster level to hit points", "施法者等级转生命", 1, true,
                Guids.Derived.MysticVitality, Guids.Derived.SoulBulwark),
            Explicit("AttributeToAc", "Attribute bonuses to armor class", "属性加护甲等级", 2, true,
                Guids.Specialized.Defensive.Dex, Guids.Specialized.Defensive.Con,
                Guids.Stance.Feature.Dex, Guids.Stance.Feature.Wis,
                Guids.Replacement.Extended.InnerSentinel, Guids.ExtendedReplacement2.LightfootDefense,
                Guids.Conditional.EndlessResolve),
            SameAttribute("Str", Guids.str_main_to_everything, Guids.Specialized.Defensive.Str, Guids.Specialized.Maneuver.Str,
                Guids.Specialized.Skilled.Str, Guids.Specialized.Arcane.Str, Guids.Stance.Feature.Str),
            SameAttribute("Dex", Guids.dex_main_to_everything, Guids.Specialized.Defensive.Dex, Guids.Specialized.Maneuver.Dex,
                Guids.Specialized.Skilled.Dex, Guids.Specialized.Arcane.Dex, Guids.Stance.Feature.Dex),
            SameAttribute("Con", Guids.con_main_to_everything, Guids.Specialized.Defensive.Con, Guids.Specialized.Maneuver.Con,
                Guids.Specialized.Skilled.Con, Guids.Specialized.Arcane.Con, Guids.Stance.Feature.Con),
            SameAttribute("Int", Guids.int_main_to_everything, Guids.Specialized.Defensive.Int, Guids.Specialized.Maneuver.Int,
                Guids.Specialized.Skilled.Int, Guids.Specialized.Arcane.Int, Guids.Stance.Feature.Int),
            SameAttribute("Wis", Guids.wis_main_to_everything, Guids.Specialized.Defensive.Wis, Guids.Specialized.Maneuver.Wis,
                Guids.Specialized.Skilled.Wis, Guids.Specialized.Arcane.Wis, Guids.Stance.Feature.Wis),
            SameAttribute("Cha", Guids.cha_main_to_everything, Guids.Specialized.Defensive.Cha, Guids.Specialized.Maneuver.Cha,
                Guids.Specialized.Skilled.Cha, Guids.Specialized.Arcane.Cha, Guids.Stance.Feature.Cha),
        }.Concat(DistanceStyles()).ToArray();

        /// <summary>Normalizes "N" and "D" GUID spellings for comparison.</summary>
        public static string Key(string guid) => guid.Replace("-", "").ToLowerInvariant();

        /// <summary>One entry per configurable switch, in display order.</summary>
        public static IEnumerable<FeatGroup> Settings => All.GroupBy(group => group.SettingId).Select(group => group.First());

        /// <summary>Resolves each group's members from registered (guid, family) feats.</summary>
        public static Dictionary<FeatGroup, HashSet<string>> Resolve(IEnumerable<(string guid, string family)> feats)
        {
            var list = feats.ToList();
            return All.ToDictionary(group => group, group => new HashSet<string>(
                group.Members.Concat(list.Where(feat => group.Family != null && feat.family != null
                        && feat.family.StartsWith(group.Family, System.StringComparison.Ordinal)).Select(feat => feat.guid))
                    .Select(Key)));
        }

        /// <summary>
        /// Groups that block <paramref name="candidate"/>. Only distinct other members count,
        /// so re-taking a parametrized member for a new parameter is never blocked by its own group.
        /// </summary>
        public static List<FeatGroup> Blocking(IReadOnlyDictionary<FeatGroup, HashSet<string>> members,
            System.Func<FeatGroup, FeatGroupLimit> limit, ISet<string> owned, string candidate)
        {
            var blocking = new List<FeatGroup>();
            foreach (var entry in members)
            {
                if (!entry.Value.Contains(candidate)) continue;
                var groupLimit = limit(entry.Key);
                if (!groupLimit.Enabled) continue;
                var others = entry.Value.Count(member => member != candidate && owned.Contains(member));
                if (others + 1 > groupLimit.Max) blocking.Add(entry.Key);
            }
            return blocking;
        }
    }
}
