using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableCreator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Question
{
    [Serializable]
    public class QuestionData
    {
        public string idQuestion;
        public string question;
        public OptionItem[] options = new OptionItem[4];
        public string option1;
        public string option2;
        public string option3;
        public string option4;
        public string idCorrectOption;
        public ProgressItem progressItem;
        public int idTask;
        public QuestionItem questionItem;
    }
    public class QuestionController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private QuestionInformation _questionInformation;
        [SerializeField] private IncorrectQuestionsSO _incorrectQuestions;
        [SerializeField] private UnityEvent _onSelectOption;
        [SerializeField] private UnityEvent<int> _onCorrectOption;//evento con parametro int
        [SerializeField] private UnityEvent _onIncorrectOption;
        [SerializeField] private ProgressQuestion _progressQuestion;
        [SerializeField] private bool useProgressQuestion = true;
        [SerializeField] private bool hasProgressController;
        [SerializeField] private UnityEvent _onEndQuestions;
        [SerializeField] private UnityEvent _onNextQuestion;
        [SerializeField] private UnityEvent _onWonGame;
        [SerializeField] private UnityEvent _onLostGame;
        [SerializeField] private UnityEvent OnLatestQuestionAchieved;
        [SerializeField] private DataToRegisterSO _toRegisterSo;
        private List<QuestionData> _session = new List<QuestionData>();
        public QuestionData _currentQuestion;//provisionalmente en public para ver respuestas
        private List<int> _indexes = new List<int>() { 0, 1, 2, 3 };
        private int _currentIndex;
        private int _numberOfConsecutiveAnswers;

        public int GetCountSession
        {
            get => _session.Count;
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set => _currentIndex = value;
        }

        public QuestionData CurrentQuestion
        {
            get => _currentQuestion;
            set => _currentQuestion = value;
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            GameEvents.QuestionReady += GameEvents_QuestionReady;
        }

        private void OnDisable()
        {
            GameEvents.QuestionReady -= GameEvents_QuestionReady;
        }

        private void GameEvents_QuestionReady()
        {
            SetData(_toRegisterSo.questionInformation.listaPreguntas);
            ConfigurateQuestion();
            UIEvents.ShowQuestionView?.Invoke();
        }
        // Start is called before the first frame update
        void Start()
        {
            _progressQuestion?.CalculateWidth(10);

            // for (int i = 0; i < 10; i++)
            // {
            //     QuestionData questionData = new QuestionData();
            //     var randomValue = Random.value;
            //     questionData.idQuestion = randomValue.ToString();
            //     questionData.question = $"Question nº {randomValue}";
            //     questionData.options[0]. = $"Question {randomValue} option 1";
            //     questionData.options[1] = $"Question {randomValue} option 2";
            //     questionData.options[2] = $"Question {randomValue} option 3";
            //     questionData.options[3] = $"Question {randomValue} option 4";
            //     questionData.idCorrectOption = $"Question {randomValue} option 4";
            //     questionData.progressItem = _progressQuestion.CreateItem();
            //     _session.Add( questionData);
            // }
        }
        #endregion

        #region Methods

        public void SetData(QuestionItem[] questions)
        {
            if (useProgressQuestion)
            {
                _progressQuestion.CalculateWidth(questions.Length);
            }
            for (int i = 0; i < questions.Length; i++)
            {

                QuestionData questionData = new QuestionData();
                questionData.questionItem = questions[i];
                questionData.idQuestion = questions[i].pregunta.id.ToString();
                questionData.question = questions[i].pregunta.enunciado;

                var randomvalue = Random.Range(0, _indexes.Count);
                questionData.options[_indexes[randomvalue]] = questions[i].pregunta.respuesta[0];
                _indexes.RemoveAt(randomvalue);

                randomvalue = Random.Range(0, _indexes.Count);
                questionData.options[_indexes[randomvalue]] = questions[i].pregunta.respuesta[1];
                _indexes.RemoveAt(randomvalue);

                randomvalue = Random.Range(0, _indexes.Count);
                questionData.options[_indexes[randomvalue]] = questions[i].pregunta.respuesta[2];
                _indexes.RemoveAt(randomvalue);

                questionData.options[_indexes[0]] = questions[i].pregunta.respuesta[3];
                _indexes.RemoveAt(0);

                questionData.idCorrectOption = questions[i].pregunta.respuesta.FirstOrDefault(x => x.correcto == "true")?.id.ToString();
                if (useProgressQuestion)
                {
                    questionData.progressItem = _progressQuestion.CreateItem(i);
                }

                _session.Add(questionData);
                _indexes.Add(0);
                _indexes.Add(1);
                _indexes.Add(2);
                _indexes.Add(3);
                questionData.idTask = questions[i].idSimuladorPmpTarea;
            }
        }
        public void ConfigurateQuestion()
        {
            _currentQuestion = _session[_currentIndex];
            if (useProgressQuestion)
            {
                _currentQuestion.progressItem.SetCurrentItem();
            }
            _questionInformation.SetData(_currentQuestion);
            //_currentIndex++;
        }

        public void NextQuestion()
        {
            _currentIndex++;

            if (_currentIndex == _session.Count)
            {
                _onWonGame?.Invoke();
                GameEvents.GameWon?.Invoke();
                _onEndQuestions?.Invoke();
                return;
            }
            if (hasProgressController)
            {
                _currentQuestion.progressItem.RemoveCurrentItem();//remover marcador de pregunta 

            }
            var tempQuestion = _session[_currentIndex];
            _currentQuestion = tempQuestion;
            if (useProgressQuestion)
            {
                _currentQuestion.progressItem.SetCurrentItem();
            }
            _questionInformation.SetData(_currentQuestion);
            _onNextQuestion?.Invoke();
            GameEvents.RecoveryTime?.Invoke();
        }

        public void BackQuestion()
        {
            if (_currentIndex <= 0)
                return;
            _currentQuestion.progressItem.RemoveCurrentItem();//remover marcador de pregunta
            _currentIndex--;

            var tempQuestion = _session[_currentIndex];
            _currentQuestion = tempQuestion;
            if (useProgressQuestion)
            {
                _currentQuestion.progressItem.SetCurrentItem();
            }
            _questionInformation.SetData(_currentQuestion);
            _onNextQuestion?.Invoke();
            GameEvents.RecoveryTime?.Invoke();
        }

        public void SetIncorrectQuestion()
        {
            if (useProgressQuestion)
            {
                _currentQuestion.progressItem.SetIncorrectSelection();
            }
        }
        public bool ValidateResponse(string id)
        {
            _questionInformation.DisableOptions();//deshabilita opciones de respuestas una vez seleccionada una
            _onSelectOption?.Invoke();
            AchievementControlller _achievementController = FindObjectOfType<AchievementControlller>();//referencio achievement controller
            int lastElement = _achievementController.maxGoodStreakList.Last();//accedo al ultimo elemento de la lista maxGoodStreakList en AchievementController
        
            if (_currentQuestion.idCorrectOption == id)//compara el id de la opcion seleccionada con el de la correcta
            {
                if (_numberOfConsecutiveAnswers < lastElement)
                {
                    _numberOfConsecutiveAnswers++;//incrementa contador de respuestas correctas
                }
                else
                {
                    _numberOfConsecutiveAnswers = 1;
                }
                if (useProgressQuestion)
                {
                    _currentQuestion.progressItem.SetCorrectSelection();
                    _progressQuestion.Label = _numberOfConsecutiveAnswers.ToString();
                }
                if (_numberOfConsecutiveAnswers == GetCountSession)//Verifica si se alcanzo el numero de respuestas deseado
                {
                    OnLatestQuestionAchieved?.Invoke();//si alcanzo el numero dispara el evento
                }
                _questionInformation.SetMessage("¡Correcto! ¡Eres un experto en este tema!", true);//muestra mensaje
                GameEvents.CorrectlyAnswered?.Invoke();//dispara evento
                _onCorrectOption?.Invoke(_numberOfConsecutiveAnswers);//llamar al evento local onCorrectOption con el contador de respuestas correctas como parametro
                return true;//retorna verdadero si es correcta la respuesta    
            }
            //SI ES INCORRECTA
            _incorrectQuestions.SaveIncorrectQuestion(_currentQuestion.questionItem);//guarda la pregunta incorrecta
            if (useProgressQuestion)
            {
                _currentQuestion.progressItem.SetIncorrectSelection();
            }
            _questionInformation.SetMessage("Esa no es la respuesta correcta, pero cada error es una oportunidad de aprendizaje.", false);
            GameEvents.IncorrectlyAnswered?.Invoke();//dispara evento
            _onIncorrectOption?.Invoke();
            return false;//retorna falso si es incorrecta la respuesta
        }

        public void SetCurrentQuestionProgress()
        {
            if (useProgressQuestion)
            {
                _currentQuestion.progressItem.SetCurrentItem();
            }
        }
        public bool CompareResponse(string id)
        {
            return _currentQuestion.idCorrectOption == id;
        }

        public void ReturnTrueOption()
        {
            var idCorrectOption = _currentQuestion.idCorrectOption;
            if (_questionInformation.Opt1.ID == idCorrectOption)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt1.gameObject,
                    new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                return;
            }
            if (_questionInformation.Opt2.ID == idCorrectOption)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt2.gameObject,
                    new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                return;
            }
            if (_questionInformation.Opt3.ID == idCorrectOption)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt3.gameObject,
                    new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                return;
            }
            ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt4.gameObject,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            return;
        }

        public void SendGameLost()
        {
            _onLostGame?.Invoke();
            GameEvents.GameLost?.Invoke();
        }

        private void OnDrawGizmos()
        {
            if (_currentQuestion == null)
            {
                return;
            }
            Gizmos.color = Color.green;
            if (_questionInformation.Opt1.ID == _currentQuestion.idCorrectOption)
            {
                Gizmos.DrawSphere(_questionInformation.Opt1.transform.position, 15f);
                return;
            }
            if (_questionInformation.Opt2.ID == _currentQuestion.idCorrectOption)
            {
                Gizmos.DrawSphere(_questionInformation.Opt2.transform.position, 15f);
                return;
            }
            if (_questionInformation.Opt3.ID == _currentQuestion.idCorrectOption)
            {
                Gizmos.DrawSphere(_questionInformation.Opt3.transform.position, 15f);
                return;
            }
            Gizmos.DrawSphere(_questionInformation.Opt4.transform.position, 15f);
        }
        #endregion
    }
}