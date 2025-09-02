using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Login
{
    /// <summary>
    /// Controla Splash -> (Loading opcional) -> flujo de Login (Login/Username/Instructor).
    /// Mantiene la lógica legacy y añade una simulación de carga con slider/porcentaje.
    /// </summary>
    public sealed class LoginInterfaceController : MonoBehaviour
    {
        [Header("Pantallas principales")]
        [SerializeField] private Canvas _GUILogin;
        [SerializeField] private Canvas _GUIUsername;
        [SerializeField] private Canvas _GUIInstructor;
        [SerializeField] private Canvas _GUILoading;
        [SerializeField] private Canvas _SplashScreen;

        [Header("Datos / Dependencias")]
        [Tooltip("Scriptable con los datos del usuario (banderas haveUser/haveUsername/haveInstructor).")]
        [SerializeField] private ScriptableObjectUser _objectUser;
        [Tooltip("Scriptable con datos del instructor (si aplica).")]
        [SerializeField] private ScriptableObjectInstructor _objectInstructor;
        [Tooltip("Gestor que avisa cuando terminó de cargar datos/avatares.")]
        [SerializeField] private UserManager _userManager;

        [Header("Eventos")]
        [Tooltip("Se invoca cuando el flujo de Login ha finalizado (ya hay instructor).")]
        [SerializeField] private UnityEvent _onFinishLoginConfiguration;

        [Header("Pantalla de Carga")]
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private TMP_Text _loadingPercentTMP;

        [Header("Ajustes de Carga")]
        [Tooltip("Si está activo, se muestra una pantalla de carga simulada antes de comprobar el login.")]
        [SerializeField] private bool _showLoadingAtStart = true;
        [Tooltip("Duración mínima de la simulación (evita parpadeos si la carga real es instantánea).")]
        [SerializeField, Min(0f)] private float _minLoadingSeconds = 1.5f;
        [Tooltip("Si hay UserManager, el progreso se sincroniza: 0..90% mientras carga y 100% al completar banderas.")]
        [SerializeField] private bool _syncWithUserManagerFlags = true;

        // Estado interno para evitar dobles inicios y detener corrutinas específicas
        private bool _isStarting;
        private Coroutine _startLoadingRoutine;
        private Coroutine _loadingRoutine;

        private void OnEnable()
        {
            GameEvents.UsernameSelected += GameEvents_UsernameSelected;
            GameEvents.InstructorSelected += GameEvents_InstructorSelected;
        }

        private void OnDisable()
        {
            GameEvents.UsernameSelected -= GameEvents_UsernameSelected;
            GameEvents.InstructorSelected -= GameEvents_InstructorSelected;

            // Detenemos SOLO las corrutinas de este flujo (evita matar otras corrutinas que este componente pudiera tener).
            if (_startLoadingRoutine != null) { StopCoroutine(_startLoadingRoutine); _startLoadingRoutine = null; }
            if (_loadingRoutine != null) { StopCoroutine(_loadingRoutine); _loadingRoutine = null; }
            _isStarting = false;
        }

        /// <summary>
        /// Hook del botón del Splash. Previenes dobles llamdas, ocultas el splash
        /// y arrancas la secuencia de carga + comprobación de login.
        /// </summary>
        public void StartApplication()
        {
            if (_isStarting) return;           // evita dobles clics
            _isStarting = true;

            HideSplash();
            HideAllMainCanvases();             // asegura que nada quede por encima
            _startLoadingRoutine = StartCoroutine(StartLoading());
        }

        /// <summary>
        /// Arranca el flujo: opcionalmente muestra una pantalla de carga simulada.
        /// Tras finalizar, llama a ComprobeLogin() como en el comportamiento original.
        /// </summary>
        public IEnumerator StartLoading()
        {
            // Si hay loading y está habilitado, ejecútalo.
            if (_showLoadingAtStart && _GUILoading != null)
            {
                _loadingRoutine = StartCoroutine(LoadingSequence());
                yield return _loadingRoutine;  // esperamos a que termine
                _loadingRoutine = null;
            }

            // Continúa con la lógica original.
            ComprobeLogin();

            // Fin del arranque; permite un nuevo intento si fuera necesario.
            _isStarting = false;
            _startLoadingRoutine = null;
        }

        private void GameEvents_UsernameSelected() => ComprobeUsername();
        private void GameEvents_InstructorSelected() => ComprobeInstructor();

        /// <summary>Decide si mostrar Login o avanzar a Username según haveUser.</summary>
        public void ComprobeLogin()
        {
            if (_objectUser == null || _objectUser.userInfo == null)
            {
                Debug.LogWarning($"{nameof(LoginInterfaceController)}: Datos de usuario no asignados, mostrando Login por defecto.", this);
                ShowLogin();
                return;
            }

            if (_objectUser.userInfo.haveUser) ComprobeUsername();
            else ShowLogin();
        }

        /// <summary>Decide si mostrar Username o avanzar a Instructor según haveUsername.</summary>
        public void ComprobeUsername()
        {
            if (_objectUser == null || _objectUser.userInfo == null) { ShowLogin(); return; }
            if (_objectUser.userInfo.haveUsername) ComprobeInstructor();
            else ShowUsername();
        }

        /// <summary>Decide si mostrar Instructor o finalizar el flujo según haveInstructor.</summary>
        public void ComprobeInstructor()
        {
            if (_objectUser == null || _objectUser.userInfo == null) { ShowLogin(); return; }

            if (_objectUser.userInfo.haveInstructor)
            {
                _onFinishLoginConfiguration?.Invoke();
                Debug.Log("Finish Login Configuration");
            }
            else
            {
                ShowInstructor();
            }
        }

        /// <summary>
        /// Simula una carga con barra de progreso y porcentaje.
        /// Si _syncWithUserManagerFlags está activo y hay UserManager, avanza hasta 90% y
        /// completa a 100% cuando EndFinishLoadData y EndFinishLoadAvatar estén en true.
        /// </summary>
        private IEnumerator LoadingSequence()
        {
            _GUILoading.gameObject.SetActive(true);
            UpdateLoadingUI(0f);

            float elapsed = 0f;
            float current = 0f;
            const float nearComplete = 0.9f;

            while (true)
            {
                elapsed += Time.deltaTime;

                // Calcula una diana de progreso según flags o solo por tiempo.
                float target;
                if (_syncWithUserManagerFlags && _userManager != null)
                {
                    bool ready = _userManager.EndFinishLoadData && _userManager.EndFinishLoadAvatar;
                    target = ready ? 1f : nearComplete;
                }
                else
                {
                    target = Mathf.Clamp01(elapsed / Mathf.Max(_minLoadingSeconds, 0.001f));
                }

                // Interpola suavemente hacia la diana.
                current = Mathf.MoveTowards(current, target, Time.deltaTime * 0.6f);
                UpdateLoadingUI(current);

                // Condición de salida: cumplida duración mínima y progreso completo.
                if (elapsed >= _minLoadingSeconds && Mathf.Approximately(current, 1f))
                    break;

                yield return null;
            }

            _GUILoading.gameObject.SetActive(false);
        }

        /// <summary>Actualiza slider y texto de porcentaje en la pantalla de carga.</summary>
        private void UpdateLoadingUI(float progress)
        {
            if (_loadingSlider != null) _loadingSlider.value = progress;

            int percent = Mathf.RoundToInt(progress * 100f);
            string text = percent.ToString() + "%";

            if (_loadingPercentTMP != null)
                _loadingPercentTMP.text = text;
        }

        // === Helpers de visibilidad ===

        /// <summary>Desactiva las tres pantallas principales.</summary>
        private void HideAllMainCanvases()
        {
            if (_GUILogin != null) _GUILogin.gameObject.SetActive(false);
            if (_GUIUsername != null) _GUIUsername.gameObject.SetActive(false);
            if (_GUIInstructor != null) _GUIInstructor.gameObject.SetActive(false);
        }

        /// <summary>Oculta el Splash si existe.</summary>
        private void HideSplash()
        {
            if (_SplashScreen != null) _SplashScreen.gameObject.SetActive(false);
        }

        /// <summary>Activa/desactiva pantallas con una sola llamada.</summary>
        private void SetUIState(bool login, bool username, bool instructor)
        {
            if (_GUILogin != null) _GUILogin.gameObject.SetActive(login);
            if (_GUIUsername != null) _GUIUsername.gameObject.SetActive(username);
            if (_GUIInstructor != null) _GUIInstructor.gameObject.SetActive(instructor);
        }

        private void ShowLogin()      => SetUIState(login: true,  username: false, instructor: false);
        private void ShowUsername()   => SetUIState(login: false, username: true,  instructor: false);
        private void ShowInstructor() => SetUIState(login: false, username: false, instructor: true);
    }
}
