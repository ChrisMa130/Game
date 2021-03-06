﻿using UnityEngine;
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
        TopText = gameObject.GetChildByName<Text>("Text");
        RewindTxt = gameObject.GetChildByName<Text>("RewindTxt");
        PauseTxt = gameObject.GetChildByName<Text>("PauseTxt");
        ForwardTxt = gameObject.GetChildByName<Text>("ForwardTxt");

        LastCount = -1;
    }

	private void AdjustSize(Text text) {
		int defaultSize = text.fontSize;
		text.fontSize = Mathf.RoundToInt (defaultSize * Screen.width / (325 * 1.0f));
	}

    void UpdateCount()
    {
        if (LastCount == GameMgr.Instance.PlayerLogic.DiariesCount)
            return;
        int count = GameMgr.Instance.PlayerLogic.DiariesCount;
		int total = GameObject.FindGameObjectsWithTag ("Crystal").Length;
		TopText.text = string.Format("{0}/{1}", count, total);

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
            RewindTxt.enabled = false;
            // RewindTxt.color = Color.gray;
            return;
        }

        if (TimeController.Instance.CurrentState == TimeControllState.Rewinding || TimeController.Instance.CurrentState == TimeControllState.Rewinding)
            RewindTxt.color = Color.gray;
        else
            RewindTxt.color = Color.white;
    }

    void UpdatePause()
    {

        if (GameObject.Find("Faylisa").GetComponent<Player>().IsDead)
        {
            PauseTxt.enabled = true;
            PauseTxt.text = "Press Space to Restart";
            return;
        }

        if (GameMgr.Instance.WorldSwith.ForbidTimeOperation)
        {
            PauseTxt.enabled = false;
            return;
        }

        else if (GameMgr.Instance.IsPause)
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
            ForwardTxt.enabled = false;
            return;
        }

        if (TimeController.Instance.CurrentState == TimeControllState.Forward || TimeController.Instance.CurrentState == TimeControllState.Recording || !TimeController.Instance.IsOpTime())
            ForwardTxt.color = Color.gray;
        else
            ForwardTxt.color = Color.white;

    }
}
