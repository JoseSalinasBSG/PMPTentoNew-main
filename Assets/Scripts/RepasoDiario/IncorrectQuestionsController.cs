using ScriptableCreator;
using UnityEngine;

///<summary>
/// Controlador que gestiona la recuperación y almacenamiento de preguntas incorrectas.
/// Carga las preguntas incorrectas guardadas en `PlayerPrefs` al habilitar el objeto
/// y las almacena en un objeto ScriptableObject.
///</summary>

public class IncorrectQuestionsController : MonoBehaviour
{
    [SerializeField] private IncorrectQuestionsSO _incorrectQuestions;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("IncorrectQuestions"))
        {
            var stringQuestions = PlayerPrefs.GetString("IncorrectQuestions");
            _incorrectQuestions.questions = JsonUtility.FromJson<IncorrectQuestionsContainer>(stringQuestions);
        }
        else
        {
            _incorrectQuestions.questions = new IncorrectQuestionsContainer();
        }
    }
}
