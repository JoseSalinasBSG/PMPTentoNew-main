using UnityEngine;
using UnityEngine.UI;

public class GlosaryChallengeProgress : MonoBehaviour
{
    [SerializeField] private Image _imageProgress;

    private void Start()
    {
        _imageProgress.fillAmount = 0;
    }

    public void UpdateProgress(float progressPercentage)
    {
        _imageProgress.fillAmount = progressPercentage;
    }
}
