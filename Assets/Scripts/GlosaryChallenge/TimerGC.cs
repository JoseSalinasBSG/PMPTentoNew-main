using System;
using TMPro;
using UnityEngine;

public class TimerGC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerField;
    [SerializeField] private float _maxTime;

    private float _initTimer;

        
    public void InitValue(float initTimer)
    {
        _initTimer = initTimer;
    }

    // private void Update()
    // {
    //     _timerField.text = $"{((int)(_currentTime / 60)).ToString("00")} : {((int)(_currentTime % 60)).ToString("00")}";
    //     _currentTime -= Time.deltaTime;
    // }

    public void SetValueTimer(float percentage)
    {
        var time = _initTimer * percentage;

        _timerField.text = $"{((int)(time / 60)).ToString("00")} : {((int)(time % 60)).ToString("00")}";
    }
}
