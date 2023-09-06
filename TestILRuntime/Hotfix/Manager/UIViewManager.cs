using Hotfix.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix.Manager
{
    public class UIViewManager : Singleton<UIViewManager>
    {
        //存放所有在场景中的UIView
        List<UIView> m_UIViewList;

        public void Init()
        {
            m_UIViewList = new List<UIView>();
        }

        public void Update()
        {
            for (int i = 0; i < m_UIViewList.Count; i++)
            {
                //销毁UIView
                if (m_UIViewList[i].isWillDestroy)
                {
                    m_UIViewList[i].DestroyImmediately();
                    m_UIViewList.RemoveAt(i);
                    i--;
                    continue;
                }

                if (m_UIViewList[i].isVisible)
                {
                    m_UIViewList[i].Update();
                }
            }
        }

        public void LateUpdate()
        {

            for (int i = 0; i < m_UIViewList.Count; i++)
            {
                if (m_UIViewList[i].isVisible)
                {
                    m_UIViewList[i].LateUpdate();
                }
            }
        }

        public void FixedUpdate()
        {

            for (int i = 0; i < m_UIViewList.Count; i++)
            {
                if (m_UIViewList[i].isVisible)
                {
                    m_UIViewList[i].FixedUpdate();
                }
            }
        }

        public T CreateView<T>(string url) where T : UIView
        {
            return CreateView<T>(url);
        }

        public T CreateView<T>( string url, RectTransform parent = null) where T : UIView,new()
        {
            T view = new T();
            GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(url)) as GameObject;
            view.SetGameObject(gameObject, parent);
            view.Init();
            AddUIView(view);
            return view as T;
        }

        public void AddUIView(UIView view)
        {
            if (view != null)
                m_UIViewList.Add(view);
        }

        public void DestroyAll()
        {
            for (int i = m_UIViewList.Count - 1; i >= 0; i--)
                m_UIViewList[i].Destroy();
        }

        public void DestroyViewOnLoadScene()
        {
            for (int i = m_UIViewList.Count - 1; i >= 0; i--)
                if (!m_UIViewList[i].isDontDestroyOnLoad)
                    m_UIViewList[i].Destroy();
        }
    }
}
