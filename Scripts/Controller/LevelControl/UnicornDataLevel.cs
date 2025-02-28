using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn
{
    /// <summary>
    /// Example về implementation của IDataLevel, dùng luôn cũng được.
    /// </summary>
    [Serializable]
    public class UnicornDataLevel : IDataLevel, ILevelInfo
    {
        [JsonProperty] private LevelType levelType = LevelType.Surf;
        [JsonProperty] private int displayLevel = 1;
        [JsonProperty] private bool isKeyCollected = false;

        //dict này sẽ link hai đối tượng lại với nhau, nếu thay đổi 1 trong 2
        //thì data của 2 cái vẫn link với nhau, cứ mỗi 1 cái Info thì có 1 cái type bên trong
        //không thể lẫn cái nào với cái nào được, kiểu edit đơn lẻ 1 cái Info thì nó vẫn link vào type
        [JsonProperty] private Dictionary<LevelType, LevelTypeInfo> levelTypesIndex;

        private List<int> loopLevels = new List<int>();
        private LevelConstraint levelConstraint;


        //để đảm bảo level chuẩn chúng ta cần 1 cái tool, và đây chính là nó
        //chúng ta đã định nghĩa các công cụ trong đó, giờ chỉ cần lấy ra dùng thôi
        public LevelConstraint LevelConstraint
        {
            get
            {
                //nếu chưa có thì tạo 1 cái
                if (levelConstraint == null)
                {
                    Debug.LogError(nameof(levelConstraint) + " is not set, using default values!");
                    levelConstraint = new LevelConstraint();
                    Debug.Log("Ê TAO VỪA ĐƯỢC CÁI NÀO ĐÓ KHỞI TẠO NÈ : ");
                }

                return levelConstraint;
            }
            set => levelConstraint = value;
        }

        public LevelType LevelType => levelType;
        public int DisplayLevel => displayLevel;

        [JsonIgnore]
        public bool IsKeyCollected
        {
            get => isKeyCollected;
            set
            {
                isKeyCollected = value;
                Save();
            }
        }

        [JsonIgnore]
        private Dictionary<LevelType, LevelTypeInfo> LevelTypesIndex
        {
            get
            {
                if (levelTypesIndex.Count < Enum.GetValues(typeof(LevelType)).Length)
                {
                    foreach (LevelType levelType in Enum.GetValues(typeof(LevelType)))
                    {
                        //nếu nó nằm trong enum thì thôi
                        if (levelTypesIndex.ContainsKey(levelType)) continue;
                        //còn nếu mà không nằm bên trong thì add thêm vào bên trong
                        //ví dụ chơi gameplay 1 thì có mỗi nó, chơi gameplay 2 thì nó chưa có trong dict
                        //nên sẽ add thêm vào trong dict loại này

                        levelTypesIndex.Add(levelType, new LevelTypeInfo(levelType));
                    }
                }

                return levelTypesIndex;
            }
            set => levelTypesIndex = value;
        }

        //levelConstraint để làm gì ? có thể bỏ đi
        public UnicornDataLevel(LevelConstraint levelConstraint)
        {
            //1 con datalevel hay 1 con unicorn datalevel sẽ có gì?
            //1 con để chứa all bao gồm các level, level bao nhiêu ( type, info)
            levelTypesIndex = new Dictionary<LevelType, LevelTypeInfo>();
            //và con game thì chỉ chứa các level được định nghĩa trong enum, nếu có các cái giống nhau thì có thể gây lỗi
            foreach (LevelType value in Enum.GetValues(typeof(LevelType)))
            {
                levelTypesIndex.Add(value, new LevelTypeInfo(value));
            }
        }

        public override string ToString()
        {
            return $"level: {GetCurrentLevel()}, " +
                   $"{nameof(levelType)}: {levelType}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelType"></param>
        /// <param name="increment">Nếu <code>true</code> thì sẽ chuyển sang <see cref="LevelType"/> và tăng level</param>
        /// set level tăng l
        public void SetLevel(LevelType levelType, bool increment = false)
        {
            Debug.Log(" SetLevel: ");
            if (increment)
            {
                LevelTypesIndex[levelType].IncreaseLevel(LevelConstraint);
            }

            this.levelType = levelType;
            Save();
        }

        /// <summary>
        /// Chuyển sang <see cref="LevelType"/> và nhảy tới level
        /// </summary>
        /// <param name="levelType"></param>
        /// <param name="level"></param>
        public void SetLevel(LevelType levelType, int level)
        {
            //set vào LevelTypeInfo
            LevelTypesIndex[levelType].SetLevel(level, LevelConstraint);
            this.levelType = levelType;
            //sau khi lưu thay đổi của level này rồi thì lưu tổng thể(những cái khác giữ nguyên)
            Save();
        }

        public void SetLevel(int buildIndex)
        {
            var levelType = levelConstraint.GetLevelTypeFromBuildIndex(buildIndex);
            var levelIndex = levelConstraint.GetLevelIndexFromBuildIndex(buildIndex);
            SetLevel(levelType, levelIndex);
        }


        //level hiện tại của gameplay này để load scen
        public int GetBuildIndex()
        {
            //levelType ban đầu là Duck, sau khi engame và tăng level nó sẽ chuyển sang kiểu tiếp theo
            //levelType là type của level hiện tại, lúc trước khi chuyển sang level mới(win)(vì unicorn datalevel này sống cùng gamemana nên dù
            //có unload scene cũng không sao)
            int startIndex = LevelConstraint.GetStartIndex(levelType);

            //nếu mà cố tình đặt start index của gameplay nào đó ở vị trí 0
            if (startIndex < 1)
            {
                //get gameplayzdva tiếp
                levelType = GetNextLevelType();
                startIndex = LevelConstraint.GetStartIndex(levelType);
                //nếu vẫn tiếp tục là 0
                if (startIndex < 1)
                {
                    // No valid scene found!
                    //thì lỗi
                    return -1;
                }
            }

            int level = GetCurrentLevel();
            Debug.Log("Load Level : LevelType  " + LevelType + "  startIndex RUN:  " + startIndex);
            //đáng ra là + thêm level chỗ này nhưng không cần vì tăng level không tăng scene
            return startIndex + level - 1;
        }

        private LevelType GetNextLevelType()
        {
            int levelTypeIntValue = (int)levelType;
            if (++levelTypeIntValue >= Enum.GetValues(typeof(LevelType)).Length)
            {
                //đây là lí do khiến các level có thể chơi lại, loop hết tất cả các scene từ đầu
                levelTypeIntValue = 0;
            }
            return (LevelType)levelTypeIntValue;
           
            List<LevelType> listTemp;
            int ranNum;
            switch (levelType)
            {
                case LevelType.Surf:
                    listTemp = new List<LevelType>
                    {
                        LevelType.Boat,
                        LevelType.Drive,
                        LevelType.Jump
                    };
                    ranNum = Random.Range(0, listTemp.Count);
                    return listTemp[ranNum];
                case LevelType.Boat:
                case LevelType.Drive:
                case LevelType.Jump:
                    listTemp = new List<LevelType>
                    {
                        LevelType.Sheep,
                        LevelType.Fly,
                    };
                    ranNum = Random.Range(0, listTemp.Count);
                    return listTemp[ranNum];
                case LevelType.Sheep:
                case LevelType.Fly:
                    listTemp = new List<LevelType>
                    {
                        LevelType.Candy,
                        LevelType.House,
                        LevelType.Kiss
                    };
                    ranNum = Random.Range(0, listTemp.Count);
                    return listTemp[ranNum];
                  case LevelType.Candy:
                case LevelType.House:
                case LevelType.Kiss:
                    return LevelType.Duck;
                case LevelType.Duck:
                    return LevelType.Surf;
                default:
                    return LevelType.Surf;
            }
           
            
            
        }


        //cùng type level này thì đang level bao nhiêu
        public int GetCurrentLevel() => LevelTypesIndex[levelType].CurrentLevel; //levelInfo.current level

        //tăng thông số cho level hiện tại khi win -> endgame(lv) -> endlevel(gm) -> increase 
        public void IncreaseLevel()
        {
            Debug.Log("IncreaseLevel : ");
            displayLevel++;
            IsKeyCollected = false;
            //level gameplay này(hiện tại) sẽ tăng lên
            LevelTypesIndex[levelType].IncreaseLevel(LevelConstraint);
            //sau khi tăng level thì gameplay khác sẽ được gán vào
            levelType = GetNextLevelType();
            //lưu vào APP
            Save();
        }

        public void ChangeLevelType()
        {
            levelType = GetNextLevelType();
            Save();
        }

        public void IncreaseLevelNotChangeLevelType()
        {
            Debug.Log("ĐÃ IncreaseLevelNotChangeLevelType");
            displayLevel++;
            IsKeyCollected = false;
            //level gameplay này(hiện tại) sẽ tăng lên, nhưng level type vẫn giữ như cũ
            LevelTypesIndex[levelType].IncreaseLevel(LevelConstraint);
            Save();
        }


        public void Save()
        {
            //this là datalevel của Game
            //sau mỗi lần thì các biến trong đây sẽ được cập nhật mới và set lại vào trong  PlayerPrefs
            //datalevel sẽ được lấy ra ở 1 chỗ khác và thay đổi sau đó được lưu bởi các thứ trong này
            PlayerDataManager.Instance.SetDataLevel(this);
        }
    }
}


//Thay đổi các thuộc tính như current level rồi lưu vào bằng save
//Trong 1 game thì có 1 thằng datalevel, nó sẽ chứa data của các gameplay