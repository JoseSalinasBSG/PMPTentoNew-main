using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerDS : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Color _initColor;
    [SerializeField] private Color _lastColor;

    private float _initTimer;
    public void InitValue(float initTimer)
    {
        _initTimer = initTimer;
    }
    public void SetValueTimer(float percentage)
    {
        _image.fillAmount = percentage;
        var time = _initTimer * percentage;

        _timerText.text = $"{((int)(time / 60)).ToString("00")} : {((int)(time % 60)).ToString("00")}";
        _image.color = Color.Lerp(_initColor, _lastColor, 1 - percentage);
    }
}
