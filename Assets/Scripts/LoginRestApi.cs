using System;
using System.Collections;
using System.IO;
using System.Text;
using Login;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Networking;

//<summary>
//LoginRestApi se encarga de realizar las peticiones a la API para obtener datos del usuario
//</summary>

public class LoginRestApi : MonoBehaviour
{
    [SerializeField] private string url = "https://api-portalweb.bsginstitute.com/api/AspNetUser/authenticate";
    [SerializeField] private string urlToMoreDetail = "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ObtenerCaracteristicasGamificacion";
    [SerializeField] private string urlToCredentials = "https://api-portalweb.bsginstitute.com/api/CredencialPortalPmp";
    [SerializeField] private string urlGetAchievements = "http://simuladorpmp-servicio.bsginstitute.com/api/Gamificacion/ObtenerLogroAlumno?IdRegistroAlumno=0&IdAlumno=";

    [SerializeField] private ScriptableObjectUser _objectUser;
    [SerializeField] private LoginController _loginController;
    // [SerializeField] private string username = "pruebaxjrivera@bsginstitute.net";
    // [SerializeField] private string password = "BSgrupo123";

    public bool _finishRequest;
    public bool _haveError;
    public IEnumerator PostLoginRoutine(string username, string password)
    {
        _finishRequest = _haveError = false;
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            DataLogin dataLogin = new DataLogin() { username = username, password = password };

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);//para subir datos en formato RAW
            request.downloadHandler = new DownloadHandlerBuffer();//para manejar la descarga de la respuesta

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

            yield return request.SendWebRequest();//envia solicitud y espera hasta una respuesta
            if (request.responseCode == 401)
            {
                _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);//se deserializa respuesta JSON a un objeto user

                _objectUser.userInfo.haveUser = false;
                GameEvents.ErrorLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
            }
            else if (request.responseCode >= 400)
            {
                _objectUser.userInfo.haveUser = false;
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);//se deserializa respuesta JSON y llenamos el objeto user
                    //Debug.Log("PostLoginCORUOTUINE");
                    if (_objectUser.userInfo.user.excepcion.excepcionGenerada)//fallo
                    {
                        _objectUser.userInfo.haveUser = false;
                        GameEvents.FailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                        // _loginController._onFailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                    }
                    else//es exitosa
                    {
                        StartCoroutine(GetGamificationData(_objectUser.userInfo.user.idAlumno));//llamo endpoint, para llenar el detail
                        StartCoroutine(GetAchievementData(_objectUser.userInfo.user.idAlumno));//llamo endpoint, para llenar el achievement
                        _objectUser.userInfo.haveUser = true;
                        // GameEvents.SuccessfulLogin?.Invoke(_objectUser.userInfo.user);

                        GameEvents.GetUserExam?.Invoke(_objectUser.userInfo.user.userName);//para crear un ID para generar un ID de examen
                        GameEvents.GetNameExam?.Invoke(_objectUser.userInfo.user.idAlumno.ToString());
                        // _loginController._onSuccessLogin?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text + " - " + e.ToString());
                    _objectUser.userInfo.haveUser = false;
                    _loginController._onErrorInLogin?.Invoke("Se tuvo un error interno, vuelva a intentarlo mas tarde");
                }

            }
            // PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));
            _finishRequest = true;
        }
    }

    public IEnumerator GetGamificationData(int userId)
    {
        _finishRequest = _haveError = false;
        using (UnityWebRequest request = new UnityWebRequest(urlToMoreDetail + "/" + userId, "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();//manejar descarga de respuesta
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

            yield return request.SendWebRequest();
            if (request.responseCode == 401)
            {
                _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                _objectUser.userInfo.haveUser = false;
                GameEvents.ErrorLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
            }
            else if (request.responseCode >= 400)
            {
                _objectUser.userInfo.haveUser = false;
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    var detail = JsonUtility.FromJson<UserDetail>(request.downloadHandler.text);
                    _objectUser.userInfo.user.detail = detail;//llenamos el detail
                    if (_objectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _objectUser.userInfo.haveUser = false;
                        GameEvents.FailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                        // _loginController._onFailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                    }
                    else
                    {
                        if (_objectUser.userInfo.user.detail.usernameG != "UserName")//verificamos si tiene un nombre de usuario o no, si es diferente a UserName que se coloca por defecto
                        {
                            _objectUser.userInfo.haveUser = true;
                            GameEvents.NewUsername?.Invoke(_objectUser.userInfo.user.detail.usernameG);//evento para actualizar, le pasamos usernameG que es el numero nombre de usuario
                        }
                        else
                        {
                            _objectUser.userInfo.haveUsername = false;
                        }

                        if (_objectUser.userInfo.user.detail.idCaracteristicaGamificacion != 0)//0 es que es un objeto nuevo y 1 que ya hay informacion almacenada
                        {
                            //Debug.Log("idcaracteristicaGamificacion 1");
                            GameEvents.NewInstuctorId?.Invoke(_objectUser.userInfo.user.detail.instructorID);//le colocamos el id del instructor que ya se tiene para que se use
                        }
                        GameEvents.SuccessfulLogin?.Invoke(_objectUser.userInfo.user);

                        // GameEvents.GetUserExam?.Invoke(_objectUser.userInfo.user.userName);
                        // GameEvents.GetNameExam?.Invoke(_objectUser.userInfo.user.idAlumno.ToString());
                        // _loginController._onSuccessLogin?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text + " - " + e.ToString());
                    _objectUser.userInfo.haveUser = false;
                    _loginController._onErrorInLogin?.Invoke("Se tuvo un error interno, vuelva a intentarlo mas tarde");
                }

            }
            // PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));
            _finishRequest = true;
        }
    }

    public IEnumerator GetAchievementData(int userId)//obtiene datos de achievement en login
    {
        Debug.Log("GetAchievementData");
        _finishRequest = _haveError = false;
        using (UnityWebRequest request = new UnityWebRequest(urlGetAchievements + userId, "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();//manejar descarga de respuesta
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

            yield return request.SendWebRequest();
            if (request.responseCode == 401)
            {
                _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                _objectUser.userInfo.haveUser = false;
                GameEvents.ErrorLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
            }
            else if (request.responseCode >= 400)
            {
                _objectUser.userInfo.haveUser = false;
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    var achievements = JsonUtility.FromJson<UserAchievements>(request.downloadHandler.text);
                    _objectUser.userInfo.user.achievements = achievements;//llenamos el achivements
                    if (_objectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _objectUser.userInfo.haveUser = false;
                        GameEvents.FailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                        // _loginController._onFailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                    }
                    else//es exitosa
                    {
                        if (_objectUser.userInfo.user.detail.usernameG != "UserName")//verificamos si tiene un nombre de usuario o no, si es diferente a UserName que se coloca por defecto
                        {
                            _objectUser.userInfo.haveUser = true;
                            GameEvents.NewUsername?.Invoke(_objectUser.userInfo.user.detail.usernameG);//evento para actualizar, le pasamos usernameG que es el numero nombre de usuario
                        }
                        else
                        {
                            _objectUser.userInfo.haveUsername = false;
                        }

                        if (_objectUser.userInfo.user.detail.idCaracteristicaGamificacion != 0)//0 es que es un objeto nuevo y 1 que ya hay informacion almacenada
                        {
                            GameEvents.NewInstuctorId?.Invoke(_objectUser.userInfo.user.detail.instructorID);//le colocamos el id del instructor que ya se tiene para que se use
                        }
                        GameEvents.SuccessfulLogin?.Invoke(_objectUser.userInfo.user);
                        // GameEvents.GetUserExam?.Invoke(_objectUser.userInfo.user.userName);
                        // GameEvents.GetNameExam?.Invoke(_objectUser.userInfo.user.idAlumno.ToString());
                        // _loginController._onSuccessLogin?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text + " - " + e.ToString());
                    _objectUser.userInfo.haveUser = false;
                    _loginController._onErrorInLogin?.Invoke("Se tuvo un error interno, vuelva a intentarlo mas tarde");
                }
            }
            // PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));
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

            yield return request.SendWebRequest();//se envia solicitud y se espera respuesta
            if (request.responseCode >= 400)
            {
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");
                Debug.Log("->" + request.error);
            }
            else
            {
                try
                {
                    var credential = JsonUtility.FromJson<ResponseToAvatar>(request.downloadHandler.text);
                    Debug.Log("->" + request.downloadHandler.text);
                    if (credential.contieneAvatar)
                    {
                        _objectUser.userInfo.haveAvatar = true;
                        _objectUser.userInfo.urlAvatar = GenerateUrlToAvatar(credential.avatar);
                        StartCoroutine(downloadSVG(_objectUser.userInfo.urlAvatar));
                    }
                    else
                    {
                        _objectUser.userInfo.haveAvatar = false;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("_>" + request.downloadHandler.text);
                    _objectUser.userInfo.haveAvatar = false;
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
            _objectUser.userInfo.spriteAvatar = sprite;
            Debug.Log("creado: " + _objectUser.userInfo.urlAvatar);
            Debug.Log("creado: " + _objectUser.userInfo.spriteAvatar);
            Debug.Log("creado: " + _objectUser.userInfo.haveAvatar);
            Debug.Log("creado");
            GameEvents.SuccessGetAvatar?.Invoke();
        }
    }
    private string GenerateUrlToAvatar(AvatarInfo avatarInfo)
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
    public void PostLogin(string username, string password)
    {
        StartCoroutine(PostLoginRoutine(username, password));
        StartCoroutine(GetAvatar(username, password));
    }
    [ContextMenu(nameof(PostLogin))]
    public void PostLogin()
    {
        StartCoroutine(PostLoginRoutine("pruebaxjrivera@bsginstitute.net", "BSgrupo123"));
    }
}

[Serializable]
public class DataLogin
{
    public string username;
    public string password;
}
[Serializable]
public class DataLoginToCredentials
{
    public string username;
    public string clave;
}
[Serializable]
public class ResponseToAvatar
{
    public AvatarInfo avatar;
    public bool contieneAvatar;
}
[Serializable]
public class AvatarInfo
{
    public string top;
    public string accessories;
    public string hair_Color;
    public string facial_Hair;
    public string facial_Hair_Color;
    public string clothes;
    public string eyes;
    public string eyesbrow;
    public string mouth;
    public string skin;
    public string clothes_Color;
}

[Serializable]
public class DataGetUserGami
{
    public int IdAlumno;
}