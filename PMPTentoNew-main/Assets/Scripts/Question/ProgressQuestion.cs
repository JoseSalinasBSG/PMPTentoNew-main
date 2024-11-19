using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Question
{
    public class ProgressQuestion : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float _spacing;
        [SerializeField] private RectTransform _widthReference;
        [SerializeField] private RectTransform _container;
        [SerializeField] private ProgressItem _prefab;
        [SerializeField] private TextMeshProUGUI _label;

        private float width;
        private int _maxQuestions;
        public string Label
        {
            get => _label.text;
            set
            {
                _label.text = $"{value}/{_maxQuestions}";
                
                
            }
        }
                
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            
            
            
            // for (int i = 0; i < _count; i++)
            // {
            //     var item = Instantiate(_prefab, _container);
            //     item.RectOwnTransform.sizeDelta = new Vector2(width, 23);
            // }
            FillCountOfQuestions();
        }

        private void FillCountOfQuestions()//metodo para enumerar las cajitas de numero de preguntas
        {
            for (int i = 0; i < _container.childCount; i++)
            {
                _container.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();

            }

        }

        #endregion

        #region Methods

        public void CalculateWidth(int count)
        {
            _maxQuestions = count;
            _label.text = $"0/{count}";
            width = _widthReference.rect.width;
            width /= count;
            if (_prefab.RectOwnTransform.sizeDelta.x < width)
            {
                width = _prefab.RectOwnTransform.sizeDelta.x;
            }
        }

        public ProgressItem CreateItem(int number)
        {
           
            var item = Instantiate(_prefab, _container);
            //item.RectOwnTransform.sizeDelta = new Vector2(width, 23);
            return item;
        }
        
        #endregion

    }

}