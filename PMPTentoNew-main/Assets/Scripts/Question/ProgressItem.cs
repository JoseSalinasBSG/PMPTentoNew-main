using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Question
{
    public class ProgressItem : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _correctColor;
        [SerializeField] private Color _incorrectColor;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Image _currentItemMarker;
        [SerializeField] private RectTransform _rectOwnTransform;
        //[SerializeField] private TextMeshProUGUI numberQuestionText;

        public RectTransform RectOwnTransform
        {
            get => _rectOwnTransform;
            set => _rectOwnTransform = value;
        }

        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            _itemImage.color = _defaultColor;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods

        public void SetCurrentItem()
        {
            _currentItemMarker.gameObject.SetActive(true);
        }

        public void SetCorrectSelection()
        {
            //_currentItemMarker.gameObject.SetActive(false);
            _itemImage.color = _correctColor;
            //numberQuestionText.color = Color.white;

        }
        public void RemoveCurrentItem()
        {
            _currentItemMarker.gameObject.SetActive(false);
            // _itemImage.color = _correctColor;

        }
        public void SetIncorrectSelection()
        {
            //_currentItemMarker.gameObject.SetActive(false);
            _itemImage.color = _incorrectColor;
            //numberQuestionText.color = Color.white;
        }

        /*
        public void SetNumberQuestion(int number)
        {
            int numQuestion = number + 1;
            numberQuestionText.SetText(numQuestion.ToString());
        }*/

        #endregion

    }

}