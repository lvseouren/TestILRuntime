

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotfix
{
    public enum ItemType
    {
        Type1,
        Type2,
        Type3,
        Type4,
        TypeAll,
    }
    public class Item
    {
        public ItemType itemType;
        public int id;
        public int count;
        public Item(int id, int count)
        {
            itemType = (ItemType)(id % 4);
            this.id = id;
            this.count = count;
        }
    }
    public class BackpackDataManager:Singleton<BackpackDataManager>
    {
        Dictionary<int, Item> mItemDict;
        
        public Action OnDataChanged;

        public void Init()
        {
            mItemDict = new Dictionary<int, Item>();
            mItemDict.Add(1, new Item(1, 100));
            mItemDict.Add(11, new Item(11, 100));
            mItemDict.Add(111, new Item(111, 100));
        }

        public List<Item> GetItemList(ItemType itemType)
        {
            var lst = mItemDict.Select(kv => kv.Value).Where(item=>item.itemType == itemType || itemType == ItemType.TypeAll).ToList();
            if (itemType == ItemType.TypeAll)
            {
                lst.Sort((a, b) =>
                  {
                      if (a.itemType != b.itemType)
                          return a.itemType - b.itemType;
                      return a.id - b.id;
                  });
            }
            return lst;
        }

        public void AddItem(int id, int count)
        {
            if(mItemDict.TryGetValue(id, out Item item))
                item.count += count;
            else
                mItemDict.Add(id, new Item(id, count));
            OnDataChanged();
        }

        public void RemoveItem(int id)
        {
            if (mItemDict.ContainsKey(id))
            {
                mItemDict.Remove(id);
                OnDataChanged();
            }
        }
    }
}
