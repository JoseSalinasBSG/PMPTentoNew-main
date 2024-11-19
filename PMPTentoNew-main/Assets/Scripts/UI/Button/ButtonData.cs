using Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Button
{
    public class ButtonData : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _tittleButton;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private ButtonAnimation _buttonAnimation;

        [SerializeField] private ButtonDomainController _butonDomainController;
        [SerializeField] private string _description;
        [SerializeField] private int _index;
        [SerializeField] private string _indexS;

        public int Index
        {
            get => _index;
            set => _index = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public string IndexS
        {
            get => _indexS;
            set => _indexS = value;
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

        public void SetData(string tittle, Color color, ButtonDomainController buttonDomainController, int index, string description, string indexS = "")
        {
            _tittleButton.text = tittle;
            _buttonImage.color = color;
            _butonDomainController = buttonDomainController;
            _index = index;
            _description = description;
            _indexS = indexS;
        }

        public void SetCurrentButton()
        {
            _butonDomainController.CurrentButton = this;
            transform.SetSiblingIndex(transform.parent.childCount );
            _buttonAnimation.StartAnimation();
        }

        public void ExitCurrentButton()
        {
            _buttonAnimation.StartInverseCustomAnimation();
        }
        #endregion

    }
}

