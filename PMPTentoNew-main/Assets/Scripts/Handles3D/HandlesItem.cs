using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Handles3D
{
    public class HandlesItem : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private RectTransform _rect;

        private string _id;

        public RectTransform Rect
        {
            get => _rect;
            set => _rect = value;
        }
        public string ID
        {
            get => _id;
            set => _id = value;
        }

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

        public void SetData(string label, string id)
        {
            _label.text = label;
            _id = id;
        }

        
        #endregion

    }

}