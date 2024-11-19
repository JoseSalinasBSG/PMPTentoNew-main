using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformDomainInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tittle;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private ChooseDomainInformation _chooseDomainInformation;

    public void SetTittle(string tittle)
    {
        _tittle.text = tittle;
    }

    public void SetDescription(string description)
    {
        _description.text = description;
    }

    public void SetDetail(string data, string title)
    {
        _chooseDomainInformation.SetData( data, title);
    }
}
