using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Question
{
    [Serializable]
    public class QuestionDataVP
    {
        public string idQuestion;
        public string question;
        public OptionItem[] options = new OptionItem[4];
        public string idCorrectOption;
        public int idTask;
        public string retroalimentacion;
    }
    public class QuestionControllerVP : MonoBehaviour
    {
        #region Variables

        [SerializeField] private QuestionInformationVP _questionInformation;
        [SerializeField] private UnityEvent _onSelectOption;
        [SerializeField] private UnityEvent _onCorrectOption;
        [SerializeField] private UnityEvent _onIncorrectOption;
        [SerializeField] private UnityEvent _onEndQuestions;
        [SerializeField] private UnityEvent _onNextQuestion;
        [SerializeField] private UnityEvent _onPreviousQuestion;
        [SerializeField] private DataToRegisterSO _toRegisterSo;
        [SerializeField] private List<QuestionDataVP> _session = new List<QuestionDataVP>();
        private QuestionDataVP _currentQuestion;
        private List<int> _indexes = new List<int>(){0,1,2,3} ;
        private int _currentIndex;
        private int _numberOfCorrectQuestions;

        public int GetCountSession
        {
            get => _session.Count;
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                if (CurrentIndex <_session.Count && CurrentIndex >= 0)
                {
                    CurrentQuestion = _session[CurrentIndex];                    
                }

            }
        }

        public QuestionDataVP CurrentQuestion
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
       
        #endregion

        #region Methods

        public void SetData(QuestionItem[] questions)
        {
            for (int i = 0; i < questions.Length; i++)
            {
                
                QuestionDataVP questionData = new QuestionDataVP();
                questionData.retroalimentacion = questions[i].pregunta.retroalimentacion;
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

                _session.Add( questionData);
                _indexes.Add(0);
                _indexes.Add(1);
                _indexes.Add(2);
                _indexes.Add(3);
                questionData.idTask = questions[i].idSimuladorPmpTarea;
            }
        }
        public void ConfigurateQuestion()
        {
            _currentQuestion = _session[CurrentIndex];
            // _questionInformation.SetData(_currentQuestion);
            // _currentIndex++;
        }
        
        public QuestionDataVP GetNextQuestion()
        {            
            if (CurrentIndex + 1 == _session.Count)
            {
                // GameEvents.GameWon?.Invoke();
                _onEndQuestions?.Invoke();
                // UIEvents.ShowLoadingView?.Invoke();
                return null;
            }
            var tempQuestion = _session[CurrentIndex + 1];
            // CurrentIndex++;
            // _onNextQuestion?.Invoke();
            return tempQuestion;
        }

        public QuestionDataVP GetPreviousQuestion()
        {
            if (CurrentIndex - 1 < 0)
            {
                return null;
            }
            var tempQuestion = _session[CurrentIndex - 1];
            _onPreviousQuestion?.Invoke();            
            return tempQuestion;
        }
        public QuestionDataVP[] GetQuestions(int index)
        {
            QuestionDataVP[] questionList = new QuestionDataVP[3];
            if (index == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    questionList[i] = _session[CurrentIndex + i];
                }
            }
            return questionList;
        }

        public void SetCurrentQuestionProgress()
        {
        }
        // public bool CompareResponse(string id)
        // {
        //     return _currentQuestion.idCorrectOption == id;
        // }

        // public void ReturnTrueOption()
        // {
        //     var idCorrectOption = _currentQuestion.idCorrectOption;
        //     if (_questionInformation.Opt1.ID == idCorrectOption)
        //     {
        //         ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt1.gameObject,
        //             new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        //         return;
        //     }
        //     if (_questionInformation.Opt2.ID == idCorrectOption)
        //     {               
        //         ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt2.gameObject,
        //             new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        //         return;
        //     }
        //     if (_questionInformation.Opt3.ID == idCorrectOption)
        //     {               
        //         ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt3.gameObject,
        //             new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        //         return;
        //     }
        //     ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt4.gameObject,
        //         new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        //     return;
        // }

        public bool IsLastQuestion()
        {
            return CurrentIndex + 1 >= _session.Count;
        }

        public void SendGameLost()
        {
            GameEvents.GameLost?.Invoke();
        }

        private void OnDrawGizmos()
        {
            // if (_currentQuestion == null)
            // {
            //     return;
            // }
            // Gizmos.color = Color.green;
            // if (_questionInformation.Opt1.ID == _currentQuestion.idCorrectOption)
            // {
            //     Gizmos.DrawSphere(_questionInformation.Opt1.transform.position, 15f);
            //     return;
            // }
            // if (_questionInformation.Opt2.ID == _currentQuestion.idCorrectOption)
            // {
            //     Gizmos.DrawSphere(_questionInformation.Opt2.transform.position, 15f);
            //     return;
            // }
            // if (_questionInformation.Opt3.ID == _currentQuestion.idCorrectOption)
            // {
            //     Gizmos.DrawSphere(_questionInformation.Opt3.transform.position, 15f);
            //     return;
            // }
            // Gizmos.DrawSphere(_questionInformation.Opt4.transform.position, 15f);
        }

        #endregion

    }

}