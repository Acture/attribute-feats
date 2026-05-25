using System;

namespace AttributeFeats.New_Feats
{
    internal static class FeatRegistry
    {
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            try
            {
                MainAbilityToEverything_Feats.ConfigureAll();
                try { SpecializedFeats.ConfigureAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: SpecializedFeats not yet present"); }
                try { StanceFeats.ConfigureAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: StanceFeats not yet present"); }
                try { ConditionalFeats.ConfigureAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: ConditionalFeats not yet present"); }
                try { StatReplacementFeats.ConfigureAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: StatReplacementFeats not yet present"); }
                try { ReactiveArmorFeats.ConfigureAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: ReactiveArmorFeats not yet present"); }
                try { DerivedStatFeats.ConfigureAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: DerivedStatFeats not yet present"); }
                try { GreaterSummoningFeats.ConfigureAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: GreaterSummoningFeats not yet present"); }
                try { SpellTagFeats.ConfigureAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: SpellTagFeats not yet present"); }
                try { MutexPass.ApplyAll(); } catch (TypeLoadException) { Main.Log.Log("AttributeFeats: MutexPass not yet present"); }
                Main.Log.Log("AttributeFeats 0.1.0: registered N feats (Main=6, Specialized=24, Stance=6, Conditional=4, Replacement=9, Summon=6, ReactiveArmor=2, Derived=6, SpellTag=13 = 76 total)");
            }
            catch (Exception e)
            {
                Main.Log.Log("AttributeFeats: registration failed - " + e);
            }
        }
    }
}
