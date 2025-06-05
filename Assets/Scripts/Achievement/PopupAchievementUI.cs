using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupAchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI achievementText;
    [SerializeField] private Image awardSprite;
    [SerializeField] private AchievementRewardsSO AchievementRewardsSO;
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private TextMeshProUGUI _message_clone;
    [SerializeField] private ShareSocial valueToShare;
    public void ShowAchievementMaxGoodStreakText(int value)//metodo que se llama en editor para setear valores de UI achivement
    {
        string text = $"!Alcanzaste una racha de {value} respuestas correctas seguidas!";
        achievementText.SetText(text);
        AddMessageNotificationPanel(text);

        switch (value)
        {
            case 4:
                awardSprite.sprite = AchievementRewardsSO.awardSprite[0];
                break;
            case 6:
                awardSprite.sprite = AchievementRewardsSO.awardSprite[1];
                break;
            case 8:
                awardSprite.sprite = AchievementRewardsSO.awardSprite[2];
                break;
            case 10:
                awardSprite.sprite = AchievementRewardsSO.awardSprite[3];
                break;
        }
        _message_clone.text = _message.text;
        valueToShare.valueToShare = value;

    }

    public void ShowAchievementGoodWithoutErrors()
    {
        string text = "Complet� la ronda sin errores";
        achievementText.SetText(text);
        AddMessageNotificationPanel(text);
    }

    private void AddMessageNotificationPanel(string text)
    {
        NotificationPanel notificationPanel = new NotificationPanel { 
            Title = text,
            Message = text
        };
        GameEvents.AddNotificationPanel?.Invoke(notificationPanel);
    }
}
