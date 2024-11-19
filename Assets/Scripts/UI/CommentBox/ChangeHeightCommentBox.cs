using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CommentBox
{
    public class ChangeHeightCommentBox : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _referenceTransform;
        [SerializeField] private int height;
        [SerializeField] private UnityEngine.UI.Button _buttonHeight;
        [Tooltip("Altura maxima a la que se expandirá la caja de comentarios")]
        [SerializeField] private float _maxHeight;
        [SerializeField] private float _timeTransition;

        public Action OnUpCommentBox;
        public Action OnDownCommentBox;
        private bool _isPressed;
        private Vector2 _vector;
        private float offset;

        private float _middleHeight;
        private Vector3 localPositionPointer;
        private Vector2 ownTransformSizeDelta;

        private bool inTransition;
        private bool _isChangeHeight;

        public float magnitude;
        private float yOld;
        public bool IsChangeHeight
        {
            get => _isChangeHeight;
            set => _isChangeHeight = value;
        }
        public float MaxHeight
        {
            get => _maxHeight;
            set => _maxHeight = value;
        }
        private void Start()
        {
            _middleHeight = _maxHeight / 2; 
        }

        // private void Update()
        // {
        //     
        //     // if (isPressed)
        //     // {
        //         if (Input.GetMouseButtonDown(0))
        //         {
        //             
        //         }
        //         else if (Input.GetMouseButtonUp(0))
        //         {
        //             magnitude = Pointer.current.delta.y.value;
        //         }
        //     // }
        // }

        public void OnPointerDown(PointerEventData eventData)
        {
            localPositionPointer = _referenceTransform.InverseTransformPoint(eventData.position);
            offset = _referenceTransform.sizeDelta.y - localPositionPointer.y;
            _isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Debug.Log(magnitude);
            _isPressed = false;
            
            if (ownTransformSizeDelta.y >= _middleHeight)
            {
                // ownTransformSizeDelta.y = _maxHeight;
                // _referenceTransform.sizeDelta = ownTransformSizeDelta;
                ChangeHeightAnimation(_maxHeight);
                // StopAllCoroutines();
                // StartCoroutine(AnimTransition(_maxHeight));
            }
            else
            {
                ChangeHeightAnimation(0);
                HideComments();
                // StopAllCoroutines();
                // StartCoroutine(AnimTransition(0));
                // ownTransformSizeDelta.y = 0;
                // _referenceTransform.sizeDelta = ownTransformSizeDelta;
            }
        }

        public void ChangeHeightAnimation(float heightTarget)
        {
            StopAllCoroutines();
            StartCoroutine(AnimTransition(heightTarget));
        }

        public void HideComments()
        {
            StartCoroutine(IHideComments());

        }

        public void OnDrag(PointerEventData eventData)
        {
            _isChangeHeight = true;
            ownTransformSizeDelta = _referenceTransform.sizeDelta;
            localPositionPointer = _referenceTransform.InverseTransformPoint(eventData.position);
            ownTransformSizeDelta.y = localPositionPointer.y + offset;
            if (ownTransformSizeDelta.y <= _maxHeight)
            {
                _referenceTransform.sizeDelta = ownTransformSizeDelta;
            }
        }

        IEnumerator IHideComments()
        {
            yield return new WaitUntil(() => !inTransition);
            _referenceTransform.gameObject.SetActive(false);
            OnDownCommentBox?.Invoke();
        }

        IEnumerator AnimTransition(float targetHeight)
        {
           
            inTransition = true;
            var initialTime = 0f;
            var actualTime = initialTime;
            var initValue = _referenceTransform.sizeDelta.y;
            var actualHeight = 0f;
            while (actualTime <= 1)
            {
                actualTime += Time.deltaTime / _timeTransition;
                actualHeight = Mathf.Lerp(initValue, targetHeight, actualTime);
                ownTransformSizeDelta.y = actualHeight;
                _referenceTransform.sizeDelta = ownTransformSizeDelta;
                yield return null;
            }
            inTransition = false;
            _isChangeHeight = false;
            if (targetHeight != 0)
            {
                OnUpCommentBox?.Invoke();
            }
        }
    }
}
