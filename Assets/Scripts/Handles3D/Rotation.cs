using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Handles3D
{
    public class Rotation : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Scrollbar _scrollbar;

        private float currentTime = 0;
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            currentTime += Time.deltaTime;
            currentTime %= 1;
            _scrollbar.value = currentTime;
        }

        #endregion

        #region Methods
        
        

        
        #endregion

    }

}