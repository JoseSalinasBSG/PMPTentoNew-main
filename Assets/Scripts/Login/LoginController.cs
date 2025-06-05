using System.Collections;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Clase encargada de gestionar el proceso de validación y autenticación de un usuario en el sistema.
/// Proporciona mecanismos para verificar campos vacíos, enviar credenciales al servidor,
/// y manejar respuestas de éxito, error o fallos de inicio de sesión mediante eventos.
///</summary>

namespace Login
{
    public class LoginController : MonoBehaviour
    {
        [SerializeField] private InputBase _emailInput;
        [SerializeField] private InputBase _passwordInput;
        [SerializeField] public UnityEvent _onSuccessLogin;
        [SerializeField] public UnityEvent _onMissingFields;
        [SerializeField] public UnityEvent<string> _onErrorInLogin;
        [SerializeField] public UnityEvent<string> _onFailedLogin;
        [SerializeField] private LoginRestApi _loginRestApi;

        private void Start()
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.AudioSettings.MainSound, true);
        }

        public bool ComprobeMissFields()
        {
            var emptyEmail = _emailInput.HaveError;
            var emptyPassword = _passwordInput.HaveError;
            return emptyEmail || emptyPassword;
        }

        public void ComprobeUser()
        {
            var email = _emailInput.InputField.text;
            var password = _passwordInput.InputField.text;

            if (password != password.Trim())
            {
                _onErrorInLogin?.Invoke("La contraseña no debe contener espacios al principio o al final.");
                return;
            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _onErrorInLogin?.Invoke("El correo o la contraseña no pueden estar vacíos.");
                return;
            }

            _loginRestApi.PostLogin(email, password);
        }

        public void Login()
        {
            StopAllCoroutines();
            StartCoroutine(ILogin());
        }

        IEnumerator ILogin()
        {
            if (ComprobeMissFields())
            {
                _onMissingFields?.Invoke();
                yield break;
            }
            ComprobeUser();
        }

        private void OnEnable()
        {
            GameEvents.ErrorLogin += GameEvents_ErrorLogin;
            GameEvents.FailedLogin += GameEvents_FailedLogin;
            GameEvents.SuccessfulLogin += GameEvents_SuccessfulLogin;
        }

        private void GameEvents_SuccessfulLogin(User obj)
        {
            PlayerPrefs.SetString("username", _emailInput.InputField.text);
            PlayerPrefs.SetString("password", _passwordInput.InputField.text);
            PlayerPrefs.Save();
            _onSuccessLogin?.Invoke();
        }

        private void GameEvents_FailedLogin(string obj)
        {
            _onFailedLogin?.Invoke(obj);
        }

        private void GameEvents_ErrorLogin(string obj)
        {
            _onErrorInLogin?.Invoke(obj);
        }
    }
}
