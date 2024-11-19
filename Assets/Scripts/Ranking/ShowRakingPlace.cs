using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DataUserAll;

public class ShowRakingPlace : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI firstPlaceUserNameText;
    [SerializeField] TextMeshProUGUI secondPlaceUserNameText;
    [SerializeField] TextMeshProUGUI thirdPlaceUserNameText;
    [SerializeField] TextMeshProUGUI firstPlaceUserExperienceText;
    [SerializeField] TextMeshProUGUI secondPlaceUserExperienceText;
    [SerializeField] TextMeshProUGUI thirdPlaceUserExperienceText;

    public void ShowFirstPlace(DataUsers dataUser) 
    {
        firstPlaceUserNameText.SetText(dataUser.userName);
        firstPlaceUserExperienceText.SetText(dataUser.totalExperience.ToString());
    }

    public void ShowSecondPlace(DataUsers dataUser) 
    {
        secondPlaceUserNameText.SetText(dataUser.userName);
        secondPlaceUserExperienceText.SetText(dataUser.totalExperience.ToString());
    }

    public void ShowThirdPlace(DataUsers dataUser)
    {
        thirdPlaceUserNameText.SetText(dataUser.userName);
        thirdPlaceUserExperienceText.SetText(dataUser.totalExperience.ToString());
    }
}
