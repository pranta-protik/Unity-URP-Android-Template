using UnityEngine;

namespace Project.IS
{
	public class InventoryItemCollector : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.TryGetComponent(out InventoryItemObject itemObject))
			{
				itemObject.HandleItemPickup();
			}
		}
	}
}