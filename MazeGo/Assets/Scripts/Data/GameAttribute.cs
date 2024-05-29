using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectAttr
{
    //  对象基本属性.
    BaseAttr_Start = -1,
    GUID,               // GUID 0.
    Type,               // 0 无效, 1 Player 2 Enemy 3 Npc.
    BaseAttr_Count,

    // 玩家数据段.
    PlayerAttr_Start = 299,
    Coins,              // 300 玩家金币数.
    Diamond,            // 301 玩家钻石数
    Belief,             // 玩家信仰 (战场内).
    Food,               // 玩家食物 (战场内).
    InventoryLevel,     // 玩家等级.
    Achievements,       // 玩家已获得成就. List
    Title,              // 玩家称号.
    CreateTime,         // 创建时间.
    PlayerAttr_Count,

    // 成就累积数据.
    PlayerAchievemt_Start = 499,
    AchiKillCount,          // 总击杀数.
    AchiDeathCount,         // 总角色死亡数.
    AchiCoinCost,           // 总金币支出累积.
    AchiDiamondCost,        // 总钻石支出累积.
    AchiConsumeCost,        // 总消耗品累积.
    AchiFightingCount,      // 总战斗次数.
    AchiSkillUpgrade,       // 总技能升级总数。
    AchiThrowCount,         // 总丢弃物品总数.
    AchiWinCount,           // 总胜利数.
    AchiDefeatedCount,      // 总失败数.
    PlayerAchievemt_Count,

    // 通用设置数据段
    OptionAttr_Start = 899,
    Sound,              // 音效.
    Music,              // 音乐.
    RendingQuality,     // 渲染质量.
    Language,           // 语言.
    OptionAttr_Count,

    // 迷宫数据段
    MazeAttr_Start = 999,
    MazeLevelPassTime,      // 每关通关时间.
    MazeLevelBestTime,      // 每关最佳通关时间.
    MazeBuffArrow,          // Buff功能 - 终点箭头.



}
