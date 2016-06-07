using UnityEngine;
using System.Collections;

namespace MG
{
    public class autodestory : MonoBehaviour
    {
        public float DestoryTime = 0;
        void Update()
        {
            if (DestoryTime <= 0)
            {
                GameObject.Destroy(gameObject);
            }

            DestoryTime -= Time.unscaledDeltaTime;
        }
    }

}

