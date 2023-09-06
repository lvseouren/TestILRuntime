using Hotfix.Manager;
using Hotfix.UI;
using MainProject;
using UnityEngine;

namespace Hotfix
{
    public class HotfixLuanch
    {
        public static void Start()
        {
            Launch.OnUpdate = Update;
            Launch.OnLateUpdate = LateUpdate;
            Launch.OnFixedUpdate = FixedUpdate;
            Launch.OnApplicationQuitAction = OnApplicationQuit;

            UIPanelManager.Instance.Init();
            UIViewManager.Instance.Init();
            BackpackDataManager.Instance.Init();

            UIPanelManager.Instance.ShowPanel<MainPanel>("MainPanel");
        }

        public static void Update()
        {
            UIViewManager.Instance.Update();
        }

        public static void LateUpdate()
        {
            UIViewManager.Instance.LateUpdate();
        }

        public static void FixedUpdate()
        {
            UIViewManager.Instance.FixedUpdate();
        }

        public static void OnApplicationQuit()
        {
            Debug.Log("Hotfix ApplicationQuit");
        }
    }
}
