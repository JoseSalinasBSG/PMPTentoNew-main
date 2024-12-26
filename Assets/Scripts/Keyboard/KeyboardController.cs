using TMPro;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// Este script controla el desplazamiento de la pantalla y el ajuste del canvas en respuesta al teclado virtual (en dispositivos móviles).
// Permite que el contenido del ScrollRect se desplace para asegurar que el campo de entrada (input field) sea visible cuando el teclado está activo.
// Además, maneja el comportamiento del canvas para ajustar su posición dependiendo de si el teclado está visible o no, asegurando una experiencia de usuario fluida al escribir en los campos de entrada.
// Se toma en cuenta la altura del teclado y el tiempo de desplazamiento del canvas para proporcionar un movimiento suave tanto cuando el teclado aparece como cuando desaparece.
// </summary>

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _relativeRect;
    private RectTransform _inputFieldRectTransform;
    private RectTransform _scrollRectTransform;
    private TMP_InputField _inputField;
    private float _keyboardHeight;
    private float _timeToMoveCanvasWithKeyboard;
    private float _timeToMoveCanvasWithoutKeyboard;
    private float _movementDuration = .3f;
    private Vector3[] _scrollCoorners = new Vector3[4];

    // private TouchScreenKeyboard _keyboard;
    private InputBase _inputFieldManager;
    private void Start()
    {
        _scrollRectTransform = _scrollRect.GetComponent<RectTransform>();
    }

    public void SetInputField(TMP_InputField rectTransform)
    {
        _inputFieldRectTransform = rectTransform.GetComponent<RectTransform>();
        _inputFieldManager = rectTransform.GetComponent<InputBase>();
        _inputField = rectTransform;
    }
    private void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
            _timeToMoveCanvasWithoutKeyboard = 0;
#if UNITY_ANDROID

            if (_keyboardHeight == 0)
            {
                _keyboardHeight = GetRelativeKeyboardHeight(_relativeRect, true);
            }
#elif UNITY_IOS
            _keyboardHeight = TouchScreenKeyboard.area.height;

#endif
            if (_timeToMoveCanvasWithKeyboard == 0)
            {
                _timeToMoveCanvasWithKeyboard = Time.time;
            }
            var actualOffset = Mathf.SmoothStep(0, _keyboardHeight, (Time.time - _timeToMoveCanvasWithKeyboard) / _movementDuration);
            var positionTemp = new Vector2(_scrollRect.content.anchoredPosition.x, actualOffset);

            _scrollRectTransform.offsetMin = positionTemp;
            _scrollRectTransform.GetWorldCorners(_scrollCoorners);
            var diff = _scrollCoorners[0].y - (_inputFieldRectTransform.position.y - _inputFieldRectTransform.rect.height);
            if (diff >= 0)
            {
                _scrollRect.verticalNormalizedPosition -= Time.deltaTime;

            }
        }
        else
        {
            _timeToMoveCanvasWithKeyboard = 0;

            if (_timeToMoveCanvasWithoutKeyboard == 0)
            {
                _timeToMoveCanvasWithoutKeyboard = Time.time;
            }

            var actualOffset = Mathf.SmoothStep(_keyboardHeight, 0, (Time.time - _timeToMoveCanvasWithoutKeyboard) / _movementDuration);
            var positionTemp = new Vector2(_scrollRect.content.anchoredPosition.x, actualOffset);
            _scrollRectTransform.offsetMin = positionTemp;
        }

    }

    public static int GetRelativeKeyboardHeight(RectTransform rectTransform, bool includeInput)
    {
        int keyboardHeight = GetKeyboardHeight(includeInput);
        float screenToRectRatio = Screen.height / rectTransform.rect.height;
        float keyboardHeightRelativeToRect = keyboardHeight / screenToRectRatio;

        return (int)keyboardHeightRelativeToRect * 5;//JS: multiplique por 5 para darle un mayor offset ya que no se mostraban los inputfields
    }
    private static int GetKeyboardHeight(bool includeInput)
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        using (AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            AndroidJavaObject view = unityPlayer.Call<AndroidJavaObject>("getView");
            AndroidJavaObject dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
            if (view == null || dialog == null)
                return 0;
            var decorHeight = 0;
            if (includeInput)
            {
                AndroidJavaObject decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
                if (decorView != null)
                    decorHeight = decorView.Call<int>("getHeight");
            }
            using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                return Screen.height - rect.Call<int>("height") + decorHeight;
            }
        }
#elif UNITY_IOS
        return (int)TouchScreenKeyboard.area.height;
#endif
    }

}
