using Extras;
using UnityEngine;
using Assets.Scripts.Services;

namespace Assets.Scripts.Utils
{
    public class Installer : MonoBehaviour
    {
        //protected override void Awake()
        private void Awake()
        {
            //base.Awake();
            ServiceLocator.Instance.Register<IDataParser>(new JsonDataParser());
            ServiceLocator.Instance.Register<IApiClient>(new ApiService());
        }
    }
}