using KBCore.Refs;
using Project.IS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
	public class InventoryItemSlotUI : ValidatedMonoBehaviour
	{
		[Header("References")]
		[SerializeField, Anywhere] private Image _icon;
		[SerializeField, Anywhere] private TextMeshProUGUI _label;
		[SerializeField, Anywhere] private TextMeshProUGUI _stackLabel;

		public void Set(InventoryItem item)
		{
			_icon.sprite = item.ItemData.icon;
			_label.text = item.ItemData.displayName;
			_stackLabel.text = item.GetTotalValue().ToString();
		}
	}
}