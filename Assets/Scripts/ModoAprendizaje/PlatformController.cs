using System;
using System.Collections;
using System.Collections.Generic;
using CameraUI;
using Unity.Mathematics;
using UnityEngine;

namespace ModoAprendizaje
{
    public class PlatformController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject _prefabs;
        [SerializeField] private PlatformDomainInformation _domainInfoPlatform;
        [SerializeField] private float _distanceFix;
        [SerializeField] private float _multiplierDistance;
        [SerializeField] private int _multiplier;
        [SerializeField] private int _numIntances;
        [SerializeField] private BezierCurve _bezierCurve;
        [SerializeField] private RectTransform _rectTransform;

        private float _currentPercentage;
        private float _lastPosition;
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            // CreatePlatforms();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods

        public void SetInstances(int instances)
        {
            _numIntances = instances;
        }
        [ContextMenu("Create platforms")]
        public void CreatePlatforms()
        {
            for (int i = 0; i < _numIntances; i++)
            {
                Instantiate(_prefabs, transform.TransformPoint(_bezierCurve.GetPositionInPercentage(_distanceFix / _multiplierDistance * i  ) )* _multiplier, quaternion.identity, transform);
            }
        }

        public PlatformItem CreatePlatformInformation(string tittle, string description, string detail)
        {
            var instance = Instantiate(_domainInfoPlatform, transform.TransformPoint(_bezierCurve.GetPositionInPercentage(_distanceFix / _multiplierDistance * _numIntances  ) )* _multiplier, quaternion.identity, transform);
            instance.SetDescription(description);
            instance.SetTittle(tittle);
            instance.SetDetail(detail, description);
            var vector3 = instance.transform.localPosition;
            vector3.x = 0;
            instance.transform.localPosition = vector3;
            _numIntances++;
            _lastPosition = instance.transform.localPosition.y - 150;
            _lastPosition *= -1;
            var rTemp = _rectTransform.sizeDelta;
            rTemp.y = _lastPosition;
            _rectTransform.sizeDelta = rTemp;
            return instance.GetComponent<PlatformItem>();
        }
        public PlatformItem CreatePlatform()
        {
            var instance = Instantiate(_prefabs, transform.TransformPoint(_bezierCurve.GetPositionInPercentage(_distanceFix / _multiplierDistance * _numIntances  ) )* _multiplier, quaternion.identity, transform);
            _numIntances++;
            _lastPosition = instance.transform.localPosition.y - 150;
            _lastPosition *= -1;
            var rTemp = _rectTransform.sizeDelta;
            rTemp.y = _lastPosition;
            _rectTransform.sizeDelta = rTemp;
            return instance.GetComponent<PlatformItem>();
        }
        
        #endregion

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < _numIntances; i++)
            {
                Gizmos.DrawSphere(transform.TransformPoint(_bezierCurve.GetPositionInPercentage(_distanceFix / _multiplierDistance  * i )), 100);
            }
        }
    }

}