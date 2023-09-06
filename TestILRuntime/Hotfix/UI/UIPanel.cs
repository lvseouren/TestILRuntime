using System;
using TMPro;
using UnityEngine;


namespace Hotfix.UI
{
    public class UIPanel:UIView
    {
        //需要加载的prefab的路径，也作为唯一标识符
        public string url { get; set; }

        //UIPanel间的自定义传递数据
        public object data;

        protected TextMeshProUGUI mTitle;

        public UIPanel()
        {
            
        }

        protected override void GetChild()
        {
            base.GetChild();
            mTitle = transform.Find("Top/Title").GetComponent<TextMeshProUGUI>();
        }


        public override void Show()
        {
            base.Show();
            rectTransform?.SetAsLastSibling();
        }

        public virtual void Load(Action callback = null)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(url)) as GameObject;
            if (gameObject != null)
            {
                SetGameObject(gameObject);
                Init();
                callback?.Invoke();
            }
        }
    }
}
