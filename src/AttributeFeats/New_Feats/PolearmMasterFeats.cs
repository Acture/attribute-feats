using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;

namespace AttributeFeats.New_Feats
{
    internal static class PolearmMasterFeats
    {
        private const ModifierDescriptor Desc = ModifierDescriptor.None;
        private static bool Initialized;

        public static void ConfigureAll()
        {
            if (Initialized) return;
            Initialized = true;

            FeatureConfigurator.New("PolearmMaster", Guids.PolearmMaster.Feature.PolearmMaster, FeatureGroup.Feat)
                .SetDisplayName(Common.L("PolearmMaster.Name", "Polearm Master"))
                .SetDescription(Common.L("PolearmMaster.Desc", BuildDescription(), tagEncyclopediaEntries: true))
                .AddReachMultiplicator(Desc, multiplicator: 2)
                .AddStatBonus(descriptor: Desc, stat: StatType.AdditionalDamage, value: -4)
                .Configure();
        }

        private static string BuildDescription()
            => "<i>Polearm Master · Reach Tradeoff</i>\n<i>Length Over Weight.</i> You sacrifice striking force to command more space with the weapon's full extension.\n\n<b>Effect:</b> Doubles your reach. You take a -4 untyped penalty to weapon damage.\n\n<b>Restrictions:</b> Polearm Master is a single feat with no intra-family variants.";
    }
}
