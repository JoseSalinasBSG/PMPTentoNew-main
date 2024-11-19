using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Inputfield
{
    [RequireComponent(typeof(TMP_InputField))]
    public class CheckInputField : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onEmptyField;
        [SerializeField] private UnityEvent _onFillField;
        private TMP_InputField _tmpInput;

        private bool _isEmpty = true;
        private void OnEnable()
        {
            _tmpInput = GetComponent<TMP_InputField>();
            _tmpInput.onValueChanged.AddListener(Call);
        }

        private void Call(string arg0)
        {
            if (arg0.Length == 0)
            {
                if (!_isEmpty)
                {
                    _isEmpty = true;
                    _onEmptyField?.Invoke();
                }
            }
            else
            {
                if (_isEmpty)
                {
                    _isEmpty = false;
                    _onFillField?.Invoke();
                }
            }
        }
    }
}
