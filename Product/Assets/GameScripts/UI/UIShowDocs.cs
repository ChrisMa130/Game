using UnityEngine;
using UnityEngine.UI;

namespace MG
{
    public class UIShowDocs : MonoBehaviour
    {
        public int Id { get; set; }

        void Start()
        {
            Text TxtObj = gameObject.GetChildByName<Text>("Text");
            string txt = GameMgr.Instance.Config.GetDiarieText(Id);
            TxtObj.text = txt;
        }
    }
}

