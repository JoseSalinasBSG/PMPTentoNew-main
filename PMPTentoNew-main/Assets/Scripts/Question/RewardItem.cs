using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Question
{
    public class RewardItem : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _amountLabel;  
        
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

        public void SetData(int amount)
        {
            _amountLabel.text = amount.ToString();
        }

        
        #endregion

    }

}