using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace Configuration
{
    /// <summary>
    /// Muestra saludo con el username si ya existe en el SO; de lo contrario,
    /// habilita el panel para establecerlo y lo sincroniza con el backend.
    /// </summary>
    public class UsernameSelector : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField _inputUsername;
        [SerializeField] private GameObject setUsernamePanel;
        [SerializeField] private GameObject userGreetingPanel;
        [SerializeField] private TMP_Text userNameText;
        [SerializeField] private EventTrigger _buttonEventTriggerChangeUsername;

        [Header("Datos / Dependencias")]
        [SerializeField] private ScriptableObjectUser _objectUser;

        [Header("Red")]
        [SerializeField] private string url = "https://simuladorpmp-api.bsginstitute.com/api/ConfiguracionSimulador/ActualizarCaracteristicasGamificacion";

        [Header("Eventos")]
        [SerializeField] private UnityEvent OnUsernameSetted;

        // Evita envíos múltiples
        private bool _isSubmitting;

        private void Start()
        {
            RefreshPanelsBasedOnSO();
        }

        /// Refresca visibilidad de paneles según SO.
        private void RefreshPanelsBasedOnSO()
        {
            var hasUserInfo = _objectUser != null && _objectUser.userInfo != null;
            var detail = hasUserInfo ? _objectUser.userInfo.user?.detail : null;

            string username = detail?.usernameG;
            bool hasUsernameFlag = hasUserInfo && _objectUser.userInfo.haveUsername;
            bool hasUsername = hasUsernameFlag || !string.IsNullOrWhiteSpace(username);

            if (hasUsername)
            {
                SetPanelsVisible(showGreeting: true);
                userNameText.text = string.IsNullOrWhiteSpace(username) ? "Usuario" : username;
            }
            else
            {
                SetPanelsVisible(showGreeting: false);
                // Opcional: precargar placeholder con sugerencia
                if (_inputUsername != null) _inputUsername.text = string.Empty;
            }
        }

        /// Activa solo el panel correspondiente.
        private void SetPanelsVisible(bool showGreeting)
        {
            if (setUsernamePanel != null) setUsernamePanel.SetActive(!showGreeting);
            if (userGreetingPanel != null) userGreetingPanel.SetActive(showGreeting);
        }

        public void InvokeUsernameSetted()
        {
            OnUsernameSetted?.Invoke();
        }

        /// Llamado por el botón para enviar el nuevo username.
        public void SetUsername()
        {
            if (_isSubmitting) return; // doble clic protegido
            if (_inputUsername == null || string.IsNullOrWhiteSpace(_inputUsername.text))
            {
                Debug.LogWarning("No puedes enviar un username vacío.");
                return;
            }

            _isSubmitting = true;
            if (_buttonEventTriggerChangeUsername != null)
                _buttonEventTriggerChangeUsername.enabled = false;

            StartCoroutine(GetGamificationData(_inputUsername.text.Trim()));
        }

        /// <summary>
        /// Envía al backend el username. Si responde OK (bool true), actualiza SO,
        /// disemina eventos y refresca paneles. Rehabilita UI según resultado.
        /// </summary>
        public IEnumerator GetGamificationData(string username)
        {
            if (_objectUser == null || _objectUser.userInfo == null || _objectUser.userInfo.user == null)
            {
                Debug.LogError("User SO no asignado o incompleto. No se puede actualizar el username.");
                FinishSubmitWithUIReset();
                yield break;
            }

            var detail = _objectUser.userInfo.user.detail;
            if (detail == null)
            {
                Debug.LogError("User detail no asignado en el SO.");
                FinishSubmitWithUIReset();
                yield break;
            }

            // Guardar valor previo para revertir en caso de error
            string prevUsername = detail.usernameG;

            using (var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                // Mutamos el SO temporalmente, pero revertimos si falla
                detail.usernameG = username;
                detail.idAlumno = _objectUser.userInfo.user.idAlumno;

                var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(detail));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json");
                request.SetRequestHeader("User-Agent",
                    "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

                yield return request.SendWebRequest();

                long code = request.responseCode;
                Debug.Log($"Username update → HTTP {code}");

#if UNITY_2020_2_OR_NEWER
                bool okTransport = request.result == UnityWebRequest.Result.Success;
#else
                bool okTransport = !request.isNetworkError && !request.isHttpError;
#endif

                if (!okTransport || code >= 400)
                {
                    // Falló: revertimos el SO y rehabilitamos UI
                    detail.usernameG = prevUsername;
                    LogWebError(request);
                    FinishSubmitWithUIReset();
                    yield break;
                }

                // Parseo robusto del body a bool (acepta true/false con o sin comillas)
                bool success = ParseBoolSafely(request.downloadHandler.text);

                if (success)
                {
                    // Éxito: marcamos banderas y UI
                    _objectUser.userInfo.haveUsername = true; // si existe este flag
                    // _objectUser.userInfo.haveUser NO cambiar
                    TryRaiseUsernameEvents(username);
                    RefreshPanelsBasedOnSO();
                    FinishSubmit(); // deja el botón habilitado por si quieren cambiar nuevamente
                }
                else
                {
                    // El backend respondió false: revertimos username y reactivamos UI
                    detail.usernameG = prevUsername;
                    Debug.LogWarning($"El servidor no aceptó el username: {username}");
                    FinishSubmitWithUIReset();
                }
            }
        }

        /// Convierte "true"/"false", con o sin comillas, case-insensitive. Si no entiende, intenta parsear JSON simple.
        private bool ParseBoolSafely(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return false;
            var s = raw.Trim().Trim('"').Trim(); // maneja "true" o true
            if (bool.TryParse(s, out bool result)) return result;
            // fallback extremadamente conservador
            return string.Equals(s, "1") || string.Equals(s, "ok", StringComparison.OrdinalIgnoreCase);
        }

        private void TryRaiseUsernameEvents(string username)
        {
            // Evento Unity
            OnUsernameSetted?.Invoke();

            // Evento global legado (si existe)
            try
            {
                GameEvents.NewUsername?.Invoke(username);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"No se pudo invocar GameEvents.NewUsername: {e.Message}");
            }
        }

        private void FinishSubmit()
        {
            _isSubmitting = false;
            if (_buttonEventTriggerChangeUsername != null)
                _buttonEventTriggerChangeUsername.enabled = true;
        }

        private void FinishSubmitWithUIReset()
        {
            _isSubmitting = false;
            if (_buttonEventTriggerChangeUsername != null)
                _buttonEventTriggerChangeUsername.enabled = true;

            // Mantén visible el panel de set username para reintentar
            SetPanelsVisible(showGreeting: false);
        }

        private void LogWebError(UnityWebRequest req)
        {
#if UNITY_2020_2_OR_NEWER
            Debug.LogError($"Request error: {req.result} | code: {req.responseCode} | {req.error} | body: {req.downloadHandler.text}");
#else
            Debug.LogError($"Request error: net={req.isNetworkError} http={req.isHttpError} | code: {req.responseCode} | {req.error} | body: {req.downloadHandler.text}");
#endif
        }
    }
}
