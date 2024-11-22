using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Question;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private QuestionControllerVP _questionController;
    [SerializeField] private ByteToAudioSource _audioControll;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _viewPortTransform;
    [SerializeField] private RectTransform _contentPanelTransform;
    [SerializeField] private VerticalLayoutGroup _vlg;
    [SerializeField] private RectTransform _canvasTransform;
    [SerializeField] private RectTransform _puntoPartida;
    [SerializeField] private ScrollViewItem scrollViewItem;
    [Range(0, 5)]
    [SerializeField] private float _dragThreshold;
    [Range(0, 3)]
    [SerializeField] private float _duration;

    private ScrollViewItem _currentPanel;
    private List<ScrollViewItem> _listItems = new List<ScrollViewItem>();
    private float _bottomLimit;
    private RectTransform _rCurrent;
    private Vector2 _deltaMouse;


    private int initCount;
    private bool inLerp = false;
    public float scale;

    private Vector3 _ContentInitPosition;
    private void Start()
    {
        _rCurrent = _canvasTransform;
        ScrollViewItem rt = null;
        var questionList = _questionController.GetQuestions(0);
        for (int i = 0; i < 3; i++)
        {
            rt = Instantiate(scrollViewItem, _contentPanelTransform);
            rt.SetDataInformation(questionList[i]);
            rt.OwnRectTransform.sizeDelta = new Vector2(_rCurrent.sizeDelta.x, _rCurrent.sizeDelta.y);
            rt.name = i.ToString();
            _listItems.Add(rt);
        }
        initCount = 3;
        _bottomLimit = _listItems[0].OwnRectTransform.rect.height * initCount;
        _currentPanel = _listItems[0];
        inLerp = true;
        _currentPanel.StartAnimation();
        _audioControll.StartTTS(_currentPanel.GetTextToTTS());
        // CleanOtherPanels();
        // StartCoroutine(FitPanel(_currentPanel));
    }
    private void LateUpdate()
    {
        Time.timeScale = scale;
        if (!inLerp && Input.touchCount > 0)
        {
            if (_contentPanelTransform.localPosition.y > 0)
            {
                Canvas.ForceUpdateCanvases();
                _contentPanelTransform.localPosition -= new Vector3(0, initCount * (_listItems[0].OwnRectTransform.rect.height + _vlg.spacing), 0);
            }
            if (_contentPanelTransform.localPosition.y < 0)
            {
                Canvas.ForceUpdateCanvases();
                _contentPanelTransform.localPosition += new Vector3(0, initCount * (_listItems[0].OwnRectTransform.rect.height + _vlg.spacing), 0);
            }
        }
    }

    private void OnEnable()
    {
        UIEvents.ShowResponseVP += UIEvents_ShowResponseVp;
    }

    private void OnDisable()
    {
        UIEvents.ShowResponseVP -= UIEvents_ShowResponseVp;
    }

    private void UIEvents_ShowResponseVp(string obj)
    {
        _audioControll.StopTTS();
        _audioControll.StartTTS(obj);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        inLerp = true;
        _deltaMouse = Input.GetTouch(0).deltaPosition;
        if (Mathf.Approximately(_ContentInitPosition.y - _contentPanelTransform.localPosition.y, 0))
        {
            return;
        }
        if (Mathf.Abs(_deltaMouse.y) > _dragThreshold)
        {
            var currentPanel = GetNearTransform(_currentPanel, _deltaMouse.y >= 0);
            StartCoroutine(FitPanel(currentPanel, currentPanel != _currentPanel));
        }
        else
        {
            var currentPanel = GetNearTransform();
            StartCoroutine(FitPanel(currentPanel, currentPanel != _currentPanel));
        }
    }

    private ScrollViewItem GetNearTransform(ScrollViewItem target = null, bool dragUp = false)
    {
        ScrollViewItem currentPanel = null;
        float minDistance = Single.MaxValue;
        for (int i = 0; i < _listItems.Count; i++)
        {
            var distance = Vector3.Distance(_listItems[i].OwnRectTransform.position, _puntoPartida.position);
            if (minDistance > distance /*&&( !target || currentPanel != target)*/)
            {
                if (target)
                {
                    if (_listItems[i] == target) continue;
                    minDistance = distance;
                    currentPanel = _listItems[i];
                }
                else
                {
                    minDistance = distance;
                    currentPanel = _listItems[i];
                }

            }
        }
        return currentPanel;
    }
    IEnumerator FitPanel(ScrollViewItem target, bool isOther = false)
    {
        var currentTime = 0f;
        var nitPosition = _contentPanelTransform.position;
        var difference = -target.OwnRectTransform.position + _puntoPartida.position;
        // Debug.Log(difference + " " +
        //      contentPanelTransform.TransformPoint(difference) + " " + contentPanelTransform.InverseTransformPoint(difference));
        if (isOther)
        {
            if (_deltaMouse.y > 0)
            {
                // Debug.Log("next question");
                _questionController.CurrentIndex++;
            }
            else
            {
                // Debug.Log("previous question");
                _questionController.CurrentIndex--;
            }
        }

        while (currentTime <= 1)
        {

            // Debug.DrawLine(target.OwnRectTransform.position, _puntoPartida.position, Color.red);
            currentTime += Time.deltaTime / _duration;
            var movementInY = Mathf.Lerp(nitPosition.y, (nitPosition + difference).y, currentTime);
            _contentPanelTransform.position =
                new Vector3(0, movementInY, 0);
            yield return null;
        }
        _contentPanelTransform.position = new Vector3(0, (nitPosition + difference).y, 0);
        _currentPanel = target;
        if (_currentPanel.transform.GetSiblingIndex() == 2)
        {
            if (!_questionController.IsLastQuestion())
            {
                _contentPanelTransform.transform.GetChild(0).SetAsLastSibling();
                Canvas.ForceUpdateCanvases();
                _contentPanelTransform.localPosition -=
                    new Vector3(0, _listItems[0].OwnRectTransform.rect.height + _vlg.spacing, 0);
                // yield return new WaitForEndOfFrame();
                var childTemp = _contentPanelTransform.transform.GetChild(2);
                var panel = findPanelFromTransform(childTemp);
                if (panel)
                {
                    QuestionDataVP nextQuestion = _questionController.GetNextQuestion();
                    if (nextQuestion != null)
                    {
                        panel.SetDataInformation(nextQuestion);
                    }
                }
            }
        }
        if (_currentPanel.transform.GetSiblingIndex() == 0)
        {
            if (_questionController.CurrentIndex > 0)
            {
                _contentPanelTransform.transform.GetChild(2).SetAsFirstSibling();
                Canvas.ForceUpdateCanvases();
                _contentPanelTransform.localPosition +=
                    new Vector3(0, _listItems[0].OwnRectTransform.rect.height + _vlg.spacing, 0);
                var panel = findPanelFromTransform(_contentPanelTransform.transform.GetChild(0));
                if (panel)
                {
                    var previousQuestion = _questionController.GetPreviousQuestion();
                    if (previousQuestion != null)
                    {
                        //Debug.Log("current: " + _questionController.CurrentQuestion.idQuestion + " prev: " + previousQuestion.idQuestion);
                        panel.SetDataInformation(previousQuestion);
                    }
                }
            }
        }

        if (isOther)
        {
            _currentPanel.StartAnimation();
            _audioControll.StopTTS();
            _audioControll.StartTTS(_currentPanel.GetTextToTTS());
            //CleanOtherPanels();
        }
        inLerp = false;
    }

    private ScrollViewItem findPanelFromTransform(Transform transformToFind)
    {
        return _listItems.FirstOrDefault(x => x.transform == transformToFind);
    }
    private void CleanOtherPanels()
    {
        for (int i = 0; i < _listItems.Count; i++)
        {
            if (_currentPanel == _listItems[i])
            {
                continue;
            }
            _listItems[i].CleanPanel();
        }
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _listItems.Count; i++)
        {
            Gizmos.DrawLine(_listItems[i].OwnRectTransform.position, _puntoPartida.position);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ContentInitPosition = _contentPanelTransform.localPosition;
    }
}