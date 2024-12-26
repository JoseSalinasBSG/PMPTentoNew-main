using System;
using System.Collections;
using System.Text;
using Button;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

///<summary>
/// Clase encargada de gestionar la selección de instructores en el sistema, incluyendo la navegación entre instructores,
/// activación/desactivación de botones, y la asignación del instructor seleccionado al usuario. 
/// Además, realiza la actualización de datos relacionados mediante una solicitud a una API.
///</summary>

public class InstructorSelector : MonoBehaviour
{
    [SerializeField] private ScriptableObjectInstructor _objectInstructor;
    [SerializeField] private ScriptableObjectUser _objectUser;
    [SerializeField] private ButtonAnimation _buttonNext;
    [SerializeField] private ButtonAnimation _buttonPrevious;
    [SerializeField] private string url = "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ActualizarCaracteristicasGamificacion";
    [SerializeField] private UnityEvent _onSelectInstructor;
    [SerializeField] private UnityEvent _onStart;
    [SerializeField] private UnityEvent OnPreviousButtonSelected;
    [SerializeField] private UnityEvent OnNextButtonSelected;

    private int index = 0;
    private void Start()
    {
        _onStart?.Invoke();
    }

    public void InitValues()
    {
        index = 0;
        ComprobeNext();
        ComprobePrevious();
    }

    private void OnEnable()
    {
        InitValues();
    }

    public void GetNextInstructor()
    {
        index++;
        ComprobeNext();
        ComprobePrevious();
        OnNextButtonSelected?.Invoke();
    }

    public void ComprobeNext()//comprobar boton de siguiente
    {
        if ((index + 1) >= _objectInstructor.instructors.Length)
        {
            _buttonNext.gameObject.SetActive(false);//desactivar boton siguiente
            OnNextButtonSelected?.Invoke();
        }
        else
        {
            _buttonNext.gameObject.SetActive(true);//activar boton siguiente
            OnPreviousButtonSelected?.Invoke();
        }
    }
    public void ComprobePrevious()//comprobar boton de anterior
    {
        if ((index - 1) < 0)
        {
            _buttonPrevious.gameObject.SetActive(false);
            OnPreviousButtonSelected?.Invoke();
        }
        else
        {
            _buttonPrevious.gameObject.SetActive(true);
            OnNextButtonSelected?.Invoke();
        }
    }
    public void GetPreviousInstructor()
    {
        index--;
        ComprobeNext();
        ComprobePrevious();
        OnPreviousButtonSelected?.Invoke();
    }
    public void SelectInstructor()
    {
        _buttonNext.DisableButton();
        _buttonPrevious.DisableButton();
        GameEvents.RequesNewUsername?.Invoke();
        StartCoroutine(GetGamificationData(index));
    }

    public IEnumerator GetGamificationData(int instructorId)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            _objectUser.userInfo.user.detail.instructorID = instructorId;
            UserDetail dataLogin = _objectUser.userInfo.user.detail;

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

            yield return request.SendWebRequest();
            if (request.responseCode >= 400)
            {
                _objectUser.userInfo.haveUser = false;
                GameEvents.WrongWhenNewUsername?.Invoke();
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    bool detail = Convert.ToBoolean(request.downloadHandler.text);
                    if (detail)
                    {
                        _onSelectInstructor?.Invoke();
                        GameEvents.NewInstuctorId?.Invoke(index);
                    }
                    else
                    {
                        GameEvents.WrongWhenNewUsername?.Invoke();
                    }

                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text +" - Error: "+e.Message);
                    _objectUser.userInfo.haveUser = false;
                    GameEvents.WrongWhenNewUsername?.Invoke();
                }
            }
        }
    }
}
