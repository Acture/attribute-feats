using static WotRHomebrew.Feats.FeatBuilders;

namespace WotRHomebrew.Feats
{
    /// <summary>Penetration (weapons): critical hits and precision damage against immune creatures.</summary>
    internal static class WeaponPenetrationFeats
    {
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            Feat(FeatSelection.Penetration, "BoneBreaker", Guids.Penetration.BoneBreaker, "Bone Breaker", "碎骨断筋",
                    Desc("Penetration", "穿透",
                        "Your critical hits apply even to creatures immune to critical hits.",
                        "你的重击对免疫重击的生物同样生效。"))
                .AddComponent<Kingmaker.Designers.Mechanics.Facts.IgnoreCritImmunity>()
                .Configure();

            Feat(FeatSelection.Penetration, "HiddenVitals", Guids.Penetration.HiddenVitals, "Hidden Vitals", "寻隙刺要",
                    Desc("Penetration", "穿透",
                        "Your sneak attacks and other precision damage apply even to creatures immune to precision damage.",
                        "你的偷袭与其他精准伤害对免疫精准伤害的生物同样生效。"))
                .AddComponent<ImmunityPiercer>(c => c.Other = PiercedImmunity.Precision)
                .Configure();
        }
    }
}
