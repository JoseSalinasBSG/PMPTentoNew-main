using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InterfaceControllerMainMenu : MonoBehaviour
{
    [SerializeField] private UIScreen[] _screens;

    private UIScreen _currentScreen;

    private UIScreen GetScreen(string name)
    {        
        return _screens.FirstOrDefault(x => x.name == name)!;
    }
    private void OnEnable()
    {
        GameEvents.WrongWhenNewUsername	 += GameEvents_WrongWhenNewUsername;
        GameEvents.RequesNewUsername += GameEvents_RequesNewUsername;
        GameEvents.NewUsername	 += GameManager_NewUsername;
        GameEvents.NewInstuctorId	+= GameManager_NewUsername;
    }

    private void GameManager_NewUsername(string obj)
    {
        if (_currentScreen != null)
        {
            _currentScreen.screen.gameObject.SetActive(false);
        }
    }
    private void GameManager_NewUsername(int id)
    {
        Debug.Log(_currentScreen);
        if (_currentScreen != null)
        {
            _currentScreen.screen.gameObject.SetActive(false);
        }
    }

    private void GameEvents_RequesNewUsername()
    {
        Debug.Log("Encender");
        EnableScreen(PMPScenes.ChangeRequest);
    }

    private void GameEvents_WrongWhenNewUsername()
    {
        EnableScreen(PMPScenes.WrongInChange);
    }
    
    private void OnDisable()
    {
        GameEvents.WrongWhenNewUsername	 -= GameEvents_WrongWhenNewUsername;
        GameEvents.RequesNewUsername -= GameEvents_RequesNewUsername;
    }



    public void EnableScreen(string viewName)
    {
        Debug.Log(viewName);
        if (_currentScreen != null)
        {
            _currentScreen.screen.gameObject.SetActive(false);
        }
        var screen = GetScreen(viewName);
        screen.screen.gameObject.SetActive(true);
        screen.screen.FadeInTransition();
        _currentScreen = screen;
    }
}
