using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.IS
{
	[CreateAssetMenu(menuName = "ScriptableObjects/Inventory Item Data", fileName = "NewInventoryItemData")]
	public class InventoryItemDataSO : ScriptableObject
	{
		[PreviewField(60), HideLabel]
		[HorizontalGroup("InventoryItemData", 60)]
		public Sprite icon;

		[HorizontalGroup("InventoryItemData", MarginLeft = 10)]
		[VerticalGroup("InventoryItemData/Right"), LabelWidth(120)]
		public GameObject prefab;

		[VerticalGroup("InventoryItemData/Right"), LabelWidth(120)]
		public string id;

		[VerticalGroup("InventoryItemData/Right"), LabelWidth(120)]
		public string displayName;

		[VerticalGroup("InventoryItemData/Right"), LabelWidth(120)]
		public int value;
	}
}