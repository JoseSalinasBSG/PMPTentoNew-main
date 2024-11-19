using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Button
{
    public class ButtonDomainController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private DomainsAndTaskSO _domainsAndTask;
        [SerializeField]
        private List<ButtonData> _buttons;
        [SerializeField] private ButtonData _buttonPrefab;
        [SerializeField] private float _heightButton;
        [SerializeField] private Vector2 _initPosition;
        [SerializeField] private List<Color> buttonColors = new List<Color>{ Color.red, Color.green, Color.blue, Color.yellow, Color.magenta };
        [SerializeField] private TextMeshProUGUI _domainDescription;
        [SerializeField] private GameObject _buttonToContinue;
        
        // [SerializeField] private float _hMin;
        // [SerializeField] private float _hMax;
        // [SerializeField] private float _sMin;
        // [SerializeField] private float _sMax;
        // [SerializeField] private float _vMin;
        // [SerializeField] private float _vMax;

        private ButtonData _currentButton;
        private bool _chooseOption;
        private List<Color> _colorsBackup;
        public ButtonData CurrentButton
        {
            get => _currentButton;
            set
            {
                if (_currentButton)
                {
                    
                    _currentButton.ExitCurrentButton();
                    _currentButton.transform.SetSiblingIndex(transform.childCount - (_buttons.Count - _currentButton.Index));
                }
                else
                {
                    _chooseOption = true;
                    //_buttonToContinue.SetActive(_chooseOption);
                }
                _currentButton = value;
                _domainDescription.text = _currentButton.Description;
                GameEvents.GetIdDomain?.Invoke(Int32.Parse(_currentButton.IndexS));
            } 
        }

        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            _colorsBackup = new List<Color>(buttonColors);
            // CreateButton("Persona");
            // CreateButton("Proceso");
            // CreateButton("Entorno \nempresarial");
        }

        
        #endregion

        #region Methods

        public void ResetValues()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                Destroy(_buttons[i].gameObject);
            }
            _buttons.Clear();
            buttonColors = new List<Color>(_colorsBackup);
            //_buttonToContinue.SetActive(false);
        }
        [ContextMenu("CreateButton")]
        public void CreateButton()
        {
            CreateButton("Test");
        }
        
        public void CreateButton(string tittle, string id = "")
        {
            var item = Instantiate(_buttonPrefab, transform, true);
            var rTransform = item.GetComponent<RectTransform>();
            // rTransform.anchoredPosition = new Vector2(0, rTransform.sizeDelta.y);
            rTransform.sizeDelta = new Vector2(0, rTransform.sizeDelta.y);
            var indexRandom = Random.Range(0, buttonColors.Count);
            var color = buttonColors[indexRandom];
            buttonColors.RemoveAt(indexRandom);
            item.SetData(tittle, color, this, _buttons.Count, _domainsAndTask.DomainDetail.First(x => x.id == int.Parse(id)).description, id);
            item.transform.localScale = Vector3.one;
            var posTemp = new Vector2(item.transform.localPosition.x,_initPosition.y);
            posTemp.y -= _heightButton * (_buttons.Count );
            item.transform.localPosition = posTemp;
            _buttons.Add(item);
        }
        // Color GenerateRandomColor()
        // {
        //     // Puedes ajustar estos valores según tus preferencias
        //     float hue = Random.Range(_hMin, _hMax);
        //     float saturation = Random.Range(_sMin, _sMax);
        //     float value = Random.Range(_vMin, _vMax);
        //
        //     // Crea y devuelve el color en formato RGB
        //     return Color.HSVToRGB(hue, saturation, value);
        // }
        #endregion
    }
}

