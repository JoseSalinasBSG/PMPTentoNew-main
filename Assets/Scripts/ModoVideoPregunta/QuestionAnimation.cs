using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationPositionCurve;
    [SerializeField] private AnimationCurve _animationAphaCurve;
    [SerializeField] private Vector3 _initOffset;
    private Vector3 _initPosition;
    [SerializeField] private Vector3 _lastPosition;
    [SerializeField] private Vector3 _initScale;
    [SerializeField] private Vector3 _lastScale;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _delay;
    [SerializeField] private float _positionAnimationTime;
    [SerializeField] private float _alphaAnimationTime;

    public void StartAnimation()
    {
        StopAllCoroutines();

    StartCoroutine(IStartAnimationAlpha());
        StartCoroutine(IStartAnimationPosition());
    }
    public void CleanOption()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.transform.localPosition = _initPosition; 

    }

    private void Start()
    {
        _initPosition = _initOffset + transform.localPosition;
        _lastPosition = transform.localPosition;
        _lastScale = Vector3.one;
    }

    IEnumerator IStartAnimationPosition()
    {
        // _options.transform.localPosition = _initPosition;
        yield return new WaitForSeconds(_delay);
        float currentTime = 0;

        // Debug.Log(initPosition);
        while (currentTime <=1)
        {
            currentTime += Time.deltaTime / _positionAnimationTime;
            transform.localPosition = Vector3.Lerp(_initPosition, _lastPosition, _animationPositionCurve.Evaluate(currentTime)); 
            transform.localScale = Vector3.Lerp(_initScale, _lastScale, _animationPositionCurve.Evaluate(currentTime)); 
            yield return null;
        }
    }
    IEnumerator IStartAnimationAlpha()
    {
        _canvasGroup.alpha = 0;
        float currentTime = 0;
        while (currentTime <=1)
        {
            currentTime += Time.deltaTime / _alphaAnimationTime;
            _canvasGroup.alpha = _animationAphaCurve.Evaluate(currentTime);
            yield return null;
        }
    }
    
}
