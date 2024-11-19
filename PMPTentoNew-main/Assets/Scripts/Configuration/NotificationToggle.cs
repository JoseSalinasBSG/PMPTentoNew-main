using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationToggle : MonoBehaviour
{
    [SerializeField] private Toggle _toggleNotification;
    [SerializeField] private Image _imageToYes;
    [SerializeField] private Image _imageToNo;
    private bool _activeNotification;

    public Toggle ToggleNotification
    {
        get => _toggleNotification;
        set => _toggleNotification = value;
    }

    public bool ActiveNotification
    {
        get => _activeNotification;
        set => _activeNotification = value;
    }
    private void Start()
    {
        // _activeNotification = false;
        // _toggleNotification.isOn = false;
        // Call(true);
    }

    private void OnEnable()
    {
        _toggleNotification.onValueChanged.AddListener(Call);
    }

    public void InitToggle()
    {
        _toggleNotification.isOn = _activeNotification;
        if (_activeNotification)
        {
            _imageToYes.gameObject.SetActive(true);
            _imageToNo.gameObject.SetActive(false);
        }
        else
        {
            _imageToYes.gameObject.SetActive(false);
            _imageToNo.gameObject.SetActive(true);
        }
    }

    private void Call(bool arg0)
    {
        // Debug.Log("asda");
        _activeNotification = !_activeNotification;
        if (_activeNotification)
        {
            _imageToYes.gameObject.SetActive(true);
            _imageToNo.gameObject.SetActive(false);
        }
        else
        {
            _imageToYes.gameObject.SetActive(false);
            _imageToNo.gameObject.SetActive(true);
        }
    }
}
