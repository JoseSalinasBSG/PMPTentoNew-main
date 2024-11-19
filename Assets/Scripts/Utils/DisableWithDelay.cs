using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWithDelay : MonoBehaviour
{
    [SerializeField] private float _delay;

    private void OnEnable()
    {
     Invoke	(nameof(Disable), _delay);   
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
