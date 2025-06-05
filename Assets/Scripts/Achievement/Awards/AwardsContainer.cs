using TMPro;
using UnityEngine;

public class AwardsContainer : MonoBehaviour
{
    [SerializeField] GameObject awardImage;
    [SerializeField] TextMeshProUGUI streaksText;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] GameObject awardContent;
    [SerializeField] int typeOfStreak;
    [SerializeField] private ScriptableObjectUser _objectUser;


    public void SetValuesAwards()
    {
        ClearAwards();
        FillAwards();
    }

    private void FillAwards()
    {

        switch (typeOfStreak)
        {
            case 4:
                for (int i = 0; i < _objectUser.userInfo.user.achievements.streak4; i++)
                {
                    Instantiate(awardImage, awardContent.transform);
                    streaksText.text = $"Tienes {_objectUser.userInfo.user.achievements.streak4} rachas de {typeOfStreak} estrellas";
                    dateText.text = $"<b>Ultima racha</b>: {_objectUser.userInfo.user.achievements.streak4Date} - {_objectUser.userInfo.user.achievements.streak4Origin}";

                }
                break;
            case 6:
                for (int i = 0; i < _objectUser.userInfo.user.achievements.streak6; i++)
                {
                    Instantiate(awardImage, awardContent.transform);
                    streaksText.text = $"Tienes {_objectUser.userInfo.user.achievements.streak6} rachas de {typeOfStreak} estrellas";
                    dateText.text = $"<b>Ultima racha</b>: {_objectUser.userInfo.user.achievements.streak6Date} - {_objectUser.userInfo.user.achievements.streak6Origin}";
                }
                break;
            case 8:
                for (int i = 0; i < _objectUser.userInfo.user.achievements.streak8; i++)
                {
                    Instantiate(awardImage, awardContent.transform);
                    streaksText.text = $"Tienes {_objectUser.userInfo.user.achievements.streak8} rachas de {typeOfStreak} estrellas";
                    dateText.text = $"<b>Ultima racha</b>: {_objectUser.userInfo.user.achievements.streak8Date} - {_objectUser.userInfo.user.achievements.streak8Origin}";

                }
                break;
            case 10:
                for (int i = 0; i < _objectUser.userInfo.user.achievements.streak10; i++)
                {
                    Instantiate(awardImage, awardContent.transform);
                    streaksText.text = $"Tienes {_objectUser.userInfo.user.achievements.streak10} rachas de {typeOfStreak} estrellas";
                    dateText.text = $"<b>Ultima racha</b>: {_objectUser.userInfo.user.achievements.streak10Date} - {_objectUser.userInfo.user.achievements.streak10Origin}";
                }
                break;
        }
    }

    private void ClearAwards()
    {
        for (int i = awardContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(awardContent.transform.GetChild(i).gameObject);
        }
    }
}
