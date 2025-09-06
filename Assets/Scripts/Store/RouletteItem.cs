using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RouletteItem : MonoBehaviour
{
    [Header("Vista (asigna en Inspector)")]
    [Tooltip("Image del slice (uGUI) ya configurado como Filled/Radial360. Solo cambia el color.")]
    [FormerlySerializedAs("_imageItem")]
    [SerializeField] private Image _sliceImage;

    [Tooltip("Image del ícono que se muestra sobre el slice.")]
    [SerializeField] private Image _iconImage;

    private RouletteItemData _rouletteItemData;
    private bool _haveInformation;
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

    /// <summary>
    /// Aplica color (slice), ícono y cantidad desde el SO.
    /// No modifica rotación ni fillAmount.
    /// </summary>
    public void ApplyVisualFromSO(RouletteItemData data)
    {
        _rouletteItemData = data;

        // Cantidad (guardada para asignar premio y mostrar en popup)
        _amount = !_rouletteItemData.UseAllAmount
            ? Random.Range(1, _rouletteItemData.Amount + 1)
            : _rouletteItemData.Amount;

        var so = _rouletteItemData._ItemRouletteSo;

        // Color del slice
        if (_sliceImage != null)
            _sliceImage.color = (so != null) ? so.colorPowerUp : Color.white;

        // Icono
        if (_iconImage != null && so != null)
        {
            _iconImage.sprite = so.spriteIconPowerUp;
            _iconImage.enabled = _iconImage.sprite != null;
        }

        _haveInformation = true;
    }
}
