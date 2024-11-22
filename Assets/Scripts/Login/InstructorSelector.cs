using System;
using System.Collections;
using System.Text;
using Button;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

//<summary>
//clase que se encarga de manejar la seleccion y asignacion del instructor al usuario
//</summary>

public class InstructorSelector : MonoBehaviour
{
    [SerializeField] private ScriptableObjectInstructor _objectInstructor;
    [SerializeField] private ScriptableObjectUser _objectUser;
    //[SerializeField] private PathToInstantiateInstructor _pathToInstantiateInstructor;
    [SerializeField] private ButtonAnimation _buttonNext;
    [SerializeField] private ButtonAnimation _buttonPrevious;
    //[SerializeField] private Transform _container;
    //[SerializeField] private GameObject _instructorPlatform;
    [SerializeField] private string url = "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ActualizarCaracteristicasGamificacion";
    [SerializeField] private UnityEvent _onSelectInstructor;
    [SerializeField] private UnityEvent _onStart;
    [SerializeField] private UnityEvent OnPreviousButtonSelected;
    [SerializeField] private UnityEvent OnNextButtonSelected;

    private int index = 0;
    //private List<GameObject> _instructors = new List<GameObject>();

    //<summary>
    //Ya no se esta instanciando el instructor ahora solo se maneja la activacion y desactivacion del elemento UI para visualizarlo
    //</summary>

    // public void ClearListInsructor()
    // {
    //     Debug.Log("limpiando ");

    //     for (int j = 0; j < _instructors.Count; j++)
    //     {
    //         Destroy(_instructors[j]);
    //     }
    //     _instructors.Clear();
    // }

    // public void InstantiateInstructor()
    // {
    //     _pathToInstantiateInstructor.transform.rotation = Quaternion.Euler(0, 0, 0);
    //     for (int i = 0; i < _objectInstructor.instructors.Length; i++)
    //     {
    //         (Vector3, Vector3) posRot = _pathToInstantiateInstructor.GetPositionToInstantiate(i);
    //         var instant = Instantiate(_objectInstructor.instructors[i].prefab, posRot.Item1, quaternion.identity, _container);
    //         instant.layer = 3;
    //         foreach (Renderer child in instant.GetComponentsInChildren<Renderer>())
    //         {
    //             child.gameObject.layer = 3;
    //         }
    //         // for (int j = 0; j < instant.transform.childCount; j++)
    //         // {
    //         //     instant.transform.GetChild(j).gameObject.layer = 6;
    //         // }
    //         instant.transform.forward = posRot.Item2;
    //         Instantiate(_instructorPlatform, posRot.Item1, Quaternion.identity).transform.parent = instant.transform;

    //         _instructors.Add(instant);
    //     }
    // }

    private void Start()
    {
        _onStart?.Invoke();
    }

    public void InitValues()
    {
        index = 0;
        //InstantiateInstructor();
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
        //_pathToInstantiateInstructor.RotateToItem(index); //Ya no se esta instanciando el instructor
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
        //_pathToInstantiateInstructor.RotateToItem(index);
        OnPreviousButtonSelected?.Invoke();
    }
    public void SelectInstructor()
    {
        _buttonNext.DisableButton();
        _buttonPrevious.DisableButton();
        GameEvents.RequesNewUsername?.Invoke();
        StartCoroutine(GetGamificationData(index));
        // GameEvents.NewInstuctorId?.Invoke(index);
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
                    Debug.Log(request.downloadHandler.text);
                    _objectUser.userInfo.haveUser = false;
                    GameEvents.WrongWhenNewUsername?.Invoke();
                }
            }
        }
    }
}
