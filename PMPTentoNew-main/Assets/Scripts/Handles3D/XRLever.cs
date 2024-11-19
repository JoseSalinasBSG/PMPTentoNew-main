using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// An interactable lever that snaps into an on or off position by a direct interactor
    /// </summary>
    public class XRLever : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        const float k_LeverDeadZone = 0.3f; // Prevents rapid switching between on and off states when right in the middle

        [SerializeField]
        [Tooltip("The object that is visually grabbed and manipulated")]
        Transform m_Handle = null;

        [SerializeField]
        [Tooltip("The value of the lever")]
        bool m_Value = false;

        [SerializeField]
        [Tooltip("If enabled, the lever will snap to the value position when released")]
        bool m_LockToValue;

        [SerializeField]
        [Tooltip("Angle of the lever in the 'on' position")]
        [Range(-90.0f, 90.0f)]
        float m_MaxAngle = 90.0f;

        [SerializeField]
        [Tooltip("Angle of the lever in the 'off' position")]
        [Range(-90.0f, 90.0f)]
        float m_MinAngle = -90.0f;

        [SerializeField]
        [Tooltip("Events to trigger when the lever activates")]
        UnityEvent m_OnLeverActivate = new UnityEvent();

        [SerializeField]
        [Tooltip("Events to trigger when the lever deactivates")]
        UnityEvent m_OnLeverDeactivate = new UnityEvent();
        [SerializeField]private RectTransform _marker;
        [SerializeField] private Collider _collider;
        public Vector3 _direction;

        /// <summary>
        /// The object that is visually grabbed and manipulated
        /// </summary>
        public Transform handle
        {
            get => m_Handle;
            set => m_Handle = value;
        }

        /// <summary>
        /// The value of the lever
        /// </summary>
        public bool value
        {
            get => m_Value;
            set => SetValue(value, true);
        }

        /// <summary>
        /// If enabled, the lever will snap to the value position when released
        /// </summary>
        public bool lockToValue { get; set; }

        /// <summary>
        /// Angle of the lever in the 'on' position
        /// </summary>
        public float maxAngle
        {
            get => m_MaxAngle;
            set => m_MaxAngle = value;
        }

        /// <summary>
        /// Angle of the lever in the 'off' position
        /// </summary>
        public float minAngle
        {
            get => m_MinAngle;
            set => m_MinAngle = value;
        }

        public bool isSelected;
        [SerializeField] private float maxAngleDistance;
        [SerializeField] private float minAngleDistance;
        [SerializeField] private float lookAngle;
        [SerializeField]private float _offset;

        /// <summary>
        /// Events to trigger when the lever activates
        /// </summary>
        public UnityEvent onLeverActivate => m_OnLeverActivate;

        /// <summary>
        /// Events to trigger when the lever deactivates
        /// </summary>
        public UnityEvent onLeverDeactivate => m_OnLeverDeactivate;

        private Vector3 _firstPosition;
        void Start()
        {
            SetValue(m_Value, true);
        }

        // void OnEnable()
        // {
        //     // selectEntered.AddListener(StartGrab);
        //     // selectExited.AddListener(EndGrab);
        // }
        //
        // void OnDisable()
        // {
        //     // selectEntered.RemoveListener(StartGrab);
        //     // selectExited.RemoveListener(EndGrab);
        // }

        // void StartGrab(SelectEnterEventArgs args)
        // {
        //     m_Interactor = args.interactorObject;
        // }

        // void EndGrab(SelectExitEventArgs args)
        // {
        //     SetValue(m_Value, true);
        //     m_Interactor = null;
        // }

        // public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        // {
        //     base.ProcessInteractable(updatePhase);
        //
        //     if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        //     {
        //         if (isSelected)
        //         {
        //             UpdateValue();
        //         }
        //     }
        // }
        public void OnPointerDown(PointerEventData eventData)
        {
            isSelected = true;
            _firstPosition = Touchscreen.current.position.value;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isSelected = false;

            SetValue(m_Value, true);

        }
        // void OnEnable()
        // {
        //     EnhancedTouchSupport.Enable();
        //
        //     InputSystem.EnhancedTouch.Touch.onFingerMove += TouchOnonFingerMove;
        // }
        //
        // private void OnDisable()
        // {
        //     InputSystem.EnhancedTouch.Touch.onFingerMove -= TouchOnonFingerMove;
        //     EnhancedTouchSupport.Disable();
        //
        // }
        private void TouchOnonFingerMove(Finger obj)
        {
            // if (isSelected)
            // {
                UpdateValue();
            // }
        }
        // private void Update()
        // {
        //     UpdateValue();
        // }

        Vector3 GetLookDirection()
        {
            // Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
            //     _direction = (Vector3)Touchscreen.current.position.ReadValue() - m_Handle.position;
            // // Debug.Log(direction);
            // _direction = transform.InverseTransformDirection(_direction);
            // _direction.x = 0;
            // Debug.Log(Touchscreen.current.position.value + " " + _marker.position);
            _direction = new Vector3(_direction.x,  _direction.y,((Vector3)Touchscreen.current.position.value - _marker.position).y + _offset);
            // var direction = _direction;
            Debug.DrawRay(m_Handle.position, _direction);
            return _direction.normalized;
        }

        void UpdateValue()
        {
            var lookDirection = GetLookDirection();
            lookAngle = Mathf.Atan2(lookDirection.z, lookDirection.y) * Mathf.Rad2Deg;

            if (m_MinAngle < m_MaxAngle)
                lookAngle = Mathf.Clamp(lookAngle, m_MinAngle, m_MaxAngle);
            else
                lookAngle = Mathf.Clamp(lookAngle, m_MaxAngle, m_MinAngle);

            maxAngleDistance = Mathf.Abs(m_MaxAngle - lookAngle);
            minAngleDistance = Mathf.Abs(m_MinAngle - lookAngle);

            if (m_Value)
                maxAngleDistance *= (1.0f - k_LeverDeadZone);
            else
                minAngleDistance *= (1.0f - k_LeverDeadZone);

            var newValue = (maxAngleDistance < minAngleDistance);
            SetHandleAngle(lookAngle);

            SetValue(newValue);
        }

        void SetValue(bool isOn, bool forceRotation = false)
        {
            m_Value = isOn;
            if (Mathf.Abs(lookAngle) >= 5)
            {
                if (!isSelected)
                {
                    if (forceRotation)
                        SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
                    // Debug.Log("no selected");
                    if (m_Value)
                    {
                        // Debug.Log("on lever activate");
                        m_OnLeverActivate.Invoke();
                    }
                    else
                    {
                        // Debug.Log("on lever deactivate");
                        m_OnLeverDeactivate.Invoke();
                        enabled = false;
                    }
                }
            }
            else
            {
                if (!isSelected)
                {
                    SetHandleAngle(m_MaxAngle);
                }
            }
            // if (m_Value == isOn || isSelected)
            // {
            //     if (forceRotation)
            //         SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
            //
            //     return;
            // }
            //
            // m_Value = isOn;
            // Debug.Log(m_Value);
            // if (!isSelected)
            // {
            //     
            // }
            // else
            // {
            //     Debug.Log("selected");
            // }
            //

            // if (!isSelected && (m_LockToValue || forceRotation))
            //     SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }

        void SetHandleAngle(float angle)
        {
            if (m_Handle != null)
                m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0);
        }

        void OnDrawGizmosSelected()
        {
            var angleStartPoint = transform.position;

            if (m_Handle != null)
                angleStartPoint = m_Handle.position;

            const float k_AngleLength = 2f;

            var angleMaxPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MaxAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;
            var angleMinPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MinAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(angleStartPoint, angleMaxPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(angleStartPoint, angleMinPoint);
        }

        void OnValidate()
        {
            SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }

        private void Update()
        {
            if (isSelected)
            {
                UpdateValue();

            }
        }
    }
}
