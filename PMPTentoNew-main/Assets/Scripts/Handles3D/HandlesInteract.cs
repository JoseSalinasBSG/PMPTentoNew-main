using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Handles3D
{
    public class HandlesInteract : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Variables

        [SerializeField] private Vector3 initTouch;

        private bool isPressed;
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void OnEnable()
        {
            EnhancedTouchSupport.Enable();

            Touch.onFingerMove += TouchOnonFingerMove;
        }

        private void OnDisable()
        {
            Touch.onFingerMove -= TouchOnonFingerMove;
            EnhancedTouchSupport.Disable();

        }

        private void TouchOnonFingerMove(Finger obj)
        {
            if (isPressed)
            {
               Debug.Log("sdsd");
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods
        
        public void OnPointerDown(PointerEventData eventData)
        {
            initTouch = eventData.position;
            isPressed = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
        }
        
        #endregion


        
    }

}