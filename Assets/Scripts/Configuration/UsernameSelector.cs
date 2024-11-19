using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace Configuration
{
    public class UsernameSelector : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputUsername;

        [SerializeField] private string url =
            "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ActualizarCaracteristicasGamificacion";

        [SerializeField] private ScriptableObjectUser _objectUser;
        [SerializeField] private EventTrigger _buttonEventTriggerChangeUsername;
        [SerializeField] private UnityEvent OnUsernameSetted;

        public void SetUsername()
        {
            _buttonEventTriggerChangeUsername.enabled = false;
            StartCoroutine(GetGamificationData(_inputUsername.text));
            OnUsernameSetted?.Invoke();
            // GameEvents.NewUsername?.Invoke(_inputUsername.text);
        }

        public IEnumerator GetGamificationData(string username)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                _objectUser.userInfo.user.detail.usernameG = username;
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
                    _buttonEventTriggerChangeUsername.enabled = true;
                    Debug.Log(request.error);
                }
                else
                {
                    try
                    {
                        bool detail = Convert.ToBoolean(request.downloadHandler.text);
                        if (detail)
                        {
                            GameEvents.NewUsername?.Invoke(_inputUsername.text);
                        }
                        else
                        {
                            _buttonEventTriggerChangeUsername.enabled = true;
                        }

                    }
                    catch (Exception e)
                    {
                        Debug.Log(request.downloadHandler.text);
                        _objectUser.userInfo.haveUser = false;
                        _buttonEventTriggerChangeUsername.enabled = true;
                    }
                }
            }
        }
    }
}