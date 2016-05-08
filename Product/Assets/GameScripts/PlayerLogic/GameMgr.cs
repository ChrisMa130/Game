using UnityEngine;
using System.Collections;

// 游戏主体逻辑
public class GameMgr : MonoBehaviour
{
    private Player MySelf;

    void Start ()
    {
        MySelf = gameObject.AddMissingComponent<Player>();
    }

	void Update ()
    {
        MySelf.Activate(Time.deltaTime);
    }
}
