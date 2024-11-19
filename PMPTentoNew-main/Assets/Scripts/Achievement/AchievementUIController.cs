using TMPro;
using UnityEngine;

public class AchievementUIController : MonoBehaviour
{
    [SerializeField] private AchievementData achievementData;
    [SerializeField] private GameObject achievementPrefab;
    [SerializeField] private Transform contentAchievements;
    [SerializeField] private Transform contentScrollViewStreaks;


    [SerializeField] private GameObject popupContainer;
    [SerializeField] private TextMeshProUGUI popUpGift1Text;
    [SerializeField] private TextMeshProUGUI popUpGift2Text;

    private AchievementData.Achievement _achievement;//variable tipo Achievement
    private UnityEngine.UI.Button _button;

    private void OnEnable()
    {
        GameEvents.OnSetGiftsFromAchievement += SetGiftsObtained;
    }

    private void OnDisable()
    {
        GameEvents.OnSetGiftsFromAchievement -= SetGiftsObtained;
    }

    public void SetAllAchievements()//se llama al presionar boton de Logros en footer de MainMenu
    {
        ClearChildrenAchievement();

        var list = achievementData.achievementListContainer.achievementList;
        //for (int i = 0; i < list.Count; i++)
        //{
            var item = Instantiate(achievementPrefab, contentAchievements);
            item.GetComponent<AchievementUI>().SetValues(list[0]);//setear valores en contenedor de logros, el primer elemento de la lista de achievement Data        
        //}

        for (int i = 0; i < contentScrollViewStreaks.childCount; i++)//recorrer contenedores de rachas de n preguntas
        {
            contentScrollViewStreaks.GetChild(i).GetComponent<AwardsContainer>().SetValuesAwards();//accedo al metodo de cada uno de los AwardsContainers
        }

    }

    public void ClearChildrenAchievement()//limpio los logros del contenedor y de los trofeos
    {
        for (int i = contentAchievements.childCount - 1; i >= 0; i--)
        {
            Destroy(contentAchievements.GetChild(i).gameObject);
        }
       


    }

    public void GetGifts()
    {
        achievementData.RemoveGiftsObtained(_achievement);
        _button.gameObject.SetActive(false);
    }

    public void SetGiftsObtained(AchievementData.Achievement achievement, UnityEngine.UI.Button button)//recibe variable tipo achievment y boton
    {
        _achievement = achievement;
        _button = button;

        popupContainer.SetActive(true);
        popupContainer.GetComponent<FadeUI>().FadeInTransition();

        int totalGift1 = achievement.GiftsObtained * achievement.GiftData[0].Amount;
        int totalGift2 = achievement.GiftsObtained * achievement.GiftData[1].Amount;

        popUpGift1Text.SetText(totalGift1.ToString());
        popUpGift2Text.SetText(totalGift2.ToString());
    }

   

}
