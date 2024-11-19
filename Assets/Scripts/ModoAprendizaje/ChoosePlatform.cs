using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePlatform : MonoBehaviour
{
    public void Choose()
    {
        GameEvents.ChosenPlatform?.Invoke(transform.position);
    }
}
