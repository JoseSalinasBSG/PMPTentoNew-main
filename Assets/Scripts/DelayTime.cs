using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayTime : MonoBehaviour
{
    // [SerializeField] private float _delayTimeForStart;
    [SerializeField] private GameObject LoadingScreen;
    // [SerializeField] private UnityEvent OnBeforeDelay;
    // [SerializeField] private UnityEvent OnAfterDelay;

    // private IEnumerator DelayRoutine()
    // {
    //     OnBeforeDelay?.Invoke();
    //     yield return new WaitForSeconds(_delayTimeForStart);
    //     OnAfterDelay?.Invoke();
    // }

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
