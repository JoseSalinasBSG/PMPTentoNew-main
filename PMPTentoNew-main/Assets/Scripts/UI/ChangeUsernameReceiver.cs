using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeUsernameReceiver : MonoBehaviour
{
    private TextMeshProUGUI _usernameLabel;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        if (!_usernameLabel)
        {
            _usernameLabel = GetComponent<TextMeshProUGUI>();
        }
        GameEvents.UsernameChanged += GameEvents_NameChanged;
    }

    private void GameEvents_NameChanged(string obj)
    {
        _usernameLabel.text = obj;
    }
}
