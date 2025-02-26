#region JoseS

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using System.Linq;
// using Unity.VectorGraphics;
// using UnityEngine;
// using UnityEngine.Networking;

// //<summary>
// //Clase para obtener informacion de experiencia de los usuarios de la API y crear sprites para los avatars y usarlos en el ranking
// //</summary>

// [Serializable]
// public class DataToRequireRanking
// {
//     public int MinExperience;
//     public int MaxExperience;
// }
// public class GetUsersApi : MonoBehaviour
// {
//     [SerializeField] private string URL = "http://simuladorpmp-servicio.bsginstitute.com/api/Gamificacion/ObtenerRankingGamificacion";
//     [SerializeField] private DataUserAll dataUserAll;
//     [SerializeField] private ScriptableObjectUser _user;
//     public enum Categories
//     {
//         Novato,
//         Experto,
//         Master,
//         Leyenda
//     }

//     public void GetRankingInformation()//este metodo se llama al presionar en seccion ranking
//     {
//         var tExperience = _user.userInfo.user.detail.totalExperience;//obtenemos experiencia y colocamos en variable
//         if (tExperience < 0)
//         {
//             return;
//         }
//         GetGlobal();//para que en principio se ejecute el filtro global
//     }
//     private void Start()
//     {
//         GetRankingInformation();
//     }

//     public void GetNovato()//filtro de experiencia en rango de novato
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(0, 4500));//con metodo GetData hago la solicitud al endpoint con los parametros de experiencia minima 0 y maxima 2000
//         Debug.Log("GetNovato");
//     }
//     public void GetExperto()
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(4501, 9500));
//         Debug.Log("GetExperto");
//     }
//     public void GetMaster()
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(9501, 15000));
//         Debug.Log("GetMaster");
//     }
//     public void GetLeyenda()
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(15001, 100000));
//         Debug.Log("GetLeyenda");
//     }
//     public void GetGlobal()
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(0, 100000));
//     }

//     private IEnumerator GetData(int minExperience, int maxExperience)
//     {
//         GameEvents.RequestRanking?.Invoke();
//         using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
//         {
//             DataToRequireRanking dataLogin = new DataToRequireRanking() { MinExperience = minExperience, MaxExperience = maxExperience };
//             var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
//             request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//             request.downloadHandler = new DownloadHandlerBuffer();

//             request.SetRequestHeader("Content-Type", "application/json");
//             request.SetRequestHeader("Accept", "application/json");
//             request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

//             yield return request.SendWebRequest();
//             if (request.result == UnityWebRequest.Result.ConnectionError)
//             {
//                 Debug.LogError(request.error);
//             }
//             else//si no tuvo error
//             {
//                 string json = request.downloadHandler.text;//almacena respuesta del endpoint
//                 Debug.Log(json);
//                 SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);//convierte respuesta a json
//                 SaveDataAllUsers(stats);
//             }
//         }
//     }

//     private void SaveDataAllUsers(SimpleJSON.JSONNode stats)
//     {
//         dataUserAll.Users.Clear();
//         // Crear lista temporal para procesar los usuarios
//         List<DataUserAll.DataUsers> tempUsers = new List<DataUserAll.DataUsers>();
//         for (int i = 0; i < stats.Count; i++) // esto cargaria todos los usuarios pero actualmente tarda mucho en cargar los sprites
//         {
//             DataUserAll.DataUsers user = new DataUserAll.DataUsers();
//             user.id = stats[i]["idAlumno"];
//             user.userName = stats[i]["usernameG"];
//             user.totalExperience = stats[i]["totalExperience"];

//             //objeto avatar
//             DataUserAll.AvatarUsers avatar = new DataUserAll.AvatarUsers();
//             avatar.idAvatar = stats[i]["avatar"]["idAvatar"];
//             avatar.idAlumno = stats[i]["avatar"]["idAlumno"];
//             avatar.nombres = stats[i]["avatar"]["nombres"];
//             avatar.topC = stats[i]["avatar"]["topC"];
//             avatar.accessories = stats[i]["avatar"]["accessories"];
//             avatar.hair_Color = stats[i]["avatar"]["hair_Color"];
//             avatar.facial_Hair = stats[i]["avatar"]["facial_Hair"];
//             avatar.facial_Hair_Color = stats[i]["avatar"]["facial_Hair_Color"];
//             avatar.clothes = stats[i]["avatar"]["clothes"];
//             avatar.clothes_Color = stats[i]["avatar"]["clothes_Color"];
//             avatar.eyes = stats[i]["avatar"]["eyes"];
//             avatar.eyesbrow = stats[i]["avatar"]["eyesbrow"];
//             avatar.mouth = stats[i]["avatar"]["mouth"];
//             avatar.skin = stats[i]["avatar"]["skin"];
//             //----
//             user.avatar = avatar;//asignar objeto avatar al usuario
//             user.urlAvatarUser = GenerateUrlToAvatarForRanking(avatar);
//             // Agregar a la lista temporal
//             tempUsers.Add(user);
//         }

//         // Ordenar usuarios
//         tempUsers = tempUsers.OrderByDescending(user => user.totalExperience).ToList();

//         // Agregar los usuarios ordenados a la lista final
//         dataUserAll.Users.AddRange(tempUsers);

//         // Ejecutar corrutinas para los 10 primeros usuarios
//         for (int i = 0; i < 11; i++)
//         {
//             StartCoroutine(downloadSVGUsersRanking(dataUserAll.Users[i].urlAvatarUser, i));
//         }
//         //Debug.Log("Se completo llenado de Avatars usuario");
//         //GameEvents.RankingRetrieved?.Invoke();
//     }

//     private string GenerateUrlToAvatarForRanking(DataUserAll.AvatarUsers avatar)
//     {
//         string url = $"https://avataaars.io/?avatarStyle=Circle";
//         url += $"&topType={avatar.topC}";
//         url += $"&accessoriesType={avatar.accessories}";
//         url += $"&hairColor={avatar.hair_Color}";
//         url += $"&facialHairType={avatar.facial_Hair}";
//         url += $"&facialHairColor={avatar.facial_Hair_Color}";
//         url += $"&clotheType={avatar.clothes}";
//         url += $"&clotheColor={avatar.clothes_Color}";
//         url += $"&eyeType={avatar.eyes}";
//         url += $"&eyebrowType={avatar.eyesbrow}";
//         url += $"&mouthType={avatar.mouth}";
//         url += $"&skinColor={avatar.skin}";
//         return url;
//     }

//     IEnumerator downloadSVGUsersRanking(string url, int i)
//     {
//         UnityWebRequest www = UnityWebRequest.Get(url);
//         yield return www.SendWebRequest();
//         if (www.isHttpError || www.isNetworkError)
//         {
//             Debug.Log("Error while Receiving: " + www.error);
//         }
//         else
//         {
//             //Convert byte[] data of svg into string
//             string bitString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
//             var tessOptions = new VectorUtils.TessellationOptions()
//             {
//                 StepDistance = 100.0f,
//                 MaxCordDeviation = 0.5f,
//                 MaxTanAngleDeviation = 0.1f,
//                 SamplingStepSize = 0.01f
//             };

//             // Dynamically import the SVG data, and tessellate the resulting vector scene.
//             var sceneInfo = SVGParser.ImportSVG(new StringReader(bitString));
//             var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);

//             // Build a sprite with the tessellated geometry
//             Sprite sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
//             dataUserAll.Users[i].spriteAvatarUser = sprite;
//         }
//     }
// }

#endregion

#region Miguel

/********************** Codigo Miguel ****************************/

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using TMPro;
// using Unity.VectorGraphics;
// using UnityEngine;
// using UnityEngine.Networking;

// [Serializable]
// public class DataToRequireRanking
// {
//     public int MinExperience;
//     public int MaxExperience;
// }
// public class GetUsersApi : MonoBehaviour
// {
//     //[SerializeField] private EndpointsConfigSO endpointsConfigSO;
//     private string URL = "http://simuladorpmp-servicio.bsginstitute.com/api/Gamificacion/ObtenerRankingGamificacion";
//     [SerializeField] private DataUserAll dataUserAll;
//     [SerializeField] private ScriptableObjectUser _user;
//     [SerializeField] private TextMeshProUGUI _categoryTitle;
//     private const int batchSize = 10;

//     public enum Categories
//     {
//         Novato,
//         Experto,
//         Master,
//         Leyenda
//     }

//     public void GetRankingInformation()//este metodo se llama al presionar en seccion ranking
//     {
//         var tExperience = _user.userInfo.user.detail.totalExperience;//obtenemos experiencia y colocamos en variable
//         if (tExperience < 0)
//         {
//             return;
//         }
//         /*
//         //Si se quiere que se ponga el filtro en base a la experiencia del usuario local
//         switch (tExperience)//Setear titulo de categoria(nivel) 
//         {
//             case <= 4500:
//                 GetNovato();
//                 _categoryTitle.text = "Categoría:\nNovato";
//                 break;
//             case <= 9500:
//                 GetExperto();
//                 _categoryTitle.text = "Categoría:\nExperto";
//                 break;
//             case <= 15000:
//                 GetMaster();
//                 _categoryTitle.text = "Categoría:\nMaster";
//                 break;
//             default:
//                 GetLeyenda();
//                 _categoryTitle.text = "Categoría:\nLeyenda";
//                 break;           
//         }*/
//         GetGlobal();//para que en principio se ejecute el filtro global
//     }
//     private void Start()
//     {
//         GetRankingInformation();
//     }
//     public void GetNovato()//filtro de experiencia en rango de novato
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(0, 4500));//con metodo GetData hago la solicitud al endpoint con los parametros de experiencia minima 0 y maxima 2000
//     }
//     public void GetExperto()
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(4501, 9500));
//     }
//     public void GetMaster()
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(9501, 15000));
//     }
//     public void GetLeyenda()
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(15001, 100000));
//     }
//     public void GetGlobal()
//     {
//         StopAllCoroutines();
//         StartCoroutine(GetData(0, 100000));
//     }

//     private IEnumerator GetData(int minExperience, int maxExperience)
//     {
//         GameEvents.RequestRanking?.Invoke();
//         //using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
//         using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
//         {
//             DataToRequireRanking dataLogin = new DataToRequireRanking() { MinExperience = minExperience, MaxExperience = maxExperience };
//             var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
//             request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//             request.downloadHandler = new DownloadHandlerBuffer();
//             request.SetRequestHeader("Content-Type", "application/json");
//             request.SetRequestHeader("Accept", "application/json");
//             request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

//             yield return request.SendWebRequest();
//             if (request.result == UnityWebRequest.Result.ConnectionError)
//             {
//                 Debug.LogError(request.error);
//             }
//             else//si no tuvo error
//             {
//                 string json = request.downloadHandler.text;//almacena respuesta del endpoint
//                 Debug.Log(json);
//                 // dataUserAll.Users = JsonUtility.FromJson<DataUserAll.DataUsers>(json).Users;
//                 SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);//convierte respuesta a json
//                 SaveDataAllUsers(stats);
//             }
//         }
//     }

//     private async void SaveDataAllUsers(SimpleJSON.JSONNode stats)
//     {
//         dataUserAll.Users.Clear();
//         int maxAvatarsToDownload = 10;
//         for (int i = 0; i < stats.Count && i < maxAvatarsToDownload; i++)
//         {
//             DataUserAll.DataUsers user = new DataUserAll.DataUsers();
//             user.id = stats[i]["idAlumno"];
//             user.userName = stats[i]["usernameG"];
//             user.totalExperience = stats[i]["totalExperience"];

//             //objeto avatar
//             DataUserAll.AvatarUsers avatar = new DataUserAll.AvatarUsers();
//             avatar.idAvatar = stats[i]["avatar"]["idAvatar"];
//             avatar.idAlumno = stats[i]["avatar"]["idAlumno"];
//             avatar.nombres = stats[i]["avatar"]["nombres"];
//             avatar.topC = stats[i]["avatar"]["topC"];
//             avatar.accessories = stats[i]["avatar"]["accessories"];
//             avatar.hair_Color = stats[i]["avatar"]["hair_Color"];
//             avatar.facial_Hair = stats[i]["avatar"]["facial_Hair"];
//             avatar.facial_Hair_Color = stats[i]["avatar"]["facial_Hair_Color"];
//             avatar.clothes = stats[i]["avatar"]["clothes"];
//             avatar.clothes_Color = stats[i]["avatar"]["clothes_Color"];
//             avatar.eyes = stats[i]["avatar"]["eyes"];
//             avatar.eyesbrow = stats[i]["avatar"]["eyesbrow"];
//             avatar.mouth = stats[i]["avatar"]["mouth"];
//             avatar.skin = stats[i]["avatar"]["skin"];

//             user.avatar = avatar;//asignar objeto avatar al usuario
//             dataUserAll.Users.Add(user);
//             user.urlAvatarUser = GenerateUrlToAvatarForRanking(avatar);
//             //StartCoroutine(downloadSVGUsersRanking(user.urlAvatarUser,i));
//         }
//         // Descarga de avatares en lotes
//         for (int i = 0; i < dataUserAll.Users.Count; i += batchSize)
//         {
//             var tasks = new List<System.Threading.Tasks.Task>();
//             for (int j = i; j < i + batchSize && j < dataUserAll.Users.Count; j++)
//             {
//                 tasks.Add(DownloadAvatarAsync(dataUserAll.Users[j], j));
//             }
//             await System.Threading.Tasks.Task.WhenAll(tasks);
//         }
//         //Debug.Log("Se completó el llenado de Avatars de usuarios");
//         //GameEvents.RankingRetrieved?.Invoke();
//     }

//     private async System.Threading.Tasks.Task DownloadAvatarAsync(DataUserAll.DataUsers user, int index)
//     {
//         UnityWebRequest www = UnityWebRequest.Get(user.urlAvatarUser);
//         www.SendWebRequest();
//         while (!www.isDone)
//         {
//             await System.Threading.Tasks.Task.Yield();
//         }
//         if (www.result == UnityWebRequest.Result.ConnectionError)
//         {
//             Debug.Log(www.error);
//         }
//         else
//         {
//             string bitString = Encoding.UTF8.GetString(www.downloadHandler.data);
//             var tessOptions = new VectorUtils.TessellationOptions
//             {
//                 StepDistance = 100.0f,
//                 MaxCordDeviation = 0.5f,
//                 MaxTanAngleDeviation = 0.1f,
//                 SamplingStepSize = 0.01f
//             };
//             var sceneInfo = SVGParser.ImportSVG(new StringReader(bitString));
//             var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);
//             Sprite sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
//             dataUserAll.Users[index].spriteAvatarUser = sprite;
//         }
//     }

//     private string GenerateUrlToAvatarForRanking(DataUserAll.AvatarUsers avatar)
//     {
//         string url = $"https://avataaars.io/?avatarStyle=Circle";
//         url += $"&topType={avatar.topC}";
//         url += $"&accessoriesType={avatar.accessories}";
//         url += $"&hairColor={avatar.hair_Color}";
//         url += $"&facialHairType={avatar.facial_Hair}";
//         url += $"&facialHairColor={avatar.facial_Hair_Color}";
//         url += $"&clotheType={avatar.clothes}";
//         url += $"&clotheColor={avatar.clothes_Color}";
//         url += $"&eyeType={avatar.eyes}";
//         url += $"&eyebrowType={avatar.eyesbrow}";
//         url += $"&mouthType={avatar.mouth}";
//         url += $"&skinColor={avatar.skin}";
//         return url;
//     }

//     IEnumerator downloadSVGUsersRanking(string url, int i)
//     {
//         UnityWebRequest www = UnityWebRequest.Get(url);

//         yield return www.SendWebRequest();
//         if (www.isHttpError || www.isNetworkError)
//         {
//             Debug.Log("Error while Receiving: " + www.error);
//         }
//         else
//         {
//             //Convert byte[] data of svg into string
//             string bitString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
//             var tessOptions = new VectorUtils.TessellationOptions()
//             {
//                 StepDistance = 100.0f,
//                 MaxCordDeviation = 0.5f,
//                 MaxTanAngleDeviation = 0.1f,
//                 SamplingStepSize = 0.01f
//             };
//             // Dynamically import the SVG data, and tessellate the resulting vector scene.
//             var sceneInfo = SVGParser.ImportSVG(new StringReader(bitString));
//             var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);

//             // Build a sprite with the tessellated geometry
//             Sprite sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
//             dataUserAll.Users[i].spriteAvatarUser = sprite;
//         }
//     }
// }

#endregion

#region Frank

/************************* Solucion Frank *************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Networking;
using static DataUserAll;

[Serializable]
public class DataToRequireRanking
{
    public int MinExperience;
    public int MaxExperience;
}
public class GetUsersApi : MonoBehaviour
{
    [SerializeField] private string URL = "http://simuladorpmp-servicio.bsginstitute.com/api/Gamificacion/ObtenerRankingGamificacion";
    [SerializeField] private DataUserAll dataUserAll;
    [SerializeField] private ScriptableObjectUser _user;
    [SerializeField] private TextMeshProUGUI _categoryTitle;


    private Queue<(string url, int index)> avatarQueue = new Queue<(string, int)>();
    private bool isDownloadingAvatars = false;
    private Dictionary<string, Sprite> avatarCache = new Dictionary<string, Sprite>();

    private int _firstBatch = 10;
    public enum Categories
    {
        Novato,
        Experto,
        Master,
        Leyenda
    }

    public void GetRankingInformation()//este metodo se llama al presionar en seccion ranking
    {

        var tExperience = _user.userInfo.user.detail.totalExperience;//obtenemos experiencia y colocamos en variable
        if (tExperience < 0)
        {
            return;
        }

        /*
        //Si se quiere que se ponga el filtro en base a la experiencia del usuario local
        switch (tExperience)//Setear titulo de categoria(nivel) 
        {
            case <= 4500:
                GetNovato();
                _categoryTitle.text = "Categoría:\nNovato";
                break;
            case <= 9500:
                GetExperto();
                _categoryTitle.text = "Categoría:\nExperto";
                break;
            case <= 15000:
                GetMaster();
                _categoryTitle.text = "Categoría:\nMaster";
                break;
            default:
                GetLeyenda();
                _categoryTitle.text = "Categoría:\nLeyenda";
                break;           
        }*/
        GetGlobal();//para que en principio se ejecute el filtro global
    }
    private void Start()
    {
        // Debug.Log(_user.userInfo.user.detail.totalExperience);
        //GetNovato();
        GetRankingInformation();
    }

    public void GetNovato()//filtro de experiencia en rango de novato
    {
        StopAllCoroutines();
        StartCoroutine(GetData(0, 4500));//con metodo GetData hago la solicitud al endpoint con los parametros de experiencia minima 0 y maxima 2000
        Debug.Log("GetNovato");
    }
    public void GetExperto()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(4501, 9500));
        Debug.Log("GetExperto");
    }
    public void GetMaster()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(9501, 15000));
        Debug.Log("GetMaster");
    }
    public void GetLeyenda()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(15001, 100000));
        Debug.Log("GetLeyenda");
    }
    public void GetGlobal()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(0, 100000));
        Debug.Log("GetGlobal");
    }



    private IEnumerator GetData(int minExperience, int maxExperience)
    {
        GameEvents.RequestRanking?.Invoke();
        using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
        {
            DataToRequireRanking dataLogin = new DataToRequireRanking() { MinExperience = minExperience, MaxExperience = maxExperience };
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");


            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else//si no tuvo error
            {
                string json = request.downloadHandler.text;//almacena respuesta del endpoint
                Debug.Log(json);
                // dataUserAll.Users = JsonUtility.FromJson<DataUserAll.DataUsers>(json).Users;
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);//convierte respuesta a json
                SaveDataAllUsers(stats);
            }
        }
    }

    private void SaveDataAllUsers(SimpleJSON.JSONNode stats)
    {
        dataUserAll.Users.Clear();
        // Crear lista temporal para procesar los usuarios
        List<DataUserAll.DataUsers> tempUsers = new List<DataUserAll.DataUsers>();

        for (int i = 0; i < stats.Count; i++)
        {
            DataUserAll.DataUsers user = new DataUserAll.DataUsers();
            user.id = stats[i]["idAlumno"];
            user.userName = stats[i]["usernameG"];
            user.totalExperience = stats[i]["totalExperience"];

            // Objeto avatar
            DataUserAll.AvatarUsers avatar = new DataUserAll.AvatarUsers();
            avatar.idAvatar = stats[i]["avatar"]["idAvatar"];
            avatar.idAlumno = stats[i]["avatar"]["idAlumno"];
            avatar.nombres = stats[i]["avatar"]["nombres"];
            avatar.topC = stats[i]["avatar"]["topC"];
            avatar.accessories = stats[i]["avatar"]["accessories"];
            avatar.hair_Color = stats[i]["avatar"]["hair_Color"];
            avatar.facial_Hair = stats[i]["avatar"]["facial_Hair"];
            avatar.facial_Hair_Color = stats[i]["avatar"]["facial_Hair_Color"];
            avatar.clothes = stats[i]["avatar"]["clothes"];
            avatar.clothes_Color = stats[i]["avatar"]["clothes_Color"];
            avatar.eyes = stats[i]["avatar"]["eyes"];
            avatar.eyesbrow = stats[i]["avatar"]["eyesbrow"];
            avatar.mouth = stats[i]["avatar"]["mouth"];
            avatar.skin = stats[i]["avatar"]["skin"];
            // ----
            user.avatar = avatar; // Asignar objeto avatar al usuario
            user.urlAvatarUser = GenerateUrlToAvatarForRanking(avatar);
            // Agregar a la lista temporal
            tempUsers.Add(user);
        }

        // Ordenar usuarios 
        tempUsers = tempUsers.OrderByDescending(user => user.totalExperience).ToList();

        // Agregar los usuarios ordenados a la lista final
        dataUserAll.Users.AddRange(tempUsers);

        // Procesar solo los 10 primeros usuarios
        for (int i = 0; i < _firstBatch; i++)
        {
            StartCoroutine(downloadSVGUsersRanking(dataUserAll.Users[i].urlAvatarUser, i));
        }

        // Procesar los usuarios restantes
        for (int i = _firstBatch; i < stats.Count; i++)
        {
            QueueAvatarDownload(dataUserAll.Users[i].urlAvatarUser, i);
        }
    }

    private string GenerateUrlToAvatarForRanking(DataUserAll.AvatarUsers avatar)
    {
        string url = $"https://avataaars.io/?avatarStyle=Circle";
        url += $"&topType={avatar.topC}";
        url += $"&accessoriesType={avatar.accessories}";
        url += $"&hairColor={avatar.hair_Color}";
        url += $"&facialHairType={avatar.facial_Hair}";
        url += $"&facialHairColor={avatar.facial_Hair_Color}";
        url += $"&clotheType={avatar.clothes}";
        url += $"&clotheColor={avatar.clothes_Color}";
        url += $"&eyeType={avatar.eyes}";
        url += $"&eyebrowType={avatar.eyesbrow}";
        url += $"&mouthType={avatar.mouth}";
        url += $"&skinColor={avatar.skin}";
        return url;
    }



    IEnumerator downloadSVGUsersRanking(string url, int i)
    {
        // Descargar el SVG del avatar
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error downloading SVG: {request.error}");
            }
            else
            {
                string svgData = request.downloadHandler.text;
                //Convert byte[] data of svg into string
                string bitString = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                var tessOptions = new VectorUtils.TessellationOptions()
                {
                    StepDistance = 200.0f,
                    MaxCordDeviation = 0.5f,
                    MaxTanAngleDeviation = 0.1f,
                    SamplingStepSize = 0.01f
                };

                // Dynamically import the SVG data, and tessellate the resulting vector scene.
                var sceneInfo = SVGParser.ImportSVG(new StringReader(bitString));
                var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);

                // Construye el sprite a partir de los datos de la escena
                Sprite sprite = GetOrCreateAvatar(url, bitString, tessOptions);
                dataUserAll.Users[i].spriteAvatarUser = sprite;
                Debug.Log($"SVG data for index {i} downloaded successfully.");
            }
        }
    }

    private void QueueAvatarDownload(string url, int index)
    {
        avatarQueue.Enqueue((url, index));

        // Esta corutina se encarga de procesar la cola de avatares en segundo plano
        if (!isDownloadingAvatars) // Si no se está procesando la cola, iniciar la corutina
        {
            StartCoroutine(ProcessAvatarQueue());
        }
    }

    private IEnumerator ProcessAvatarQueue()
    {
        isDownloadingAvatars = true;

        // Procesar la cola de avatares
        while (avatarQueue.Count > 0) // Mientras haya elementos en la cola
        {
            var (url, index) = avatarQueue.Dequeue(); // Obtener el primer elemento de la cola
            yield return downloadSVGUsersRanking(url, index); // Descargar el avatar
        }

        isDownloadingAvatars = false;
    }

    // Método para obtener un avatar de la caché o crear uno nuevo si no existe
    private Sprite GetOrCreateAvatar(string url, string svgData, VectorUtils.TessellationOptions tessOptions)
    {
        if (avatarCache.TryGetValue(url, out Sprite cachedSprite))  // Verificar si el avatar ya está en caché
        {
            return cachedSprite; // Devolver el avatar de la caché
        }

        // Generar nuevo sprite
        var sceneInfo = SVGParser.ImportSVG(new StringReader(svgData));
        var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);
        Sprite sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);

        avatarCache[url] = sprite; // Guardar el sprite en caché para evitar generación redundante
        return sprite;
    }
}

#endregion
