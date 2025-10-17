using UnityEngine;

public class LevelRewardSystem : MonoBehaviour
{
    [System.Serializable]
    public class LevelReward
    {
        public int levelIndex;
        public ItemData rewardItem;
        public int minBlueprint;
        public int maxBlueprint;
    }

    [Header("Level Rewards")]
    public LevelReward[] levelRewards;
}
