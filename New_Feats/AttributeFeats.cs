using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Globalmap.State.InputManager;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Class;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using UnityModManagerNet;

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

	public class ModSettings : UnityModManager.ModSettings
	{
		public bool EnableAttributes = true;
		public bool EnableBattles = true;
		public bool EnableSavings = true;
		public bool EnableChecks = true;
		public bool EnableSkills = true;
		public bool EnableCaster = true;


		public ContextRankProgression progression = ContextRankProgression.AsIs;

		public override void Save(UnityModManager.ModEntry modEntry)
		{
			Save(this, modEntry);
		}
	}


	internal static class MainAbilityToEverything_Feats
	{
		private static readonly ModifierDescriptor Desc = ModifierDescriptor.Inherent;

		private static readonly StatType[] AttributeStats = new[]
		{
			StatType.Strength,
			StatType.Dexterity,
			StatType.Constitution,
			StatType.Intelligence,
			StatType.Wisdom,
			StatType.Charisma,
		};
		private static readonly StatType[] BattleStats = new[]
		{
			StatType.BaseAttackBonus,
			StatType.AdditionalAttackBonus,
			StatType.AdditionalDamage,
			StatType.AttackOfOpportunityCount,
			StatType.AC,
			StatType.AdditionalCMB,
			StatType.AdditionalCMD,

			StatType.HitPoints,
			StatType.Initiative,
			StatType.Speed,
			StatType.SneakAttack,
			StatType.Reach,
		};
		private static readonly StatType[] SaveStats = new[]
		{
			StatType.SaveFortitude,
			StatType.SaveReflex,
			StatType.SaveWill,
		};
		private static readonly StatType[] CheckStats = new[]
		{
			StatType.CheckBluff,
			StatType.CheckDiplomacy,
			StatType.CheckIntimidate,
		};
		private static readonly StatType[] SkillStats = new[]
		{
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

		private static readonly StatType[] CasterStats = new[]
		{
			StatType.BonusCasterLevel,
		};




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

			foreach (var f in feats)
			{
				var fc = FeatureConfigurator.For(f);
				foreach (var other in feats.Where(x => x != f))
					fc.AddPrerequisiteNoFeature(other);
				fc.Configure();
			}

		}

		private static void AddRank(FeatureConfigurator cfg, StatType stat, ContextRankProgression prog)
		{
			// 创建基础配置
			var config = ContextRankConfigs.StatBonus(stat: stat, min: 0);

			// 根据枚举值，链式调用对应的封装方法
			switch (prog)
			{
				case ContextRankProgression.AsIs:
					break;
				case ContextRankProgression.Div2:
					config.WithDiv2Progression();
					break;
				case ContextRankProgression.HalfMore:
					config.WithHalfMoreProgression();
					break;
				default:
					break;
			}

			cfg.AddContextRankConfig(config);
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
				.SetDescription(L(descKey, descValue, tagEncyclopediaEntries: true))
				;
			var s = Main.Settings;

			AddRank(cfg, baseStat, s.progression);

			Action<StatType[]> addBonuses = (stats) => {
				foreach (var stat in stats)
				{
					cfg.AddComponent<AddContextStatBonus>(c => {
						c.Stat = stat;
						c.Descriptor = Desc;
						c.Value = new ContextValue { ValueType = ContextValueType.Rank };
					});
				}
			};

			if (s.EnableAttributes) addBonuses(AttributeStats);
			if (s.EnableBattles) addBonuses(BattleStats);
			if (s.EnableSavings) addBonuses(SaveStats);
			if (s.EnableChecks) addBonuses(CheckStats);
			if (s.EnableSkills) addBonuses(SkillStats);

			if (s.EnableCaster)
			{
				addBonuses(CasterStats);


				cfg.AddComponent<IncreaseAllSpellsDC>(c =>
				{
					c.Value = new ContextValue()
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
					c.Descriptor = Desc;
					c.SpellsOnly = false;
				});

				cfg.AddComponent<IncreaseCasterLevel>(c =>
				{
					c.Value = new ContextValue()
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
					c.Descriptor = Desc;
				});
				cfg.AddComponent<SpellPenetrationBonus>(c =>
				{
					c.Value = new ContextValue()
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});


				cfg.AddComponent<AddAbilityUseTrigger>(c =>
				{
					c.FromSpellbook = true;
					c.Action = new ActionList
					{
						Actions = new GameAction[] {
							new ContextActionCastSpell() {
								MarkAsChild = true,
							}
						}
					};
				});

			}
			cfg.AddRecalculateOnStatChange(stat: baseStat);


			return cfg.Configure();
		}
	}
}
