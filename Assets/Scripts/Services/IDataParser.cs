namespace Assets.Scripts.Services
{
    public interface IDataParser
    {
        T Deserialize<T>(string json);
        string Serialize<T>(T obj);
    }
}