using System;
using System.Collections;
using System.Collections.Generic;
using Question;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScrollViewItem : MonoBehaviour
{
    [SerializeField] private QuestionInformationVP _questionInformationVp;
    [SerializeField] private AnimationScrollItem _animationScrollItem;
    private RectTransform _ownRectTransform;

    public RectTransform OwnRectTransform
    {
        get
        {
            if (!_ownRectTransform)
            {
                _ownRectTransform = GetComponent<RectTransform>();
            }
            return _ownRectTransform;   
        }
        set => _ownRectTransform = value;
    }

    public string GetTextToTTS()
    {
        return _questionInformationVp.QuestiontextforAudio + ". " + _questionInformationVp.Opt1.Label.text + ". " +
               _questionInformationVp.Opt2.Label.text + ". " + _questionInformationVp.Opt3.Label.text + ". O " +
               _questionInformationVp.Opt4.Label.text;
    }

    public void SetDataInformation(QuestionDataVP questionDataVp)
    {
        _questionInformationVp.SetData(questionDataVp);
    }

    public void StartAnimation()
    {
        _animationScrollItem.StartAnimation();
    }
    public void CleanPanel()
    {
        _questionInformationVp.EnableOptions();
        _animationScrollItem.CleanPanel();
    }
}
