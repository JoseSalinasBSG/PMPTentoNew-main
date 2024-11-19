using UnityEditor;
using UnityEngine;

namespace CameraUI
{
    

    public class BezierCurve : MonoBehaviour
    {
        #region Variables

        [SerializeField] public ScriptableObjectPath _path;
        [SerializeField] private float _beta1;
        [SerializeField] private float _beta2;
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods
        
        Vector3 CalculateBezierPoint(float t, Vector3 startPoint, Vector3 startRotation, Vector3 endRotation, Vector3 endPoint)
        {
            // float u = 1 - t;
            float tt = t * t;
            // float uu = u * u;
            // float uuu = uu * u;
            float ttt = tt * t;
            //
            // Vector3 p = uuu * startPoint;
            // p += 3 * uu * t * startRotation;
            // p += 3 * u * tt * endRotation;
            // p += ttt * endPoint;
            
            // Freya homer
            Vector3 p = startPoint * (-ttt + 3 * tt - 3 * t + 1) +
                        startRotation * (3 * ttt - 6 * tt + 3 * t) +
                        endRotation * (-3 * ttt + 3 * tt) +
                        endPoint * (ttt);
            return p;
        }
        public void ConfigurateStartTangents(int index)
        {
            SetStarPointPosition(index);

            _path.controlPoints[index].startTangent = 2 * _path.controlPoints[index-1].endPoint - _path.controlPoints[index-1].endTangent;
            // _path.controlPoints[index].endTangent = _path.controlPoints[index-1].startPoint + 4 * ( _path.controlPoints[index-1].endPoint - _path.controlPoints[index-1].endTangent);
        }
        public void ConfigurateGeometricContinues(int index)
        {
            SetStarPointPosition(index);
            _path.controlPoints[index].startTangent = _path.controlPoints[index-1].endPoint + (_path.controlPoints[index-1].endPoint - _path.controlPoints[index-1].endTangent) * _beta1;
            // _path.controlPoints[index].endTangent = _path.controlPoints[index-1].endPoint + (_path.controlPoints[index-1].endPoint - _path.controlPoints[index-1].endTangent) * ( 2 * _beta1 - _beta1 * _beta1 + _beta2 / 2 ) + _beta1 * _beta1 * (_path.controlPoints[index-1].startTangent - _path.controlPoints[index-1].endTangent);
        }

        private void SetStarPointPosition(int index)
        {
            _path.controlPoints[index].startPoint = _path.controlPoints[index - 1].endPoint;
        }

        public Vector3 GetPositionInPercentage(float percentage)
        {
            var index = (int)Mathf.Floor(percentage * _path.controlPoints.Length);
            var maxRange = 1f / _path.controlPoints.Length; 
            // Debug.Log("index: " + index + " current value in trace: " + (percentage - index * maxRange) + " maxRange: " + maxRange);
            return CalculateBezierPoint((percentage - index * maxRange) / (maxRange),
                _path.controlPoints[index].startPoint, _path.controlPoints[index].startTangent,
                _path.controlPoints[index].endTangent, _path.controlPoints[index].endPoint);
        }
        
        // public float distanceToT(float[] LUT, float distance )
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < _path.controlPoints.Length; i++)
            {
            

                // Handles.PositionHandle(_controlPoints[i].startPoint.position, Quaternion.identity);
                Handles.DrawBezier(transform.TransformPoint(_path.controlPoints[i].startPoint), transform.TransformPoint(_path.controlPoints[i].endPoint), transform.TransformPoint(_path.controlPoints[i].startTangent), transform.TransformPoint(_path.controlPoints[i].endTangent), _path.colorPath, null,10 );
                Gizmos.DrawLine(transform.TransformPoint(_path.controlPoints[i].startPoint), transform.TransformPoint(_path.controlPoints[i].startTangent));
                Gizmos.DrawLine(transform.TransformPoint(_path.controlPoints[i].endPoint), transform.TransformPoint(_path.controlPoints[i].endTangent));
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.TransformPoint(_path.controlPoints[i].startPoint), .03f);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(transform.TransformPoint(_path.controlPoints[i].endPoint), .03f);
                if (i -1 > -1)
                {
                    ConfigurateStartTangents(i);
                    // Gizmos.color = Color.red;
            
                }
                // else
                // {
                Gizmos.color = Color.green;
                // }
            
                Gizmos.DrawSphere(transform.TransformPoint(_path.controlPoints[i].startTangent), .03f);
                Gizmos.DrawSphere(transform.TransformPoint(_path.controlPoints[i].endTangent), .03f);
            }
        }

#endif
        #endregion


       
    }
    

}