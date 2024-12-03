using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailInput : InputBase
{
    protected override void Start()
    {
        base.Start();
    }

    public override void CheckNullField()
    {
        if (_inputField.text == string.Empty || _inputField.text.Length == 0)
        {
            // Debug.Log("xd");
            // Dejo campos vacios
            SetAppearanceToError();
            _placeholderText.text = _placeholderTextDefault;
            _inputField.text = GetTextFromCache(_inputField.name);
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
        IsEmptyField(message);
        var indexAtSign = message.IndexOf("@", StringComparison.Ordinal);
        if (indexAtSign != -1 && indexAtSign != 0 && indexAtSign != message.Length -1)
        {
            SetAppearanceToNormal();
            _message.gameObject.SetActive(false);
            haveError = false;
        }
        else
        {
            haveError = true;
            SetAppearanceToError();
            _message.text = _constants.errorFormatField;
            _message.gameObject.SetActive(true);
        }
    }
}
