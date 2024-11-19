using CameraUI;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class BombAnimation : MonoBehaviour
{
    [SerializeField] private FXController _fxController;
    [SerializeField] private BezierCurve _bezierCurve;
    [SerializeField] private Transform _originTransform;
    private Transform _targetTransform;
    [SerializeField] private Transform _objectToMove;
    [SerializeField] private UIParticle _objectToMoveParticle;
    [SerializeField] private Vector3 _offset = new Vector3(200, -100, 0);
    [SerializeField][Range(0,2f)] private float _movementVelocity = .3f;
    [SerializeField][Range(100,1000f)] private float _rotationVelocity = 100f;
    [SerializeField] private GameObject _bombAnimationPrefab;
    [SerializeField] private UnityEvent _onFinishAnimation;
    private bool _animate;
    private float _currentTime;
    private void OnEnable()
    {
        _animate = false;
        // ConfigurePath();
    }

    public void SetTarget(Transform target)
    {
        _targetTransform = target;
    }
    private void ConfigurePath()
    {
        _bezierCurve._path.controlPoints[0].startPoint = transform.InverseTransformPoint(_originTransform.position);
        _bezierCurve._path.controlPoints[0].startTangent = _offset;
        _bezierCurve._path.controlPoints[0].endPoint = transform.InverseTransformPoint(_targetTransform.position);
        _bezierCurve._path.controlPoints[0].endTangent = _bezierCurve._path.controlPoints[0].endPoint + new Vector3(_offset.x, -_offset.y, _offset.z);
    }

    [ContextMenu("Start animation")]
    public void StartAnimation()
    {
        ConfigurePath();
        _objectToMove.gameObject.SetActive(true);
        _objectToMoveParticle.Play();
        _currentTime = 0;
        _animate = true;
        UIEvents.ActivateSmoke?.Invoke();
    }

    private void Update()
    {
        if (_animate)
        {
            
            _currentTime += Time.deltaTime * _movementVelocity;
            if (_currentTime > 1)
            {
                _animate = false;
                _objectToMove.gameObject.SetActive(false);
                _fxController.SetUIParticle(_targetTransform);
                _fxController.SetParticlePrefab(_bombAnimationPrefab);
                _fxController.SpawnParticle();
                UIEvents.ActivateExplosion?.Invoke();
                _onFinishAnimation?.Invoke();
                return;
            }
            _objectToMove.position = transform.TransformPoint(_bezierCurve.GetPositionInPercentage(_currentTime));
            _objectToMove.Rotate(0,0,Time.deltaTime * _rotationVelocity);
            
        }
    }
}
