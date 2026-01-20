using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using UnityEngine;

namespace AttributeFeats.New_Component
{
	[AllowedOn(typeof(BlueprintFact), false)]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	[AllowMultipleComponents]
	[TypeId("A9675332-F963-4F5F-AA97-7E6E85E0D8D1")]
	[Serializable]
	internal class DRComponent : AddDamageResistanceBase
	{
		[SerializeField]
		public StatType stat = StatType.Charisma;
		[SerializeField]
		public bool m_IsStackable = true;
		[SerializeField]
		public int Multiplier = 1;

		protected override bool Bypassed(AddDamageResistanceBase.ComponentRuntime runtime, BaseDamage damage, ItemEntityWeapon weapon)
		{
			return false;
		}
		protected override int CalculateValue(AddDamageResistanceBase.ComponentRuntime runtime)
		{
			int x = runtime.Owner.Stats.GetStat(this.stat).PermanentValue / 2 - 5;
			x *= this.Multiplier;
			return x;
		}
		public override bool IsStackable
		{
			get
			{
				return this.m_IsStackable;
			}
		}
	}
}