using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filters : MonoBehaviour
{
    [SerializeField] private GameObject buttonFilter;

    public void Toggle()
    {
        buttonFilter.SetActive(!buttonFilter.activeSelf);  
    }

}
