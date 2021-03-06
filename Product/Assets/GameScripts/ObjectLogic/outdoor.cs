﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Com.LuisPedroFonseca.ProCamera2D;

namespace MG
{
    public class outdoor : MonoBehaviour
    {
        public int NeedCollects;

        public string NextLevelName;

		private bool inTransition;

		private ProCamera2DTransitionsFX _transitionFX;

		void Awake() {
			_transitionFX = Camera.main.GetComponent<ProCamera2DTransitionsFX> ();
			inTransition = false;
		}

        void Start()
        {
             GameMgr.Instance.ExitCount = NeedCollects;

			_transitionFX.OnTransitionExitEnded = () => {
			};
			_transitionFX.OnTransitionExitEnded += OnTransitionExitEnded;
        }

        void Update()
        {
            Player player = GameObject.Find("Faylisa").GetComponent<Player>();
            if (EnoughCount(player))
            {
                transform.FindChild("Light").gameObject.SetActive(true);
                transform.FindChild("Flash").gameObject.SetActive(true);
            }
        }

        private bool EnoughCount(Player player)
        {
            if (player.DiariesCount < NeedCollects)
                return false;
            return true;
        }

        void OnTriggerStay2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
                return;

            if (!EnoughCount(player))
                return;
//            for (int i = 0; i < NeedCollects.Length; i++)
//            {
//                var count = NeedCollects[i];
//                if (count > 0)
//                {
//                    var playerGet = player.GetCollectCount(i);
//                    if (count > playerGet)
//                    {
//                        finish = false;
//                        break;
//                    }
//                }
//            }

//            if (!finish)
//            {
//                // 没通关
//                Debug.Log("没通关，谢谢");
//                return;
//            }
//
//            // 胜利通关逻辑，什么切换下一关，显示胜利界面拉。balbala
//
//            if (string.IsNullOrEmpty(NextLevelName))
//            {
//                Debug.Log("呵呵哒了，没下一关了。GGWP");
//                return;
//            }

			if (GameMgr.Instance.InputMgr.UpUp && !inTransition) {
				_transitionFX.TransitionExit ();
				inTransition = true;
				//SceneManager.LoadSceneAsync (NextLevelName);
			}
        }
			
		void OnTransitionExitEnded() {
			_transitionFX.OnTransitionExitEnded -= OnTransitionExitEnded;
			SceneManager.LoadSceneAsync (NextLevelName);
		}
    }
}

