using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OptionAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationPositionCurve;
    [SerializeField] private AnimationCurve _animationAphaCurve;
    [SerializeField] private float _positionAnimationTime;
    [SerializeField] private float _alphaAnimationTime;
    [SerializeField] private CanvasGroup _options;
    [SerializeField] private float _positionDelay;
    [SerializeField] private float _alphaDelay;

    private Vector3 _initPosition;
    private Vector3 _lastPosition;
    
    private void Start()
    {
        _initPosition = transform.localPosition + new Vector3(-200, -200, 0);
        _lastPosition = transform.localPosition;

    }

    public void StartAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(IStartAnimationAlpha());
        StartCoroutine(IStartAnimationPosition());
    }

    public void CleanOption()
    {
        _options.alpha = 0;
        _options.transform.localPosition = _initPosition; 

    }

    IEnumerator IStartAnimationPosition()
    {
        // _options.transform.localPosition = _initPosition;
        yield return new WaitForSeconds(_positionDelay);
        float currentTime = 0;

        // Debug.Log(initPosition);
        while (currentTime <=1)
        {
            currentTime += Time.deltaTime / _positionAnimationTime;
            _options.transform.localPosition = Vector3.Lerp(_initPosition, _lastPosition, _animationPositionCurve.Evaluate(currentTime)); 
            yield return null;
        }
    }
    IEnumerator IStartAnimationAlpha()
    {
        _options.alpha = 0;
        yield return new WaitForSeconds(_alphaDelay);
        float currentTime = 0;
        while (currentTime <=1)
        {
            currentTime += Time.deltaTime / _alphaAnimationTime;
            _options.alpha = _animationAphaCurve.Evaluate(currentTime);
            yield return null;
        }
    }
}
