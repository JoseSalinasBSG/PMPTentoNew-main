
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RouletteItem : MonoBehaviour
{
    [SerializeField] private Transform _originPoint;
    [SerializeField] private RouletteItemData _rouletteItemData;
    [SerializeField] private Image _imageItem;
    [SerializeField] private Sprite _iconItem;
    [SerializeField] private TextMeshProUGUI _amountLabel;
    public bool _haveInformation;
    
    public Image ImageItem => _imageItem;
    public Sprite IconItem => _iconItem;
    public TextMeshProUGUI AmountLabel => _amountLabel;
    private int _amount;

    public int Amount => _amount;
    public bool HaveInformation
    {
        get => _haveInformation;
        set => _haveInformation = value;
    }

    public RouletteItemData RouletteItemData
    {
        get => _rouletteItemData;
        set => _rouletteItemData = value;
    }
    public void SetData(RouletteItemData rouletteItemData)
    {
        _rouletteItemData = rouletteItemData;
        
        //Setear cantidad del item en cuestion
        _amount = !_rouletteItemData.UseAllAmount ? Random.Range(1, _rouletteItemData.Amount + 1) : _rouletteItemData.Amount;//segun valor de _rouletteItemData.UseAllAmount se ejecutara expresion1 o expresion 2
        
        _imageItem.sprite = _rouletteItemData._ItemRouletteSo.spritePowerUp;//asignar la imagen de powerup desde el SO
        _iconItem = _rouletteItemData._ItemRouletteSo.spriteIconPowerUp;//asignar el icono de powerup desde el SO
        
        _amountLabel.text = $"x{_amount}";
        HaveInformation = true;//Indica que ya se seteo informacion en el espacio de ruleta en cuestion
    }
}
