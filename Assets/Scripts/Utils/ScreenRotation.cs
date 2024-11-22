using UnityEngine;

//<summary>
//Este script se encarga de configurar la rotación de la pantalla
//</summary>

public class ScreenRotation : MonoBehaviour
{
    [SerializeField] private bool _autorotateToLandscapeLeft;
    [SerializeField] private bool _autorotateToLandscapeRight;
    [SerializeField] private bool _autorotateToPortraitUpsideDown;
    [SerializeField] private bool _initInStart;
    [SerializeField] private ScreenOrientation _orientation = ScreenOrientation.Portrait;
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
        Screen.orientation = _orientation;
        //Debug.Log(_orientation);
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
