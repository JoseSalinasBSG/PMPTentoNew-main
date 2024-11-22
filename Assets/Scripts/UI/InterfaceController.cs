using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UIScreen
{
    public string name;
    public FadeUI screen;
}
public class InterfaceController : MonoBehaviour
{
    [SerializeField] private UIScreen[] _screens;

    private UIScreen _currentScreen;
    private void Start()
    {
        
    }

    private UIScreen GetScreen(string name)
    {        
        return _screens.FirstOrDefault(x => x.name == name)!;
    }
    private void OnEnable()
    {
        var screen = GetScreen(PMPScenes.LoadingViewInCategoryMode);
        screen.screen.gameObject.SetActive(true);
        screen.screen.FadeInTransition();
        _currentScreen = screen;
        GameEvents.DomainsSaved += GameEvents_DomainsSaved;
        GameEvents.TaskRetreived += GameEvents_TaskRetreived;
        UIEvents.ShowQuestionView += UIEvents_ShowQuestionView;
        UIEvents.ShowLoadingView += UIEvents_ShowLoadingView;
        UIEvents.ShowFinishView += UIEvents_ShowFinishView;
    }

    private void UIEvents_ShowFinishView()
    {
        //Debug.Log("show finish view ");
        EnableScreen(PMPScenes.FinishViewInCategoryMode);
    }

    private void UIEvents_ShowLoadingView()
    {
        //Debug.Log("show loading view ");
        EnableScreen(PMPScenes.LoadingViewInCategoryMode);
    }

    private void OnDisable()
    {
        GameEvents.DomainsSaved -= GameEvents_DomainsSaved;
        GameEvents.TaskRetreived -= GameEvents_TaskRetreived;
        UIEvents.ShowQuestionView -= UIEvents_ShowQuestionView;
        UIEvents.ShowLoadingView -= UIEvents_ShowLoadingView;
        UIEvents.ShowFinishView -= UIEvents_ShowFinishView;
    }


    private void UIEvents_ShowQuestionView()
    {
        //Debug.Log("show question view ");
        EnableScreen(PMPScenes.QuestionAndOptionViewInCategoryMode);
    }

    private void GameEvents_TaskRetreived(List<Task> obj)
    {
        //Debug.Log("interface task retreived");
        EnableScreen(PMPScenes.TaskViewInCategoryMode);
    }

    private void GameEvents_DomainsSaved(string viewName)
    {
        EnableScreen(viewName);
        //Debug.Log(" -> " + viewName);
    }

    public void EnableScreen(string viewName)
    {
        //Debug.Log(viewName);
        _currentScreen.screen.gameObject.SetActive(false);
        var screen = GetScreen(viewName);
        screen.screen.gameObject.SetActive(true);
        screen.screen.FadeInTransition();
        _currentScreen = screen;
    }
}
