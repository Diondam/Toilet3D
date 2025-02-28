using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unicorn
{
    /// <summary>
    /// Tổng level mỗi gamplay, get index start của gameplay trong build, get LevelType by Index
    /// Tool cho việc thao tác với buidindex để đưa ra các cách lấy level hay lấy index
    /// </summary>
    [Serializable]
    public class LevelConstraint
    {
        //[SerializeField] private int[] bonusStep = {5};
        //list start của các level
        [SerializeField] private List<int> startIndexes;

        private int startIndex = -1;
        //trả về index nhỏ nhất trong startIndexes
        public int GetStartIndex()
        {
            //nếu  khác -1 thì là 0,1,2,3,4 rồi nên return luôn
            if (startIndex != -1) return startIndex;
            //startIndexes = 1,2,3,4,5
            //gán bằng max để tìm ra cái nhỏ nhất
            startIndex = int.MaxValue;
            //lấy ra index nhỏ nhất trong chuỗi startIndexes
            //chỗ này có hơi cồng kềnh không, lấy ra phần tử nhỏ nhất trong mảng?
            foreach (int startIndex in startIndexes)
            {
                this.startIndex = Mathf.Min(this.startIndex, startIndex);
            }
            return startIndex;
        }

        //get start index của gameplay trong build index
        public int GetStartIndex(LevelType levelType)
        {
           try
            {
                return startIndexes[(int) levelType];
            }
            catch (ArgumentOutOfRangeException)
            {
                int levelTypeCount = Enum.GetValues(typeof(LevelType)).Length;
                if (levelTypeCount > startIndexes.Count)
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(startIndexes)} has less values than {nameof(LevelType)}. There should be {levelTypeCount} values.");

                throw;
            }
        }
        /// <summary>
        /// Tổng số lượng scene sẽ chạy trong game 
        /// </summary>
        public int GetLevelCount()
        {
            startIndex = GetStartIndex();
            return SceneManager.sceneCountInBuildSettings - startIndex + 1;
        }

        // không chuẩn lắm
        public int GetLevelCountX(LevelType levelType)
        {
            int levelTypeIntValue = (int) levelType;
            if (levelTypeIntValue > startIndexes.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(startIndexes)} lacks a value for {levelType}");
            }
            //levelStartIndex = 0,4,7,9,
            //levelTypeIntValue = 0,1,2,3,4,
            int levelStartIndex = startIndexes[levelTypeIntValue];

            if (levelStartIndex == -1)
            {
                return -1;
            }
            //cộng 1 vào vì nó bắt đầu từ 0, level 4 thì tức là có tổng 5 cái
            //cho trường hợp cuối, 
            if (levelTypeIntValue + 1 >= startIndexes.Count)
            {
                return SceneManager.sceneCountInBuildSettings - levelStartIndex;
            }
            return startIndexes[levelTypeIntValue] - levelStartIndex;  //chính là bằng 0
        }
        
        //cái này thì chuẩn, nhưng không cần lắm vì chỉ có 1 scene cho 1 gameplay
        public int GetLevelCount(LevelType levelType)
        {
            //convert type của level sang int
            int levelTypeIntValue = (int) levelType; //vd: = 2
            int levelStartIndex = startIndexes[levelTypeIntValue]; //vd = 8
        
            if (levelStartIndex == -1) 
                return -1;
        
            //nếu mà nó là TypeLevel cuối cùng, gameplay cuối thì lấy tổng scene để trừ
            if (levelTypeIntValue + 1 == startIndexes.Count)
                return SceneManager.sceneCountInBuildSettings - levelStartIndex;
            //mặc định là lấy cái index level sau trừ level trước
            return startIndexes[levelTypeIntValue + 1] - levelStartIndex;
        }

        public LevelType GetLevelTypeFromBuildIndex(int buildIndex)
        {
            // 5(4)/ 3(2) = 1
            //chia lấy phần nguyên, tức là nó chưa vượt qua level tiếp, tức là nó là 0,1,2,3
            //đến từng quãng level thì nó mới sang Type mới
            return (LevelType) (buildIndex / GetStartIndex());
        }

        
        //LevelIndex là index trong list index được gắn trong Unity
        public int GetLevelIndexFromBuildIndex(int buildIndex)
        {
            LevelType levelType = GetLevelTypeFromBuildIndex(buildIndex);
            int startIndex = GetStartIndex(levelType);
            return buildIndex - startIndex;
        }
    }

}