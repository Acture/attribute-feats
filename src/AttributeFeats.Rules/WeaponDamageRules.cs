namespace AttributeFeats
{
    public enum WeaponDamageMode
    {
        Replace,
        Add,
    }
}

namespace AttributeFeats.New_Feats
{
    internal static class WeaponDamageRules
    {
        public static int CalculateAdjustment(WeaponDamageMode mode, bool matchesWeapon,
            bool hasDamageAttribute, int currentModifier, int selectedModifier, float multiplier)
        {
            if (!matchesWeapon || !hasDamageAttribute) return 0;
            if (mode == WeaponDamageMode.Add) return System.Math.Max(0, selectedModifier);

            // Wrath does not multiply negative ability modifiers, and truncates each
            // contribution separately (important for off-hand and two-handed attacks).
            var currentDamage = (int)(currentModifier * (currentModifier < 0 ? 1f : multiplier));
            var selectedDamage = (int)(selectedModifier * (selectedModifier < 0 ? 1f : multiplier));
            return System.Math.Max(0, selectedDamage - currentDamage);
        }
    }
}
