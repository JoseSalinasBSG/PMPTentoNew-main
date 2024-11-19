using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Question;
using ScriptableCreator;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GlosaryChallengeController : MonoBehaviour
{
    [SerializeField] private ConceptAndDefinitionSO _conceptAndDefinitionSo;
    [SerializeField] private ScriptableObjectSettings _gameSettings;
    [SerializeField] private RewardItemController _rewardItemController;
    [SerializeField] private GlosaryChallengeProgress _glosaryChallengeProgress;
    [SerializeField] private ButtonsGroup _concepts;
    [SerializeField] private ButtonsGroup _definitions;
    [SerializeField] private TimerGC _timerGC;
    [SerializeField] private float _maxTime;
    [SerializeField] private int _maxNumberOfCouples;
    [SerializeField] private int _cumulativeNumberOfCouples;
    [SerializeField] private int maxNumberCouplesCorrectSelected = 5;
    [SerializeField] private UnityEvent OnMaxNumberCoupleCorrectSelected;
    [SerializeField] private UnityEvent _onGameLost;
    [SerializeField] private UnityEvent _onGameWin;

    private bool _useTimer;
    private List<int> _randomIndices;
    private List<int> _actualIndices = new List<int>();
    private readonly int _indicesPool = 207;
    private readonly int _maxSelectedIndices = 5;
    private float _currentTime;
    private List<OptionGC> _optionChoose = new List<OptionGC>();
    private int _numberOfSelectedCouple = 0;
    private int _currentIndex;
    private float _experienceAccumulated = 0;
    private float _coinsAccumulated = 0;
    public bool UseTimer
    {
        get => _useTimer;
        set => _useTimer = value;
    }

    public int CurrentIndex
    {
        get => _currentIndex;
        set => _currentIndex = value;
    }

    private void Start()
    {
        FindObjectOfType<GameplaySound>().PlayGlossaryChallengeSound();
        _glosaryChallengeProgress.SetData(_maxNumberOfCouples * 1/3, _maxNumberOfCouples * 2 / 3, _maxNumberOfCouples * 3 / 3);
        UseTimer = true;
        _timerGC.InitValue(_maxTime);
        _currentTime = 0;
        SetData();
    }

    private void OnEnable()
    {
        GameEvents.GameWon += GameEvents_GameWon;
        GameEvents.GameLost += GameEvents_GameWon;
    }

    private void OnDisable()
    {
        GameEvents.GameWon -= GameEvents_GameWon;
        GameEvents.GameLost -= GameEvents_GameWon;
    }
    private void GameEvents_GameWon()
    {
        _rewardItemController.AddCoins((int)_coinsAccumulated);
        _rewardItemController.AddExperience((int)_experienceAccumulated);
    }
    private void SetData()
    {
        _randomIndices = GenerateRandomList(1, _indicesPool);

        for (int i = 0; i < _randomIndices.Count; i++)
        {
            Debug.Log(_randomIndices[i]);
        }
        var tempIndex = GetRandomIndex();
        for (int i = 0; i < _maxSelectedIndices; i++)
        {
            var conceptAndDefinition = _conceptAndDefinitionSo.list[_randomIndices[i]];
            _actualIndices.Add(conceptAndDefinition.id);
            _concepts.Options[tempIndex.Item1[i]].SetData(conceptAndDefinition.concept, conceptAndDefinition.id);
            _definitions.Options[tempIndex.Item2[i]].SetData(conceptAndDefinition.definition, conceptAndDefinition.id);
            CurrentIndex = i;
        }
    }

    public (List<int>, List<int>) GetRandomIndex()
    {
        return (GenerateRandomList(0, 5), GenerateRandomList(0, 5));
    }

public void Evaluate()
    {
        if (_concepts.OldSelectedButton && _definitions.OldSelectedButton)
        {
            Debug.Log("evaluando");
            if (_concepts.OldSelectedButton.ID == _definitions.OldSelectedButton.ID)
            {
                Debug.Log("Se emparejo: " + _concepts.OldSelectedButton.ID + " " + _definitions	.OldSelectedButton.ID);
                _numberOfSelectedCouple++;
                _concepts.OldSelectedButton.SetCorrectOption();
                _optionChoose.Add(_concepts.OldSelectedButton);
                _definitions.OldSelectedButton.SetCorrectOption();
                _optionChoose.Add(_definitions.OldSelectedButton);
                if (_numberOfSelectedCouple == maxNumberCouplesCorrectSelected)
                {
                    var totalToChange = _optionChoose.Count / 2;
                    Dictionary<int, int> actualValuesToConcept = new Dictionary<int, int>();
                    Dictionary<int, int> actualValuesToDefinition = new Dictionary<int, int>();
                    for (int i = 0; i < totalToChange; i++)
                    {
                        var conceptAndDefinition = GetNextCouple();
                        var randomIndexInConcept = Random.Range(0, totalToChange) * 2;
                        var randomIndexInDefinition = Random.Range(0, totalToChange) * 2 + 1;
                        
                        while (actualValuesToConcept.ContainsKey(randomIndexInConcept))
                        {
                            randomIndexInConcept = Random.Range(0, totalToChange) * 2;
                        }
                        while (actualValuesToDefinition.ContainsKey(randomIndexInDefinition))
                        {
                            randomIndexInDefinition = Random.Range(0, totalToChange) * 2 + 1;
                        }
                        Debug.Log(randomIndexInConcept + " " + randomIndexInDefinition);
                        actualValuesToConcept.Add(randomIndexInConcept, randomIndexInConcept);
                        actualValuesToDefinition.Add(randomIndexInDefinition, randomIndexInDefinition);
                        _actualIndices.Remove(_optionChoose[randomIndexInConcept].ID);
                        _optionChoose[randomIndexInConcept].StartAnimationWithData(conceptAndDefinition.concept, conceptAndDefinition.id);
                        _optionChoose[randomIndexInDefinition].StartAnimationWithData(conceptAndDefinition.definition, conceptAndDefinition.id);
                        _actualIndices.Add(conceptAndDefinition.id);
                    }
                    _cumulativeNumberOfCouples += maxNumberCouplesCorrectSelected;
                    _glosaryChallengeProgress.UpdateProgress(1f * _cumulativeNumberOfCouples / _maxNumberOfCouples);
                    _numberOfSelectedCouple = 0;
                    _optionChoose.Clear();
                    if (_cumulativeNumberOfCouples >= _maxNumberOfCouples)
                    {
                        _onGameWin?.Invoke();
                        GameEvents.GameWon?.Invoke();
                        return;
                    }
                    OnMaxNumberCoupleCorrectSelected?.Invoke();
                    GameEvents_CorrectlyAnswered();
                    // Remove old indices from "_actualIndices"
                }
                Debug.Log("correcto");
            }
            else
            {
                Debug.Log("incorrecto");
                _concepts.OldSelectedButton.SetIncorrectOption();
                _definitions.OldSelectedButton.SetIncorrectOption();

                //Update data with animation
                _concepts.OldSelectedButton.StartAnimation();
                _definitions.OldSelectedButton.StartAnimation();
            }
            _concepts.CleanOldSelected();
            _definitions.CleanOldSelected();

        }
        // else
        // {
        //     Debug.Log("No Se puede evaluar, no se encuentra pareja");
        // }
    }
    private void Update()
    {
        if (!UseTimer)
        {   
            return;
        }
        _currentTime += Time.deltaTime / _maxTime;
        if (_currentTime >1)
        {
            UseTimer = false;
            _onGameLost?.Invoke();
            GameEvents.GameLost?.Invoke();
            return;
        }
        _timerGC.SetValueTimer(1 - _currentTime);
    }

    private ConceptAndDefinitionData GetNextCouple()
    {
        CurrentIndex++;
        Debug.Log("Current index: " + CurrentIndex + " RandomIndice: " + _randomIndices[CurrentIndex] +  "next id Question: " + _conceptAndDefinitionSo.list[_randomIndices[CurrentIndex]].id);
        return  _conceptAndDefinitionSo.list[_randomIndices[CurrentIndex]];
    }
    
    public List<int> GenerateRandomList(int initValue, int size)
    {
        // Crea una lista ordenada del 1 al 207
        List<int> numbers = new List<int>();
        for (int i = initValue; i < size; i++)
        {
            numbers.Add(i);
        }

        // Mezcla aleatoriamente la lista utilizando el algoritmo de Fisher-Yates
        System.Random random = new System.Random();
        for (int i = 0; i < numbers.Count; i++)
        {
            int randomIndex = random.Next(i, numbers.Count);
            (numbers[randomIndex], numbers[i]) = (numbers[i], numbers[randomIndex]);
        }
        return numbers;
    }
    
    public void StopTimer()
    {
        UseTimer = false;
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _concepts.Options.Count; i++)
        {
            Random.InitState(_concepts.Options[i].ID);
            Gizmos.color = new Color(
                Random.Range(0, 1f), 
                Random.Range(0, 1f), 
                Random.Range(0, 1f)
            );
            Gizmos.DrawSphere(_concepts.Options[i].transform.position, 20f);
        }
        for (int i = 0; i < _definitions.Options.Count; i++)
        {
            Random.InitState(_definitions.Options[i].ID);
            Gizmos.color = new Color(
                Random.Range(0, 1f), 
                Random.Range(0, 1f), 
                Random.Range(0, 1f)
            );
            Gizmos.DrawSphere(_definitions.Options[i].transform.position, 20f);
        }
    }
    
    private void GameEvents_CorrectlyAnswered()
    {
        // _numberOfConsecutiveQuestion++;
        // var clampConsecutive = Mathf.Clamp(_numberOfConsecutiveQuestion, 0, int.MaxValue);
        var exp = 
            // Base experience
            _gameSettings.settingData.MCReward.baseExperience +
            // Bonus by consecutive question
            _gameSettings.settingData.MCReward.aditionalBonusExpConsecutiveQuestion /** clampConsecutive*/ +
            // Bonus by achievement
            _gameSettings.settingData.MCReward.aditionalBonusExpForAchievement * 0;
        GameEvents.RequestExperienceChange?.Invoke(exp);
        _experienceAccumulated += exp;

        var coins =
            // Base coins
            _gameSettings.settingData.MCReward.baseCoins +
            // Bonus by consecutive question
            _gameSettings.settingData.MCReward.aditionalBonusCoinsConsecutiveQuestion /** clampConsecutive*/ +
            // Bonus by achievement
            _gameSettings.settingData.MCReward.aditionalBonusCoinsForAchievement * 0;
        _coinsAccumulated += coins;
        GameEvents.RequestCoinsChange?.Invoke(coins);
    }
}
