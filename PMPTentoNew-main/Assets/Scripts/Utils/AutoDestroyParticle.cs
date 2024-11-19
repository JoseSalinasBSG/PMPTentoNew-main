using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(IStartDestroy());
    }

    IEnumerator IStartDestroy()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        while (ps)
        {
            yield return new WaitForSeconds(.5f);
            if (!ps.IsAlive(true))
            {
                Destroy(gameObject);
                break;
            }
        }
    }
}
