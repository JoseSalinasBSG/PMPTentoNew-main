using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Domain
{
    public int id;
    public string nombre;
}
[Serializable]
public class DomainDetail
{
    public int id;
    public string description;
}

[Serializable]
public class Task
{
    public int id;
    public string nombre;
    public int idSimuladorPmpDominio;
}

[Serializable]
public class Domains
{
    public Domain[] listaDominio;
    public Task[] listaTarea;
}
[CreateAssetMenu(menuName = "Domain and Task", fileName = "DomainAndTast_Data")]
public class DomainsAndTaskSO : ScriptableObject
{
    public DomainDetail[] DomainDetail; 
    public Domains DomainContainer;

    private void OnEnable()
    {
        //Debug.Log("Enable domains and tasks");
        GameEvents.DomainsRetreived += GameEvents_DomainRetreived;
    }

    private void OnDisable()
    {
        //Debug.Log("Disable domains and tasks");
        GameEvents.DomainsRetreived += GameEvents_DomainRetreived;
    }

    private void GameEvents_DomainRetreived(Domains obj)
    {
        DomainContainer = obj;
        GameEvents.DomainsSaved?.Invoke(PMPScenes.DomainViewInCategoryMode);
    }
}
