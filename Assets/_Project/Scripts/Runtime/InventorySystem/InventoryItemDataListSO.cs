using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.IS
{
	[CreateAssetMenu(menuName = "ScriptableObjects/Inventory Item Data List", fileName = "NewInventoryItemDataList")]
	public class InventoryItemDataListSO : ScriptableObject
	{
		[TableList] public List<InventoryItemDataSO> itemDataList;
	}
}