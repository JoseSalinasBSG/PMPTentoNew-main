using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotificationWithDelay : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _timeActive;
    [SerializeField] private float _timeFadeIn;
    [SerializeField] private float _timeFadeOut;

    public void StartAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(IStartAnimation());
    }

    public void StopAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(IFadeOut());
    }
    IEnumerator IStartAnimation()
    {
        var currentTime = 0f;
        do
        {
            currentTime += Time.deltaTime / _timeFadeIn;
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1, currentTime);
            yield return null;
        } while (currentTime <= 1);

        currentTime = 0;
        do
        {
            currentTime += Time.deltaTime / _timeActive;
            yield return null;
        } while (currentTime <= 1);

        currentTime = 0;
        do
        {
            currentTime += Time.deltaTime / _timeFadeOut;
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 0, currentTime);
            yield return null;
        } while (currentTime <= 1);
    }

    IEnumerator IFadeOut()
    {
        var currentTime = 0f;
        do
        {
            currentTime += Time.deltaTime / _timeFadeOut;
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 0, currentTime);
            yield return null;
        } while (currentTime <= 1);
    }
}
