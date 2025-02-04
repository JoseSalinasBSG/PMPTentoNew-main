using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Services
{
    public struct Response
    {
        public string response;
        public bool success;
    }

    public class ApiService : IApiClient
    {
        public async Task<Response> GetRequest(string url)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            AddHeader(unityWebRequest);
            await unityWebRequest.SendWebRequest();
            return new Response
            {
                response = unityWebRequest.downloadHandler.text,
                success = unityWebRequest.result == UnityWebRequest.Result.Success
            };
        }

        public async Task<Response> PostRequest(string url, string json)
        {
            UnityWebRequest unityWebRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
            unityWebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            AddHeader(unityWebRequest);
            await unityWebRequest.SendWebRequest();
            return new Response
            {
                response = unityWebRequest.downloadHandler.text,
                success = unityWebRequest.result == UnityWebRequest.Result.Success
            };
        }

        public void AddHeader(UnityWebRequest unityWebRequest)
        {
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.SetRequestHeader("Accept", "application/json");
            unityWebRequest.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
        }
    }
}