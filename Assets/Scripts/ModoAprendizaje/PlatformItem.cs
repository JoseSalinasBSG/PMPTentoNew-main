using System;
using UnityEngine;
using UnityEngine.UI;

public class PlatformItem : MonoBehaviour
{
    [SerializeField] private Color[] _colors;
    [SerializeField] private Image _image;
    [SerializeField] private UnityEngine.UI.Button _eventTrigger;
    private int _attempts;

    public int Attempts
    {
        get => _attempts;
        set
        {
            _attempts = value;
            Information.attempt = value;
            if (_attempts <= 0)
            {
                ButtonTrigger.enabled = false;
            }
        }
    }

    public UnityEngine.UI.Button ButtonTrigger
    {
        get => _eventTrigger;
        set => _eventTrigger = value;
    }
    [Serializable]
    public class PlatformInformation
    {
        public int id;
        public int idDomain;
        public int index;
        public string description;
        public string tittle;
        public int attempt;
        public int totalTasks;
    }
    private PlatformInformation _information;

    public PlatformInformation Information
    {
        get => _information;
        set
        {
            _information = value;
            // if (_information.a)
            // {
            //     
            // }
            //_image.color = _colors[_information.idDomain];
        }
    }
    public void SendInformation()
    {
        GameEvents.SendInformation?.Invoke(Information);
    }

    public void SetColor()
    {
        _image.color = _colors[_information.idDomain];
    }

    public void EnablePlatform()
    {
        ButtonTrigger.enabled = true;
        SetColor();
    }
    private void OnEnable()
    {
        GameEvents.RecoveryAttempt += GameEvents_RecoveryAttempt;
    }

    private void OnDisable()
    {
        GameEvents.RecoveryAttempt -= GameEvents_RecoveryAttempt;
    }

    private void GameEvents_RecoveryAttempt(int obj)
    {
        if (Information.id == obj)
        {
            //Attempts++;
            if (!ButtonTrigger.enabled)
            {
                ButtonTrigger.enabled = true;
            }
        }
    }
}
