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

        private float width;
                
        #endregion

        #region Methods

        public void CalculateWidth(int count)
        {
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
            item.SetNumberQuestion(number);
            return item;
        }
        
        #endregion

    }

}