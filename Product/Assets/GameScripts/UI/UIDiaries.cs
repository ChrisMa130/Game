using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MG
{

    public class UIDiaries : MonoBehaviour
    {
        List<Button> BtnDocs;
        Component Content;

        void Start()
        {
            // 根据配置文件中的文档个数，生成对应的控件
            BtnDocs = new List<Button>();   

            Content = gameObject.GetChildByName<Component>("Content");

            int len = GameConfig.DocsDesc.Count;
            for (int i = 0; i < len; i++)
            {
                var docObj = Resources.Load("prefabs/ui/UIDocBtn") as GameObject;
                GameObject o = Instantiate(docObj);
                Button btn = o.GetChildByName<Button>("Button");
                btn.onClick.AddListener(delegate()
                {
                    CommonDocClick(i);
                });

                // 把按钮加到UI中
                o.transform.parent = Content.transform;

                BtnDocs.Add(btn);
            }
        }

        void CommonDocClick(int id)
        {
            // OPENUI，把DOC的ID传送进去
        }

        void Update()
        {

        }
    }
}
