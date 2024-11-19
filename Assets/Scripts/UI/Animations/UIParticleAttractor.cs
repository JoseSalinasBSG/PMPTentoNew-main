using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[ExecuteAlways]
public class UIParticleAttractor : MonoBehaviour
{
    public enum Movement
    {
        Linear,
        Smooth,
        Sphere
    }
    
    public enum UpdateMode
    {
        Normal,
        UnscaledTime
    }

    [SerializeField] private ParticleSystem _particleSystem;
    [Range(.1f, 10f)] [SerializeField] private float _destinationRadius = 1;
    [Range(0f, .95f)] [SerializeField] private float _delayRate = 1;
    [Range(.001f, 100f)] [SerializeField] private float _maxSpeed = 10;
    [SerializeField] private Movement _movement;
    [SerializeField] private UpdateMode _updateMode;
    [SerializeField] private UnityEvent _onAttractacted;
    private UIParticle _uiParticle;

    private void OnEnable()
    {
        ApplyParticleSystem();
        UIParticleUpdater.Register(this);
    }

    private void OnDisable()
    {
        UIParticleUpdater.Unregister(this);
    }

    private void ApplyParticleSystem()
    {
        _uiParticle = null;
        if (_particleSystem == null)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                Debug.LogError("No particle system attached to particle attractor script", this);
            }

            return;
        }

        _uiParticle = _particleSystem.GetComponentInParent<UIParticle>(true);
        if (_uiParticle && !_uiParticle.particles.Contains(_particleSystem))
        {
            _uiParticle = null;
        }
    }

    internal void Attract()
    {
        if (_particleSystem == null) return;
        var count = _particleSystem.particleCount;
        if (count == 0) return;
        var particles = ParticleSystemExtensions.GetParticleArray(count);
        _particleSystem.GetParticles(particles,count);
        var dstPos = GetDestinationPosition();
        for (int i = 0; i < count; i++)
        {
            var p = particles[i];
            if (0f < p.remainingLifetime && Vector3.Distance(p.position, dstPos) < _destinationRadius)
            {
                p.remainingLifetime = 0;
                particles[i] = p;

                if (_onAttractacted != null)
                {
                    try
                    {
                        _onAttractacted?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                continue;
            }
            var delayTime = p.startLifetime * _delayRate;
            var duration = p.startLifetime - delayTime;
            var time = Mathf.Max(0, p.startLifetime - p.remainingLifetime - delayTime);

            if (time <= 0 )
            {
                continue;
            }

            p.position = GetAttractedPosition(p.position, dstPos, duration, time);
            p.velocity *= .5f;
            particles[i] = p;
        }
        _particleSystem.SetParticles(particles, count);
        
    }

    private Vector3 GetAttractedPosition(Vector3 current, Vector3 target, float duration, float time)
    {
        var speed = _maxSpeed;
        switch (_updateMode)
        {
            case UpdateMode.Normal:
                speed *= 60 * Time.deltaTime;
                break;
            case UpdateMode.UnscaledTime:
                speed *= 60 * Time.unscaledDeltaTime;
                break;
        }

        switch (_movement)
        {
            case Movement.Linear:
                speed /= duration;
                break;
            case Movement.Smooth:
                target = Vector3.Lerp(current, target, time / duration);
                break;
            case Movement.Sphere:
                target = Vector3.Slerp(current, target, time / duration);
                break;
        }

        return Vector3.MoveTowards(current, target, speed);
    }

    private Vector3 GetDestinationPosition()
    {
        var isUI = _uiParticle && _uiParticle.enabled;
        var psPos = _particleSystem.transform.position;
        var attractorPos = transform.position;
        var dstPos = attractorPos;
        var isLocalSpace = _particleSystem.IsLocalSpace();

        if (isLocalSpace)
        {
            dstPos = _particleSystem.transform.InverseTransformPoint(dstPos);
        }

        if (isUI)
        {
            var inverseScale = _uiParticle.parentScale.Inverse();
            var scale3D = _uiParticle.scale3DForCalc;
            dstPos = dstPos.GetScaled(inverseScale, scale3D.Inverse());

            if (_uiParticle.positionMode == UIParticle.PositionMode.Relative)
            {
                var diff = _uiParticle.transform.position - psPos;
                diff.Scale(scale3D - inverseScale);
                diff.Scale(scale3D.Inverse());
                dstPos += diff;
            }
#if UNITY_EDITOR
            if (!Application.isPlaying && !isLocalSpace)
            {
                dstPos += psPos - psPos.GetScaled(inverseScale, scale3D.Inverse());
            }
#endif
        }

        return dstPos;
    }
    private void OnDestroy()
    {
        _uiParticle = null;
        _particleSystem = null;
    }
    
}
