using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class TestList : MonoBehaviour
{

    [SerializeField] private List<GameObject> itemsList;
    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 0; i < itemsList.Count; i++)
        {
            itemsList[i].GetComponent<Item>().HaveInformation = false;
        }*/

        GenerateRandomList();
    }


    void GenerateRandomList()
    {
        var list_length = itemsList.Count;
        Debug.Log($"Cantidad de items: {list_length}");
        Random.InitState(Random.Range(Int32.MinValue, Int32.MaxValue));
        for (int i = 0; i < list_length; i++) 
        {
            
            var a=Random.Range(0,itemsList.Count);
            Debug.Log(a);

            while (itemsList[a].GetComponent<Item>().HaveInformation)// Mientras el elemento _rouletteItems[s] tenga informacióna
            {
                a = Random.Range(0, itemsList.Count);//se generará otro índice aleatorio para no sobreescribir la informacion
            }
     
            Item _item=itemsList[a].GetComponent<Item>();

            _item.SetData(a);
                

        }




    }


    
    void Update()
    {
        
    }
}
