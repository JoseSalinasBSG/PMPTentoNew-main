using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{
    [SerializeField] private float _duration = 2;
    [SerializeField] private float _delayIn = 0;
    [SerializeField] private float _delayOut = 0;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private UnityEvent _onBeforeTransition;
    [SerializeField] private UnityEvent _onEndTransitionFadeout;
    [SerializeField] private UnityEvent _onEndTransitionFadein;
    [SerializeField] private bool _initInStart;
    [SerializeField] private bool _hasSection;
    [SerializeField] private GameObject iconSection;
    [SerializeField] private Sprite iconSectionSelected;
    [SerializeField] private Sprite iconSectionUnselected;


    private bool _inTransition;

    public bool InTransition
    {
        get => _inTransition;
        set => _inTransition = value;
    }

    private void OnEnable()
    {
        if (_initInStart)
        {
            FadeInTransition();
        }
        if (_hasSection)
        {
            iconSection.GetComponent<Image>().sprite = iconSectionSelected;
        }
    }

    private void OnDisable()
    {
        if (_hasSection)
        {
            iconSection.GetComponent<Image>().sprite = iconSectionUnselected;
        }
    }

    public void FadeInTransition()
    {
        _inTransition = true;
        _onBeforeTransition?.Invoke();
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void FadeOutTransition()
    {
        _inTransition = true;
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        var initTime = 0f;

        while (initTime <= _delayIn)
        {
            initTime += Time.deltaTime;
            yield return null;

        }
        initTime = 0f;
        _canvasGroup.blocksRaycasts = false;
        while (initTime <= 1)
        {
            initTime += Time.deltaTime / _duration;
            _canvasGroup.alpha = initTime;
            yield return null;
        }
        _canvasGroup.blocksRaycasts = true;
        _inTransition = false;
        _onEndTransitionFadein.Invoke();

    }

    IEnumerator FadeOut()
    {
        var initTime = 0f;

        while (initTime <= _delayOut)
        {
            initTime += Time.deltaTime;
            yield return null;

        }
        initTime = 0f;
        _canvasGroup.blocksRaycasts = false;
        while (initTime <= 1)
        {
            initTime += Time.deltaTime / _duration;
            _canvasGroup.alpha = 1 - initTime;
            yield return null;
        }
        _canvasGroup.blocksRaycasts = true;
        _inTransition = false;
        _onEndTransitionFadeout.Invoke();

    }
}
