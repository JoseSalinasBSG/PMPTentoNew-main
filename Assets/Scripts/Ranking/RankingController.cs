using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static DataUserAll;

public class RankingController : MonoBehaviour
{
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject rankingWithExtraItemPanel;

    [SerializeField] private DataUserAll dataUserAll;
    [SerializeField] private List<PodiumItem> _podio;
    [SerializeField] private Transform _rankingContainer;
    [SerializeField] private PodiumItem _rankingItemPrefab;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private int currentAmountUsersOnRanking = 0;
    private bool _hasReachedBottom = false;
    [SerializeField] private List<DataUsers> listDataUserAll;
    [SerializeField] private ScriptableObjectUser _userScriptableObject;
    [SerializeField] private Transform _myRankingItemContainer;
    private static readonly Color customColor = new Color(254f / 255f, 218f / 255f, 177f / 255f); // FEDAB1

    
    private const int TOP_USERS = 10;

    private void Awake()
    {
        listDataUserAll = new List<DataUsers>();

        rankingPanel.SetActive(false);
        rankingWithExtraItemPanel.SetActive(false);
    }

    private void OnScrollRectValueChanged(Vector2 arg0)
    {
        // Check if we've reached the bottom (y = 0) and haven't triggered the action yet
        if (arg0.y == 0 && !_hasReachedBottom)
        {
            _hasReachedBottom = true;

            // Execute your one-time method here
            // CreateItemToRankingContainer(listDataUserAll, 10);
        }
        // Reset the flag when the user scrolls away from bottom
        else if (arg0.y > 0)
        {
            _hasReachedBottom = false;
        }
    }

    private void ExecuteOnceOnScrollBottom()
    {

    }

    private void OnEnable()
    {
        _scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        GameEvents.RankingRetrieved += GameEvents_RankingRetrieved;
    }

    private void OnDisable()
    {
        _scrollRect.onValueChanged.RemoveListener(OnScrollRectValueChanged);
        GameEvents.RankingRetrieved -= GameEvents_RankingRetrieved;
    }

    public void GameEvents_RankingRetrieved()
    {
        if (dataUserAll.Users.Count == 0) return;

        if (dataUserAll.Users.Count > TOP_USERS)
        {
            rankingPanel.SetActive(false);
            rankingWithExtraItemPanel.SetActive(true);
            _scrollRect = rankingWithExtraItemPanel.GetComponentInChildren<ScrollRect>();
            var myRankingItem = _myRankingItemContainer.GetComponent<PodiumItem>();
            Debug.Log(myRankingItem.name);
            myRankingItem.SetData(dataUserAll.Users[TOP_USERS].position.ToString(), dataUserAll.Users[TOP_USERS].userName, dataUserAll.Users[TOP_USERS].totalExperience.ToString(), dataUserAll.Users[TOP_USERS].id, dataUserAll.Users[10].spriteAvatarUser);
        }
        else
        {
            rankingWithExtraItemPanel.SetActive(false);
            rankingPanel.SetActive(true);
            _scrollRect = rankingPanel.GetComponentInChildren<ScrollRect>();
        }
        _rankingContainer = _scrollRect.content;

        listDataUserAll = dataUserAll.Users;//accediendo a lista de SO DataUserAll

        listDataUserAll = OrderPositions(listDataUserAll);//devuelve lista ordenada

        DataUsers infoUsers = new DataUsers();//creamos un objeto de tipo DataUsers
        if (listDataUserAll.Count > 0)
        {
            infoUsers = listDataUserAll[0];//posicion 1 
            _podio[0].SetDataPodio(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id, infoUsers.spriteAvatarUser);//seteamos data en la posicion 1 del podio
            currentAmountUsersOnRanking++;
            if (listDataUserAll.Count > 1)
            {
                //Debug.Log("mas de 1");
                infoUsers = listDataUserAll[1];//posicion 2
                _podio[1].SetDataPodio(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id, infoUsers.spriteAvatarUser);//seteamos data en la posicion 2 del podio
                currentAmountUsersOnRanking++;
                if (listDataUserAll.Count > 2)
                {
                    infoUsers = listDataUserAll[2];//posicion 3
                    _podio[2].SetDataPodio(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id, infoUsers.spriteAvatarUser);//seteamos data en la posicion 3 del podio
                    currentAmountUsersOnRanking++;
                }
                else
                {
                    //Debug.Log("else1");
                    _podio[2].SetDataPodio("", "-", -1, null);
                }
            }
            else
            {
                //Debug.Log("else2");
                _podio[1].SetDataPodio("", "-", -1, null);
                _podio[2].SetDataPodio("", "-", -1, null);
            }
        }
        else
        {
            //Debug.Log("else3");
            _podio[0].SetDataPodio("", "-", -1, null);
            _podio[1].SetDataPodio("", "-", -1, null);
            _podio[2].SetDataPodio("", "-", -1, null);
        }
        foreach (Transform child in _rankingContainer)//eliminar lo que tiene el contenedor
        {
            Destroy(child.gameObject);
        }

        //correra a partir de la cuarta posicion
        for (int i = 3; i < TOP_USERS; i++)
        {
            var item = Instantiate(_rankingItemPrefab, _rankingContainer);
            infoUsers = listDataUserAll[i];
            int userId = _userScriptableObject.userInfo.user.idAlumno;

            if (infoUsers.id == userId)
            {
                Image rankingItemImage = item.GetComponentInParent<Image>(true); // Evita buscar por nombre
                Debug.Log(rankingItemImage.name);
                if (rankingItemImage != null)
                {
                    rankingItemImage.color = customColor; // Aplica el color personalizado
                }
            }
            item.SetData(infoUsers.position.ToString(), infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id, infoUsers.spriteAvatarUser);
        }

       
    }
    private void CreateItemToRankingContainer(List<DataUsers> listDataUserAll, int count = 0)
    {
        currentAmountUsersOnRanking += count;
        for (int i = currentAmountUsersOnRanking - 1; currentAmountUsersOnRanking < listDataUserAll.Count; currentAmountUsersOnRanking++)//correra a partir de la cuarta posicion
        {
            var item = Instantiate(_rankingItemPrefab, _rankingContainer);
            var dataUsers = listDataUserAll[currentAmountUsersOnRanking];
            item.SetData(i.ToString(), dataUsers.userName, dataUsers.totalExperience.ToString(), dataUsers.id, dataUsers.spriteAvatarUser);
        }
    }


    public List<DataUsers> OrderPositions(List<DataUsers> toOrder)//metodo para ordenar una lista del tipo DataUsers, retorna lo mismo
    {
        var listOrdered = toOrder.OrderByDescending(d => d.totalExperience);//se ordena descendentemente en base al parametro de experiencia
        return listOrdered.ToList();//convirtiendo el IEnumerable a List   
    }
}
