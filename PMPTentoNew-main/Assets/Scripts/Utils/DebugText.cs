using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;

    [Range(0, 1)] [SerializeField] private float _value;

    public float heicht;
    private void Update()
    {
        var s = _scrollRect.normalizedPosition;
        s.y = _value;
        _scrollRect.normalizedPosition = s;
    }

    public void ShowText(Vector2 value)
    {
        heicht = value.y;
    }
}
