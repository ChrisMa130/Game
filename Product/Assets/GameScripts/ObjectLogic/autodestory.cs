using UnityEngine;
using System.Collections;

namespace MG
{
    public class autodestory : MonoBehaviour
    {
        public float DestoryTime = 0;
        void FixedUpdate()
        {
            if (DestoryTime <= 0)
            {
                var unit = gameObject.GetComponent<TimeUnit>();
                if (unit != null)
                {
                    unit.DestoryMe();
                }
                else
                {
                    GameObject.Destroy(gameObject);
                }
                
            }

            DestoryTime -= Time.unscaledDeltaTime;
        }
    }

}

