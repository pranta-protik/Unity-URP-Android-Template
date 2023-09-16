using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;

namespace Project.IS
{
	[CreateAssetMenu(menuName = "ScriptableObjects/Inventory Item Data List", fileName = "NewInventoryItemDataList")]
	public class InventoryItemDataListSO : ScriptableObject
	{
		[TableList] public List<InventoryItemDataSO> itemDataList;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (itemDataList == null || (itemDataList != null && itemDataList.Count == 0))
			{
				DebugUtils.LogWarning("Item data list should not be null.");
			}
		}
#endif
	}
}