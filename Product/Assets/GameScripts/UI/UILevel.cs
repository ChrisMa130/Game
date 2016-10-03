using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;
using MG;

// 游戏中的基本UI
public class UILevel : MonoBehaviour
{

    Text TopText;
    Text RewindTxt;
    Text PauseTxt;
    Text ForwardTxt;

    int LastCount;
    // Use this for initialization
    void Start()
    {
        var obj = gameObject.GetChildByName("Text");
        TopText = obj.GetComponent<Text>();

        obj = gameObject.GetChildByName("RewindTxt");
        RewindTxt = obj.GetComponent<Text>();

        obj = gameObject.GetChildByName("PauseTxt");
        PauseTxt = obj.GetComponent<Text>();

        obj = gameObject.GetChildByName("ForwardTxt");
        ForwardTxt = obj.GetComponent<Text>();
    }

    void UpdateCount()
    {
        if (LastCount == GameMgr.Instance.PlayerLogic.GetCollectCount(0))
            return;

        int count = GameMgr.Instance.PlayerLogic.GetCollectCount(0);
        TopText.text = string.Format("{0}/{1}", count, GameMgr.Instance.ExitCount);

        LastCount = count;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMgr.Instance == null)
            return;

        UpdateCount();
        UpdateRewind();
        UpdatePause();
        UpdateForward();
    }

    void UpdateRewind()
    {
        if (GameMgr.Instance.WorldSwith.ForbidTimeOperation || !TimeController.Instance.IsOpTime())
        {
            RewindTxt.color = Color.gray;
            return;
        }

        if (TimeController.Instance.CurrentState == TimeControllState.Rewinding)
            RewindTxt.color = Color.white;
        else
            RewindTxt.color = Color.gray;
    }

    void UpdatePause()
    {
        if (GameMgr.Instance.IsPause)
        {
            PauseTxt.text = "F：Resume";
        }
        else
        {
            PauseTxt.text = "F：Pause";
        }
    }

    void UpdateForward()
    {
        if (GameMgr.Instance.WorldSwith.ForbidTimeOperation || !TimeController.Instance.IsOpTime())
        {
            ForwardTxt.color = Color.gray;
            return;
        }

        if (TimeController.Instance.CurrentState == TimeControllState.Forward)
            ForwardTxt.color = Color.white;
        else
            ForwardTxt.color = Color.gray;
    }
}
