using UnityEngine.UI;
using UnityEngine;
using GKBase;
using GKData;
using GKUI;
using System.Collections;

public class UIMazes_StepChange : SingletonUIBase<UIMazes_StepChange>
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {
        public Text TimeDetaileText;
    }
    #endregion

    #region PublicField

    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    private int _time;
    #endregion

    #region PublicMethod
    public void SetData(int time)
    {
        _time = time;
    }
    #endregion

    #region PrivateMethod
    private void Start()
    {
        Serializable();
        InitListener();
        Init();
    }

    private void Serializable()
    {
        GK.FindControls(this.gameObject, ref m_ctl);
    }

    private void InitListener()
    {

    }

    private void Init()
    {
        m_ctl.TimeDetaileText.text = string.Format(DataController.Instance().GetLocalization(1, LocalizationSubType.Maze), _time.ToString()) ;
        StartCoroutine(Dismiss());
    }

    IEnumerator Dismiss()
    {
        yield return new WaitForSeconds(5);
        MyGame.Instance.isPause = false;
        Close();
        #endregion
    }
}
