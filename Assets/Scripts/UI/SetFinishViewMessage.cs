using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SetFinishViewMessage : MonoBehaviour
{
    [SerializeField] private TextMessageSO _textMessage;
    [SerializeField] private TextMeshProUGUI _messageLabel;
    [SerializeField] private bool _won;

    private void OnEnable()
    {
        GameEvents.GameWon += GameEvents_GameWon;
        GameEvents.GameLost += GameEvents_GameLost;
    }

    private void GameEvents_GameLost()
    {
        _messageLabel.text = _textMessage.lostMessage[Random.Range(0, _textMessage.lostMessage.Length)];
    }

    private void GameEvents_GameWon()
    {
        _messageLabel.text = _textMessage.wonMessage[Random.Range(0, _textMessage.wonMessage.Length)];
    }
}
