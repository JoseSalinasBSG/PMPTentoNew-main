using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemValue;
    public bool _haveInformation;

    public bool HaveInformation
    {
        get => _haveInformation;
        set => _haveInformation = value;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetData(int a)
    {
        itemValue = a;
        HaveInformation = true;
    }
}
