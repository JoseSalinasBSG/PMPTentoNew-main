using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathToInstantiateInstructor : MonoBehaviour
{
    [SerializeField] private float _radious;
    [SerializeField] private float _angle;
    [SerializeField] private int numInstants;
    [SerializeField] private float _movementDuration;

    private float _angleRad = 0;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, _radious);
        _angleRad =Mathf.Deg2Rad *_angle;

        for (int i = 0; i < numInstants; i++)
        {
            var pos = new Vector3(-_radious * Mathf.Sin(-_angleRad * i), 0, -_radious * Mathf.Cos(-_angleRad * i));
            Gizmos.DrawSphere(pos, .1f);
            Gizmos.DrawRay(pos, (pos - Vector3.zero).normalized);
        }
        
    }
#endif
    

    private void Start()
    {
        _angleRad =Mathf.Deg2Rad *_angle;
    }

    public (Vector3, Vector3) GetPositionToInstantiate(int index)
    {
        var positionTemp = new Vector3(-_radious * Mathf.Sin(-_angleRad * index), 0,
            -_radious * Mathf.Cos(-_angleRad * index));
        return (positionTemp,(positionTemp - Vector3.zero).normalized) ;
    }
    public void RotateToItem(int index)
    {
        StopAllCoroutines();
        StartCoroutine(RotateWithAngle(index));
    }

    IEnumerator RotateWithAngle(int index)
    {
        var currentTime = 0f;
        Quaternion targetRotation = Quaternion.Euler(0f,   _angleRad * index * Mathf.Rad2Deg, 0);
        Quaternion initRotation = transform.rotation;
        while (currentTime <= 1)
        {
            currentTime += Time.deltaTime / _movementDuration;
            transform.rotation = Quaternion.Slerp(initRotation, targetRotation, currentTime);
            yield return null;
        }
    }
}
