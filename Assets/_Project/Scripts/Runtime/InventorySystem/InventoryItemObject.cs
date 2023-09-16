using KBCore.Refs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.IS
{
	public abstract class InventoryItemObject : ValidatedMonoBehaviour
	{
		[InlineEditor][SerializeField, Anywhere] private InventoryItemDataSO _itemData;
		public InventoryItemDataSO ItemData => _itemData;
		public bool IsPickedUp { get; set; }

		public void HandleItemPickup()
		{
			InventorySystem.Instance.Add(_itemData);
			IsPickedUp = true;
			gameObject.SetActive(false);
		}
	}
}