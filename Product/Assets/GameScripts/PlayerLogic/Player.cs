using UnityEngine;
using System.Collections;

// 角色逻辑

public class Player : MonoBehaviour
{
	void Start ()
    {
	    // 出生后一些创建代码
	}

	void Update ()
    {
        // 讲道理，这里应该只做表现逻辑，游戏逻辑丢这里的话，可能会产生时序的bug。
	}

    public virtual void Activate(float deltaTime)
    {
        // 这里处理游戏逻辑。    
    }


}
