using UnityEngine;

// <summary>
// Este script se encarga de configurar la rotación de la pantalla en función de los valores asignados a las variables correspondientes.
// Permite definir si la pantalla debe rotarse automáticamente a ciertas orientaciones (paisaje izquierdo, paisaje derecho, o retrato invertido).
// Además, permite establecer la orientación predeterminada o una personalizada, especificada mediante el valor del enumerado ScreenOrientation.
// El comportamiento de la rotación se puede configurar al inicio o modificar en tiempo de ejecución mediante los métodos proporcionados.
// </summary>

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
