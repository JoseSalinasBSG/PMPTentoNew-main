using UnityEngine;

namespace Question
{
    public class ProgressQuestion : MonoBehaviour
    {
        #region Variables

        [SerializeField] private RectTransform _container;
        [SerializeField] private ProgressItem _prefab;
                
        #endregion

        #region Methods

        public ProgressItem CreateItem(int number)
        {           
            var item = Instantiate(_prefab, _container);
            item.SetNumberQuestion(number);
            return item;
        }
        
        #endregion

    }

}