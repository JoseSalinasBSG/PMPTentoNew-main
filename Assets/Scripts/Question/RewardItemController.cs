using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Question
{
    public class RewardItemController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private RewardItem _prefabCoin;
        [SerializeField] private RewardItem _prefabExperience;
        [SerializeField] private RectTransform _rectContainer;
        [SerializeField] private float _timeBetweenItems;

        private Queue<RewardItem> _listToInstantiate = new Queue<RewardItem>();
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            InitRewards();
        }


        #endregion

        #region Methods
        public void AddCoins(int amount)
        {
            _prefabCoin.SetData(amount);
            _listToInstantiate.Enqueue(_prefabCoin);
        }
        public void AddExperience(int amount)
        {
            _prefabExperience.SetData(amount);
            _listToInstantiate.Enqueue(_prefabExperience);
        }
        public void InitRewards()
        {
            StartCoroutine(StartInstantiate());
        }

        IEnumerator StartInstantiate()
        {
            while (_listToInstantiate.Count > 0)
            {
                var currentTime = 0f;
                var currentItem = _listToInstantiate.Dequeue();
                var itemIntantiated = Instantiate(currentItem, transform);
                itemIntantiated.gameObject.SetActive(true);
                while (currentTime <= _timeBetweenItems)
                {
                    currentTime += Time.deltaTime;
                    yield return null;
                }
            }
        }

        #endregion
    }
}