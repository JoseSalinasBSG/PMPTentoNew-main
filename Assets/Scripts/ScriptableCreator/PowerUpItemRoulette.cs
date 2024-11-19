using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using UnityEngine;

[CreateAssetMenu(menuName = "Roulette-PowerUp", fileName = "RoulettePowerUpSO")]
public class PowerUpItemRoulette : ItemRouletteSO
{
    public ScripableObjectPowerUp powerUpSO;
    public virtual void Raise()
    {
        
    }
}
