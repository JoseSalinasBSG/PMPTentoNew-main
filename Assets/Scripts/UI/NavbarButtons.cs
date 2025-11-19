using System;
using UnityEngine;

public class NavbarButtons : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button _mainButton;
    [SerializeField] private UnityEngine.UI.Button _shopButton;
    [SerializeField] private UnityEngine.UI.Button _trainButton;
    [SerializeField] private UnityEngine.UI.Button _achievementButton;
    [SerializeField] private UnityEngine.UI.Button _rankingButton;

    [SerializeField] private GameObject _mainButtonActive;
    [SerializeField] private GameObject _shopButtonActive;
    [SerializeField] private GameObject _trainButtonActive;
    [SerializeField] private GameObject _achievementButtonActive;
    [SerializeField] private GameObject _rankingButtonActive;

    private void Start()
    {
        _mainButtonActive.SetActive(true);
        _shopButtonActive.SetActive(false);
        _trainButtonActive.SetActive(false);
        _achievementButtonActive.SetActive(false);
        _rankingButtonActive.SetActive(false);
    }



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

        _mainButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            _mainButtonActive.SetActive(true);
            _shopButtonActive.SetActive(false);
            _trainButtonActive.SetActive(false);
            _achievementButtonActive.SetActive(false);
            _rankingButtonActive.SetActive(false);
        });

        _shopButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            _mainButtonActive.SetActive(false);
            _shopButtonActive.SetActive(true);
            _trainButtonActive.SetActive(false);
            _achievementButtonActive.SetActive(false);
            _rankingButtonActive.SetActive(false);
        });

        _trainButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            _mainButtonActive.SetActive(false);
            _shopButtonActive.SetActive(false);
            _trainButtonActive.SetActive(true);
            _achievementButtonActive.SetActive(false);
            _rankingButtonActive.SetActive(false);
        });

        _achievementButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            _mainButtonActive.SetActive(false);
            _shopButtonActive.SetActive(false);
            _trainButtonActive.SetActive(false);
            _achievementButtonActive.SetActive(true);
            _rankingButtonActive.SetActive(false);
        });

        _rankingButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            _mainButtonActive.SetActive(false);
            _shopButtonActive.SetActive(false);
            _trainButtonActive.SetActive(false);
            _achievementButtonActive.SetActive(false);
            _rankingButtonActive.SetActive(true);
        });
    }

    private void OnDisable()
    {
        UIEvents.StartFooterButtonAnimation -= UIEvents_StartFooterButtonAnimation;
        UIEvents.EndFooterButtonAnimation -= UIEvents_EndFooterButtonAnimation;

        _mainButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        _shopButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        _trainButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        _achievementButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        _rankingButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
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