using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EJ
{
    public class Util : MonoBehaviour
    {
        public static Util Instance;
        public bool isTest;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            int frame = ES3.Load<int>("Frame", 144);
            Application.runInBackground = false;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            switch (frame)
            {
                case 30:
                    TargetFrame30();
                    break;
                case 60:
                    TargetFrame60();
                    break;
                case 144:
                    TargetFrame144();
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TargetFrame60();
            }
        }


        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                try
                {
                    SaveGame();
                    
                    
                }
                catch (Exception e)
                {
                    $"게임종료 이벤트 에러 = { e}".Log();
                }
            }
        }


        /// <summary>
        /// 급작 스럽게 종료시 서버에 데이터 전송
        /// </summary>
        public void OnApplicationQuit()
        {
            "게임 종료시 발동 이벤트".Log();
            try
            {
                
                SaveGame();
                
                
            }
            catch (Exception e)
            {
                e.Log();
                "게임종료 이벤트 에러".Log();
            }
        }

        public void GameExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
        }

        public void SaveGame()
        {
            if (isTest)
            {
                Debug.Log("Util Bool Test Enable Not Save");
                return;
            }

            // 새로운 프로젝트일시 주의할것
            if (SceneManager.GetActiveScene().name != "Main")
                return;
            
            // 플레이팹매니저 데이터 전송
           
           
          
            // EnhancePresenter.Instance.UpLoadData();
            // WeaponPresenter.Instance.UpLoadData();
            // SkillPresenter.Instance.UpLoadData();
            // LaboratoryPresenter.Instance.UpLoaData();
            // RobotEnhancePresenter.Instance.UpLoadData();
            // GachaPresenter.Instance.UploadData();
            
            // 플레이팹 데이터 업데이트
            "플팹 세이브".Log();
            
        }

        public void TargetFrame144()
        {
            Application.targetFrameRate = 144;
        }
        public void TargetFrame60()
        {
            Application.targetFrameRate = 60;
        }

        public void TargetFrame30()
        {
            Application.targetFrameRate = 30;
        }
        
        public static float HealthPercent(float currentHp, float maxHp)
        {
            float hpPercent =currentHp /maxHp;

            return hpPercent;
        }

        public static float HealthPercent(BigInteger currentHp, BigInteger maxHp)
        {
            float hpPercent = (float)currentHp / (float)maxHp;

            return hpPercent;
        }
        
        public static string HealthPercentText(BigInteger currentExp, BigInteger maxExp)
        {
            double hpPercent = Math.Truncate(((double)currentExp / (double)maxExp) * 100);
            if (hpPercent < 0)
                hpPercent = 0;
            //Debug.Log(expPercent);
            //return $"{expPercent}<size=20> %</size>";
            return $"{hpPercent} %";
        }
        
        public static void SetExpSlider(Slider slider,float currentExp, float maxExp)
        {
            float expPercent = (currentExp / maxExp);
            slider.value = expPercent;
            //Debug.Log(expPercent);
        }

        public static float expPercent(float currentExp, float maxExp)
        {
            float expPercent = (currentExp / maxExp);
            //Debug.Log(expPercent);
            return expPercent;
        }
        
        public static float expPercent(BigInteger currentExp, BigInteger maxExp)
        {
            float expPercent = ((float)(currentExp*100 / maxExp))*0.01f;
            //Debug.Log(expPercent);
            return expPercent;
        }

        public static string expPercentText(float currentExp, float maxExp)
        {
            double expPercent = Math.Truncate((currentExp / maxExp) * 100);
            Debug.Log(expPercent);
            return $"{expPercent}<size=20> %</size>";
        }
        
        public static string expPercentText(BigInteger currentExp, BigInteger maxExp)
        {
            double expPercent = Math.Truncate(((double)currentExp / (double)maxExp) * 100);
            //Debug.Log(expPercent);
            //return $"{expPercent}<size=20> %</size>";
            return $"{expPercent} %";
        }

        public static string stageHyphen(int stage)
        {
            if (stage % 10 == 0)
            {
                return $"<size=30>BOSS</size>";
            }
            else
            {
                //return $"Stage\n<size=30>{stage}</size>";
                return $"{stage}";
            }
        }

        public static string expText(float currentExp, float maxExp)
        {
            return $"{currentExp} / {maxExp}";
        }
    }
}