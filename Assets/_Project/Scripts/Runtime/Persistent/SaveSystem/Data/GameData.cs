using System;
using UnityEngine;

namespace Project.Persistent.SaveSystem
{
	[Serializable]
	public class GameData
	{
		public SerializableDictionary<int, Vector3> playerPositionDictionary;
		public SerializableDictionary<string, int> inventorySystemDictionary;
		public SerializableDictionary<int, SerializableDictionary<string, int>> dictionaryOfInventorySystemDictionary;

		public GameData()
		{
			playerPositionDictionary = new SerializableDictionary<int, Vector3>();
			inventorySystemDictionary = new SerializableDictionary<string, int>();
			dictionaryOfInventorySystemDictionary = new SerializableDictionary<int, SerializableDictionary<string, int>>();
		}
	}
}