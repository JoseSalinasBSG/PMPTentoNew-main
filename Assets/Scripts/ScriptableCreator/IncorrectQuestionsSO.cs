using System;
using System.Collections.Generic;
using UnityEngine;

//<summary>
//Scriptable Object que guarda las preguntas incorrectas
//</summary>

namespace ScriptableCreator
{
    [Serializable]
    public class IncorrectQuestionsContainer
    {
        [SerializeField]
        public List<QuestionItem> IncorrectQuestionsList = new List<QuestionItem>();
    }

    [CreateAssetMenu(menuName = "Incorrect Questions", fileName = "IncorrectQuestionSO")]
    public class IncorrectQuestionsSO : ScriptableObject
    {
        public IncorrectQuestionsContainer questions;

    public void SaveIncorrectQuestion(QuestionItem questionItem)
        {
            if (questions.IncorrectQuestionsList.Exists(x => x.idSimuladorPmpPregunta == questionItem.idSimuladorPmpPregunta))
            {
                return;
            }
            questions.IncorrectQuestionsList.Add(questionItem);   
            PlayerPrefs.SetString("IncorrectQuestions", JsonUtility.ToJson( questions));
            Debug.Log(PlayerPrefs.GetString("IncorrectQuestions"));
        }
    }
}