using System.Collections.Generic;
using KBCore.Refs;
using Project.Managers;
using Project.Persistent.SaveSystem;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Project.IS
{
	public class InventorySystem : Singleton<InventorySystem>, IDataPersistence
	{
		public event UnityAction OnInventoryUpdated;

		[InlineEditor][SerializeField, Anywhere] private InventoryItemDataListSO _inventoryItemDataList;
		private Dictionary<InventoryItemDataSO, string> _inventoryItemDataDictionary;
		private Dictionary<string, InventoryItemDataSO> _inventoryItemDataInverseDictionary;
		[ShowInInspector][ShowIf("@_inventoryItemsDictionary != null")] private Dictionary<InventoryItemDataSO, InventoryItem> _inventoryItemsDictionary;
		public List<InventoryItem> InventoryItemsList { get; private set; }

#if UNITY_EDITOR
		private void OnValidate() => this.ValidateRefs();
#endif

		protected override void OnAwake()
		{
			base.OnAwake();

			_inventoryItemDataDictionary = new Dictionary<InventoryItemDataSO, string>();
			_inventoryItemDataInverseDictionary = new Dictionary<string, InventoryItemDataSO>();

			foreach (var itemData in _inventoryItemDataList.itemDataList)
			{
				_inventoryItemDataDictionary.Add(itemData, itemData.name);
				_inventoryItemDataInverseDictionary.Add(itemData.name, itemData);
			}

			_inventoryItemsDictionary = new Dictionary<InventoryItemDataSO, InventoryItem>();
			InventoryItemsList = new List<InventoryItem>();
		}

		public InventoryItem Get(InventoryItemDataSO itemData)
		{
			if (_inventoryItemsDictionary.TryGetValue(itemData, out InventoryItem item))
			{
				return item;
			}

			return null;
		}

		public void Add(InventoryItemDataSO itemData)
		{
			if (_inventoryItemsDictionary.TryGetValue(itemData, out InventoryItem item))
			{
				item.AddToStack();
			}
			else
			{
				var newItem = new InventoryItem(itemData);
				InventoryItemsList.Add(newItem);
				_inventoryItemsDictionary.Add(itemData, newItem);
			}

			OnInventoryUpdated?.Invoke();
		}

		public void Remove(InventoryItemDataSO itemData, int amount)
		{
			if (_inventoryItemsDictionary.TryGetValue(itemData, out InventoryItem item))
			{
				item.RemoveFromStack(amount);

				if (item.StackSize == 0)
				{
					InventoryItemsList.Remove(item);
					_inventoryItemsDictionary.Remove(itemData);
				}
			}

			OnInventoryUpdated?.Invoke();
		}

		public void LoadData(GameData gameData)
		{
			if (gameData.dictionaryOfInventorySystemDictionary.TryGetValue(SceneUtils.GetActiveSceneIndex(), out var inventorySystemDictionary))
			{
				foreach (var keyValuePair in inventorySystemDictionary)
				{
					if (_inventoryItemDataInverseDictionary.TryGetValue(keyValuePair.Key, out var itemData))
					{
						var newItem = new InventoryItem(itemData, keyValuePair.Value);
						InventoryItemsList.Add(newItem);
						_inventoryItemsDictionary.Add(itemData, newItem);
					}
				}
			}
			else
			{
				foreach (var keyValuePair in gameData.inventorySystemDictionary)
				{
					if (_inventoryItemDataInverseDictionary.TryGetValue(keyValuePair.Key, out var itemData))
					{
						var newItem = new InventoryItem(itemData, keyValuePair.Value);
						InventoryItemsList.Add(newItem);
						_inventoryItemsDictionary.Add(itemData, newItem);
					}
				}
			}
		}

		public void SaveData(GameData gameData)
		{
			if (gameData.dictionaryOfInventorySystemDictionary.ContainsKey(SceneUtils.GetActiveSceneIndex()))
			{
				gameData.dictionaryOfInventorySystemDictionary.Remove(SceneUtils.GetActiveSceneIndex());
			}

			if (!GameManager.Instance.IsGameOver())
			{
				SerializableDictionary<string, int> inventorySystemDictionary = new();

				foreach (var keyValuePair in _inventoryItemsDictionary)
				{
					inventorySystemDictionary.Add(_inventoryItemDataDictionary[keyValuePair.Key], keyValuePair.Value.StackSize);
				}

				gameData.dictionaryOfInventorySystemDictionary.Add(SceneUtils.GetActiveSceneIndex(), inventorySystemDictionary);
			}

			if (GameManager.Instance.CurrentGameState == GameManager.GameState.Completed)
			{
				gameData.inventorySystemDictionary.Clear();

				foreach (var keyValuePair in _inventoryItemsDictionary)
				{
					gameData.inventorySystemDictionary.Add(_inventoryItemDataDictionary[keyValuePair.Key], keyValuePair.Value.StackSize);
				}
			}
		}
	}
}