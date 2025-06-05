using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ChangeUsernameSender : SenderEvent
{
    private TMP_InputField _usernameField;
    [SerializeField] private string url =
        "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ActualizarCaracteristicasGamificacion";
    [SerializeField] private ScriptableObjectUser _objectUser;

    private void OnEnable()
    {
        if (!_usernameField)
        {
            _usernameField = GetComponent<TMP_InputField>();
        }
        
        _usernameField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnEndEdit(string username)
    {
        Debug.Log("termino de editar username");
        SendEvent();
    }

    protected override void SendEvent()
    {
        GameEvents.RequesNewUsername?.Invoke();
        StartCoroutine(GetGamificationData(_usernameField.text));
    }
    public IEnumerator GetGamificationData(string username)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                _objectUser.userInfo.user.detail.usernameG = username;
                _objectUser.userInfo.user.detail.idAlumno = _objectUser.userInfo.user.idAlumno;
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
                            GameEvents.NewUsername?.Invoke(_usernameField.text);
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
