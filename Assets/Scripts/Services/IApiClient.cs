using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public interface IApiClient
    {
        Task<Response> GetRequest(string url);
        Task<Response> PostRequest(string url, string json);
    }
}