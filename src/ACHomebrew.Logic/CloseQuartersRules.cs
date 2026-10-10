using System;

namespace ACHomebrew.Feats
{
    public enum CloseQuartersMeasure
    {
        // Close range (melee).
        Distance,
        Weapon,
        Reach,
        // Mid range (melee, reach weapons).
        MidDistance,
        ReachWeapon,
        LongReach,
        // Long range (ranged and thrown weapons).
        LongDistance,
        RangedWeapon,
        Unengaged,
    }

    /// <summary>
    /// Melee damage scores for the three close-quarters feats, each worth up to +4.
    /// Shorter weapons, shorter total reach and closer hits score higher.
    /// </summary>
    internal static class CloseQuartersRules
    {
        public const int Max = 4;

        /// <summary>Light, unarmed and natural 4; one-handed 3; two-handed 2; reach weapons 0.</summary>
        public static int WeaponScore(bool light, bool twoHanded, bool natural, bool unarmed, bool reachWeapon)
        {
            if (reachWeapon) return 0;
            if (light || natural || unarmed) return 4;
            return twoHanded ? 2 : 3;
        }

        /// <summary>Total reach (weapon plus creature, in nominal feet): 5 ft 4, 10 ft 2, 15 ft or more 0.</summary>
        public static int ReachScore(int totalReachFeet)
        {
            var steps = (int)Math.Ceiling(Math.Max(0, totalReachFeet - 5) / 5.0);
            return Math.Max(0, Max - 2 * steps);
        }

        /// <summary>
        /// Distance from the attacker's centre to the target's edge at the moment of the hit.
        /// Each 1.5 ft costs one point, so smaller attackers hugging their target score highest.
        /// </summary>
        public static int DistanceScore(float distanceFeet)
            => Math.Max(0, Max - (int)Math.Floor(Math.Max(0f, distanceFeet) / 1.5f));

        /// <summary>
        /// Wrath stores melee weapon range as edge-to-edge distance: 5 ft weapons become
        /// at most 1-ft, 10 ft reach weapons 6 ft. Converts back to the nominal 5/10 ft.
        /// </summary>
        public static int NominalWeaponReach(float effectiveFeet) => effectiveFeet > 5f ? 10 : 5;

        /// <summary>Melee measures score close range; mid-range measures score melee at the edge of reach.</summary>
        public static bool IsRanged(CloseQuartersMeasure measure)
            => measure is CloseQuartersMeasure.LongDistance or CloseQuartersMeasure.RangedWeapon or CloseQuartersMeasure.Unengaged;

        /// <summary>Mid range: edge-to-edge distance at the hit; each 1.5 ft farther adds one point.</summary>
        public static int MidDistanceScore(float edgeFeet)
            => Math.Min(Max, (int)Math.Floor(Math.Max(0f, edgeFeet) / 1.5f));

        /// <summary>Reach weapons 4, two-handed weapons 2, others 0.</summary>
        public static int ReachWeaponScore(bool twoHanded, bool reachWeapon) => reachWeapon ? 4 : twoHanded ? 2 : 0;

        /// <summary>Total reach: 5 ft 0, 10 ft 2, 15 ft or more 4 (the mirror of ReachScore).</summary>
        public static int LongReachScore(int totalReachFeet) => Max - ReachScore(totalReachFeet);

        /// <summary>Long range: +1 per full 10 ft beyond 10 ft.</summary>
        public static int LongDistanceScore(float feet)
            => Math.Max(0, Math.Min(Max, (int)Math.Floor((feet - 10f) / 10f)));

        /// <summary>Ranged weapon range: 50 ft+ 4, 40 ft 3, 30 ft 2, 20 ft 1, shorter 0.</summary>
        public static int RangedWeaponScore(int rangeFeet)
            => rangeFeet >= 50 ? 4 : rangeFeet >= 40 ? 3 : rangeFeet >= 30 ? 2 : rangeFeet >= 20 ? 1 : 0;

        /// <summary>+4 while no enemy engages you in melee.</summary>
        public static int UnengagedScore(bool engaged) => engaged ? 0 : Max;
    }
}
