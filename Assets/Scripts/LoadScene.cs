using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

        #region Unity Methods

        // private void Update()
        // {
        //     if (!_initLoadAsync)
        //     {
        //         return;
        //     }
        //     if (loadingOperation.progress >= .9f )
        //     {
        //         loadingOperation.allowSceneActivation = true;
        //     }
        // }
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

            // loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            // loadingOperation.allowSceneActivation = false;
            // _initLoadAsync = true;
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