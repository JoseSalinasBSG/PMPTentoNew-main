using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PathItem
{
    public SmoothPath smoothPath;
    public bool isInverse;
}

public class PathController : MonoBehaviour
{
    // [SerializeField]PathItem
    [Header("Paths")] 
    [SerializeField] private SmoothPath _mainToRanking;
    [SerializeField] private SmoothPath _mainToStore;
    [SerializeField] private SmoothPath _mainToTrain;
    [SerializeField] private SmoothPath _mainToAchievement;
    [SerializeField] private SmoothPath _storeToTrain;
    [SerializeField] private SmoothPath _storeToAchievement;
    [SerializeField] private SmoothPath _storeToRanking;
    [SerializeField] private SmoothPath _storeToMain;
    [SerializeField] private SmoothPath _achievementToStore;
    [SerializeField] private SmoothPath _achievementToTrain;
    [SerializeField] private SmoothPath _achievementToRanking;
    [SerializeField] private SmoothPath _achievementToMain;
    [SerializeField] private SmoothPath _rankingToStore;
    [SerializeField] private SmoothPath _rankingToTrain;
    [SerializeField] private SmoothPath _rankingToAchievement;
    [SerializeField] private SmoothPath _rankingToMain;

    [SerializeField] private SmoothPath _trainToMain;
    [SerializeField] private SmoothPath _trainToRanking;
    [SerializeField] private SmoothPath _trainToStore;
    [SerializeField] private SmoothPath _trainToAchievement;       

    [SerializeField] private PathState _currentPathState;

    private SmoothPath _toMain;
    private SmoothPath _toStore;
    private SmoothPath _toTrain;
    private SmoothPath _toAchievement;
    private SmoothPath _toRanking;

    private SmoothPath _currentPath;

    public enum PathState
    {
        none,
        mainToRanking,
        mainToStore,
        mainToTrain,
        maintoAchievement,
        storeToTrain,
        storeToMain,
        storeToAchievement,
        storeToRanking,
        achievementToStore,
        achievementToTrain,
        achievementToRanking,
        achievementToMain,
        rankingToStore,
        rankingToTrain,
        rankingToAchievement,
        rankingToMain,

        trainToMain,
        trainToRanking,
        trainToStore,
        trainToAchievement,
           
            
    }

    public enum EActualPath
    {
        main,
        shop,
        train,
        achievement,
        ranking
    }

    private EActualPath _actualPath;
    public PathState CurrentPath
    {
        get => _currentPathState;
        set
        {
            _currentPathState = value;
            switch (_currentPathState)
            {
                case PathState.mainToRanking:
                case PathState.storeToRanking:
                case PathState.achievementToRanking:

                case PathState.trainToRanking:
                    
                    _toMain = _rankingToMain;
                    _toStore = _rankingToStore;
                    _toTrain = _rankingToTrain;
                    _toAchievement = _rankingToAchievement;
                    _actualPath = EActualPath.ranking;
                    break;

                case PathState.mainToStore:
                case PathState.achievementToStore:
                case PathState.rankingToStore:

                case PathState.trainToStore:

                    _toMain = _storeToMain;
                    _toTrain = _storeToTrain;
                    _toAchievement = _storeToAchievement;
                    _toRanking = _storeToRanking;
                    _actualPath = EActualPath.shop;
                    break;
                
                case PathState.maintoAchievement:
                case PathState.storeToAchievement:
                case PathState.rankingToAchievement:

                case PathState.trainToAchievement:

                    _toMain = _achievementToMain;
                    _toStore = _achievementToStore;
                    _toTrain = _achievementToTrain;
                    _toRanking = _achievementToRanking;
                    _actualPath = EActualPath.achievement;
                    break;
                
                case PathState.storeToMain:
                case PathState.achievementToMain:
                case PathState.rankingToMain:

                case PathState.trainToMain:

                    _toStore = _mainToStore;
                    _toTrain = _mainToTrain;
                    _toAchievement = _mainToAchievement;
                    _toRanking = _mainToRanking;
                    _actualPath = EActualPath.main;
                    break;

                case PathState.storeToTrain:
                case PathState.achievementToTrain:
                case PathState.rankingToTrain:

                case PathState.mainToTrain:

                    _toStore = _trainToStore;
                    _toMain = _trainToMain;
                    _toAchievement = _trainToAchievement;
                    _toRanking = _trainToRanking;
                    _actualPath = EActualPath.train;
                    break;
            }
        }
    }

    private void Start()
    {
        _actualPath = EActualPath.main;
        _toMain = null;
        _toAchievement = _mainToAchievement;
        _toRanking = _mainToRanking;
        _toTrain = _mainToTrain;
        _toStore = _mainToStore;
        gameObject.SetActive(false);
    }

    public void ToMain()
    {
        if (_actualPath == EActualPath.main)
        {
            return;
        }
        UIEvents.StartFooterButtonAnimation.Invoke();
        gameObject.SetActive(true);
        CurrentPath = PathState.storeToMain;
        _toMain.StartTransition();
    }

    public void ToStore()
    {
        if (_actualPath == EActualPath.shop)
        {
            return;
        }
        UIEvents.StartFooterButtonAnimation.Invoke();
        gameObject.SetActive(true);
        CurrentPath = PathState.mainToStore;
        _toStore.StartTransition();
    }

    public void ToTrain()
    {
        if (_actualPath == EActualPath.train)
        {
            return;
        }
        UIEvents.StartFooterButtonAnimation.Invoke();
        gameObject.SetActive(true);
        CurrentPath = PathState.mainToTrain;
        _toTrain.StartTransition();
    }

    public void ToAchievement()
    {
        if (_actualPath == EActualPath.achievement)
        {
            return;
        }
        UIEvents.StartFooterButtonAnimation.Invoke();
        gameObject.SetActive(true);
        CurrentPath = PathState.maintoAchievement;
        _toAchievement.StartTransition();
    }

    public void ToRanking()
    {
        if (_actualPath == EActualPath.ranking)
        {
            return;
        }
        UIEvents.StartFooterButtonAnimation.Invoke();
        gameObject.SetActive(true);
        CurrentPath = PathState.mainToRanking;
        _toRanking.StartTransition();
    }
}