using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics;

namespace WotRHomebrew.Feats
{
    /// <summary>Shared builders for feat families: bilingual descriptions, feats and buffs.</summary>
    internal static class FeatBuilders
    {
        public static (string en, string zh) Desc(string familyEn, string familyZh, string effectEn, string effectZh)
            => ($"<i>{familyEn}</i>\n\n<b>Effect:</b> {effectEn}", $"<i>{familyZh}</i>\n\n<b>效果：</b>{effectZh}");

        public static FeatureConfigurator Feat(FeatSelection family, string name, string guid, string nameEn, string nameZh, (string en, string zh) desc)
        {
            desc = FeatLore.Apply(name, desc);
            return family.NewFeat(name, guid)
                .SetDisplayName(Common.L($"{name}.Name", nameEn, nameZh))
                .SetDescription(Common.L($"{name}.Desc", desc.en, desc.zh, tagEncyclopediaEntries: true))
                .SetIconIfPresent(name);
        }

        public static BuffConfigurator Buff(string name, string guid, string nameEn, string nameZh, string descEn, string descZh)
            => BuffConfigurator.New(name, guid)
                .SetDisplayName(Common.L($"{name}.Name", nameEn, nameZh))
                .SetDescription(Common.L($"{name}.Desc", descEn, descZh))
                .SetIconIfPresent(name);

        public static ContextDurationValue Rounds(int rounds) => ContextDuration.Fixed(rounds);
    }
}
