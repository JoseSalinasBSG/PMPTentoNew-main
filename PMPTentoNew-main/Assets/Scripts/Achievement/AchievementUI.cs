using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private UnityEngine.UI.Button getGiftButton;
    [SerializeField] private Slider counterSlider;
    [SerializeField] private Image[] startLevelImages;
    [SerializeField] private Color starNormalColor;
    [SerializeField] private Color starFillColor;

    private AchievementData.Achievement _achievement;

    public void SetValues(AchievementData.Achievement achievement)
    {
        _achievement = achievement;
        nameText.SetText(achievement.Name);
        counterText.SetText($"{achievement.CurrentCounter}/{achievement.MaxCounter}");
        SetSliderValues();
        FillStarImages();//llenar estrellas
        //CheckGiftsObtained();
    }
    /*
    private void CheckGiftsObtained()
    {
        if (_achievement.GiftsObtained <= 0)
            getGiftButton.gameObject.SetActive(false);
        else
            getGiftButton.gameObject.SetActive(true);
    }*/

    public void SetGiftObtained()
    {
        GameEvents.OnSetGiftsFromAchievement?.Invoke(_achievement, getGiftButton);
    }

    private void SetSliderValues()
    {
        counterSlider.maxValue = _achievement.MaxCounter;
        counterSlider.value = _achievement.CurrentCounter;
    }

    private void FillStarImages()
    {
        //var currentLvl = _achievement.CurrentLevel;//accedemos al campo current level de achievement
        var consecutiveAnswer = _achievement.ConsecutiveAnswer;

        foreach (var item in startLevelImages)//recorrer lista de estrellas
        {
            item.color = starNormalColor;//seteo todas las estrellas en su color por defecto
        }

        for (int i = 0; i < consecutiveAnswer; i++)//recorro las estrellas hasta el valor de consecutiveAnswer
        {
            startLevelImages[i].color = starFillColor;//lleno las estrellas cambiando el color
        }
    }
}
