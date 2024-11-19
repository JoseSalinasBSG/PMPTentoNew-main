using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRotation : MonoBehaviour
{
    [SerializeField] private bool _autorotateToLandscapeLeft;
    [SerializeField] private bool _autorotateToLandscapeRight;
    [SerializeField] private bool _autorotateToPortraitUpsideDown;
    [SerializeField] private ScreenOrientation _orientation = ScreenOrientation.Portrait;
    [SerializeField] private bool _initInStart;
    private void Start()
    {
        if (_initInStart)
        {
            SetConfiguration();
        }
        else
        {
            SetDefaultConfiguration();
        }
    }

    public void SetConfiguration()
    {
        Screen.autorotateToLandscapeLeft = _autorotateToLandscapeLeft;
        Screen.autorotateToLandscapeRight = _autorotateToLandscapeRight;

        Screen.autorotateToPortraitUpsideDown = _autorotateToPortraitUpsideDown;

        Debug.Log(_orientation);
        Screen.orientation = _orientation;
    }

    public void SetDefaultConfiguration()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Screen.autorotateToPortraitUpsideDown = true;

        Screen.orientation = ScreenOrientation.AutoRotation;
    }
    public void SetConfigurationOnlyPortrait()
    {
        Screen.autorotateToLandscapeLeft = _autorotateToLandscapeLeft;
        Screen.autorotateToLandscapeRight = _autorotateToLandscapeRight;

        Screen.autorotateToPortraitUpsideDown = _autorotateToPortraitUpsideDown;

        Screen.orientation = ScreenOrientation.Portrait;
    }
}
