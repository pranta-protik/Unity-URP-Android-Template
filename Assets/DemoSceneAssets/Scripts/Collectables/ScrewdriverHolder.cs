using System.Collections.Generic;
using System.Linq;
using Project.Managers;
using Project.Persistent.SaveSystem;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;

namespace DemoScene.Collectables
{
	public class ScrewdriverHolder : MonoBehaviour, IDataPersistence
	{
		[Header("All collectable screwdrivers must be added")]
		[Header("as a child of this gameobject")]
		[SerializeField] private List<Screwdriver> _screwdriversList;

		[Button, PropertySpace]
		private void FindAllScrewdrivers() => _screwdriversList = GetComponentsInChildren<Screwdriver>().ToList();

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_screwdriversList == null || (_screwdriversList != null && _screwdriversList.Count != GetComponentsInChildren<Screwdriver>().Length))
			{
				DebugUtils.LogWarning("The number of screwdrivers in the scene does not match with the list which indicates something might be wrong.");
			}
		}
#endif

		public List<Screwdriver> GetCollectableCoinsList() => _screwdriversList;

		public void LoadData(GameData gameData)
		{
			if (gameData.dictionaryOfScrewdriverHolderDictionary.TryGetValue(SceneUtils.GetActiveSceneIndex(), out var screwdriverHolderDictionary))
			{
				foreach (var keyValuePair in screwdriverHolderDictionary)
				{
					if (keyValuePair.Value == true)
					{
						var screwdriver = _screwdriversList[keyValuePair.Key];
						screwdriver.IsPickedUp = true;
						screwdriver.gameObject.SetActive(false);
					}
				}
			}
		}

		public void SaveData(GameData gameData)
		{
			if (gameData.dictionaryOfScrewdriverHolderDictionary.ContainsKey(SceneUtils.GetActiveSceneIndex()))
			{
				gameData.dictionaryOfScrewdriverHolderDictionary.Remove(SceneUtils.GetActiveSceneIndex());
			}

			if (GameManager.Instance.IsGameOver()) return;

			SerializableDictionary<int, bool> screwdriverHolderDictionary = new();

			for (var i = 0; i < _screwdriversList.Count; i++)
			{
				if (_screwdriversList[i].IsPickedUp)
				{
					screwdriverHolderDictionary.Add(i, true);
				}
				else
				{
					screwdriverHolderDictionary.Add(i, false);
				}
			}

			gameData.dictionaryOfScrewdriverHolderDictionary.Add(SceneUtils.GetActiveSceneIndex(), screwdriverHolderDictionary);
		}
	}
}