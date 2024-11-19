using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RouletteItemData
{
    public ItemRouletteSO _ItemRouletteSo;
    [FormerlySerializedAs("amount")] public int Amount;
    public bool UseAllAmount;
}
[CreateAssetMenu(menuName = "RouletteData", fileName = "RouletteSO")]
public class RouletteSO : ScriptableObject
{
    public RouletteItemData[] RouletteItems;
    
}
