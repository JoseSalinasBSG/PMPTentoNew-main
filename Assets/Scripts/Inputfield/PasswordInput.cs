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
        if (_inputField.text == "")
        {
            // Dejo campos vacios
            SetAppearanceToError();
            _placeholderText.text = _placeholderTextDefault;
            _label.gameObject.SetActive(false);
            _message.gameObject.SetActive(true);
            _message.text = _constants.fieldEmpty;

        }
        else
        {
            if (!haveError)
            {
                _message.gameObject.SetActive(false);
                _inputField.image.sprite = _spriteDefault;
            }
            
        }
    }

    public override void ComprobeFormat(string message)
    {
        if (!IsEmptyField(message))
        {
            SetAppearanceToNormal();
            _message.gameObject.SetActive(false);
            haveError = false;

        }
    }

}
