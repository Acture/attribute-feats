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
            // Stable blueprint names map to the final feat artwork.
            { "ApexPredator", "TitansApotheosis" },
            { "EmbodiedGrace", "QuicksilverIncarnate" },
            { "LivingBulwark", "AdamantineVessel" },
            { "WellspringOfInsight", "OcularOfTheCosmos" },
            { "CrownOfWill", "SovereignOfWills" },
            { "CrushingForm", "TitansMomentum" },
            { "DuelistsEye", "AldoriFinesse" },
            { "IronStance", "StoutGrounding" },
            { "TacticalStrike", "GeometersEdge" },
            { "PredictiveCut", "KarmicInterception" },
            { "TheatricalCombat", "SwashbucklersFlourish" },
            { "InnerSentinel", "AsceticsWard" },
            { "CalculatedGrip", "AnatomicalLeverage" },
            { "UnyieldingWill", "MonarchsStature" },
            { "BrutalDefender", "TitansFooting" },
            { "LightfootDefense", "ZephyrsGrace" },
            { "IronEndurance", "AdamantineMettle" },
            { "TitansStance", "ColossusBastion" },
            { "FlowingForm", "WindDancersShroud" },
            { "IronBulwark", "InuredCarapace" },
            { "CalculatedDefense", "AnalyticalAegis" },
            { "StoicVigilance", "ThirdEyeVigil" },
            { "IndomitablePresence", "MajestysReproach" },
            { "CrushingGrip", "GripoftheBehemoth" },
            { "DeftHand", "FulcrumoftheViper" },
            { "UnyieldingHold", "DeepRootClinch" },
            { "TacticalBind", "AnatomicalPivot" },
            { "PredictiveLock", "CranesAnticipation" },
            { "DomineeringThrow", "AudaciousOverthrow" },
            { "PracticedHand", "GiantwrightsCraft" },
            { "EffortlessSkill", "ThiefKingsPanache" },
            { "TirelessPractice", "AsceticDiligence" },
            { "PolymathsTouch", "EncyclopedicSynthesis" },
            { "QuietMastery", "WanderersLucidity" },
            { "InspiredVersatility", "SilverTonguedVirtuoso" },
            { "SpellForgedWill", "MageHammerInscription" },
            { "QuickcastReflex", "SomaticVelocity" },
            { "SpellTemperedBody", "CrucibleoftheConduit" },
            { "ScholarOfTheWeave", "ArchmagesCodex" },
            { "OraclesIntuition", "GnosticChannel" },
            { "SorcerousPresence", "SovereignDecrees" },
            { "BrutalStance", "BerserkersOverrun" },
            { "LiquidForm", "FlowingReed" },
            { "EndlessVigor", "TitansRespiration" },
            { "TacticalMind", "GrandmastersGambit" },
            { "CenteredMind", "MirrorOfStillWaters" },
            { "CommandingPresence", "VanguardsBanner" },
            { "EndlessResolve", "DefianceAtThePrecipice" },
            { "FirstBlood", "AmbushoftheViper" },
            { "Vendetta", "OathofRetribution" },
            { "PatientHunter", "CranesSeverance" },
            { "BerserkersLastStand", "GorumsLastStand" },
            { "TacticalReading", "CadenceDecoded" },
            { "MartialInsight", "WarHardenedReflexes" },
            { "SkilledDefender", "ScholarsPositioning" },
            { "MysticVitality", "LeyInfusedVitality" },
            { "SoulBulwark", "DawnoftheSoul" },
            { "SwordSaint", "BladeoftheSpellSaint" },
            { "BloodlineOfBeasts", "BehemothsHeritage" },
            { "QuickenedPact", "ZephyrsCovenant" },
            { "VitalPact", "TitansLifespring" },
            { "TacticalBinding", "AegisoftheSchema" },
            { "InsightfulSummons", "EmpathicCommunion" },
            { "MagneticCalling", "DominatorsCalling" },
            { "BodyOfMyPact", "MartyrsTransference" },
            { "DoubledBond", "EldritchCrucible" },
            { "EmpoweredSacrifice", "TributeofIronDominion" },
            { "AggressorsEdge", "PointBlankRuin" },
            { "MarksmansFocus", "HorizonsDeadeye" },
            { "OptimalRange", "HarmonicCleave" },
            { "PureWarder", "AegisofthePureWarder" },
            { "MasterCaller", "SovereignGatekeeper" },
            { "SeersEdge", "EyeoftheChronomancer" },
            { "HeartsTyrant", "SovereignoftheHeart" },
            { "Spellforge", "PyreoftheArchitect" },
            { "Veilweaver", "PhantasmagoriaMaestro" },
            { "DeathSpeaker", "HarvesteroftheBoneyard" },
            { "ShapeShifter", "SculptorofPrimeMatter" },
            { "InnerFlame", "PyreofthePhoenix" },
            { "FrozenHeart", "StillnessoftheGlacialVoid" },
            { "StormChannel", "TempestDancer" },
            { "EtchingMind", "VitriolicEquation" },
            { "ResonantVoice", "HeraldoftheShatteredSky" },
            { "EthericMind", "AxiomofUnseenForce" },
            { "RadiantSoul", "FountainofSolarDawn" },
            { "HollowHeart", "VigiloftheGloom" },
            { "SubtleTyrant", "PuppeteeroftheMind" },
            { "SpikedDefense", "BarbedCarapace" },
            { "BulwarkOfSteel", "CitadelofSteel" },
            { "PolearmMaster", "LongReachGambit" },
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
