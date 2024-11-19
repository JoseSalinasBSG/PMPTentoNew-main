using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] private UnityEngine.Camera _camera;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _firstPosition;
    [SerializeField] private Vector3 _lastPosition;
    [SerializeField] private float _timeTransition;
    
    private Vector3 _velocity = Vector3.zero;
    public float smoothTime = 0.3F;

    // private void Start()
    // {
    //     
    //     StartTransition();
    // }

    private void Update()
    {
    }

    public void PrintSomething()
    {
        Debug.Log("sadasd");
    }

    [ContextMenu("Transition")]
    public void StartTransition()
    {
        StopAllCoroutines();
        StartCoroutine(ITransition());
    }

    IEnumerator ITransition()
    {
        var currentTime = 0f;
        while (currentTime < _timeTransition)
        {
            Debug.Log(currentTime);
            currentTime += Time.deltaTime;
            _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, _lastPosition,ref _velocity, smoothTime * Time.deltaTime);
            yield return null;

        }
        yield return null;
    }
}
