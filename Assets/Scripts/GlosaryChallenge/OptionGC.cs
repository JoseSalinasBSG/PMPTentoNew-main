using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionGC : MonoBehaviour
{
    [SerializeField] private ButtonsGroup _buttonsGroup;
    [SerializeField] private TextMeshProUGUI _label;
    
    [SerializeField] private Image _image;
    [SerializeField] private Color _correctColor;
    [SerializeField] private Color _incorrectColor;
    [SerializeField] private Color _colorInGC;
    [SerializeField] private Color defaultColor;
    [SerializeField] private EventTrigger _eventTrigger;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private UnityEvent OnOptionSelected;
    [SerializeField] private UnityEvent OnCorrectOptionSetted;
    [SerializeField] private UnityEvent OnIncorrectOptionSetted;
    [SerializeField] private UnityEvent OnAnimationRestarted;

    private int _id;
    private bool _selectedInGC;
    private readonly float _timeToAnimation = .5f;

    public int ID
    {
        get => _id;
        set => _id = value;
    }

    public void SetData(string description, int id)
    {
        _label.text = description;
        ID = id;
    }
    
    public void DisableOption()
    {
        _eventTrigger.enabled = false;
    }
        
    public void EnableOption()
    {
        _label.color = Color.white;
        _image.color = defaultColor;
        _eventTrigger.enabled = true;
        ShowOption();
        OnAnimationRestarted?.Invoke();
    }
    public void HideOption()
    {
        gameObject.SetActive(false);
    }
    public void ShowOption()
    {
        gameObject.SetActive(true);
    }
    public bool IsDisable()
    {
        return !gameObject.activeInHierarchy;
    }
    public void SendSelectOption()
    {
        _buttonsGroup.SelectButton(this);
        OnOptionSelected?.Invoke();
    }

    public void SelectButton()
    {
        _image.color = _colorInGC;
        _selectedInGC = true;
    }

    public void DeselectButton()
    {
        _image.color = defaultColor;
        _selectedInGC = false;
    }
    public void SetCorrectOption()
    {
        _image.color = _correctColor;
        DisableOption();
        OnCorrectOptionSetted?.Invoke();
    }
    public void SetIncorrectOption()
    {
        _image.color = _incorrectColor;
        DisableOption();
        OnIncorrectOptionSetted?.Invoke();
    }

    public void StartAnimation()
    {
        DisableOption();
        StopAllCoroutines();
        StartCoroutine(IStartAnimation());
    }
    public void StartAnimationWithData(string description, int id)
    {
        DisableOption();
        StopAllCoroutines();
        StartCoroutine(IStartAnimationWithData(description, id));
    }

    private IEnumerator IStartAnimation()
    {
        yield return StartCoroutine(IDisable());
        yield return StartCoroutine(IEnable());
        
    }
    private IEnumerator IStartAnimationWithData(string description, int id)
    {
        yield return StartCoroutine(IDisable());
        _label.text = description;
        ID = id;
        yield return StartCoroutine(IEnable());
        
    }
    private IEnumerator IDisable()
    {
        var currentTime = 0f;
        while (currentTime <=1)
        {
            currentTime += Time.deltaTime / _timeToAnimation;
            _canvasGroup.alpha = Mathf.Lerp(1, 0, currentTime);
            yield return null;
        }
        
    }

    public void AnimationDisable()
    {
        StopAllCoroutines();
        StartCoroutine(IDisable());
    }
    
    public void AnimationEnable()
    {
        StopAllCoroutines();
        StartCoroutine(IEnable());
    }
    private IEnumerator IEnable()
    {
        EnableOption();

        var currentTime = 0f;
        while (currentTime <=1)
        {
            currentTime += Time.deltaTime / _timeToAnimation;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, currentTime);
            yield return null;
        }
    }
}
