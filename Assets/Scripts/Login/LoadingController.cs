using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Login
{
    public class LoadingController : MonoBehaviour
    {
        [SerializeField] private GameObject LoadingScreen;
        
        public void LoadScene(int index)
        {
            StartCoroutine(LoadSceneAsync(index));
        }
    
        private IEnumerator LoadSceneAsync(int indexScene)
        {
            AsyncOperation loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(indexScene);
        
            LoadingScreen.SetActive(true);
        
            while (!loadingOperation.isDone)
            {
                if (loadingOperation.progress >= 0.9f)
                    break;
                yield return null;
            }
        }
    }
}