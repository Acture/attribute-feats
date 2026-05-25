using System;

namespace AttributeFeats.New_Feats
{
    /// <summary>
    /// Cross-family same-attribute mutex pass.
    ///
    /// Historically this pass linked every same-attribute feat across families
    /// (Main(X), Specialized(X), Stance(X), Weapon Insight(X), themed
    /// Conditional/Extended/SpellTag) with bidirectional mutex prerequisites.
    ///
    /// As of 0.1.1 the cross-family mutex is intentionally removed:
    /// AttributeFeats is a build enabler, so combining e.g. `Apex Predator`
    /// (Str Main) with `Titan's Stance` (Str Defensive) is now allowed.
    /// Intra-family mutex is still enforced inside each feat file
    /// (Main 6-way, Specialized 6-way per subfamily, Stance 6-way,
    /// Weapon Insight 6-way, Greater Summoning 6-way, Summoner Sacrifice 3-way,
    /// SpellTag school 8-way + descriptor 9-way).
    ///
    /// All mutex (intra-family and the now-empty cross-family pass) can be
    /// disabled globally with the `EnableMutex` mod setting.
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

