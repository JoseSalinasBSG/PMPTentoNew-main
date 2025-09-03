using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class InputBase : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] protected TMP_InputField _inputField;
    [SerializeField] protected Constants _constants;

    [Tooltip("Imagen del Input si necesitas cambiar el sprite actual (opcional).")]
    [SerializeField] private Image _image;

    [Header("Sprites de estado")]
    [SerializeField] protected Sprite _spriteError;
    [SerializeField] protected Sprite _spriteDefault;
    [SerializeField] protected Sprite _spriteSelect;

    [Header("Placeholder")]
    [SerializeField] protected string _placeholderTextDefault;

    // Acepta tanto TMP_Text como TextMeshProUGUI
    protected TMP_Text _placeholderText;
    protected bool haveError;
    protected bool hasTextOnCache;
    protected Dictionary<string, string> _textCache;

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

    /// <summary>
    /// Inicializa referencias críticas ANTES de OnEnable/OnApplicationFocus.
    /// </summary>
    protected virtual void Awake()
    {
        // Asegurar inputField
        if (_inputField == null)
            _inputField = GetComponent<TMP_InputField>();

        // Inicializar cache
        _textCache = new Dictionary<string, string>();

        // Resolver placeholder (acepta TMP_Text o TextMeshProUGUI)
        if (_inputField != null && _inputField.placeholder != null)
        {
            _placeholderText = _inputField.placeholder.GetComponent<TMP_Text>();
            if (_placeholderText == null)
                _placeholderText = _inputField.placeholder.GetComponent<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Setup visual y flags.
    /// </summary>
    protected virtual void Start()
    {
        haveError = true;
        hasTextOnCache = false;

        if (_placeholderText != null && !string.IsNullOrEmpty(_placeholderTextDefault))
        {
            _placeholderText.text = _placeholderTextDefault;
        }
    }

    // No necesitas Update; lo dejamos por si heredan.
    protected virtual void Update() { }

    protected virtual void OnEnable()
    {
        if (_inputField != null)
        {
            _inputField.onEndEdit.AddListener(OnInputFieldFocusLost);
            _inputField.onValueChanged.AddListener(OnInputFieldTextChanged);
        }
    }

    protected virtual void OnDisable()
    {
        if (_inputField != null)
        {
            _inputField.onEndEdit.RemoveListener(OnInputFieldFocusLost);
            _inputField.onValueChanged.RemoveListener(OnInputFieldTextChanged);
        }
    }

    /// <summary>
    /// Llamado automáticamente por Unity cuando cambia el foco de la app.
    /// </summary>
    private void OnApplicationFocus(bool hasFocus)
    {
        if (_inputField == null) return;

        if (!hasFocus)
        {
            // Guarda el texto actual si no está vacío
            SaveTextOnCache(_inputField.text);
        }
        else
        {
            // Al volver a foco: restaura el texto tipeado (si lo tenías cacheado)
            string cached = GetTextFromCache(_inputField.name); // devuelve "" si no existe
            _inputField.text = cached;

            // Placeholder: muestra el texto guía si no hay texto del usuario
            if (_placeholderText != null)
                _placeholderText.text = string.IsNullOrEmpty(cached) ? _placeholderTextDefault : string.Empty;
        }
    }

    private void OnInputFieldFocusLost(string _)
    {
        if (_inputField == null) return;
        SaveTextOnCache(_inputField.text);
    }

    private void OnInputFieldTextChanged(string _)
    {
        if (_inputField == null) return;
        SaveTextOnCache(_inputField.text);
    }

    /// <summary>
    /// Guarda en cache solo si hay contenido; evita claves vacías.
    /// </summary>
    protected void SaveTextOnCache(string value)
    {
        if (_inputField == null) return;

        if (!string.IsNullOrEmpty(value))
        {
            _textCache[_inputField.name] = value;
            hasTextOnCache = true;
        }
        else
        {
            // Si limpian el campo, podemos decidir eliminar la clave
            if (_textCache.ContainsKey(_inputField.name))
                _textCache.Remove(_inputField.name);
        }
    }

    /// <summary>
    /// Obtiene del cache; si no existe, devuelve string.Empty para evitar KeyNotFound.
    /// </summary>
    protected string GetTextFromCache(string key)
    {
        if (_textCache != null && _textCache.TryGetValue(key, out var val))
            return val;

        return string.Empty; // Seguro por defecto
    }

    public abstract void CheckNullField();
    public abstract void ComprobeFormat(string message);

    public void ResetColorText()
    {
        if (_inputField != null && _inputField.textComponent != null)
            _inputField.textComponent.color = Color.black;

        if (_placeholderText != null)
            _placeholderText.color = Color.black;
    }

    public void SetAppearanceToError()
    {
        if (_inputField == null) return;

        var spriteState = new SpriteState
        {
            selectedSprite = _spriteError,
        };
        _inputField.spriteState = spriteState;

        if (_inputField.image != null && _spriteError != null)
            _inputField.image.sprite = _spriteError;

        if (_inputField.textComponent != null)
            _inputField.textComponent.color = Color.red;

        if (_placeholderText != null)
            _placeholderText.color = Color.red;
    }

    public void SetAppearanceToNormal()
    {
        if (_inputField == null) return;

        var spriteState = new SpriteState
        {
            selectedSprite = _spriteSelect,
        };
        _inputField.spriteState = spriteState;

        if (_inputField.image != null && _spriteDefault != null)
            _inputField.image.sprite = _spriteDefault;

        if (_inputField.textComponent != null)
            _inputField.textComponent.color = Color.black;

        if (_placeholderText != null)
            _placeholderText.color = Color.black;
    }

    /// <summary>
    /// Devuelve true si el texto está vacío y setea el estado visual de error.
    /// </summary>
    public bool IsEmptyField(string textToComporove)
    {
        if (string.IsNullOrEmpty(textToComporove))
        {
            SetAppearanceToError();
            if (_placeholderText != null)
                _placeholderText.text = _placeholderTextDefault;
            return true;
        }
        return false;
    }
}
