using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PMPService : MonoBehaviour
{
    [SerializeField] private DomainsAndTaskSO _domainsAndTaskSo;
    [SerializeField] private DataToRegisterSO _dataToRegisterSo;

    private void OnEnable()
    {
        GameEvents.ExamObtained += GameEvents_ExamObtained;
    }
    private void OnDisable()
    {
        GameEvents.ExamObtained -= GameEvents_ExamObtained;
    }

    public (string, string) Service_GetDomainAndTaskNames(int idTask)
    {
        var domAndTask =_domainsAndTaskSo.DomainContainer.listaTarea.FirstOrDefault(x => x.id == idTask);
        if (domAndTask != null)
        {
            return (
                _domainsAndTaskSo.DomainContainer.listaDominio
                    .FirstOrDefault(x => x.id == domAndTask.idSimuladorPmpDominio)!.nombre, domAndTask.nombre);
        }
        else
        {
            return (null, null);
        }
    }

    private void GameEvents_ExamObtained(int obj)
    {
        Service_GetQuestions(obj);
    }

    #region Service 

    public void Service_GetDomainAndTasks()
    {
        StartCoroutine(IGetDomainAndTask());
    }

    public void Service_GetTask(int idDomain)
    {
        var list = _domainsAndTaskSo.DomainContainer.listaTarea.Select(x => x).Where(x => x.idSimuladorPmpDominio == idDomain).ToList();
        GameEvents.TaskRetreived?.Invoke(list);
        GameEvents.GetIdDomain?.Invoke(idDomain);
    }

    public void Service_GetExam()
    {
        StartCoroutine(IRegisterExam());
    }
    public void Service_GetExamToTrainingChallenge()
    {
        StartCoroutine(IRegisterExam());
    }
    public void Service_GetQuestions(int idExam)
    {
        StartCoroutine(IGetQuestions(idExam.ToString()));

    }

    #endregion

    IEnumerator IGetQuestions(string idExam)
    {
        UnityWebRequest request = UnityWebRequest.Get(PMPUrl.GetQuestions + idExam);
        AddHeader(request);
       
        yield return request.SendWebRequest();
        if (request.responseCode >= 400)
        {
            Debug.Log("error " + request.error);
        }
        else
        {
            var response = request.downloadHandler.text;
            var questions = JsonUtility.FromJson<QuestionInformationExam>(response);
            GameEvents.QuestionsRetrieved?.Invoke(questions);
            Debug.Log("success " + response );
        }
    }
    
    IEnumerator IRegisterExam()
    {
        UnityWebRequest request = new UnityWebRequest(PMPUrl.RegisterExam, "POST");
        
        
        var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(_dataToRegisterSo.dataToRegisterExam));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
            
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
        yield return request.SendWebRequest();
        
        if (request.responseCode >= 400)
        {
            Debug.Log("error " + request.error);
        }
        else
        {
            var response = request.downloadHandler.text;
            Debug.Log(response);
            var domains = JsonUtility.FromJson<ResponseOfRegisterExam>(response);
            GameEvents.ExamCreated?.Invoke(domains);
        }
    }
IEnumerator IGetDomainAndTask()
    {
        UnityWebRequest request = UnityWebRequest.Get(PMPUrl.GetDomainsAndTask);
        AddHeader(request);
        yield return request.SendWebRequest();
        if (request.responseCode >= 400)
        {
            Debug.Log("error " + request.error);
        }
        else
        {
            var response = request.downloadHandler.text;
            var domains = JsonUtility.FromJson<Domains>(response);
            GameEvents.DomainsRetreived?.Invoke(domains);
        }
    }

    private void AddHeader(UnityWebRequest request)
    {
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
    }
}
