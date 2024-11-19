using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GiftData", menuName = "ScriptableObjects/GiftData")]
public class GiftData : ScriptableObject
{
    [Serializable]
    public class Gift
    {
        public string Name;
        public int Amount;
    }

    public Gift gift;
}
