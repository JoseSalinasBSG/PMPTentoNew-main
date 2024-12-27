using ScriptableCreator;
using UnityEngine;

public interface IStoreItem
{
    void SetData(StoreController storeController, float cost, int amount, Sprite sprite, PowerUpSO powerUp);
}