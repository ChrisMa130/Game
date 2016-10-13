using UnityEngine;

namespace MG
{
    public class pickup : TimeUnit
    {
        public int Key      = 0;
        public int Value    = 0;
        public int Id       = 0;

        void Start()
        {
            // 从gamedata里获取自己是否存在
            if (GameData.Instance.HasCollect(Id))
                DestoryMe();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.AddCollectItem(Key, Value);
                    DestoryMe();
                }
            }
        }
    }
}

