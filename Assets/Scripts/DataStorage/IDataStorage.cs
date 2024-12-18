using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStorage
{
    public interface IDataStorage
    {
        void Save<T>(string key, T data);
        T Load<T>(string key);
        bool HasKey(string key);
        void DeleteAll();
    }
}
