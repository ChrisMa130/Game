using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MG
{
    public class UIOptions : MonoBehaviour
    {
        Button BtnResume;
        Button BtnLevels;
        //Button BtnDiaries;
        Button BtnCheckPoint;
        Button BtnCredits;
        Button BtnQuit;

        void Start()
        {
            BtnResume = gameObject.GetChildByName<Button>("BtnResume");
            BtnLevels = gameObject.GetChildByName<Button>("BtnLevels");
            //BtnDiaries = gameObject.GetChildByName<Button>("BtnDiaries");
            BtnCheckPoint = gameObject.GetChildByName<Button>("BtnCheckPoint");
            BtnCredits  = gameObject.GetChildByName<Button>("BtnCredits");
            BtnQuit = gameObject.GetChildByName<Button>("BtnQuit");

            BtnResume.onClick.AddListener(OnBtnResumeClick);
            BtnLevels.onClick.AddListener(OnBtnLevelsClick);
            //BtnDiaries.onClick.AddListener(OnBtnDiariesClick);
            BtnCheckPoint.onClick.AddListener(OnBtnCheckPointClick);
            BtnCredits.onClick.AddListener(OnBtnCreditsClick);
            BtnQuit.onClick.AddListener(OnBtnQuitClick);
        }

        public void OnDestroy()
        {
            BtnResume.onClick.RemoveAllListeners();
            BtnLevels.onClick.RemoveAllListeners();
            //BtnDiaries.onClick.RemoveAllListeners();
            BtnCheckPoint.onClick.RemoveAllListeners();
            BtnCredits.onClick.RemoveAllListeners();
            BtnQuit.onClick.RemoveAllListeners();
        }

        private void OnBtnResumeClick()
        {
            Debug.Log("OnBtnResumeClick");
        }

        private void OnBtnLevelsClick()
        {
            Debug.Log("OnBtnLevelsClick");
        }

        private void OnBtnDiariesClick()
        {
            Debug.Log("OnBtnDiariesClick");
        }

        private void OnBtnCheckPointClick()
        {
            Debug.Log("OnBtnCheckPointClick");
        }

        private void OnBtnCreditsClick()
        {
            Debug.Log("OnBtnCreditsClick");
        }

        private void OnBtnQuitClick()
        {
            Debug.Log("OnBtnBtnQuitClick");
        }
    }
}

