﻿namespace Unicorn
{
    /// <summary>
    /// Nối scene build index với level
    /// Thao tác với các data trong level info
    /// </summary>
    public interface IDataLevel: ILevelInfo
    {
        LevelConstraint LevelConstraint { get; set; }
        void SetLevel(LevelType levelType, bool increment);
        void SetLevel(LevelType levelType, int level);
        void SetLevel(int buildIndex);
        int GetBuildIndex();
        void IncreaseLevel();
        void Save();
    }
}
