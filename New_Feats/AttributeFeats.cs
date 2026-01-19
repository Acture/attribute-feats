using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;

namespace AttributeFeats.New_Feats
{
	internal static class Guids
	{
		public const string str_main_to_everything = "11111111-1111-1111-1111-111111111111";
		public const string dex_main_to_everything = "22222222-2222-2222-2222-222222222222";
		public const string con_main_to_everything = "33333333-3333-3333-3333-333333333333";
		public const string int_main_to_everything = "44444444-4444-4444-4444-444444444444";
		public const string wis_main_to_everything = "55555555-5555-5555-5555-555555555555";
		public const string cha_main_to_everything = "66666666-6666-6666-6666-666666666666";
	}
	internal static class MainAbilityToEverything_Feats
	{
		private static readonly ModifierDescriptor Desc = ModifierDescriptor.Inherent;

		private static readonly StatType[] AbilityStats = new[]
		{
	  StatType.Strength,
	  StatType.Dexterity,
	  StatType.Constitution,
	  StatType.Intelligence,
	  StatType.Wisdom,
	  StatType.Charisma,
	};

		private static readonly StatType[] DerivedTargets = new[]
		{
	  StatType.AC,
	  StatType.Initiative,
	  StatType.SaveFortitude,
	  StatType.SaveReflex,
	  StatType.SaveWill,
	  StatType.BaseAttackBonus,
	  StatType.AdditionalDamage,
	  StatType.Speed,

	  StatType.SkillAthletics,
	  StatType.SkillKnowledgeArcana,
	  StatType.SkillKnowledgeWorld,
	  StatType.SkillLoreNature,
	  StatType.SkillLoreReligion,
	  StatType.SkillMobility,
	  StatType.SkillPerception,
	  StatType.SkillPersuasion,
	  StatType.SkillStealth,
	  StatType.SkillThievery,
	  StatType.SkillUseMagicDevice,
	};

		public static void ConfigureAll()
		{
			// 1) 先创建 6 个 feat
			var feats = new List<BlueprintFeature>
	  {
		CreateOne(StatType.Strength,     "StrMainToEverything", Guids.str_main_to_everything, "StrMainToEverything.Name", "StrMainToEverything.Description"),
		CreateOne(StatType.Dexterity,    "DexMainToEverything", Guids.dex_main_to_everything, "DexMainToEverything.Name", "DexMainToEverything.Description"),
		CreateOne(StatType.Constitution, "ConMainToEverything", Guids.con_main_to_everything, "ConMainToEverything.Name", "ConMainToEverything.Description"),
		CreateOne(StatType.Intelligence, "IntMainToEverything", Guids.int_main_to_everything, "IntMainToEverything.Name", "IntMainToEverything.Description"),
		CreateOne(StatType.Wisdom,       "WisMainToEverything", Guids.wis_main_to_everything, "WisMainToEverything.Name", "WisMainToEverything.Description"),
		CreateOne(StatType.Charisma,     "ChaMainToEverything", Guids.cha_main_to_everything, "ChaMainToEverything.Name", "ChaMainToEverything.Description"),
	  };

			// 2) 互斥：每个 feat 禁止同时拥有另外 5 个
			foreach (var f in feats)
			{
				var fc = FeatureConfigurator.For(f);
				foreach (var other in feats.Where(x => x != f))
				{
					// 方式 A：如果你的 BlueprintCore 版本有该 helper（多数都有）
					fc.AddPrerequisiteNoFeature(other);
				}
				fc.Configure();
			}

			// 3) 全部加入通用专长池（玩家升级选 feat 时能看到）
			foreach (var f in feats)
			{
				FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
				  .AddToAllFeatures(f)
				  .Configure();
			}
		}

		private static BlueprintFeature CreateOne(
		  StatType baseStat,
		  string internalName,
		  string guid,
		  string nameKey,
		  string descKey)
		{
			var cfg = FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
			  .SetDisplayName(nameKey)
			  .SetDescription(descKey);

			// X -> 六维属性（含 X->X 自指）
			foreach (var abil in AbilityStats)
				cfg.AddDerivativeStatBonus(baseStat: baseStat, descriptor: Desc, derivativeStat: abil);

			// X -> 全覆盖派生项
			foreach (var t in DerivedTargets)
				cfg.AddDerivativeStatBonus(baseStat: baseStat, descriptor: Desc, derivativeStat: t);

			// 只监听主属性变化
			cfg.AddRecalculateOnStatChange(stat: baseStat);

			return cfg.Configure();
		}
	}
}
