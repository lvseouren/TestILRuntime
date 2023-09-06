using TMPro;

namespace Hotfix.UI
{
    public class BackpackItemView:UIView
    {
        TextMeshProUGUI mNameTxt;
        TextMeshProUGUI mCountTxt;
        TextMeshProUGUI mTypeTxt;

        public override void Init()
        {
            base.Init();

        }

        protected override void GetChild()
        {
            base.GetChild();
            mNameTxt = transform.Find("Bg/NameTxt").GetComponent<TextMeshProUGUI>();
            mCountTxt = transform.Find("CountTxt").GetComponent<TextMeshProUGUI>();
            mTypeTxt = transform.Find("TypeTxt").GetComponent<TextMeshProUGUI>();
        }

        public void Setting(Item item)
        {
            mNameTxt.text = string.Format("item@{0}", item.id);
            mCountTxt.text = string.Format("x{0}", item.count);
            mTypeTxt.text = string.Format("type@{0}", item.itemType);
        }
    }
}
