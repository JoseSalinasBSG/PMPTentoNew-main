using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RegisterExam
{
    public int Id;
    public int IdSimuladorPmpModo = 1;
    public string NombreExamen;
    public int Tiempo;
    public string IdAspNetUsers;
    public string Usuario;
    public object EstadoExamen;
    public object Puntaje;
    public object Desempenio;
    public object Percentil;
    public int IdSimuladorPmpTarea;
    public int IdSimuladorPmpDominio;
}

[Serializable]
public class ResponseOfRegisterExam
{
    public int idSimuladorPmpModo;
    public string nombreExamen;
    public int tiempo;
    public string fechaInicio;
    public string idAspNetUsers;
    public int estadoExamen;
    public object puntaje;
    public object desempenio;
    public object percentil;
    public object progreso;
    public object numeroIntento;
    public int id;
    public string fechaCreacion;
    public string fechaModificacion;
    public bool estado;
    public string usuarioCreacion;
    public string usuarioModificacion;
    public int IdSimuladorPmpDominio;
    public object rowVersions;
}

[Serializable]
public class QuestionInformationExam
{
    public int id;
    public string nombreExamen;
    public int tiempo;
    public string fechaInicio;
    public string idAspNetUsers;
    // public object usuario;
    public int estadoExamen;
    // public object puntaje;
    // public object desempenio;
    // public object percentil;
    public int preguntasRespondidas;
    public int preguntasPendientes;
    public QuestionItem[] listaPreguntas;
}

[Serializable]
public class QuestionItem
{
    public int id;
    public int idSimuladorPmpDominio;
    public int idSimuladorPmpTarea;
    public int idSimuladorPmpPregunta;
    public bool ejecutado;
    // public object idSimuladorPmpPreguntaRespuesta;
    // public object puntaje;
    public QuestionAndOptions pregunta;
    public string dominioNombre;
    public string tareaNombre;
    // public object nombreExamen;
    
}

[Serializable]
public class QuestionAndOptions
{
    public int id;
    public string enunciado;
    public int idSimuladorTipoRespuesta;
    public bool tieneRetroalimentacion;
    public string urlRetroalimentacionVideo;
    public string retroalimentacion;
    // public object urlImagenPregunta;
    // public object idPmpTipoPreguntaClasificacion;
    public bool activo;
    public OptionItem[] respuesta;
}

[Serializable]
public class OptionItem
{
    public int id;
    public string respuesta;
    public int valor;
    public string correcto;
    public int respuestaSelecionada;
}
[CreateAssetMenu(fileName = "ToRegister_Data", menuName = "Data to register")]
public class DataToRegisterSO : ScriptableObject
{
    public RegisterExam dataToRegisterExam;
    public ResponseOfRegisterExam responseOfRegisterExam;
    public QuestionInformationExam questionInformation; 
    public void OnEnable()
    {
        //Debug.Log("On enable Data to register");
        GameEvents.GetIdDomain += GameEvents_GetIdDomain;
        GameEvents.GetIdTask += GameEvents_GetIdTask;
        GameEvents.GetNameExam += GameEvents_GetNameExam;
        GameEvents.GetUserExam += GameEvents_GetUserExam;
        GameEvents.ExamCreated += GameEvents_ExamCreated;
        GameEvents.QuestionsRetrieved += GameEvents_QuestionsRetrieved;
    }

    private void GameEvents_QuestionsRetrieved(QuestionInformationExam obj)
    {
        questionInformation = obj;
        GameEvents.QuestionReady?.Invoke();
        
    }

    private void GameEvents_ExamCreated(ResponseOfRegisterExam obj)
    {
        responseOfRegisterExam = obj;
        GameEvents.ExamObtained?.Invoke(responseOfRegisterExam.id); 
    }

    private void OnDisable()
    {
        //Debug.Log("On disable Data to register");
        GameEvents.GetIdDomain -= GameEvents_GetIdDomain;
        GameEvents.GetIdTask -= GameEvents_GetIdTask;
        GameEvents.GetNameExam -= GameEvents_GetNameExam;
        GameEvents.GetUserExam -= GameEvents_GetUserExam;
        GameEvents.ExamCreated -= GameEvents_ExamCreated;
        GameEvents.QuestionsRetrieved -= GameEvents_QuestionsRetrieved;

    }

    private void GameEvents_GetIdTask(int obj)
    {
        dataToRegisterExam.IdSimuladorPmpTarea = obj;
    }
    private void GameEvents_GetIdDomain(int obj)
    {
        dataToRegisterExam.IdSimuladorPmpDominio = obj;
    }
    private void GameEvents_GetUserExam(string obj)
    {
        dataToRegisterExam.Usuario = obj;
    }
    private void GameEvents_GetNameExam(string obj)
    {
        dataToRegisterExam.NombreExamen = obj;
    }

}
