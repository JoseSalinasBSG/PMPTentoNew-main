using UnityEngine;

namespace Assets.Scripts.Services
{
    public class JsonDataParser : IDataParser
    {
        public T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}