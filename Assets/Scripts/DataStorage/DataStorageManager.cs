namespace DataStorage
{
    public class DataStorageManager
    {
        public IDataStorage currentStorage;

        public DataStorageManager(IDataStorage storage)
        {
            currentStorage = storage;
        }

        public void Save<T>(string key, T data)
        {
            currentStorage.Save(key, data);
        }

        public T Load<T>(string key)
        {
            return currentStorage.Load<T>(key);
        }

        public bool HasKey(string key)
        {
            return currentStorage.HasKey(key);
        }

        public void DeleteAll()
        {
            currentStorage.DeleteAll();
        }
    }
}