using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlosaryChallengeProgress : MonoBehaviour
{
    [SerializeField] private Image _imageProgress;
    [SerializeField] private TextMeshProUGUI _firstThird;
    [SerializeField] private TextMeshProUGUI _secondThird;
    [SerializeField] private TextMeshProUGUI _thirdThird;

    private void Start()
    {
        _imageProgress.fillAmount = 0;
    }

    public void SetData(int firstThird, int secondThird, int thirdThird)
    {
        _firstThird.text = firstThird.ToString();
        _secondThird.text = secondThird.ToString();
        _thirdThird.text = thirdThird.ToString();
    }

    public void UpdateProgress(float progressPercentage)
    {
        _imageProgress.fillAmount = progressPercentage;
    }
}
