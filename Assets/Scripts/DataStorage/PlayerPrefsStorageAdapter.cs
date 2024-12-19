using System;
using System.Collections;
using UnityEngine;

namespace DataStorage
{
    public class PlayerPrefsStorageAdapter : IDataStorage
    {

        public void Save<T>(string key, T data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public T Load<T>(string key)
        {
            // Verificar si la clave existe
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"Key {key} does not exist");
                return default;
            }

            string json = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError($"No data found for key '{key}' or data is empty.");
                return default;
            }

            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)json;
                }
                else
                {
                    T data = JsonUtility.FromJson<T>(json);
                    return data;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse JSON for key '{key}': {e.Message}");
                return default;
            }
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }

}