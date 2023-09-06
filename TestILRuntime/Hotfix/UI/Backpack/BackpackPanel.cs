using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hotfix.Manager;
using System;
using TMPro;

namespace Hotfix.UI
{
    public class ObjectPool<T>
    {
        private Queue<T> mPool = new Queue<T>();
        Func<T> mGenerator;
        public ObjectPool(Func<T> generator)
        {
            mGenerator = generator;
        }

        public T GetItem()
        {
            if (mPool.Count > 0)
                return mPool.Dequeue();
            return mGenerator();
        }

        public void Free(T item)
        {
            mPool.Enqueue(item);
        }

    }
    public class BackpackPanel:UIPanel
    {
        ScrollRect mScrollRect;
        GridLayoutGroup mGrid;
        HorizontalLayoutGroup mItemTypeLayout;
        TMP_InputField mInputFieldId;
        TMP_InputField mInputFieldNum;
        Button mAddItemBtn;
        Button mRemoveItemBtn;
        TextMeshProUGUI mTipsTxt;

        List<BackpackItemView> mCurrItemList;
        ObjectPool<BackpackItemView> mItemPool;
        List<BackpackFilterItemView> mFilterItemList;

        ItemType mCurrItemType;
        public override void Init()
        {
            base.Init();
            mTitle.text = "Backpack";
            mAddItemBtn.onClick.AddListener(OnAddItemBtnClick);
            mRemoveItemBtn.onClick.AddListener(OnRemoveItemBtnClick);
            mTipsTxt.text = "";

            mItemPool = new ObjectPool<BackpackItemView>(() =>
            {
                BackpackItemView view = UIViewManager.Instance.CreateView<BackpackItemView>("BackpackItem", mGrid.transform as RectTransform);
                return view;
            });
            mCurrItemList = new List<BackpackItemView>();
            mFilterItemList = new List<BackpackFilterItemView>();
            BackpackDataManager.Instance.OnDataChanged += RefreshItemList;

            mCurrItemType = ItemType.Type1;
            foreach (var itemType in Enum.GetValues(typeof(ItemType)))
            {
                BackpackFilterItemView view = UIViewManager.Instance.CreateView<BackpackFilterItemView>("BackpackFilterItem", mItemTypeLayout.transform as RectTransform);
                view.Setting((ItemType)itemType);
                view.UpdateSelected(mCurrItemType);
                view.OnClickCallback = OnFilterClick;

                mFilterItemList.Add(view);
            }

            RefreshItemList();
        }

        private void OnRemoveItemBtnClick()
        {
            if (string.IsNullOrEmpty(mInputFieldId.text))
            {
                mTipsTxt.text = "Please Enter Id First";
                return;
            }
            
            int id = Convert.ToInt32(mInputFieldId.text);
            BackpackDataManager.Instance.RemoveItem(id);
            mTipsTxt.text = "Remove Item Succeed";
        }

        private void OnAddItemBtnClick()
        {
            if (string.IsNullOrEmpty(mInputFieldId.text))
            {
                mTipsTxt.text = "Please Enter Id First";
                return;
            }

            int id = Convert.ToInt32(mInputFieldId.text);
            try
            {
                int count = string.IsNullOrEmpty(mInputFieldNum.text) ? 1 : Convert.ToInt32(mInputFieldNum.text);
                BackpackDataManager.Instance.AddItem(id, count);
                mTipsTxt.text = "Add Item Succeed";
            }
            catch(Exception e)
            {
                mTipsTxt.text = e.Message;
            }
        }

        protected override void GetChild()
        {
            base.GetChild();

            mScrollRect = transform.Find("ScrollRect").GetComponent<ScrollRect>();
            mGrid = transform.Find("ScrollRect/Viewport/Content").GetComponent<GridLayoutGroup>();
            mItemTypeLayout = transform.Find("FilterContent").GetComponent<HorizontalLayoutGroup>();
            mInputFieldId = transform.Find("InputField_Id").GetComponent<TMP_InputField>();
            mInputFieldNum = transform.Find("InputField_Num").GetComponent<TMP_InputField>();
            mAddItemBtn = transform.Find("ButtonAdd").GetComponent<Button>();
            mRemoveItemBtn = transform.Find("ButtonRemove").GetComponent<Button>();
            mTipsTxt = transform.Find("TxtTips").GetComponent<TextMeshProUGUI>();
        }

        private void OnFilterClick(ItemType itemType)
        {
            if(mCurrItemType != itemType)
            {
                mCurrItemType = itemType;
                foreach(var item in mFilterItemList)
                {
                    item.UpdateSelected(mCurrItemType);
                }
                RefreshItemList();
            }
        }

        private void RefreshItemList()
        {
            foreach(var item in mCurrItemList)
            {
                item.Hide();
                mItemPool.Free(item);
            }
            mCurrItemList.Clear();
            var itemList = BackpackDataManager.Instance.GetItemList(mCurrItemType);
            if (itemList == null)
                return;
            for (int i = 0; i < itemList.Count; i++)
            {
                //m_grid.transform is RectTransform
                BackpackItemView view = mItemPool.GetItem();
                view.Setting(itemList[i]);
                view.Show();
                mCurrItemList.Add(view);
            }
        }
    }
}
