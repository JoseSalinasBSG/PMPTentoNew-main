using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class InputBase : MonoBehaviour
{
#region serializeFields variables
    [SerializeField] protected TMP_InputField _inputField;
    [SerializeField] protected Constants _constants;
    [SerializeField] protected TextMeshProUGUI _label;
    [SerializeField] protected TextMeshProUGUI _message;
    [SerializeField] private Image _image;
    [SerializeField] protected Sprite _spriteError;
    [SerializeField] protected Sprite _spriteDefault;
    [SerializeField] protected Sprite _spriteSelect;    
    [SerializeField] protected string _placeholderTextDefault;
   
    #endregion

    #region private variables

    protected TextMeshProUGUI _placeholderText;
    protected bool haveError;
    protected bool hasTextOnCache;
    protected Dictionary<string,string> _textCache;

    #endregion

    #region public variables


    public TMP_InputField InputField
    {
        get => _inputField;
        set => _inputField = value;
    }

    public bool HaveError
    {
        get => haveError;
        set => haveError = value;
    }

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    protected virtual void Start()
    {
        haveError = true;
        hasTextOnCache = false;
        // _inputField = GetComponent<TMP_InputField>();
        _placeholderText = _inputField.placeholder.GetComponent<TextMeshProUGUI>();
        _textCache = new Dictionary<string, string>();
        _placeholderText.text = _textCache.GetValueOrDefault(_inputField.name, _placeholderTextDefault);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected void OnEnable()
    {
        _inputField.onEndEdit.AddListener(OnInputFieldFocusLost);
        _inputField.onValueChanged.AddListener(OnInputFieldTextChanged);
        Application.focusChanged += OnApplicationFocus;
    }


    protected void OnDisable()
    {
        _inputField.onEndEdit.RemoveListener(OnInputFieldFocusLost);
        _inputField.onValueChanged.RemoveListener(OnInputFieldTextChanged);
        Application.focusChanged -= OnApplicationFocus;
    }
    private void OnInputFieldFocusLost(string arg0)
    {        
        SaveTextOnCache(_inputField.text);
    }

    private void OnInputFieldTextChanged(string arg0)
    {
        SaveTextOnCache(_inputField.text);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveTextOnCache(_inputField.text);
        }
        else
        {
            _inputField.text = GetTextFromCache(_inputField.name);
            // _placeholderText.text = GetTextFromCache(_inputField.name);
        }
    }
    
    protected void SaveTextOnCache(string arg0)
    {
        if (arg0 == string.Empty)
        {
            return;
        }
        _textCache[_inputField.name] = arg0;
    }
    
    protected string GetTextFromCache(string key)
    {
        return _textCache[key];
    }

    #endregion

    #region public Methods

    public abstract void CheckNullField();
    public abstract void ComprobeFormat(string message);
    // public void CheckNullField()
    // {
    //     if (_inputField.text == "")
    //     {
    //         // Dejo campos vacios
    //         SetErrorVisual();
    //         _placeholderText.text = _placeholderTextDefault;
    //         _label.gameObject.SetActive(false);
    //         _message.gameObject.SetActive(true);
    //         _message.text = Constants.emailFieldEmpty;
    //             
    //     }
    //     else
    //     {
    //         if (!haveError)
    //         {
    //             _message.gameObject.SetActive(false);
    //             _inputField.image.sprite = _spriteDefault;
    //         }
    //         
    //     }
    // }

    public void ShowLabel()
    {
        _label.gameObject.SetActive(true);
        _placeholderText.text = "";
    }
    public void ResetColorText()
    {
        _inputField.textComponent.color = Color.black;
        _placeholderText.color = Color.black;
    }

    // public void ComprovePasswordFormat(string textToComporove)
    // {
    //     if (!IsEmptyField(textToComporove))
    //     {
    //         SetNormalVisual();
    //         _message.gameObject.SetActive(false);
    //         haveError = false;
    //
    //     }
    // }

    // public bool IsEmptyField(string textToComporove)
    // {
    //     if (textToComporove.Length == 0)
    //     {
    //         SetErrorVisual();
    //         _message.gameObject.SetActive(true);
    //         if (_isEmail)
    //         {
    //             _message.text = Constants.emailFieldEmpty;
    //         }
    //         else
    //         {
    //             _message.text = Constants.passwordFieldEmpty;
    //         }
    //         return true;
    //     }
    //     return false;
    // }
    // public void ComproveEmailFormat(string textToComporove)
    // {
    //     IsEmptyField(textToComporove);
    //     var indexAtSign = textToComporove.IndexOf("@", StringComparison.Ordinal);
    //     if (indexAtSign != -1 && indexAtSign != 0 && indexAtSign != textToComporove.Length -1)
    //     {
    //         SetNormalVisual();
    //         _message.gameObject.SetActive(false);
    //         haveError = false;
    //     }
    //     else
    //     {
    //         haveError = true;
    //         SetErrorVisual();
    //         _message.text = Constants.errorFormatEmailField;
    //         _message.gameObject.SetActive(true);
    //     }
    // }

    public void SetAppearanceToError()
    {
        SpriteState spriteState = new SpriteState()
        {
            selectedSprite = _spriteError,
        };
        _inputField.spriteState = spriteState;
        _inputField.image.sprite = _spriteError;
        _inputField.textComponent.color = Color.red;
        _placeholderText.color = Color.red;
        _label.color = Color.white;
        _message.color = Color.white;
    }

    public void SetAppearanceToNormal()
    {
        SpriteState spriteState = new SpriteState()
        {
            selectedSprite = _spriteSelect,
        };
        _inputField.spriteState = spriteState;
        _inputField.image.sprite = _spriteDefault;
        _inputField.textComponent.color = Color.black;
        _label.color = Color.white;
    }
    public bool IsEmptyField(string textToComporove)
    {
        if (textToComporove.Length == 0)
        {
            SetAppearanceToError();
            _message.gameObject.SetActive(true);
            _message.text = _constants.fieldEmpty;
            _placeholderText.text = _placeholderTextDefault;
            _label.gameObject.SetActive(false);
            return true;
        }
        else
        {
            _label.gameObject.SetActive(true);
 
        }
        return false;
    }
    #endregion
}
