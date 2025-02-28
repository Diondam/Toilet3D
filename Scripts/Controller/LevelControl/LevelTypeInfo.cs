using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Hỗ trợ cho <see cref="UnicornDataLevel"/>, viết lại nếu cần.
    /// </summary>
    [Serializable]
    public class LevelTypeInfo
    {
        [JsonProperty] private LevelType levelType;
        [JsonProperty] private int currentLevel = 1;
        [JsonProperty] private int maxLevel = int.MinValue;

        [JsonIgnore]
        public LevelType LevelType
        {
            get => levelType;
            set => levelType = value;
        }
        
        [JsonIgnore]
        public int CurrentLevel
        {
            get => currentLevel;
            set => currentLevel = value;
        }

        [JsonIgnore]
        public int MaxLevel => maxLevel;

        public LevelTypeInfo(LevelType levelType)
        {
            this.levelType = levelType;
        }
        
        public int IncreaseLevel(LevelConstraint levelConstraint)
        {
            //max level của gameplay này,nhưng game của Đảm endless nên k cần
            var levelCount = levelConstraint.GetLevelCount(levelType);
            //nếu current(ban đầu bằng 1) mà tăng lên 2 thì max của nó sẽ là 2
            currentLevel++;
            maxLevel = Mathf.Max(maxLevel, currentLevel);
            
            if (maxLevel <= levelCount) return currentLevel;
            
            //nếu mà đã vượt qua hết các level của gameplay này
            currentLevel = 1;
            maxLevel = 1;
            return currentLevel;
        }
        // public int IncreaseLevel(LevelConstraint levelConstraint)
        // {
        //     currentLevel++;
        //     return currentLevel;
        // }
        //ném 1 con levelConstraint vào để lắp vào hàm clamp để chắc chắn level này hợp lệ
        public void SetLevel(int level, LevelConstraint levelConstraint)
        {
            var levelCount = levelConstraint.GetLevelCount(levelType);
            //đảm bảo là nó không vượt quá max, nó chỉ nằm trong khoảng level đã được định sẵn
            currentLevel = Mathf.Clamp(level, 1, levelCount);
        }
    }
}