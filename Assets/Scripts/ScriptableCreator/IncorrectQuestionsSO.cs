using System;
using System.Collections.Generic;
using UnityEngine;

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
            if (questions.IncorrectQuestionsList.Exists(x => x.id == questionItem.id))
            {
                return;
            }
            questions.IncorrectQuestionsList.Add(questionItem);   
            PlayerPrefs.SetString("IncorrectQuestions", JsonUtility.ToJson( questions));
            Debug.Log(PlayerPrefs.GetString("IncorrectQuestions"));
        }
    }
}