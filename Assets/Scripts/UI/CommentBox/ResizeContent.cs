using System;
using UnityEngine;

namespace UI
{
    public class ResizeContent : MonoBehaviour
    {
        [SerializeField] private RectTransform _referenceTransform;
        [SerializeField] private int height;

        private RectTransform _ownTransform;

        private void Start()
        {
            _ownTransform = gameObject.GetComponent<RectTransform>();
        }

        private void Update()
        {
            var ownTransformSizeDelta = _ownTransform.sizeDelta;
            ownTransformSizeDelta.y = _referenceTransform.sizeDelta.y - height;
            _ownTransform.sizeDelta = ownTransformSizeDelta;
        }
    }
}
