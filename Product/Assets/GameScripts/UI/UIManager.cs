using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private class UISystem
    {
        public int Id;
        public string Name;
        public bool LastUI;
        public GameObject UIObj = null;
    }

    private UISystem[] UIName =
    {
        new UISystem() {Id = 0, Name = "LevelUI", LastUI = true}, 
        new UISystem() {Id = 1, Name = "System", LastUI = false}, 
        new UISystem() {Id = 2, Name = "Loading", LastUI = false}, 
    };

    private Stack<UISystem> UIStack = new Stack<UISystem>(); // UI队列，用于前进和后退

    private UISystem CurrentUI;

    // Use this for initialization
    void Awake()
    {
        // 检查事件是否存在
        CheckEventSystem();
        CurrentUI = null;
    }

    void CheckEventSystem()
    {
        var ESObj = GameObject.Find("EventSystem");
        if (ESObj == null)
        {
            ESObj = new GameObject("EventSystem");
            ESObj.AddComponent<EventSystem>();
            ESObj.AddComponent<StandaloneInputModule>();
        }
    }

    public bool OpenUI(int id)
    {
        if (id < 0 || id >= UIName.Length)
            return false;

        if (CurrentUI != null && CurrentUI.Id == id)
            return true;

        UISystem ui = UIName[id];

        // 查找stack中是否有该ui
        if (HasUIInStack(ui.Name))
        {
            // Pop掉前面所有的UI
            // 显示当前这个ui
            CurrentUI = PopAndDestorUntilName(ui.Id);
            CurrentUI.UIObj.SetActive(true);

            return true;
        }

        var o = Resources.Load("prefabs/ui/" + ui.Name) as GameObject;
        if (o == null)
            return false;

        var obj = Instantiate(o);
        if (obj == null)
            return false;

        // 加载新的UI，然后把当前UI塞进堆栈
        CloseCurrentUI();

        CurrentUI = ui;
        CurrentUI.UIObj = obj;

        return true;
    }

    public bool HasUIInStack(string name)
    {
        if (UIStack.Count == 0)
            return false;

        var e = UIStack.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current.Name.Equals(name))
                return true;
        }

        return false;
    }

    public void CloseCurrentUI()
    {
        if (CurrentUI == null)
            return;

        CurrentUI.UIObj.SetActive(false);
        UIStack.Push(CurrentUI);
    }

    // 把堆栈里的UI都踢出去，并且销毁掉，如果有参数，那么就到这个UI时停止
    UISystem PopAndDestorUntilName(int id)
    {
        UISystem ui = null;

        while (true)
        {
            if (UIStack.Count == 0)
                break;

            UISystem us = UIStack.Pop();

            if (us.Id == id)
            {
                ui = us;
                break;
            }

            GameObject.DestroyImmediate(us.UIObj);
        }

        return ui;
    }
}
