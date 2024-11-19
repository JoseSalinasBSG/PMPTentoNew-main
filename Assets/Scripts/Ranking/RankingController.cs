using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using static DataUserAll;

public class RankingController : MonoBehaviour
{
    [SerializeField] private DataUserAll dataUserAll;
    [SerializeField] private List<PodiumItem> _podio;
    [SerializeField] private Transform _rankingContainer;
    [SerializeField] private PodiumItem _rankingItemPrefab;

    private void OnEnable()
    {
        GameEvents.RankingRetrieved += GameEvents_RankingRetrieved;
    }

    private void OnDisable()
    {
        GameEvents.RankingRetrieved -= GameEvents_RankingRetrieved;
    }

    public void GameEvents_RankingRetrieved()
    {

        List<DataUsers> listDataUserAll = dataUserAll.Users;//accediendo a lista de SO DataUserAll

        listDataUserAll = OrderPositions(listDataUserAll);//devuelve lista ordenada

        DataUsers infoUsers;
        if (listDataUserAll.Count > 0)
        {
            infoUsers = listDataUserAll[0];//posicion 1 
            _podio[0].SetDataPodio(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id,infoUsers.spriteAvatarUser);//seteamos data en la posicion 1 del podio
            if (listDataUserAll.Count > 1)
            {
                Debug.Log("mas de 1");
                infoUsers = listDataUserAll[1];//posicion 2
                _podio[1].SetDataPodio(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id,infoUsers.spriteAvatarUser);//seteamos data en la posicion 2 del podio
                if (listDataUserAll.Count > 2)
                {
                    infoUsers = listDataUserAll[2];//posicion 3
                    _podio[2].SetDataPodio(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id, infoUsers.spriteAvatarUser);//seteamos data en la posicion 3 del podio
                }
                else
                {
                    Debug.Log("else1");
                    _podio[2].SetDataPodio("", "-", -1,null);
                }
            }
            else
            {
                Debug.Log("else2");
                _podio[1].SetDataPodio("", "-", -1, null);
                _podio[2].SetDataPodio("", "-", -1, null);
            }
        }
        else
        {
            Debug.Log("else3");
            _podio[0].SetDataPodio("", "-", -1, null);
            _podio[1].SetDataPodio("", "-", -1, null);
            _podio[2].SetDataPodio("", "-", -1, null);
        }
        foreach (Transform child in _rankingContainer)//eliminar lo que tiene el contenedor
        {
            Destroy(child.gameObject);
        }
        for (int i = 4; i < listDataUserAll.Count; i++)//correra a partir de la cuarta posicion
        {
            var item = Instantiate(_rankingItemPrefab, _rankingContainer);
            infoUsers = listDataUserAll[i];

            item.SetData(i.ToString(), infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id, infoUsers.spriteAvatarUser);
        }

    }

    public List<DataUsers> OrderPositions(List<DataUsers> toOrder)//metodo para ordenar una lista del tipo DataUsers, retorna lo mismo
    {
        var listOrdered = toOrder.OrderByDescending(d => d.totalExperience);//se ordena descendentemente en base al parametro de experiencia
        return listOrdered.ToList();//convirtiendo el IEnumerable a List
         

    }
}
