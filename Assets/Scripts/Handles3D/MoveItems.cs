using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Handles3D
{
    public class MoveItems : MonoBehaviour
    {
        #region Variables

        [SerializeField] private RectTransform _itemsContainer;
        [SerializeField] private TextMeshProUGUI _domainText;
        [SerializeField] private float _timeVelocity;
        [SerializeField] private float _timeToTransition;
        [SerializeField] private List<HandlesItem> _options;
        [SerializeField] private RectTransform _offset;
        [SerializeField] private UnityEvent _onSelectedItem;
        [SerializeField] private HandlesItem _prefabItem;
        [SerializeField] private RectTransform _prefabOffset;
        private float _currentTime;
        private int count;
        private HandlesItem _elegido;

        public HandlesItem Elegido
        {
            get => _elegido;
            set => _elegido = value;
        }

        #endregion

        #region Methods

        public void SetTextTittle(string tittle)
        {
            _domainText.text = tittle;
        }
        public void SetData(List<Task> tags)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                Destroy(_options[i].gameObject);
            }
            _options.Clear();
            HandlesItem item;

            for (int i = 0; i < tags.Count; i++)
            {
                item = Instantiate(_prefabItem, _itemsContainer);
                item.SetData(tags[i].nombre.Substring(tags[i].nombre.IndexOf('-') + 1), tags[i].id.ToString());
                _options.Add(item);
            }

            _offset = Instantiate(_prefabOffset, _itemsContainer);
            var rect = _itemsContainer.rect;
            rect.yMin = 0;
            _itemsContainer.offsetMin = tags.Count % 2 == 0 ? new Vector2(0, -_options[0].Rect.rect.height) : new Vector2(0, 0);
        }

        public void StartTransition()
        {
            count = _options.Count - 1;
            StartCoroutine(IStartCounter());
            StartCoroutine(ITransition());
        }

        IEnumerator IStartCounter()
        {
            var currentTime = 0f;
            var rand = Random.Range(5f, 10f);
            var rand2 = Random.Range(1.7f, 100f);
            var rand3 = Random.Range(3f, 10f);

            while (currentTime <= 2)
            {
                currentTime += Time.deltaTime / _timeToTransition;
                _timeVelocity = Mathf.Lerp(.03f, 4, 1 / (1 + rand2 * Mathf.Exp(-7 * currentTime + rand)));
                yield return null;
            }
            _timeVelocity = 4;
        }

        IEnumerator ITransition()
        {
            var height = _prefabItem.Rect.sizeDelta.y;
            var i = count;
            while (true)
            {
                var currentItem = _options[i];
                _currentTime = 0f;
                var heithTemp = height;
                currentItem.Rect.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, 0);
                currentItem.Rect.SetAsFirstSibling();
                var y = 0f;
                while (_currentTime <= 1)
                {
                    _currentTime += Time.deltaTime / _timeVelocity;
                    y = (_currentTime) * heithTemp;
                    currentItem.Rect.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, y);
                    _offset.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, heithTemp - y);
                    yield return null;
                }
                y = heithTemp;
                currentItem.Rect.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, y);
                _offset.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, heithTemp - y);
                if (_timeVelocity > 3.5f)
                {
                    _elegido = _itemsContainer.GetChild((_itemsContainer.childCount - 2) / 2).GetComponent<HandlesItem>();
                    _onSelectedItem?.Invoke();
                    GameEvents.GetIdTask?.Invoke(Int32.Parse(_elegido.ID));
                    yield break;
                }
                i--;
                if (i < 0)
                {
                    i = count;
                }
            }
        }
        #endregion
    }
}