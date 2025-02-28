
namespace Unicorn
{
        /// <summary>
        ///Các biến chứa thông tin về level 
        /// </summary>
        public interface ILevelInfo
        {
                LevelType LevelType { get; }
                int DisplayLevel { get; }
                int GetCurrentLevel();
        }

}