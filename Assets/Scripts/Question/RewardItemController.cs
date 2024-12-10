using System.Collections;
using System.Collections.Generic;
using PowerUp;
using UnityEngine;

namespace Question
{
    public class RewardItemController : MonoBehaviour
    {
        #region Variables

        [SerializeField] PowerUpController _powerUpController;
        [SerializeField] private RewardItem _prefabDeleteOption;
        [SerializeField] private RewardItem _prefabMoreTime;
        [SerializeField] private RewardItem _prefabNextQuestion;
        [SerializeField] private RewardItem _prefabSecondOportunity;
        [SerializeField] private RewardItem _prefabTrueOption;
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


        /*
        public void AddDeleteOption(int amount)
        {
            _prefabDeleteOption.SetData(amount);
            _listToInstantiate.Enqueue(_prefabDeleteOption);
        }

        public void AddMoreTime(int amount)
        {
            _prefabMoreTime.SetData(amount);
            _listToInstantiate.Enqueue(_prefabMoreTime);
        }

        public void AddNextQuestion(int amount)
        {
            _prefabNextQuestion.SetData(amount);
            _listToInstantiate.Enqueue(_prefabNextQuestion);
        }

        public void AddSecondOportunity(int amount)
        {
            _powerUpController.BuySecondOportunity(amount);
            _prefabSecondOportunity.SetData(amount);
            _listToInstantiate.Enqueue(_prefabSecondOportunity);
        }

        public void AddTrueOption(int amount)
        {
            _prefabTrueOption.SetData(amount);
            _listToInstantiate.Enqueue(_prefabTrueOption);
        }
        */

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
            // var width = _rectContainer.rect.width / (_listToInstantiate.Count +1);
            // var initPosition = _rectContainer.rect.xMin;
            while (_listToInstantiate.Count >0)
            {
                var currentTime = 0f;                
                var currentItem = _listToInstantiate.Dequeue();
                var itemIntantiated = Instantiate(currentItem, transform);
                // initPosition += width;
                // itemIntantiated.transform.localPosition = new Vector3(initPosition,0,0);
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