using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/Button/Animation", fileName = "ButtonAnimationParameters")]
public class ScriptableObjectButton : ScriptableObject
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _timeDuration;
    
    [SerializeField] private AnimationCurve _animationInverseCurve;
    public float TimeDuration
    {
        get => _timeDuration;
        set => _timeDuration = value;
    }

    public AnimationCurve AnimationCurve
    {
        get => _animationCurve;
        set => _animationCurve = value;
    }

    public AnimationCurve AnimationInverseCurve
    {
        get => _animationInverseCurve;
        set => _animationInverseCurve = value;
    }
}
