using System;

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
        IsEmptyField(message);
        var indexAtSign = message.IndexOf("@", StringComparison.Ordinal);
        if (indexAtSign != -1 && indexAtSign != 0 && indexAtSign != message.Length -1)
        {
            SetAppearanceToNormal();
            haveError = false;
        }
        else
        {
            haveError = true;
            SetAppearanceToError();
        }
    }
}
