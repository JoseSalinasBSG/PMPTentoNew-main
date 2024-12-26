using System.Collections;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Controlador de la interfaz de usuario para el inicio de sesión.
/// Gestiona la lógica de visualización de diferentes pantallas (Login, Username, Instructor)
/// y verifica el estado de inicio de sesión del usuario.
///</summary>

namespace Login
{
    public class LoginInterfaceController : MonoBehaviour
    {
        [SerializeField] private Canvas _GUILogin;
        [SerializeField] private Canvas _GUIUsername;
        [SerializeField] private Canvas _GUIInstructor;
        [SerializeField] private ScriptableObjectUser _objectUser;
        [SerializeField] private ScriptableObjectInstructor _objectInstructor;
        [SerializeField] private UnityEvent _onFinishLoginConfiguration;
        [SerializeField] private UserManager _userManager;
        
        IEnumerator Start()
        {
            yield return new WaitUntil(() =>_userManager.EndFinishLoadData && _userManager.EndFinishLoadAvatar );
            ComprobeLogin();
        }

        private void OnEnable()
        {
            GameEvents.UsernameSelected += GameEvents_UsernameSelected;
            GameEvents.InstructorSelected += GameEvents_InstructorSelected;
        }

        private void GameEvents_InstructorSelected()
        {
            ComprobeInstructor();
        }

        private void OnDisable()
        {
            GameEvents.UsernameSelected -= GameEvents_UsernameSelected;
            GameEvents.InstructorSelected -= GameEvents_InstructorSelected;
        }

        private void GameEvents_UsernameSelected()
        {
            ComprobeUsername();
        }

        public void ComprobeLogin()
        {
            if (_objectUser.userInfo.haveUser)
            {
                ComprobeUsername();
            }
            else
            {
                _GUILogin.gameObject.SetActive(true);
                _GUIUsername.gameObject.SetActive(false);
                _GUIInstructor.gameObject.SetActive(false);
                
            }
        }

        public void ComprobeUsername()
        {
            if (_objectUser.userInfo.haveUsername)
            {
                ComprobeInstructor();
            }
            else
            {
                _GUILogin.gameObject.SetActive(false);
                _GUIUsername.gameObject.SetActive(true);
                _GUIInstructor.gameObject.SetActive(false);
            }
        }

        public void ComprobeInstructor()
        {
            if (_objectUser.userInfo.haveInstructor)
            {
                _onFinishLoginConfiguration?.Invoke();
            }
            else
            {
                _GUILogin.gameObject.SetActive(false);
                _GUIUsername.gameObject.SetActive(false);
                _GUIInstructor.gameObject.SetActive(true);
            }
        }
    }
}
