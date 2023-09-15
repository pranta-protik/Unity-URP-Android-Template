using KBCore.Refs;
using Project.IS;
using UnityEngine;

namespace Project.UI
{
	public class InventoryBarUI : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private Transform _inventoryItemSlotPrefab;

		private void Start()
		{
			InventorySystem.Instance.OnInventoryUpdated += InventorySystem_OnInventoryUpdated;
			InventorySystem_OnInventoryUpdated();
		}

		private void OnDestroy()
		{
			InventorySystem.Instance.OnInventoryUpdated -= InventorySystem_OnInventoryUpdated;
		}

		private void InventorySystem_OnInventoryUpdated()
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}

			DrawInventory();
		}

		private void DrawInventory()
		{
			foreach (var inventoryItem in InventorySystem.Instance.InventoryItemsList)
			{
				AddInventorySlot(inventoryItem);
			}
		}

		private void AddInventorySlot(InventoryItem item)
		{
			var inventoryItemSlotGO = Instantiate(_inventoryItemSlotPrefab, transform);

			var inventoryItemSlotUI = inventoryItemSlotGO.GetComponent<InventoryItemSlotUI>();
			inventoryItemSlotUI.Set(item);
		}
	}
}