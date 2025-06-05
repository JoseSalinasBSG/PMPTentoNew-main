using Button;
using TMPro;
using UnityEngine;

public class TaskInformation : MonoBehaviour
{

    [Header("Popup General information")]
    [SerializeField] private TextMeshProUGUI _indexLabel;
    [SerializeField] private TextMeshProUGUI _tittleLabel;
    [SerializeField] private TextMeshProUGUI _attemptLabel;
    [Header("Popup Detail information")]
    [SerializeField] private TextMeshProUGUI _detailPopupTittleLabel;
    [SerializeField] private TextMeshProUGUI _detailPopupDescriptionLabel;
    [SerializeField] private ButtonAnimation _buttonToPlay;
    
    private void OnEnable()
    {
        GameEvents.SendInformation += GameEvents_SendInformation;
    }

    private void OnDisable()
    {
        GameEvents.SendInformation -= GameEvents_SendInformation;
    }

    private void GameEvents_SendInformation(PlatformItem.PlatformInformation obj)
    {
        if (obj.attempt <= 0)
        {
            _buttonToPlay.DisableButton();
        }
        else
        {
            _buttonToPlay.EnableButton();
        }
        _tittleLabel.transform.parent.gameObject.SetActive(false);
        _indexLabel.text = $"Tarea {obj.index} de {obj.totalTasks}";
        _tittleLabel.text = $"{obj.tittle}";
        _detailPopupDescriptionLabel.text = $"{obj.tittle}";
        _detailPopupTittleLabel.text = $"{obj.tittle}";
        _attemptLabel.text = $"Te quedan {obj.attempt} intentos";
        _tittleLabel.transform.parent.gameObject.SetActive(true);
    }
}
