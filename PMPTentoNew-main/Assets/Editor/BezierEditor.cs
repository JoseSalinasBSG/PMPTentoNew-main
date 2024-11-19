using System;
using System.Collections.Generic;
using CameraUI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve)), CanEditMultipleObjects]
public class BezierEditor : Editor
{
    private List<NodePath> positions;
    private Vector3 newPosition;

    private void Awake()
    {
        BezierCurve moveableSphere = (BezierCurve)target;
        positions = new List<NodePath>();
        for (int i = 0; i < moveableSphere._path.ControlPoints.Length; i++)
        {
            var node = new NodePath();
            node.startPoint = moveableSphere._path.ControlPoints[i].startPoint;
            node.startTangent = moveableSphere._path.ControlPoints[i].startTangent;
            node.endPoint = moveableSphere._path.ControlPoints[i].endPoint;
            node.endTangent = moveableSphere._path.ControlPoints[i].endTangent;
            positions.Add(node);
        }
    }

    void OnSceneGUI()
    {
        BezierCurve moveableSphere = (BezierCurve)target;
        EditorGUI.BeginChangeCheck();

        // Mostrar una asa para mover la esfera
        if (moveableSphere._path.ControlPoints.Length != positions.Count)
        {
            positions.Clear();
            Debug.LogWarning("Actualizando marcadores");
            for (int i = 0; i < moveableSphere._path.ControlPoints.Length; i++)
            {
                var node = new NodePath();
                node.startPoint = moveableSphere._path.ControlPoints[i].startPoint;
                node.startTangent = moveableSphere._path.ControlPoints[i].startTangent;
                node.endPoint = moveableSphere._path.ControlPoints[i].endPoint;
                node.endTangent = moveableSphere._path.ControlPoints[i].endTangent;
                positions.Add(node);
            }
        }
        for (int i = 0; i < moveableSphere._path.ControlPoints.Length; i++)
        {
            if (i - 1 >-1 )
            {
                
                
            }
            else
            {
                positions[i].startPoint = Handles.PositionHandle(moveableSphere.transform.TransformPoint(moveableSphere._path.ControlPoints[i].startPoint), moveableSphere._path.ControlPoints[i].startRotation);
                // positions[i].startRotation = Handles.RotationHandle(moveableSphere._path.ControlPoints[i].startRotation, moveableSphere.transform.TransformPoint(positions[i].startPoint));

                // positions[i].startTangent = Handles.PositionHandle(moveableSphere._ControlPoints[i].startTangent, Quaternion.identity);
                // positions[i].endTangent = Handles.PositionHandle(moveableSphere._ControlPoints[i].endTangent, Quaternion.identity);
            }
            positions[i].startTangent = Handles.PositionHandle(moveableSphere.transform.TransformPoint(moveableSphere._path.ControlPoints[i].startTangent), Quaternion.identity);
            positions[i].endTangent = Handles.PositionHandle(moveableSphere.transform.TransformPoint(moveableSphere._path.ControlPoints[i].endTangent), Quaternion.identity);
            positions[i].endPoint = Handles.PositionHandle(moveableSphere.transform.TransformPoint(moveableSphere._path.ControlPoints[i].endPoint), moveableSphere._path.ControlPoints[i].endRotation);
            // positions[i].endRotation = Handles.RotationHandle(moveableSphere._path.ControlPoints[i].endRotation, moveableSphere.transform.TransformPoint(positions[i].endPoint));
        }
        if (EditorGUI.EndChangeCheck())
        {
            // Actualizar la posición de la esfera si se cambia
            // Undo.RecordObject(moveableSphere.transform, "Move Sphere");
            Undo.RecordObject(moveableSphere._path, "Move Sphere");
            Debug.Log(moveableSphere._path.name);
            AssetDatabase.Refresh();
            for (int i = 0; i < moveableSphere._path.ControlPoints.Length; i++)
            {
                moveableSphere._path.ControlPoints[i].startPoint = moveableSphere.transform.InverseTransformPoint(positions[i].startPoint);
                moveableSphere._path.ControlPoints[i].startTangent = moveableSphere.transform.InverseTransformPoint(positions[i].startTangent);
                moveableSphere._path.ControlPoints[i].endPoint = moveableSphere.transform.InverseTransformPoint(positions[i].endPoint);
                moveableSphere._path.ControlPoints[i].endTangent = moveableSphere.transform.InverseTransformPoint(positions[i].endTangent);
                moveableSphere._path.ControlPoints[i].endRotation = positions[i].endRotation;
                moveableSphere._path.ControlPoints[i].startRotation = positions[i].startRotation;
            }

            EditorUtility.SetDirty(moveableSphere._path);
        }
    }
}