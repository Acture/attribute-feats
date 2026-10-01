using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AttributeFeats.New_Feats
{
    internal static class IconLoader
    {
        private static readonly Dictionary<string, Sprite> Cache = new(StringComparer.OrdinalIgnoreCase);
        private static string s_IconsDirectory;

        public static string IconsDirectory
        {
            get
            {
                if (s_IconsDirectory == null)
                {
                    var modDir = Main.Entry?.Path;
                    if (string.IsNullOrEmpty(modDir))
                    {
                        modDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    }
                    s_IconsDirectory = Path.Combine(modDir ?? "", "Icons");
                }
                return s_IconsDirectory;
            }
        }

        private static readonly Dictionary<string, string> NameAliases = new(StringComparer.OrdinalIgnoreCase)
        {
            // Main Attribute Mastery
            { "ApexPredator", "TitansApotheosis" },
            { "EmbodiedGrace", "QuicksilverIncarnate" },
            { "LivingBulwark", "AdamantineVessel" },
            { "WellspringOfInsight", "OcularOfTheCosmos" },
            { "CrownOfWill", "SovereignOfWills" },

            // Stances
            { "BrutalStance", "BerserkersOverrun" },
            { "LiquidForm", "FlowingReed" },
            { "EndlessVigor", "TitansRespiration" },
            { "TacticalMind", "GrandmastersGambit" },
            { "CenteredMind", "MirrorOfStillWaters" },
            { "CommandingPresence", "VanguardsBanner" },

            // Conditional
            { "EndlessResolve", "DefianceAtThePrecipice" },
            { "FirstBlood", "AmbushOfTheViper" },
            { "Vendetta", "OathOfRetribution" },
            { "PatientHunter", "CranesSeverance" },
            { "BerserkersLastStand", "GorumsLastStand" },
            { "TacticalReading", "CadenceDecoded" },

            // Weapon Insight
            { "CrushingForm", "CrushingForce" },
            { "DuelistsEye", "RestovElegance" },
            { "IronStance", "StoutGrounding" },
            { "TacticalStrike", "GeometersEdge" },
            { "PredictiveCut", "KarmicInterception" },
            { "TheatricalCombat", "SwashbucklersFlourish" },

            // Extended Replacements
            { "BrutalDefender", "ColossusDefiance" },
            { "LightfootDefense", "ZephyrsGrace" },
            { "IronEndurance", "AdamantineMettle" },

            // Greater Summoning
            { "BloodlineOfBeasts", "BehemothsHeritage" },
            { "QuickenedPact", "ZephyrsCovenant" },
            { "VitalPact", "TitansLifespring" },
            { "TacticalBinding", "AegisOfTheSchema" },
            { "InsightfulSummons", "EmpathicCommunion" },
            { "MagneticCalling", "MonarchsMajesty" },

            // Summoner Sacrifice
            { "BodyOfMyPact", "MartyrsTransference" },
            { "DoubledBond", "EldritchCrucible" },
            { "EmpoweredSacrifice", "TributeOfIronDominion" },

            // Reactive Armor
            { "SpikedDefense", "BarbedCarapace" },
            { "BulwarkOfSteel", "CitadelOfSteel" },

            // Derived
            { "MartialInsight", "WarHardenedReflexes" },
            { "SkilledDefender", "ScholarlyBastion" },
            { "MysticVitality", "FontOfAnima" },
            { "SoulBulwark", "AuraOfTheOverlord" },
            { "SwordSaint", "KensaisTrance" },

            // Distance
            { "AggressorsEdge", "PointBlankRuin" },
            { "MarksmansFocus", "HorizonsDeadeye" },
            { "OptimalRange", "HarmonicCleave" },
        };

        public static Sprite Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (Cache.TryGetValue(name, out var cached)) return cached;

            var fileName = name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ? name : name + ".png";
            var filePath = Path.Combine(IconsDirectory, fileName);

            if (!File.Exists(filePath))
            {
                if (NameAliases.TryGetValue(name, out var alias))
                {
                    var aliasSprite = Get(alias);
                    if (aliasSprite != null)
                    {
                        Cache[name] = aliasSprite;
                        return aliasSprite;
                    }
                }

                string[] suffixes = { "TriggerBuff", "OuterBuff", "InnerBuff", "TempBuff", "Buff", "Activatable", "Ability", "Feature", "Toggle" };
                foreach (var suffix in suffixes)
                {
                    if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) && name.Length > suffix.Length)
                    {
                        var stripped = name.Substring(0, name.Length - suffix.Length);
                        var strippedSprite = Get(stripped);
                        if (strippedSprite != null)
                        {
                            Cache[name] = strippedSprite;
                            return strippedSprite;
                        }
                    }
                }
                return null;
            }

            try
            {
                var bytes = File.ReadAllBytes(filePath);
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (ImageConversion.LoadImage(tex, bytes))
                {
                    tex.name = name;
                    var sprite = Sprite.Create(
                        tex,
                        new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f),
                        100f);
                    sprite.name = name;
                    Cache[name] = sprite;
                    return sprite;
                }
            }
            catch (Exception ex)
            {
                Main.Log?.Log($"[IconLoader] Failed loading icon '{name}': {ex}");
            }

            return null;
        }
    }
}
