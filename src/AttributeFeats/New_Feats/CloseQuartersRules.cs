using System;

namespace AttributeFeats.New_Feats
{
    public enum CloseQuartersMeasure
    {
        Distance,
        Weapon,
        Reach,
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
    }
}
