using System;
using UnityEngine;

namespace Project.Persistent.SaveSystem
{
	[Serializable]
	public class GameData
	{
		public SerializableDictionary<int, Vector3> playerPositionDictionary;
		public SerializableDictionary<int, float> planePositionDictionary;
		public SerializableDictionary<int, string> activeControlTypeDictionary;
		public SerializableDictionary<int, SerializableDictionary<int, bool>> dictionaryOfScrewdriverHolderDictionary;
		public SerializableDictionary<string, int> inventorySystemDictionary;
		public SerializableDictionary<int, SerializableDictionary<string, int>> dictionaryOfInventorySystemDictionary;

		public GameData()
		{
			playerPositionDictionary = new SerializableDictionary<int, Vector3>();
			planePositionDictionary = new SerializableDictionary<int, float>();
			activeControlTypeDictionary = new SerializableDictionary<int, string>();
			dictionaryOfScrewdriverHolderDictionary = new SerializableDictionary<int, SerializableDictionary<int, bool>>();
			inventorySystemDictionary = new SerializableDictionary<string, int>();
			dictionaryOfInventorySystemDictionary = new SerializableDictionary<int, SerializableDictionary<string, int>>();
		}
	}
}