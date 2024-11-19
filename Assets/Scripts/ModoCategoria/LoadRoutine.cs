using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LoadRoutine : MonoBehaviour
{
    [SerializeField] private float delayTime = 1.0f;
    [SerializeField] private UnityEvent OnRoutineLoaded;

    public void Load()
    {
        StartCoroutine(LoadWithDelayRoutine());
    }

    private IEnumerator LoadWithDelayRoutine()
    {
        yield return new WaitForSeconds(delayTime);
        OnRoutineLoaded.Invoke();
    }
}
