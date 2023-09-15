using System;

namespace Project.Persistent.SaveSystem
{
	[Serializable]
	public class GameData
	{
		public SerializableDictionary<string, int> inventorySystemDictionary;
		public SerializableDictionary<int, SerializableDictionary<string, int>> dictionaryOfInventorySystemDictionary;

		public GameData()
		{
			inventorySystemDictionary = new SerializableDictionary<string, int>();
			dictionaryOfInventorySystemDictionary = new SerializableDictionary<int, SerializableDictionary<string, int>>();
		}
	}
}