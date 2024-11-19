using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShareSocial : MonoBehaviour
{
    public Texture2D predefinedImage;
    [SerializeField] private AchievementRewardsSO achievementRewardsSO;
    [SerializeField] public int valueToShare;
    public void SharePicture()//para compartir captura de pantalla
    {
        StartCoroutine(TakeScreenshotAndShare());
    }

    public void SharePredefinedImage()//para compartir imagen predefinida
    {
        switch (valueToShare)
        {
            case 4:
                predefinedImage= achievementRewardsSO.imageShare[0];
                break;
            case 6:
                predefinedImage = achievementRewardsSO.imageShare[1];
                break;
            case 8:
                predefinedImage = achievementRewardsSO.imageShare[2];
                break;
            case 10:
                predefinedImage = achievementRewardsSO.imageShare[3];
                break;
        }
        StartCoroutine(SharePredefinedImageCoroutine());   

    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Picture shared").SetText("Score")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }

    private IEnumerator SharePredefinedImageCoroutine()
    {
        string filePath = Path.Combine(Application.temporaryCachePath, "shared_img.png");
        byte[] imageBytes = predefinedImage.EncodeToPNG();

        File.WriteAllBytes(filePath, imageBytes);

        new NativeShare().AddFile(filePath)
           .SetSubject("Predefined image shared") 
           .SetText("Score") 
           .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget)) 
           .Share();

        yield return null;


    }

}
