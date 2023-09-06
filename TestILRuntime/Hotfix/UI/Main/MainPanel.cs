using Hotfix.Manager;
using UnityEngine.UI;

namespace Hotfix.UI
{
    public class MainPanel:UIPanel
    {
        Button mBackpackBtn;
        public override void Init()
        {
            base.Init();
            mTitle.text = "MainPanel";
            mBackpackBtn.onClick.AddListener(OnClickBackpackBtn);
        }
        protected override void GetChild()
        {
            base.GetChild();
            mBackpackBtn = transform.Find("BackpackBtn").GetComponent<Button>();
        }

        void OnClickBackpackBtn()
        {
            UIPanelManager.Instance.ShowPanel<BackpackPanel>("BackpackPanel");
        }
    }
}
