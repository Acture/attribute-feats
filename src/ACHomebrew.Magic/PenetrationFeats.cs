using Kingmaker.Blueprints.Classes.Spells;
using static ACHomebrew.Feats.FeatBuilders;

namespace ACHomebrew.Feats
{
    /// <summary>Penetration (spells): effects that pierce specific immunities.</summary>
    internal static class PenetrationFeats
    {
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            Piercing("MindBreaker", Guids.Penetration.MindBreaker, "Mind Breaker", "破心", SpellDescriptor.MindAffecting | SpellDescriptor.Charm | SpellDescriptor.Compulsion | SpellDescriptor.Emotion, PiercedImmunity.None,
                "Your mind-affecting, charm, compulsion and emotion effects ignore immunity to those effects (for example, undead and constructs).",
                "你的心灵效果、魅惑、强制与情绪效果无视对这些效果的免疫（例如不死生物与构装生物）。");
            Piercing("Deathbringer", Guids.Penetration.Deathbringer, "Deathbringer", "索命", SpellDescriptor.Death, PiercedImmunity.None,
                "Your death effects ignore immunity to death effects.",
                "你的死亡效果无视对死亡效果的免疫。");
            Piercing("Dread", Guids.Penetration.Dread, "Dread Presence", "慑魂", SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened, PiercedImmunity.None,
                "Your fear effects ignore immunity to fear.",
                "你的恐惧效果无视对恐惧的免疫。");
            Piercing("Plaguebearer", Guids.Penetration.Plaguebearer, "Plaguebearer", "疫毒", SpellDescriptor.Poison | SpellDescriptor.Disease, PiercedImmunity.None,
                "Your poison and disease effects ignore immunity to poison and disease.",
                "你的毒素与疾病效果无视对毒素与疾病的免疫。");
            Piercing("Paralyzer", Guids.Penetration.Paralyzer, "Overwhelming Force", "镇压", SpellDescriptor.Paralysis | SpellDescriptor.Stun | SpellDescriptor.Daze | SpellDescriptor.Sleep, PiercedImmunity.None,
                "Your paralysis, stun, daze and sleep effects ignore immunity to those effects.",
                "你的麻痹、震慑、恍惚与睡眠效果无视对这些效果的免疫。");
            Piercing("ElementalBreach", Guids.Penetration.ElementalBreach, "Elemental Breach", "破元", SpellDescriptor.None, PiercedImmunity.Energy,
                "Creatures immune to an energy type take half damage from your damage of that type instead, and are never healed by it.",
                "免疫某种能量的生物改为受到你该类型伤害的一半，且不会因此回复生命。");
        }

        private static void Piercing(string name, string guid, string nameEn, string nameZh, SpellDescriptor descriptors, PiercedImmunity other,
            string effectEn, string effectZh)
            => Feat(FeatSelection.Penetration, name, guid, nameEn, nameZh, Desc("Penetration", "穿透", effectEn, effectZh))
                .AddComponent<ImmunityPiercer>(c =>
                {
                    c.Descriptors = descriptors;
                    c.Other = other;
                })
                .Configure();
    }
}
