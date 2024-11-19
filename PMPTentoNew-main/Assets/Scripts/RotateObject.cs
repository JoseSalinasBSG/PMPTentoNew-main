using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _velocity;
    private void Update()
    {
        transform.Rotate(new Vector3(0,0,-1), Time.deltaTime * _velocity);
    }
}
