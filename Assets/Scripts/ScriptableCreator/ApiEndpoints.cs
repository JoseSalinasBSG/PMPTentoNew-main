using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ScriptableCreator
{

    [CreateAssetMenu(fileName = "ApiEndpoints", menuName = "ScriptableCreator/ApiEndpoints")]
    public class ApiEndpoints : ScriptableObject
    {
        [Header("EndPoints")]
        public string updateGamificationFeatures;
        public string getStudioComboMode;
        public string registerExam;
        public string getExamDetail;
        public string registerStudentArchivements;
        public string authentication;
        public string getGamificationFeatures;
        public string credentialPortalPmp;
        public string getStudentAchievementsById;

        [Header("EndPoints Test")]
        public string base_url;

        public string getDomains;
        public string getDomainById;
        public string getChapter;
        public string getChapterById;
        public string getQuestions;
        public string getQuestionById;
    }
}