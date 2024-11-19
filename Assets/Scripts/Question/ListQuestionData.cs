using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Question
{
    [Serializable]
    public class Question
    {
        public string text;
    }
    [Serializable]
    public class RootQuestion
    {
        public string category;
        public string id;
        public string correctAnswer;
        public List<string> incorrectAnswers;
        public Question question;
        public List<string> tags;
        public string type;

        public string difficulty;
        // public bool isNiche { get; set; }
    }
    [Serializable]
    public class ListQuestionData
    {
        public List<RootQuestion> questions;
    }

}