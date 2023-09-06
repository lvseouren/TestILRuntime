using System;
using TMPro;
using UnityEngine.UI;

namespace Hotfix.UI
{
    public class BackpackFilterItemView:UIView
    {
        ItemType mItemType;
        TextMeshProUGUI mNameTxt;
        Button mBtn;
        Image mSelectedImg;
        public Action<ItemType> OnClickCallback;
        public override void Init()
        {
            base.Init();
            mBtn.onClick.AddListener(OnClick);
        }

        protected override void GetChild()
        {
            base.GetChild();
            mBtn = transform.GetComponent<Button>();
            mSelectedImg = transform.Find("SelectedImg").GetComponent<Image>();
            mNameTxt = transform.Find("TxtName").GetComponent<TextMeshProUGUI>();
        }

        public void Setting(ItemType itemType)
        {
            mItemType = itemType;
            mNameTxt.text = String.Format("type@{0}", mItemType);
        }

        public void UpdateSelected(ItemType currItemType)
        {
            bool isSelected = mItemType == currItemType;
            mSelectedImg.enabled = isSelected;
        }

        private void OnClick()
        {
            OnClickCallback?.Invoke(mItemType);
        }

    }
}
