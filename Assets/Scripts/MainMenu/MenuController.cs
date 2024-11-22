using System;
using System.Collections;
using System.Linq;
using ScriptableCreator;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private ScriptableObjectUser _user;
        [SerializeField] private ScriptableObjectSettings _objectSettings;
        [SerializeField] private ScriptableObjectInstructor _objectInstructor;
        [SerializeField] private LevelUserSO _levelUserSO;

        [SerializeField] private TextMeshProUGUI _username;
        [SerializeField] private TextMeshProUGUI _totalExpirience;
        [SerializeField] private TextMeshProUGUI _totalCoins;
        [SerializeField] private TextMeshProUGUI _experienceToAchieve;
        [SerializeField] private Image _levelIcon;

        [SerializeField] private TMP_InputField _userInputField;

        [SerializeField] private SVGImage _mainMenuAvatar;
        [SerializeField] private SVGImage _storeAvatar;
        // [SerializeField] private Slider _soundEffects;
        // [SerializeField] private Slider _music;
        [SerializeField] private NotificationToggle _notificationToggle;
        [SerializeField] private Transform _pointToInstantiate;
        [SerializeField] private UnityEvent _onSuccessSaveInformation;
        [Header("notificators")]
        [SerializeField]
        private ScriptableObjectNotificationText _notificationTextUsername;

        private GameObject _instructorInstantiated;
        private void Start()
        {
            FindObjectOfType<GameplaySound>().PlayMainMenuSound();
            // if (PlayerPrefs.HasKey("settingInfo"))
            // {
            //     _objectSettings.settingData = JsonUtility.FromJson<ScriptableObjectSettings.SettingData>(PlayerPrefs.GetString("settingInfo"));
            // }
            PathToInstantiateInstructor();
            SetUserProperties();//Metodo que setea propiedades de usuario al iniciar
            SetConfigurationProperties();
            if (_user.userInfo.haveAvatar)
            {
                _mainMenuAvatar.sprite = _user.userInfo.spriteAvatar;
                //_storeAvatar.sprite = _user.userInfo.spriteAvatar;    
            }
            else
            {
                _mainMenuAvatar.enabled = false;
                //_storeAvatar.enabled = false;   
            }
        }

        private void OnEnable()
        {
            GameEvents.NewInstuctorId += GameEvents_InstructorChanged;
            GameEvents.CoinsChanged += GameEvents_CoinsChanged;
            GameEvents.ExperienceChanged += GameEvents_ExperienceChanged;
            GameEvents.UsernameSelected += GameEvents_UsernameSelected;
        }

        private void GameEvents_UsernameSelected()
        {
            _username.text = _user.userInfo.user.detail.usernameG;
        }

        private void OnDisable()
        {
            GameEvents.NewInstuctorId -= GameEvents_InstructorChanged;
            GameEvents.CoinsChanged -= GameEvents_CoinsChanged;
            GameEvents.ExperienceChanged -= GameEvents_ExperienceChanged;
            GameEvents.UsernameSelected -= GameEvents_UsernameSelected;
        }

        private void GameEvents_ExperienceChanged()
        {
            _totalExpirience.text = _user.userInfo.user.detail.totalExperience.ToString();
            SetUserLevel();
        }

        private void GameEvents_CoinsChanged()
        {
            _totalCoins.text = _user.userInfo.user.detail.totalCoins.ToString();
        }

        private void GameEvents_InstructorChanged(int obj)
        {
            ChangeInstructor();
        }
        public void PathToInstantiateInstructor()//metodo para instanciar el instructor
        {
            var indexInstructor = _user.userInfo.idInstructor;
            _instructorInstantiated = Instantiate(_objectInstructor.instructors.FirstOrDefault(x => x.id == indexInstructor)!.prefab,
                _pointToInstantiate.position, _pointToInstantiate.rotation, _pointToInstantiate);//instanciar instructor del prefab en el scriptable object
            _instructorInstantiated.layer = 0;
        }
        public void ChangeInstructor()
        {
            var indexInstructor = _user.userInfo.idInstructor;
            StopAllCoroutines();
            StartCoroutine(IChangeInstructor(indexInstructor));
        }

        IEnumerator IChangeInstructor(int indexInstructor)//corutina para cambiar el instructor 
        {
            Debug.Log(_instructorInstantiated.name);
            Destroy(_instructorInstantiated);//eliminar instructor anterior
            while (_instructorInstantiated)
            {
                yield return null;
            }
            _instructorInstantiated = Instantiate(_objectInstructor.instructors.FirstOrDefault(x => x.id == indexInstructor)!.prefab,
                _pointToInstantiate.position, _pointToInstantiate.rotation, _pointToInstantiate);
            _instructorInstantiated.layer = 0;
        }

        public void SetUserProperties()
        {
            _userInputField.text = _user.userInfo.user.detail.usernameG;
            _username.text = _user.userInfo.user.detail.usernameG;
            _totalCoins.text = _user.userInfo.user.detail.totalCoins.ToString();//setear monedas
            _totalExpirience.text = _user.userInfo.user.detail.totalExperience.ToString();//setear experiencia
            SetUserLevel();
        }

        public void SetConfigurationProperties()
        {
            // _soundEffects.value = _objectSettings.settingData.soundEffectVolume;
            // _music.value = _objectSettings.settingData.musicVolume;
            _notificationToggle.ActiveNotification = _objectSettings.settingData.haveNotification;
            _notificationToggle.InitToggle();

        }

        public void SaveInformation()
        {
            // _user.userInfo.username = _userInputField.text;
            // _objectSettings.settingData.musicVolume = _music.value;
            // _objectSettings.settingData.soundEffectVolume = _soundEffects.value;
            _objectSettings.settingData.haveNotification = _notificationToggle.ToggleNotification.isOn;
            // SaveUserInformation();
            SaveSettingInformation();
            _onSuccessSaveInformation?.Invoke();
        }

        public void SaveInstructor()
        {

        }
        public void SaveUserInformation()
        {
            PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_user.userInfo));
            PlayerPrefs.Save();

            // _notificationTextUsername.Raise(_user.userInfo.username );
        }

        public void SaveSettingInformation()
        {
            PlayerPrefs.SetString("MusicVolume", JsonUtility.ToJson(_objectSettings.settingData));
            PlayerPrefs.SetString("SounEffectVolume", JsonUtility.ToJson(_objectSettings.settingData));
            PlayerPrefs.Save();
        }

        public void SetUserLevel()
        {
            if (_user.userInfo.user.detail.totalExperience <= 4500)
            {
                _levelIcon.sprite = _levelUserSO.levelSprite[0];
                //Debug.Log("Nivel Novato");
                _experienceToAchieve.text = "de 4500";
            }
            else if (_user.userInfo.user.detail.totalExperience >= 4501 && _user.userInfo.user.detail.totalExperience <= 9500)
            {
                _levelIcon.sprite = _levelUserSO.levelSprite[1];
                //Debug.Log("Nivel Experto");
                _experienceToAchieve.text = "de 9500";
            }
            else if (_user.userInfo.user.detail.totalExperience >= 9501 && _user.userInfo.user.detail.totalExperience <= 15000)
            {
                _levelIcon.sprite = _levelUserSO.levelSprite[2];
                //Debug.Log("Nivel Master");
                _experienceToAchieve.text = "de 15000";
            }
            else if (_user.userInfo.user.detail.totalExperience >= 15001)
            {
                _levelIcon.sprite = _levelUserSO.levelSprite[3];
                //Debug.Log("Nivel Leyenda");
                _experienceToAchieve.text = null;
            }
        }
    }
}
