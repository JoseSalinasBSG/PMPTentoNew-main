using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KeyboardController : MonoBehaviour
{
    private TMP_InputField _inputField;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _relativeRect;
    private RectTransform _inputFieldRectTransform;
    private RectTransform _scrollRectTransform;

    
    private float _keyboardHeight;
    private float _timeToMoveCanvasWithKeyboard;
    private float _timeToMoveCanvasWithoutKeyboard;
    private float _movementDuration = .3f;
    private Vector3[] _scrollCoorners = new Vector3[4];


    // private TouchScreenKeyboard _keyboard;
    private InputBase _inputFieldManager;
    private void Start()
    {
// #if UNITY_ANDROID
//         TouchScreenKeyboard.Android.consumesOutsideTouches = true;
// #endif
        _scrollRectTransform = _scrollRect.GetComponent<RectTransform>();
        // TouchScreenKeyboard.hideInput = false;
    }

    public void SetInputField(TMP_InputField rectTransform)
    {
        _inputFieldRectTransform = rectTransform.GetComponent<RectTransform>();
        _inputFieldManager = rectTransform.GetComponent<InputBase>();
        _inputField = rectTransform;
        // if (_keyboard != null)
        // {
        //     _keyboard.active = false;
        //     TouchScreenKeyboard.hideInput = true;
        // }
        // Invoke(nameof(GetKeyboard), .3f);
    }

    // public void GetKeyboard()
    // {
    //     if (!_inputFieldManager.IsEmail)
    //     {
    //         _keyboard = TouchScreenKeyboard.Open(_inputField.text, TouchScreenKeyboardType.Default, false, _inputField.multiLine, true,
    //             false, "", _inputField.characterLimit);
    //     }
    //     else
    //     {
    //         _keyboard = TouchScreenKeyboard.Open(_inputField.text, TouchScreenKeyboardType.EmailAddress);
    //     }
    //     _keyboard.text = _inputField.text;
    //     _inputField.onValueChanged.AddListener(Call);
    // }
    // private void Call(string arg0)
    // {
    //     _inputField.caretPosition = 0;
    //     if (_inputFieldManager.IsEmail)
    //     {
    //         _inputFieldManager.ComproveEmailFormat(arg0);
    //         return;
    //     }
    //     _inputFieldManager.ComprovePasswordFormat(arg0);
    // }
// #if UNITY_ANDROID
//     private void LateUpdate()
//     {
//         if (_keyboard != null && _keyboard.status == TouchScreenKeyboard.Status.Visible)
//         {
//             _inputField.text = _keyboard.text;
//         }
//
//         
//     }
// #endif
    private void Update()
    {
        

        if (TouchScreenKeyboard.visible )
        {
            _timeToMoveCanvasWithoutKeyboard = 0;
            // _scrollRect.verticalNormalizedPosition = 0;
#if UNITY_ANDROID
            
            if (_keyboardHeight == 0)
            {
                _keyboardHeight = GetRelativeKeyboardHeight(_relativeRect, true);
            }
#elif UNITY_IOS
            _keyboardHeight = TouchScreenKeyboard.area.height;

#endif
            if( _timeToMoveCanvasWithKeyboard == 0)
            {
                _timeToMoveCanvasWithKeyboard = Time.time;
            }
            var actualOffset = Mathf.SmoothStep(0, _keyboardHeight, (Time.time - _timeToMoveCanvasWithKeyboard) / _movementDuration);
            var positionTemp = new Vector2(_scrollRect.content.anchoredPosition.x, actualOffset);
            
            _scrollRectTransform.offsetMin = positionTemp;
            // _scrollRect.verticalNormalizedPosition += (Screen.height - (_keyboardHeight + _inputFieldRectTransform )/ Screen.height;
            // Debug.Log(_keyboardHeight + " " + _scrollRect.verticalNormalizedPosition + " " + _inputFieldRectTransform.position.y + " " + Screen.height + " " + Screen.safeArea.height + " " + Screen.safeArea.yMin + " " + Screen.safeArea.yMax);
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

            if( _timeToMoveCanvasWithoutKeyboard == 0)
            {
                _timeToMoveCanvasWithoutKeyboard = Time.time;
            }
            var actualOffset = Mathf.SmoothStep(_keyboardHeight, 0, (Time.time - _timeToMoveCanvasWithoutKeyboard) / _movementDuration);
            var positionTemp = new Vector2(_scrollRect.content.anchoredPosition.x, actualOffset);
            _scrollRectTransform.offsetMin =positionTemp;

        }

    }
    
    public static int GetRelativeKeyboardHeight(RectTransform rectTransform, bool includeInput)
    {
        int keyboardHeight = GetKeyboardHeight(includeInput);
        float screenToRectRatio = Screen.height / rectTransform.rect.height;
        float keyboardHeightRelativeToRect = keyboardHeight / screenToRectRatio;
   
        return (int) keyboardHeightRelativeToRect;
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
