using UnityEngine;
using UnityEngine.UI;
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

        LastCount = -1;
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
        if (GameMgr.Instance.WorldSwith.ForbidTimeOperation)
        {
            RewindTxt.color = Color.gray;
            return;
        }

        if (TimeController.Instance.CurrentState == TimeControllState.Rewinding || TimeController.Instance.CurrentState == TimeControllState.Rewinding)
            RewindTxt.color = Color.gray;
        else
            RewindTxt.color = Color.white;
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
        if (GameMgr.Instance.WorldSwith.ForbidTimeOperation)
        {
            ForwardTxt.color = Color.gray;
            return;
        }

        if (TimeController.Instance.CurrentState == TimeControllState.Recording || !TimeController.Instance.IsOpTime())
            ForwardTxt.color = Color.gray;
        else
            ForwardTxt.color = Color.white;

    }
}
