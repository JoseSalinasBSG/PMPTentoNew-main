using System;
using UnityEngine;

namespace ScriptableCreator
{
    [CreateAssetMenu(fileName = "Game setting ", menuName = "Game setting")]
    public class ScriptableObjectSettings : ScriptableObject
    {
        [Serializable ]
        public struct Reward
        {
            public float baseExperience;
            public float baseCoins;
            public float aditionalBonusExpConsecutiveQuestion;
            public float aditionalBonusCoinsConsecutiveQuestion;
            public float aditionalBonusExpForAchievement;
            public float aditionalBonusCoinsForAchievement;
        }
        [Serializable]
        public struct SettingData
        {
            public float musicVolume;
            public float soundEffectVolume;
            public bool haveNotification;
            public Reward MCReward;
            public Reward MAReward;
            public Reward MVPReward;
            public Reward DSReward;
            public Reward DGReward;
            public Reward DEReward;
            public Reward RDReward;
        }

        public SettingData settingData;

    }
}
