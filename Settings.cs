using System.Xml.Serialization;
using UnityModManagerNet;

namespace AttributeFeats
{
    public enum PowerLevel
    {
        Balanced,
        Legacy_AllFull,
    }

    [XmlType(Namespace = "AttributeFeats")]
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

        public PowerLevel powerLevel = PowerLevel.Balanced;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
