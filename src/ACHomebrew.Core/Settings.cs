using System.Collections.Generic;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace ACHomebrew
{
    public enum PowerLevel
    {
        Balanced,
        Legacy_AllFull,
    }

    public enum CastingAttributeScope
    {
        SelectedSpellbook,
        AllSpellbooks,
    }

    public enum CastingAttributeMode
    {
        Always,
        IfHigher,
    }

    public class FeatGroupSetting
    {
        [XmlAttribute] public string Id;
        [XmlAttribute] public bool Enabled;
        [XmlAttribute] public int Max;
    }

    [XmlRoot("AttributeFeatsSettings")]
    [XmlType("AttributeFeatsSettings")]
    public class ModSettings : UnityModManager.ModSettings
    {
        public bool IncludeSelfInAttributeStack = false;

        public bool EnableAttributes = true;
        public bool EnableDefenses = true;
        public bool EnableManeuvers = true;
        public bool EnableChecks = true;
        public bool EnableSkills = true;

        public bool EnableCasterDC = true;
        public bool EnableCasterLevel = true;
        public bool EnableSpellPenetration = true;

        public bool EnableBAB = false;
        public bool EnablePowerMode = false;

        public bool EnableMutex = true;

        public WeaponDamageMode WeaponDamage = WeaponDamageMode.Replace;

        // Per-character AttributeFeats budget. Both limits may be enabled together.
        public bool EnableFeatCountLimit = false;
        public int MaxFeatCount = 6;
        public bool EnableFeatPointLimit = false;
        public int MaxFeatPoints = 10;

        // Casting Attribute feats: which spellbooks they change, and whether only when higher.
        public CastingAttributeScope CastingScope = CastingAttributeScope.SelectedSpellbook;
        public CastingAttributeMode CastingMode = CastingAttributeMode.Always;

        // Truly Solo: whether pets in the party reduce the number of absent companions.
        public bool TrulySoloCountsPets = false;

        // Meme feats are registered only when enabled (requires restarting).
        public bool EnableMemeFeats = false;

        // Logs the measured distance on each close-quarters hit, for calibrating the distance scores.
        public bool LogDistanceCalibration = false;

        // Exclusion group overrides by group setting id. Missing groups use built-in defaults.
        public List<FeatGroupSetting> FeatGroups = new List<FeatGroupSetting>();

        public PowerLevel powerLevel = PowerLevel.Balanced;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
