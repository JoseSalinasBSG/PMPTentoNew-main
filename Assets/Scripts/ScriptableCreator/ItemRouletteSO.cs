using UnityEngine;


public abstract class ItemRouletteSO : ScriptableObject
{
    public Sprite spritePowerUp;
    public Sprite spriteIconPowerUp;
    public Color colorPowerUp = Color.yellow;

    public virtual void Raise()
    {

    }
}
