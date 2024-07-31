using System.IO;
using UnityEngine;
using GKBase;
using GKData;
using GKEncryption;
using GKFile;

public class DataController : GKSingleton<DataController> 
{
    #region PublicField
    static private GameData _data;
    static public GameData Data
    {
        get
        {
            if (_data == null)
            {
                _data = GK.LoadResource<GameData>("Data/_AutoGen_/GameData");
            }
            return _data;
        }
    }
    #endregion

    #region PrivateField
    // 是否文件加密.
    private bool _Encryption = false;
    #endregion

    #region PublicMethod
    private float _lastTime = 0;
    private readonly float _cycle = 10;
    // 定时存储. 减少频繁调用时的IO压力.
    public void Update()
    {
        if(Time.realtimeSinceStartup - _lastTime > _cycle)
        {
            _lastTime = Time.realtimeSinceStartup;

            if (_playerDataChanged)
                _SavePlayerData();

            if (_achievementChanged)
                _SaveAchievement();

            if (_audioChanged)
                _SaveAudioData();

            if (_rendingChanged)
                _SaveRending();

            if (_mazeChanged)
                _SaveMaze();
        }
    }

    // 存储游戏数据.
    public void SaveData()
    {
        SavePlayerData();
        SaveAchievement();
        SaveAudioData();
        SaveRending();
        SaveMazeData();
    }

    // 加载游戏数据.
    public void LoadData()
    {
        LoadPlayerData();
        LoadAchievementList();
        LoadAudio();
        LoadRending();
        LoadMaze();
    }

    // 清除游戏数据.
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        ClearPlayerData();
        ClearAchievements();
        ClearAudio();
        ClearRending();
        ClearMaze();
    }

    // 获取本地化文本.
    // 1. Error code. 2. Unit. 3. Skill. 4. Item.
    // 5. Achievement. 6. Title. 7. Title Desc.
    public string GetLocalization(int ID, LocalizationSubType subType = LocalizationSubType.Common)
    {
        GameData.LocalizationData data = null;

        switch(subType)
        {
            // Error code.
            case LocalizationSubType.ErrorCode:
                data = Data.GetLocalizationErrorCodeData(ID);
                break;
            // Achievement.
            case LocalizationSubType.Achievement:
                data = Data.GetLocalizationAchiData(ID);
                break;
            // Achievement Desc.
            case LocalizationSubType.AchievementDesc:
                data = Data.GetLocalizationAchiDescData(ID);
                break;
            // Title.
            case LocalizationSubType.Title:
                data = Data.GetLocalizationTitleData(ID);
                break;
            // Title Description.
            case LocalizationSubType.TitleDesc:
                data = Data.GetLocalizationTitleDescData(ID);
                break;
            // Maze.
            case LocalizationSubType.Maze:
                data = Data.GetLocalizationMazeData(ID);
                break;
            // Maze Buff.
            case LocalizationSubType.MazeBuff:
                data = Data.GetLocalizationMazeBuffData(ID);
                break;
            // Maze Monster.
            case LocalizationSubType.MazeMonster:
                data = Data.GetLocalizationMazeMonsterData(ID);
                break;
            // Maze Town.
            case LocalizationSubType.MazeTown:
                data = Data.GetLocalizationMazeTownData(ID);
                break;
            default:
                data = Data.GetLocalizationData(ID);
                break;
        }

        switch(PlayerController.Instance().Language)
        {
            case (int)LanguageType.Chinese:
                return (null == data) ? string.Empty : data.chinese;
        }

        return (null == data) ? string.Empty : data.english;
    }
    #endregion

    #region PrivateMethod

    #endregion

    #region PlayerData
    //  玩家数据存储路径.
    private string _playerDataPath = string.Format("{0}/UserData", Application.persistentDataPath);
    private bool _playerDataChanged = false;

    // 存储玩家数据.
    public void SavePlayerData()
    {
        _playerDataChanged = true;
    }

    // 删除玩家数据.
    private void ClearPlayerData()
    {
        GKFileUtil.DeleteFile(_playerDataPath);
    }

    // 读取玩家数据.
    private void LoadPlayerData()
    {
        if (!File.Exists(_playerDataPath))
            return;
        //读取数据.  
        try
        {
            string file = GKBase64.Instance().LoadTextFile(_playerDataPath, _Encryption);
            if (string.IsNullOrEmpty(file))
                return;
            //反序列化对象.  
            var data = GKSerialize.Instance().DeserializeObject(file);
            PlayerController.Instance().SetDataBase(data);
        }
        catch
        {
            Debug.LogError("Load player data faile.");
        }
    }

    // 存储玩家数据.
    private void _SavePlayerData()
    {
        //Debug.Log("_SavePlayerData");

        _playerDataChanged = false;
        // 存储数据.
        string content = GKSerialize.Instance().SerializeObject(PlayerController.Instance().GetDataBase());
        //创建XML文件且写入加密数据.  
        GKBase64.Instance().CreateTextFile(_playerDataPath, content, _Encryption);
    }
    #endregion

    #region Achievement
    // 玩家成就数据存储路径.
    private string _achievementPath = string.Format("{0}/UserAchievement", Application.persistentDataPath);
    private bool _achievementChanged = false;

    // 存储玩家成就数据.
    public void SaveAchievement()
    {
        _achievementChanged = true;
    }

    // 删除成就数据.
    private void ClearAchievements()
    {
        GKFileUtil.DeleteFile(_achievementPath);
    }

    // 加载获得成就数据.
    private void LoadAchievementList()
    {
        if (!File.Exists(_achievementPath))
            return;

        AchievementController.Instance().ClearAchievements();

        //读取数据.  
        try
        {
            string file = GKBase64.Instance().LoadTextFile(_achievementPath, _Encryption);
            if (string.IsNullOrEmpty(file))
                return;
            //反序列化对象.  
            var data = GKSerialize.Instance().DeserializeObject(file);
            AchievementController.Instance().SetDataBase(data);
        }
        catch
        {
            Debug.LogError("Load achievement data faile.");
        }
    }

    // 存储玩家成就数据.
    private void _SaveAchievement()
    {
        Debug.Log("_SaveAchievement");

        _achievementChanged = false;
        // 存储数据.
        string content = GKSerialize.Instance().SerializeObject(AchievementController.Instance().GetDataBase());
        //创建XML文件且写入加密数据.  
        GKBase64.Instance().CreateTextFile(_achievementPath, content, _Encryption);
    }
    #endregion

    #region Audio
    //  音效数据存储路径.
    private string _audioPath = string.Format("{0}/UserAudio", Application.persistentDataPath);
    private bool _audioChanged = false;

    // 存储音效数据.
    public void SaveAudioData()
    {
        _audioChanged = true;
    }

    // 删除音效数据.
    private void ClearAudio()
    {
        GKFileUtil.DeleteFile(_audioPath);
    }

    // 读取音效数据.
    private void LoadAudio()
    {
        if (!File.Exists(_audioPath))
            return;
        //读取数据.  
        try
        {
            string file = GKBase64.Instance().LoadTextFile(_audioPath, _Encryption);
            if (string.IsNullOrEmpty(file))
                return;
            //反序列化对象.  
            var data = GKSerialize.Instance().DeserializeObject(file);
            AudioController.Instance().SetDataBase(data);
        }
        catch
        {
            Debug.LogError("Load audio data faile.");
        }
    }

    // 存储音效数据.
    private void _SaveAudioData()
    {
        Debug.Log("_SaveAudioData");

        _audioChanged = false;
        // 存储数据.
        string content = GKSerialize.Instance().SerializeObject(AudioController.Instance().GetDataBase());
        //创建XML文件且写入加密数据.  
        GKBase64.Instance().CreateTextFile(_audioPath, content, _Encryption);
    }
    #endregion

    #region Rending
    //  渲染数据存储路径.
    private string _rendingPath = string.Format("{0}/UserRending", Application.persistentDataPath);
    private bool _rendingChanged = false;

    // 储存渲染质量数据.
    public void SaveRending()
    {
        _rendingChanged = true;
    }

    // 删除渲染数据.
    private void ClearRending()
    {
        GKFileUtil.DeleteFile(_rendingPath);
    }

    // 储存渲染质量数据.
    private void _SaveRending()
    {
        Debug.Log("_SaveRending");

        _rendingChanged = false;
        // 存储数据.
        string content = GKSerialize.Instance().SerializeObject(RendingController.Instance().GetDataBase());
        //创建XML文件且写入加密数据.  
        GKBase64.Instance().CreateTextFile(_rendingPath, content, _Encryption);
    }

    // 读取渲染数据.
    private void LoadRending()
    {
        if (!File.Exists(_rendingPath))
            return;
        //读取数据.  
        try
        {
            string file = GKBase64.Instance().LoadTextFile(_rendingPath, _Encryption);
            if (string.IsNullOrEmpty(file))
                return;
            //反序列化对象.  
            var data = GKSerialize.Instance().DeserializeObject(file);
            RendingController.Instance().SetDataBase(data);
        }
        catch
        {
            Debug.LogError("Load audio data faile.");
        }
    }
    #endregion

    #region Maze
    // 迷宫数据.
    private bool _mazeChanged = false;
    private string _mazePath = string.Format("{0}/MazeData", Application.persistentDataPath);

    /// <summary>
    /// 更新迷宫数据. 数据会有后台统一更新.
    /// </summary>
    public void SaveMazeData()
    {
        _mazeChanged = true;
    }

    /// <summary>
    /// 迷宫数据.
    /// </summary>
    private void _SaveMaze()
    {
        //Debug.Log("_SaveMaze");

        _mazeChanged = false;
        string content = GKSerialize.Instance().SerializeObject(MazeSystem.Instance().GetData()); ;

        //创建XML文件且写入加密数据.  
        GKBase64.Instance().CreateTextFile(_mazePath, content, _Encryption);
    }

    /// <summary>
    /// 加载迷宫数据.
    /// </summary>
    private void LoadMaze()
    {
        //Debug.Log("LoadMaze");
        if (!File.Exists(_mazePath))
            return;
        //读取数据.  
        try
        {
            string file = GKBase64.Instance().LoadTextFile(_mazePath, _Encryption);
            MazeSystem.Instance().SetData(GKSerialize.Instance().DeserializeObject(file));
        }
        catch
        {
            Debug.LogError("Load LoadMaze data faile.");
        }
    }

    /// <summary>
    /// 删除迷宫数据.
    /// </summary>
    private void ClearMaze()
    {
        GKFileUtil.DeleteFile(_mazePath);

    }
    #endregion

}

public enum LanguageType
{
    English = 0,
    Chinese
}

public enum LocalizationSubType
{
    Common = 0,         // 0.
    ErrorCode,          // 1.
    Achievement,        // 2.
    AchievementDesc,    // 3.
    Title,              // 4.
    TitleDesc,          // 5.
    Maze,               // 6.
    MazeBuff,           // 7.
    MazeMonster,        // 8.
    MazeTown,           // 9.
}
