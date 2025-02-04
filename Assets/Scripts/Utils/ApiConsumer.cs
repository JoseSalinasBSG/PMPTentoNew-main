using Assets.Scripts.Models;
using Assets.Scripts.ScriptableCreator;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class ApiConsumer : MonoBehaviour
    {
        private IApiClient _apiClient;
        private IDataParser _dataParser;

        private ApiEndpoints _apiEndpoints;

        private void Awake()
        {
            _apiEndpoints = Resources.Load<ApiEndpoints>("api/ApiEndpoints");
        }
        private void Start()
        {
            _dataParser = ServiceLocator.Instance.Get<IDataParser>();
            _apiClient = ServiceLocator.Instance.Get<IApiClient>();
            LoadDomains();
        }

        private async void LoadDomains()
        { 
            Response response = await _apiClient.GetRequest(_apiEndpoints.base_url + _apiEndpoints.getDomains);
            if (response.success)
            {
                var domains = _dataParser.Deserialize<Root>(response.response);
                Debug.Log(response.response);
                Debug.Log(domains.domains[0].nombre);
            }
            else
            {
                Debug.LogError("Error al cargar los datos: " + response.response);
            }
        }
    }
}