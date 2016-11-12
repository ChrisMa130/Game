using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MG
{
    public class UIDiaries : MonoBehaviour
    {
        List<GameObject> BtnDocs;
        Component Content;

        ColorBlock colorBlock;

        void Start()
        {
            // 根据配置文件中的文档个数，生成对应的控件
            BtnDocs = new List<GameObject>();   

            Content = gameObject.GetChildByName<Component>("Content");
            
            GameMgr.Instance.Config.TraversalDiarieId(i =>
            {
                GameObject o = InitADocButton(i);

                BtnDocs.Add(o);
            });
        }

        void CommonDocClick(int id)
        {
            UIManager manager = GameMgr.Instance.UiManager;
            // 创建TXT UI
            if (!manager.OpenUI(4))
                return;

            GameObject ui = manager.GetCurrentUiObject();
            if (ui == null)
                return;

            // 得到日志内容的脚本。赋值它的ID
            UIShowDocs doc = ui.GetComponent<UIShowDocs>();
            doc.Id = id;
        }

        // 添加一个按钮，加上点击回调，判断当前文档是否可读，修改图片颜色
        GameObject InitADocButton(int id)
        {
            var docObj = Resources.Load("prefabs/ui/DocBtn") as GameObject;
            GameObject o = Instantiate(docObj);
            Button btn = o.GetComponent<Button>();

            // 判断当前日记是否已经收集了。
            bool collect = false;
            if (GameData.Instance.HasCollect(id))
            {
                btn.onClick.AddListener(delegate()
                {
                    CommonDocClick(id);
                });
                collect = true;
            }

            // 把按钮加到UI中
            o.transform.parent = Content.transform;
            o.transform.localScale = Vector3.one;

            ChangeButtionStyle(collect, btn, id);

            return o;
        }

        void ChangeButtionStyle(bool enable, Button btn, int id)
        {
            Text txt = btn.gameObject.GetChildByName<Text>("Text");
            if (enable)
            {
                // 黑色字体
                txt.text = string.Format("<color=#000000>{0}</color>", id);
                colorBlock = btn.colors;
                colorBlock.normalColor = Color.white;

                btn.colors = colorBlock;
            }
            else
            {
                // 白色字体
                txt.text = string.Format("<color=#FFFFFF>{0}</color>", id);
                colorBlock = btn.colors;
                colorBlock.normalColor = Color.black;

                btn.colors = colorBlock;
            }
        }

        void Update()
        {

        }
    }
}
