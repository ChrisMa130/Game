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
            GameMgr.Instance.Config.Traversallevels((n, nikeName) =>
            {
                InitLevelButton(n, nikeName);
            });
        }

        void InitLevelButton(string name, string nikeName)
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
            txt.text = nikeName;
        }
    }
}

