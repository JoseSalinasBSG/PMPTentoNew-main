using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerToTarget : MonoBehaviour
{
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private RectTransform _pointerContainer;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private void OnEnable()
    {
        GameEvents.ChosenPlatform += GameEvents_ChosenPlatform;
    }

    private void GameEvents_ChosenPlatform(Vector3 obj)
    {
        SetPosition(obj);
        _pointerContainer.parent.gameObject.SetActive(true);
        _pointerContainer.parent.GetComponent<FadeUI>().FadeInTransition();
    }

    private void OnDisable()
    {
        GameEvents.ChosenPlatform -= GameEvents_ChosenPlatform;

    }

    public void SetPosition(Vector3 tTarget)
    {
        var pos = tTarget;
        pos.y = tTarget.y;
        pos.x = _pointerContainer.position.x;
        _pointerContainer.transform.position = pos;
        pos = tTarget;
        pos.x = tTarget.x;
        pos.y = _pointer.position.y;
        _pointer.transform.position = pos;
    }
}
