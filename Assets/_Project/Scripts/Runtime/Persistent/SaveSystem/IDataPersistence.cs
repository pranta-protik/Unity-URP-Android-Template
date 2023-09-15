namespace Project.Persistent.SaveSystem
{
    public interface IDataPersistence
    {
        public void LoadData(GameData gameData);
        public void SaveData(GameData gameData);
    }
}