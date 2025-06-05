using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayTime : MonoBehaviour
{
    [SerializeField] private float _delayTimeForStart;
    [SerializeField] private UnityEvent OnBeforeDelay;
    [SerializeField] private UnityEvent OnAfterDelay;
    
    private IEnumerator DelayRoutine()
    {
        OnBeforeDelay?.Invoke();
        yield return new WaitForSeconds(_delayTimeForStart);
        OnAfterDelay?.Invoke();
    }
    
    public void StartDelayTime()
    {
        StartCoroutine(DelayRoutine());
    }
}
