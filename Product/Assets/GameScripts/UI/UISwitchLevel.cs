using UnityEngine;
using UnityEngine.UI;

namespace MG
{
    public class UISwitchLevel : MonoBehaviour
    {
        Component Content;

        void Start()
        {
            Content = gameObject.GetChildByName<Component>("Content");

            // 遍历已经通过的关卡，并把名字显示出来
            GameData.Instance.TraversalPassedLevelInfo(o =>
            {
                InitLevelButton(o.LevelName);
            });
        }

        void InitLevelButton(string name)
        {
            var docObj = Resources.Load("prefabs/ui/UILevelBtn") as GameObject;
            GameObject o = Instantiate(docObj);
            Button btn = o.GetComponent<Button>();

            btn.onClick.AddListener(delegate()
            {
                GameMgr.Instance.LoadLevel(name);
            });

            o.transform.parent = Content.transform;
            o.transform.localScale = Vector3.one;

            Text txt = btn.GetComponentInChildren<Text>();
            txt.text = name;
        }
    }
}

