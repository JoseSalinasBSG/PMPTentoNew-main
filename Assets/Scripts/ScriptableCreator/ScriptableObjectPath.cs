using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class NodePath
{
    public Vector3 startPoint;
    public bool haveModification;
    public Vector3 startTangent;
    public Quaternion startRotation = Quaternion.identity;
    public Vector3 endPoint;
    public Quaternion endRotation = Quaternion.identity;
    public Vector3 endTangent;
    
}
[CreateAssetMenu(menuName = "Path", fileName = "PathData")]
public class ScriptableObjectPath : ScriptableObject
{
    // [SerializeReference]
    [FormerlySerializedAs("controlPoints")] [SerializeField]
    public NodePath[] controlPoints;

    public Color colorPath;
    public NodePath[] ControlPoints
    {
        get => controlPoints;
        set => controlPoints = value;
    }
}
