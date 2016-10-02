using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;
using MG;

// 游戏中的基本UI
public class UILevel : MonoBehaviour
{

    Text TopText;
    int i = 0;
    int LastCount;
    // Use this for initialization
    void Start()
    {
        var obj = gameObject.GetChildByName("Text");
        TopText = obj.GetComponent<Text>();

        UpdateCount();
    }

    void UpdateCount()
    {
        int count = GameMgr.Instance.PlayerLogic.GetCollectCount(0);
        TopText.text = string.Format("{0}/{1}", count, GameMgr.Instance.ExitCount);

        LastCount = count;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMgr.Instance == null)
            return;

        if (LastCount == GameMgr.Instance.PlayerLogic.GetCollectCount(0))
            return;

        UpdateCount();
    }
}
