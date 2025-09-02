using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordInput : InputBase
{
    protected override void Start()
    {
        base.Start();
    }
    
    public override void CheckNullField()
    {
        if (_inputField.text == string.Empty)
        {
            // Dejo campos vacios
            SetAppearanceToError();
            _placeholderText.text = _placeholderTextDefault;

        }
        else
        {
            if (!haveError)
            {
                _inputField.image.sprite = _spriteDefault;
            }
            
        }
    }

    public override void ComprobeFormat(string message)
    {
        if (!IsEmptyField(message))
        {
            SetAppearanceToNormal();
            haveError = false;

        }
    }

}
