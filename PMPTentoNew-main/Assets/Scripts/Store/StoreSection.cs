using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreSection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sectionTitle;
    [SerializeField] private Transform _container;

    public Transform Container
    {
        get => _container;
        set => _container = value;
    }
    public void SetData(string sectionTitle)
    {
        _sectionTitle.text = sectionTitle;
    }
}
