using System;
using UnityEngine;

public class NavbarButtons : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button _mainButton;
    [SerializeField] private UnityEngine.UI.Button _shopButton;
    [SerializeField] private UnityEngine.UI.Button _trainButton;
    [SerializeField] private UnityEngine.UI.Button _achievementButton;
    [SerializeField] private UnityEngine.UI.Button _rankingButton;

    public enum Buttons
    {
        main,
        shop,
        train,
        achievement,
        ranking
    }
    private void OnEnable()
    {
        UIEvents.StartFooterButtonAnimation += UIEvents_StartFooterButtonAnimation;
        UIEvents.EndFooterButtonAnimation += UIEvents_EndFooterButtonAnimation;
    }

    private void OnDisable()
    {
        UIEvents.StartFooterButtonAnimation -= UIEvents_StartFooterButtonAnimation;
        UIEvents.EndFooterButtonAnimation -= UIEvents_EndFooterButtonAnimation;
    }

    private void UIEvents_EndFooterButtonAnimation()
    {
        _mainButton.interactable = true;
        _shopButton.interactable = true;
        _trainButton.interactable = true;
        _achievementButton.interactable = true;
        _rankingButton.interactable = true;
    }

    private void UIEvents_StartFooterButtonAnimation()
    {
        _mainButton.interactable = false;
        _shopButton.interactable = false;
        _trainButton.interactable = false;
        _achievementButton.interactable = false;
        _rankingButton.interactable = false;
    }
}