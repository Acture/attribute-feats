using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils; // LocalizationTool 在这里（你这版没有 AddString）
using BlueprintCore.Utils.Types;                // ContextValues
using Kingmaker.UnitLogic.Mechanics.Properties; // UnitProperty

using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using AttributeFeats.New_Component;

namespace AttributeFeats.New_Feats
{
	internal static class Guids
	{
		public const string str_main_to_everything = "a4c66462-a423-4a2f-8b26-770ea03d2ce0";
		public const string dex_main_to_everything = "0963babc-0579-4bb3-a33a-23949b47e68b";
		public const string con_main_to_everything = "52506f39-5c40-4780-b677-68336c44dcaa";
		public const string int_main_to_everything = "df0cb753-5704-42a0-bad0-627757af281f";
		public const string wis_main_to_everything = "41584589-3703-41c8-9a51-809805c921ad";
		public const string cha_main_to_everything = "e16525c7-ec29-4904-a69c-8b35f0c60a95";
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





			StatType.AdditionalCMB,
			StatType.AdditionalCMD,
			StatType.Reach,

			StatType.BaseAttackBonus,
			StatType.AdditionalDamage,
			StatType.AttackOfOpportunityCount,

			StatType.Speed,

			StatType.CheckBluff,
			StatType.CheckDiplomacy,
			StatType.CheckIntimidate,

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

			StatType.HitPoints,

			StatType.BonusCasterLevel,
		};

		// ✅ 用 CreateString 直接创建本地化字符串（你这版没有 AddString）
		private static LocalizedString L(string key, string value, bool tagEncyclopediaEntries = true)
			=> LocalizationTool.CreateString(key, value, tagEncyclopediaEntries);
		private static bool Initialized;


		public static void ConfigureAll()
		{
			if (Initialized) return;
			Initialized = true;
			var feats = new List<BlueprintFeature>
			{
				CreateOne(
					StatType.Strength,
					"StrMainToEverything",
					Guids.str_main_to_everything,
					"StrMainToEverything.Name",
					"Main Attribute: Strength",
					"StrMainToEverything.Description",
					"Choose Strength as your main attribute. Its modifier is added to all attributes (including itself) and to AC, saving throws, initiative, skills, BAB, additional damage, and speed. Mutually exclusive with other main-attribute feats."
				),
				CreateOne(
					StatType.Dexterity,
					"DexMainToEverything",
					Guids.dex_main_to_everything,
					"DexMainToEverything.Name",
					"Main Attribute: Dexterity",
					"DexMainToEverything.Description",
					"Choose Dexterity as your main attribute. Its modifier is added to all attributes (including itself) and to AC, saving throws, initiative, skills, BAB, additional damage, and speed. Mutually exclusive with other main-attribute feats."
				),
				CreateOne(
					StatType.Constitution,
					"ConMainToEverything",
					Guids.con_main_to_everything,
					"ConMainToEverything.Name",
					"Main Attribute: Constitution",
					"ConMainToEverything.Description",
					"Choose Constitution as your main attribute. Its modifier is added to all attributes (including itself) and to AC, saving throws, initiative, skills, BAB, additional damage, and speed. Mutually exclusive with other main-attribute feats."
				),
				CreateOne(
					StatType.Intelligence,
					"IntMainToEverything",
					Guids.int_main_to_everything,
					"IntMainToEverything.Name",
					"Main Attribute: Intelligence",
					"IntMainToEverything.Description",
					"Choose Intelligence as your main attribute. Its modifier is added to all attributes (including itself) and to AC, saving throws, initiative, skills, BAB, additional damage, and speed. Mutually exclusive with other main-attribute feats."
				),
				CreateOne(
					StatType.Wisdom,
					"WisMainToEverything",
					Guids.wis_main_to_everything,
					"WisMainToEverything.Name",
					"Main Attribute: Wisdom",
					"WisMainToEverything.Description",
					"Choose Wisdom as your main attribute. Its modifier is added to all attributes (including itself) and to AC, saving throws, initiative, skills, BAB, additional damage, and speed. Mutually exclusive with other main-attribute feats."
				),
				CreateOne(
					StatType.Charisma,
					"ChaMainToEverything",
					Guids.cha_main_to_everything,
					"ChaMainToEverything.Name",
					"Main Attribute: Charisma",
					"ChaMainToEverything.Description",
					"Choose Charisma as your main attribute. Its modifier is added to all attributes (including itself) and to AC, saving throws, initiative, skills, BAB, additional damage, and speed. Mutually exclusive with other main-attribute feats."
				),
			};

			// 互斥：每个 feat 禁止同时拥有另外 5 个
			foreach (var f in feats)
			{
				var fc = FeatureConfigurator.For(f);
				foreach (var other in feats.Where(x => x != f))
					fc.AddPrerequisiteNoFeature(other);
				fc.Configure();
			}

			//// 加入通用专长池
			//foreach (var f in feats)
			//{
			//	FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
			//		.AddToAllFeatures(f)
			//		.Configure();
			//}
		}




		private static BlueprintFeature CreateOne(
			StatType baseStat,
			string internalName,
			string guid,
			string nameKey,
			string nameValue,
			string descKey,
			string descValue)
		{
			var cfg = FeatureConfigurator.New(internalName, guid, FeatureGroup.Feat)
				.SetDisplayName(L(nameKey, nameValue, tagEncyclopediaEntries: false))
				.SetDescription(L(descKey, descValue, tagEncyclopediaEntries: true));

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
