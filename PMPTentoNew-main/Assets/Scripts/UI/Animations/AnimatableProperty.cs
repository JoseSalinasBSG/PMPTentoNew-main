using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class AnimatableProperty : ISerializationCallbackReceiver
{
    public enum ShaderPropertyType
    {
        Color,
        Vector,
        Float,
        Range,
        Texture
    }

    [SerializeField] private string _name = "";
    [SerializeField] private ShaderPropertyType _type = ShaderPropertyType.Vector;

    public int Id
    {
        get;
        private set;
    }

    public ShaderPropertyType Type
    {
        get => _type;
        set => _type = value;
    }
    
    
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Id = Shader.PropertyToID(_name);
    }

    public void UpdateMaterialProperties(Material material, MaterialPropertyBlock mpb)
    {
        if (!material.HasProperty(Id))
        {
            return;
        }

        switch (Type)
        {
            case ShaderPropertyType.Color:
                var color = mpb.GetColor(Id);
                if (color != default)
                {
                    material.SetColor(Id, color);
                }
                break;
            case ShaderPropertyType.Vector:
                var vector = mpb.GetVector(Id);
                if (vector != default)
                {
                    material.SetVector(Id, vector);
                }
                break;
            case ShaderPropertyType.Float:
            case ShaderPropertyType.Range:
                var value = mpb.GetFloat(Id);
                if (!Mathf.Approximately(value, 0))
                {
                    material.SetFloat(Id, value);
                }
                break;
            case ShaderPropertyType.Texture:
                var texture = mpb.GetTexture(Id);
                if (texture != default(Texture))
                {
                    material.SetTexture(Id, texture);
                }
                break;
        }
        
    }
}
