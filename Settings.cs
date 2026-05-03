using System.Xml.Serialization;
using Kingmaker.UnitLogic.Mechanics.Components;
using UnityModManagerNet;

namespace AttributeFeats
{
	[XmlType(Namespace = "AttributeFeats")]
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
}
