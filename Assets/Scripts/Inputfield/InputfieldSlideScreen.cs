using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class InputfieldSlideScreen : MonoBehaviour {
 
    // Assign canvas here in editor
    public Canvas canvas;
 
 
    // Used internally - set by InputfieldFocused.cs
    public bool InputFieldActive = false;
    public RectTransform childRectTransform;
    public RectTransform containerHeight;

    // private void Start()
    // {
    //     canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    // }

    // #if UNITY_ANDROID
private void Update()
{
    if(InputFieldActive)
    {
        // var height = GetRelativeKeyboardHeight(childRectTransform, true);
        // Debug.Log("TouchScreenKeyboard.area.height " + TouchScreenKeyboard.area.height);
        // Debug.Log("height " + height);
        
        // transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //
        // Vector3[] corners = {Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero};
        // Rect rect = RectTransformExtension.GetScreenRect(childRectTransform, canvas);
        // float keyboardHeight = containerHeight.rect.height;
        // Debug.Log(keyboardHeight);
        // // float keyboardHeight = height;
        //
        // float heightPercentOfKeyboard = keyboardHeight/Screen.height*100f;
        // float heightPercentOfInput = (Screen.height-(rect.y+rect.height))/Screen.height*100f;
        // if (heightPercentOfKeyboard>heightPercentOfInput)
        // {
        //     // keyboard covers input field so move screen up to show keyboard
        //     float differenceHeightPercent = heightPercentOfKeyboard - heightPercentOfInput;
        //     float newYPos = transform.GetComponent<RectTransform>().rect.height /100f*differenceHeightPercent;
        //
        //     Vector2 newAnchorPosition = Vector2.zero;
        //     newAnchorPosition.y = newYPos;
        //     transform.GetComponent<RectTransform>().anchoredPosition = newAnchorPosition;
        // } else {
        //     // Keyboard top is below the position of the input field, so leave screen anchored at zero
        //     transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        // }
    }
    else
    {
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}

// #endif
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
    
    
#if UNITY_IOS
 
 
    void LateUpdate () {
        if ((InputFieldActive)&&((TouchScreenKeyboard.visible)))
        {
            transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
 
            Vector3[] corners = {Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero};
            Rect rect = RectTransformExtension.GetScreenRect(childRectTransform, canvas);
            float keyboardHeight = TouchScreenKeyboard.area.height;
 
            float heightPercentOfKeyboard = keyboardHeight/Screen.height*100f;
            float heightPercentOfInput = (Screen.height-(rect.y+rect.height))/Screen.height*100f;
 
 
            if (heightPercentOfKeyboard>heightPercentOfInput)
            {
                // keyboard covers input field so move screen up to show keyboard
                float differenceHeightPercent = heightPercentOfKeyboard - heightPercentOfInput;
                float newYPos = transform.GetComponent<RectTransform>().rect.height /100f*differenceHeightPercent;
 
                Vector2 newAnchorPosition = Vector2.zero;
                newAnchorPosition.y = newYPos;
                transform.GetComponent<RectTransform>().anchoredPosition = newAnchorPosition;
            } else {
                // Keyboard top is below the position of the input field, so leave screen anchored at zero
                transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        } else {
            // No focus or touchscreen invisible, set screen anchor to zero
            transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        InputFieldActive = false;
    }
#endif
}
public static class RectTransformExtension {
 
    public static Rect GetScreenRect(this RectTransform rectTransform, Canvas canvas) {
 
        Vector3[] corners = new Vector3[4];
        Vector3[] screenCorners = new Vector3[2];
 
        rectTransform.GetWorldCorners(corners);
 
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
        {
            screenCorners[0] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[1]);
            screenCorners[1] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[3]);
        }
        else
        {
            screenCorners[0] = RectTransformUtility.WorldToScreenPoint(null, corners[1]);
            screenCorners[1] = RectTransformUtility.WorldToScreenPoint(null, corners[3]);
        }
 
        screenCorners[0].y = Screen.height - screenCorners[0].y;
        screenCorners[1].y = Screen.height - screenCorners[1].y;
 
        return new Rect(screenCorners[0], screenCorners[1] - screenCorners[0]);
    }
 
}    