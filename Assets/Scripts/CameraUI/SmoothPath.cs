using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class SmoothPath : MonoBehaviour
{
    [Header("External")] [SerializeField] private FadeUI _fadeUI;
    [FormerlySerializedAs("_controlPoints")]
    [Header("Path parameters")]
    //public float _velocityDuration = 1f; // Time it takes to complete the path
    //[SerializeField] public ScriptableObjectPath _pathData;
    //[SerializeField] private Transform objectToMove;
    private float _beta1;
    private float _beta2;

    [Header("Events")]
    [SerializeField] private UnityEvent _onStartTransition;
    [SerializeField] private UnityEvent _onEndTransition;
    
    [SerializeField] 
    
    [Range(0,1)]
    private float _percentageOfCompletion;
    [SerializeField] private bool _completePercentageEvent;

    [SerializeField] private UnityEvent _onPercentage;
    private float t = 0f;
    private float tinit = 0f;
    private float tt = 0f;

    private bool endTransition;
    private bool currentTransition;
    private Quaternion initQuaternion;
    private void Update()
    {
        if (currentTransition)
        {
            MoveObjectAlongPath();
        }
    }
    
    [ContextMenu("Transition")]
    public void StartTransition()
    {
        _onStartTransition?.Invoke();
        StartCoroutine(StartAnimations());
    }

    IEnumerator StartAnimations()
    {
        endTransition = false;
        _fadeUI.FadeOutTransition();
        yield return null;
        //yield return new WaitUntil(() => !_fadeUI.InTransition);//
        //objectToMove.rotation = initQuaternion;
        currentTransition = true;
        tinit = 0;
    }
    private void Start()
    {
        //initQuaternion = _pathData.controlPoints[0].startRotation;
    }

    void MoveObjectAlongPath()
    {
        /*if (objectToMove == null || _pathData.controlPoints.Length == 0)
        {
            // Debug.LogError("Please assign the objectToMove and exactly 4 control points in the Inspector.");
            return;
        }*/

       
        // Debug.Log(i);
        /*if (!_completePercentageEvent && t/ _pathData.controlPoints.Length >= _percentageOfCompletion )
        {
            _completePercentageEvent = true;
            _onPercentage?.Invoke();
        }*/

       
       
        if (!endTransition)
        {
            endTransition = true;
            //Debug.Log("Finalizo transicion");
            _onEndTransition.Invoke();
            UIEvents.EndFooterButtonAnimation.Invoke();
            currentTransition = false;
        }
        
    }
    /*
    public void ConfigurateStartTangents(int index)
    {
        _pathData.controlPoints[index].startTangent = 2 * _pathData.controlPoints[index-1].endPoint - _pathData.controlPoints[index-1].endTangent;
        _pathData.controlPoints[index].endTangent = _pathData.controlPoints[index-1].startPoint + 4 * ( _pathData.controlPoints[index-1].endPoint - _pathData.controlPoints[index-1].endTangent);
    }
    public void ConfigurateGeometricContinues(int index)
    {
        SetStarPointPosition(index);
        _pathData.controlPoints[index].startTangent = _pathData.controlPoints[index-1].endPoint + (_pathData.controlPoints[index-1].endPoint - _pathData.controlPoints[index-1].endTangent) * _beta1;
        _pathData.controlPoints[index].endTangent = _pathData.controlPoints[index-1].endPoint + (_pathData.controlPoints[index-1].endPoint - _pathData.controlPoints[index-1].endTangent) * ( 2 * _beta1 - _beta1 * _beta1 + _beta2 / 2 ) + _beta1 * _beta1 * (_pathData.controlPoints[index-1].startTangent - _pathData.controlPoints[index-1].endTangent);
    }
    private void SetStarPointPosition(int index)
    {
        _pathData.controlPoints[index].startPoint = _pathData.controlPoints[index - 1].endPoint;
        
    }*/

    /*
    Vector3 CalculateBezierPoint(float t, Vector3 startPoint, Vector3 startRotation, Vector3 endRotation, Vector3 endPoint)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * startPoint;
        p += 3 * uu * t * startRotation;
        p += 3 * u * tt * endRotation;
        p += ttt * endPoint;

        return p;
    }*/

#if UNITY_EDITOR
    /*
    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _pathData.controlPoints.Length; i++)
        {
            

            // Handles.PositionHandle(_controlPoints[i].startPoint.position, Quaternion.identity);
            Handles.DrawBezier(_pathData.controlPoints[i].startPoint, _pathData.controlPoints[i].endPoint, _pathData.controlPoints[i].startTangent, _pathData.controlPoints[i].endTangent, _pathData.colorPath, null,10 );
            Gizmos.DrawLine(_pathData.controlPoints[i].startPoint, _pathData.controlPoints[i].startTangent);
            Gizmos.DrawLine(_pathData.controlPoints[i].endPoint, _pathData.controlPoints[i].endTangent);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_pathData.controlPoints[i].startPoint, .03f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_pathData.controlPoints[i].endPoint, .03f);
            if (i -1 > -1)
            {
                ConfigurateGeometricContinues(i);
                // Gizmos.color = Color.red;
            
            }
            // else
            // {
            Gizmos.color = Color.green;
            // }
            
            Gizmos.DrawSphere(_pathData.controlPoints[i].startTangent, .03f);
            Gizmos.DrawSphere(_pathData.controlPoints[i].endTangent, .03f);
        }
    }*/
#endif
    
}
