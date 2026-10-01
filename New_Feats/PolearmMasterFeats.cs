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
                .SetDisplayName(Common.L("PolearmMaster.Name", "Polearm Master", "长柄武器宗师"))
                .SetDescription(Common.L("PolearmMaster.Desc", BuildDescriptionEn(), BuildDescriptionZh(), tagEncyclopediaEntries: true))
                .SetIconIfPresent("PolearmMaster")
                .AddReachMultiplicator(Desc, multiplicator: 2)
                .AddStatBonus(descriptor: Desc, stat: StatType.AdditionalDamage, value: -4)
                .Configure();
        }

        private static string BuildDescriptionEn()
            => "<i>Polearm Master · Reach Tradeoff</i>\n<i>Domain of the Spear-Point.</i> True spear-masters do not merely swing a weapon; they command the geometry of space. By adjusting hand placement along the haft to its maximum extension, you double your threat radius, controlling the battlefield through unyielding reach at the cost of leveraged cutting force.\n\n<b>Effect:</b> Doubles your reach. You take a -4 untyped penalty to weapon damage.\n\n<b>Restrictions:</b> Polearm Master is a single feat with no intra-family variants.";

        private static string BuildDescriptionZh()
            => "<i>武器特化 · 触及专精</i>\n<i>一寸长，一寸强。</i>真正的枪矛大宗师非但运用兵刃，更御统战场空间之维度。将握持手位退至枪杆末端极限，以牺牲近身发力杠杆为代价，换取触角倍增的绝对威慑半径，令万敌难近。\n\n<b>效果：</b>使你的触及范围翻倍。你的武器伤害检定承受-4无类型减值。\n\n<b>限制：</b>“长柄武器宗师”为独立专长，无同类变体。";
    }
}
