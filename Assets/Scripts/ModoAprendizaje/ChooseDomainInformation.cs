using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseDomainInformation : MonoBehaviour
{
    [SerializeField] private bool _reciveEvents;
    [SerializeField] private UnityEvent<string> _onReciveEvent;
    [SerializeField] private UnityEvent<string> _onReciveTitleEvent;
    private int _id;
    private string _description;
    private string _title;
    public void Show()
    {
        UIEvents.ShowDomainInformation?.Invoke(_description);
        UIEvents.ShowDomainTitle?.Invoke(_title);
    }

    public void SetData( string description, string title)
    {
        _description = description;
        _title = title;
    }
    
    private void OnEnable()
    {
        if (_reciveEvents)
        {
            UIEvents.ShowDomainInformation += UIEvents_ShowDomainInformation;
            UIEvents.ShowDomainTitle += UIEvents_ShowDomainTitle;
        }
    }

    private void UIEvents_ShowDomainTitle(string obj)
    {
        _onReciveTitleEvent?.Invoke(obj);
    }

    private void OnDisable()
    {
        if (_reciveEvents)
        {
            UIEvents.ShowDomainInformation -= UIEvents_ShowDomainInformation;
            UIEvents.ShowDomainTitle += UIEvents_ShowDomainTitle;
        }
    }

    private void UIEvents_ShowDomainInformation(string data)
    {
        _onReciveEvent?.Invoke(data);
    }
}
