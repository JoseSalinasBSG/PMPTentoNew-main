using System;
using System.Collections;
using System.IO;
using System.Text;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Networking;

// <summary>
// La clase UserService gestiona las solicitudes HTTP para obtener y actualizar los detalles del usuario y sus logros en el servidor, así como obtener el avatar del usuario.
// Los detalles del usuario se recuperan de una API externa y se almacenan en un ScriptableObject para su uso dentro del juego.
// Además, la clase maneja la autenticación del usuario y la carga de logros, actualizando las características del usuario en el servidor mediante peticiones POST.
// También se encarga de generar la URL del avatar del usuario y descargar la imagen SVG asociada, convirtiéndola en un sprite que se utiliza en el juego.
// Los eventos relacionados con la actualización de datos, logros y avatar son gestionados mediante eventos de Unity.
// </summary>

public class UserService : MonoBehaviour
{
    [SerializeField] private ScriptableObjectUser _scriptableObjectUser;

    private readonly string _urlToUpdate = "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ActualizarCaracteristicasGamificacion";
    private readonly string _urlToUpdateAchievement = "http://simuladorpmp-servicio.bsginstitute.com/api/Gamificacion/RegistrarLogroAlumno";
    private readonly string _urlToGetUser = "https://api-portalweb.bsginstitute.com/api/AspNetUser/authenticate";
    private readonly string _urlToGetUserDetail = "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ObtenerCaracteristicasGamificacion/";
    private readonly string urlToCredentials = "https://api-portalweb.bsginstitute.com/api/CredencialPortalPmp";
    private readonly string _urlToGetUserAchievements = "http://simuladorpmp-servicio.bsginstitute.com/api/Gamificacion/ObtenerLogroAlumno?IdRegistroAlumno=0&IdAlumno=";

    private bool _haveError;
    private bool _finishRequest;

    private void OnEnable()
    {
        GameEvents.RequestCoinsChange += GameEvents_RequestCoinsChange;
        GameEvents.RequestExperienceChange += GameEvents_RequestExperienceChange;
        GameEvents.RequestUpdateDetail += GameEvents_RequestUpdateDetail;
        GameEvents.RequestUpdateAchievements += GameEvents_RequestUpdateAchievements;
    }

    private void OnDisable()
    {
        GameEvents.RequestCoinsChange -= GameEvents_RequestCoinsChange;
        GameEvents.RequestExperienceChange -= GameEvents_RequestExperienceChange;
        GameEvents.RequestUpdateDetail -= GameEvents_RequestUpdateDetail;
        GameEvents.RequestUpdateAchievements -= GameEvents_RequestUpdateAchievements;

    }

    private void GameEvents_RequestUpdateDetail()//METODO QUE ACTUALIZA USER DETAIL
    {
        StartCoroutine(UpdateUserDetail());
    }

    private void GameEvents_RequestUpdateAchievements()//METODO QUE ACTUALIZA USER ACHIEVEMENTS
    {
        StartCoroutine(UpdateUserAchievements());
    }

    private void GameEvents_RequestExperienceChange(float obj)
    {
    }

    private void GameEvents_RequestCoinsChange(float obj)
    {
    }

    public void GetUserDetail(int userId)//llenar user detail en user data SO
    {
        StartCoroutine(IGetUserDetail(userId));
    }
    public void GetUserAchievement(int userId)//llenar user achievement en user data SO
    {
        StartCoroutine(IGetUserAchievements(userId));
    }

    public IEnumerator IGetUserDetail(int userId)
    {
        _finishRequest = _haveError = false;
        using (UnityWebRequest request = new UnityWebRequest(_urlToGetUserDetail + userId, "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            AddHeader(request);

            yield return request.SendWebRequest();

            if (request.responseCode >= 400)
            {
                _scriptableObjectUser.userInfo.haveUser = false;
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    var detail = JsonUtility.FromJson<UserDetail>(request.downloadHandler.text);
                    _scriptableObjectUser.userInfo.user.detail = detail;
                    if (_scriptableObjectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _scriptableObjectUser.userInfo.haveUser = false;
                    }
                    else
                    {
                        if (_scriptableObjectUser.userInfo.user.detail.usernameG != "UserName")
                        {
                            _scriptableObjectUser.userInfo.haveUsername = true;
                        }
                        else
                        {
                            _scriptableObjectUser.userInfo.haveUsername = false;
                        }
                    }
                    GameEvents.SuccessGetUserDetail?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    _scriptableObjectUser.userInfo.haveUser = false;
                }
            }
            _finishRequest = true;
        }
    }

    public IEnumerator IGetUserAchievements(int userId)
    {
        _finishRequest = _haveError = false;
        using (UnityWebRequest request = new UnityWebRequest(_urlToGetUserAchievements + userId, "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            AddHeader(request);

            yield return request.SendWebRequest();

            if (request.responseCode >= 400)
            {
                _scriptableObjectUser.userInfo.haveUser = false;
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    var achievements = JsonUtility.FromJson<UserAchievements>(request.downloadHandler.text);
                    _scriptableObjectUser.userInfo.user.achievements = achievements;
                    if (_scriptableObjectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _scriptableObjectUser.userInfo.haveUser = false;
                    }
                    else
                    {
                        if (_scriptableObjectUser.userInfo.user.detail.usernameG != "UserName")
                        {
                            _scriptableObjectUser.userInfo.haveUsername = true;
                        }
                        else
                        {
                            _scriptableObjectUser.userInfo.haveUsername = false;
                        }
                    }
                    GameEvents.SuccessGetUserDetail?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    _scriptableObjectUser.userInfo.haveUser = false;
                }
            }
            _finishRequest = true;
        }
    }

    public IEnumerator UpdateUserDetail()
    {
        using (UnityWebRequest request = new UnityWebRequest(_urlToUpdate, "POST"))
        {
            UserDetail dataLogin = _scriptableObjectUser.userInfo.user.detail;

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            AddHeader(request);
            yield return request.SendWebRequest();
            if (request.responseCode >= 400)
            {
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    bool detail = Convert.ToBoolean(request.downloadHandler.text);
                    if (detail)
                    {
                        GameEvents.DetailChanged?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text + " - " + e.Message);
                }
            }
        }
    }

    public IEnumerator UpdateUserAchievements()
    {
        using (UnityWebRequest request = new UnityWebRequest(_urlToUpdateAchievement, "POST"))
        {
            UserAchievements dataAchievement = _scriptableObjectUser.userInfo.user.achievements;

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataAchievement));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            AddHeader(request);
            yield return request.SendWebRequest();
            if (request.responseCode >= 400)
            {
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    bool achievement = Convert.ToBoolean(request.downloadHandler.text);//comprueba si devuelve true o false
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text + " - " + e.Message);
                }
            }
        }
    }
    public void GetUSer(string username, string password)
    {
        StartCoroutine(GetUser(username, password));
        StartCoroutine(GetAvatar(username, password));
    }

    public IEnumerator GetUser(string username, string password)
    {
        _finishRequest = _haveError = false;
        using (UnityWebRequest request = new UnityWebRequest(_urlToGetUser, "POST"))
        {
            DataLogin dataLogin = new DataLogin() { username = username, password = password };

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            AddHeader(request);
            yield return request.SendWebRequest();
            if (request.responseCode == 401)
            {
                _scriptableObjectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                _scriptableObjectUser.userInfo.haveUser = false;
                GameEvents.ErrorGetUser?.Invoke();
            }
            else if (request.responseCode >= 400)
            {
                _scriptableObjectUser.userInfo.haveUser = false;
                GameEvents.ErrorGetUser?.Invoke();
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    _scriptableObjectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                    if (_scriptableObjectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _scriptableObjectUser.userInfo.haveUser = false;
                        GameEvents.ErrorGetUser?.Invoke();
                    }
                    else
                    {
                        _scriptableObjectUser.userInfo.haveUser = true;
                        GameEvents.SuccessGetUser?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    _scriptableObjectUser.userInfo.haveUser = false;
                    GameEvents.ErrorGetUser?.Invoke();
                }
            }

            _finishRequest = true;
        }
    }
    public IEnumerator GetAvatar(string username, string password)
    {
        using (UnityWebRequest request = new UnityWebRequest(urlToCredentials, "POST"))
        {
            DataLoginToCredentials dataLogin = new DataLoginToCredentials() { username = username, clave = password };

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

            yield return request.SendWebRequest();

            if (request.responseCode >= 400)
            {
                Debug.Log("->" + request.error);
                GameEvents.ErrorGetAvatar?.Invoke();
            }
            else
            {
                try
                {
                    var credential = JsonUtility.FromJson<ResponseToAvatar>(request.downloadHandler.text);
                    Debug.Log("->" + request.downloadHandler.text);
                    if (credential.contieneAvatar)
                    {
                        _scriptableObjectUser.userInfo.haveAvatar = true;
                        _scriptableObjectUser.userInfo.urlAvatar = GenerateUrlToAvatar(credential.avatar);
                        StartCoroutine(downloadSVG(_scriptableObjectUser.userInfo.urlAvatar));
                    }
                    else
                    {
                        _scriptableObjectUser.userInfo.haveAvatar = false;
                        GameEvents.ErrorGetAvatar?.Invoke();

                    }
                }
                catch (Exception e)
                {
                    Debug.Log("_>" + request.downloadHandler.text + " " + e);
                    _scriptableObjectUser.userInfo.haveAvatar = false;
                    GameEvents.ErrorGetAvatar?.Invoke();
                }

            }
        }
    }
    IEnumerator downloadSVG(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Error while Receiving: " + www.error);
            GameEvents.ErrorGetAvatar?.Invoke();
        }
        else
        {
            //Convert byte[] data of svg into string
            string bitString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            var tessOptions = new VectorUtils.TessellationOptions()
            {
                StepDistance = 100.0f,
                MaxCordDeviation = 0.5f,
                MaxTanAngleDeviation = 0.1f,
                SamplingStepSize = 0.01f
            };

            // Dynamically import the SVG data, and tessellate the resulting vector scene.
            var sceneInfo = SVGParser.ImportSVG(new StringReader(bitString));
            var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);

            // Build a sprite with the tessellated geometry
            Sprite sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
            _scriptableObjectUser.userInfo.spriteAvatar = sprite;
            Debug.Log("creado: " + _scriptableObjectUser.userInfo.urlAvatar);
            Debug.Log("creado: " + _scriptableObjectUser.userInfo.spriteAvatar);
            Debug.Log("creado: " + _scriptableObjectUser.userInfo.haveAvatar);
            GameEvents.SuccessGetAvatar?.Invoke();
        }
    }
    private void AddHeader(UnityWebRequest request)
    {
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("User-Agent",
            "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
    }
    public string GenerateUrlToAvatar(AvatarInfo avatarInfo)
    {
        string url = $"https://avataaars.io/?avatarStyle=Circle";
        url += $"&topType={avatarInfo.top}";
        url += $"&accessoriesType={avatarInfo.accessories}";
        url += $"&hairColor={avatarInfo.hair_Color}";
        url += $"&facialHairType={avatarInfo.facial_Hair}";
        url += $"&facialHairColor={avatarInfo.facial_Hair_Color}";
        url += $"&clotheType={avatarInfo.clothes}";
        url += $"&clotheColor={avatarInfo.clothes_Color}";
        url += $"&eyeType={avatarInfo.eyes}";
        url += $"&eyebrowType={avatarInfo.eyesbrow}";
        url += $"&mouthType={avatarInfo.mouth}";
        url += $"&skinColor={avatarInfo.skin}";
        return url;
    }
}