using UnityEngine;
using UnityEngine.Serialization;

namespace UI.CommentBox
{
    public class CommentSystem : MonoBehaviour
    {
        #region Variables

        [Header("General")]
        [Tooltip("Button to activate comment box")]
        [SerializeField] private UnityEngine.UI.Button _commentButton;
        [Tooltip("Comment box container")]
        [SerializeField] private RectTransform _commentBox;
        [Tooltip("Activator to change comment box height ")]
        [SerializeField] private ChangeHeightCommentBox _changeHeightCommentBox;
        [Tooltip("Panel to block interaction from user with objects in scene")]
        [SerializeField] private UnityEngine.UI.Button _panelToBlockInteracttionBehind;
        [FormerlySerializedAs("heightResize")]
        [Header("Resize Content")]
        [SerializeField] private float _heightResize;
        [SerializeField] private RectTransform _contentToResize;
        private bool _onActiveCommentBox;
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            
            //Add listeners to buttons interactables
            _commentButton.onClick.AddListener(EnableOrHideCommentBox);
            _panelToBlockInteracttionBehind.onClick.AddListener(HideCommentBox);
            
            // Set if comments box is enable
            _onActiveCommentBox = _commentBox.gameObject.activeInHierarchy;
            
            
            // Events when end comment box transition  
            _changeHeightCommentBox.OnUpCommentBox += OnUpCommentBox;
            _changeHeightCommentBox.OnDownCommentBox += OnDownCommentBox;
        }

        private void HideCommentBox()
        {
            // Indicate comments box height change 
            _changeHeightCommentBox.IsChangeHeight = true;
            
            // Set transition to hide 
            _changeHeightCommentBox.ChangeHeightAnimation(0);
            _changeHeightCommentBox.HideComments();
        }

        private void OnDisable()
        {
            _commentButton.onClick.RemoveListener(EnableOrHideCommentBox);
        }

        private void Update()
        {
            // Only change height when is drag or there is trasitio
            if (_changeHeightCommentBox.IsChangeHeight)
            {
                var ownTransformSizeDelta = _contentToResize.sizeDelta;
                ownTransformSizeDelta.y = _commentBox.sizeDelta.y - _heightResize;
                _contentToResize.sizeDelta = ownTransformSizeDelta;
            }
            
        }
        
        #endregion

        #region Methods
        
        private void EnableOrHideCommentBox()
        {
            _onActiveCommentBox = !_onActiveCommentBox;
            _commentBox.gameObject.SetActive(_onActiveCommentBox);
            if (_onActiveCommentBox)
            {
                _changeHeightCommentBox.IsChangeHeight = true;
                _changeHeightCommentBox.ChangeHeightAnimation(_changeHeightCommentBox.MaxHeight);
            }
        }
        private void OnDownCommentBox()
        {
            _onActiveCommentBox = _commentBox.gameObject.activeInHierarchy;
            _panelToBlockInteracttionBehind.gameObject.SetActive(false);
        }

        private void OnUpCommentBox()
        {
            _panelToBlockInteracttionBehind.gameObject.SetActive(true);
        }
        
        #endregion

    }

}