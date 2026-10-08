using System;

namespace AttributeFeats.New_Feats
{
    /// <summary>
    /// Mutual exclusion is no longer baked into blueprints as pairwise
    /// PrerequisiteNoFeature components. Exclusion groups (FeatGroupRules) are
    /// checked when a feat is selected and follow live settings: each group has
    /// its own switch and limit, and EnableMutex turns all of them off.
    /// This pass remains as a registration step for diagnostics.
    /// </summary>
    internal static class MutexPass
    {
        private static bool Applied;

        public static void ApplyAll()
        {
            if (Applied) return;
            Applied = true;

            try
            {
                Main.Log?.Log(Main.Settings != null && !Main.Settings.EnableMutex
                    ? "AttributeFeats: MutexPass skipped (EnableMutex = OFF; all mutex disabled)."
                    : "AttributeFeats: MutexPass cross-family pass disabled by design; intra-family mutex applied per family file.");
            }
            catch (Exception e)
            {
                Main.Log?.Log("AttributeFeats: MutexPass log failed - " + e);
            }
        }
    }
}

