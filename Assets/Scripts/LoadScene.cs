using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

///<summary>
/// LoadScene gestiona la carga de escenas en el juego, incluyendo el cambio de escenas por nombre, recarga de la escena actual, carga asíncrona y salida del juego.
/// También permite manejar eventos previos a la carga de una escena.
///</summary>

namespace Scene
{
    public class LoadScene : MonoBehaviour
    {
        #region Variables
        [SerializeField] private ScriptableObjectScenes _objectScenes;
        [SerializeField] private UnityEvent _onBeforeLoadScene;
        private AsyncOperation loadingOperation;
        private bool _initLoadAsync;
        public ScriptableObjectScenes ObjectScenes
        {
            get => _objectScenes;
            set => _objectScenes = value;
        }
        #endregion

        #region Methods

        public void LoadSceneUsingName(string sceneName)
        {
            _onBeforeLoadScene?.Invoke();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        public void LoadSceneUsingScriptable()
        {
            _onBeforeLoadScene?.Invoke();
            SceneManager.LoadScene(_objectScenes.nameScene, LoadSceneMode.Single);
        }

        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void LoadSceneAscync(string sceneName)
        {
            _onBeforeLoadScene?.Invoke();
            SceneManager.LoadScene("LoadingScene");
        }

        public void ExitGame()
        {
            Application.Quit(0);
        }

        public void LogOut()
        {
            PlayerPrefs.DeleteAll();
            LoadSceneUsingName("Login");
        }
        #endregion
    }
}