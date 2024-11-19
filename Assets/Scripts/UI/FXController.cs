using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    [SerializeField] private UIParticle _uiParticle;
    private GameObject _particlePrefab;

    private GameObject _particleInstantiated;
    [SerializeField] private Canvas canvas;

    public void SetUIParticle(Transform uiParticle)
    {
        _uiParticle.transform.position = uiParticle.position;
    }

    public void SetParticlePrefab(GameObject particlePrefab)
    {
        _particlePrefab = particlePrefab;
    }
    public void SpawnParticle()
    {
        ClearParticles();
        _particleInstantiated = Instantiate(_particlePrefab);
        _particleInstantiated.transform.position = new Vector3(0, _particleInstantiated.transform.position.y, 0);
        _particleInstantiated.SetActive(true);
        _particleInstantiated.transform.localScale = Vector3.one;
        _uiParticle.SetParticleSystemInstance(_particleInstantiated);
    }

    public void ClearParticles()
    {
        foreach (Transform child in _uiParticle.transform)
        {
            Destroy(child.gameObject);
        }
        
    }
    
    public void SetCanvasRenderOverlay(bool enable)
    {
        if (enable)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        else
        {
            canvas.worldCamera = Camera.main;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.planeDistance = 5;
        }
    }
}
