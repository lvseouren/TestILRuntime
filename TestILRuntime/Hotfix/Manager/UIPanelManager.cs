using Hotfix.UI;
using System.Collections.Generic;
using Tool;
using UnityEngine;

namespace Hotfix.Manager
{
    public class UIPanelManager:Singleton<UIPanelManager>
    {
        Dictionary<string, UIPanel> m_UIPanelDic;//存放所有存在在场景中的UIPanel
        Transform m_defaultCanvas;
        private UIPanel currentPanel;

        public Transform defaultCanvas { get { return m_defaultCanvas; } }

        public void Init()
        {
            m_UIPanelDic = new Dictionary<string, UIPanel>();

            m_defaultCanvas = GameObject.Find(GlobalDefine.UI_DEFAULT_CANVAS_NAME).transform;
            GameObject.DontDestroyOnLoad(m_defaultCanvas.gameObject);
        }

        //显示一个UIPanel，参数为回调和自定义传递数据
        public void ShowPanel<T>(string url) where T:UIPanel,new()
        {
            if (m_UIPanelDic.TryGetValue(url, out UIPanel panel))
            {
                panel = m_UIPanelDic[url];
                panel.Show();
                currentPanel = panel;
            }else
            {
                m_UIPanelDic[url] = panel;
                panel = new T();
                panel.url = url;
                panel.Load(() =>
                {
                    if (panel.isLoaded)
                    {
                        panel.rectTransform.SetParentAndResetTrans(m_defaultCanvas);
                    }
                    else
                        m_UIPanelDic.Remove(url);
                });
            }
        }


        public void HidePanel()
        {
            currentPanel?.Hide();
        }


        void UnLoadPanel(string url)
        {
            if (m_UIPanelDic.TryGetValue(url, out UIPanel panel))
            {
                panel.Destroy();
                m_UIPanelDic.Remove(url);
            }
            else
                Debug.LogError("UIPanel not exist: " + url);
        }

        void UnLoadAllPanel()
        {
            foreach (var panel in m_UIPanelDic.Values)
                panel.Destroy();
            m_UIPanelDic.Clear();
        }


        public void OnApplicationQuit()
        {
            UnLoadAllPanel();
        }
    }
}
