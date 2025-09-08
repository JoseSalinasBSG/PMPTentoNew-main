using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sectionTitle;
    [SerializeField] private Transform _container;
    [SerializeField] private Image _storeIcon;

    public Transform Container
    {
        get => _container;
        set => _container = value;
    }
    public void SetData(string sectionTitle, Sprite storeIcon, Color iconColor)
    {
        _sectionTitle.text = sectionTitle;
        _storeIcon.sprite = storeIcon;
        _storeIcon.color = iconColor;
    }
}
