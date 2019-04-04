using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem
{
    public class ItemGroup : IItemGroup
    {
        private readonly List<Item> _includedItems;

        public ItemGroup()
        {
            _includedItems = new List<Item>();
        }

        public ItemGroup(ItemGroup group)
        {
            _includedItems = new List<Item>(group._includedItems);
        }

        public virtual IItemGroup Add(Item item)
        {
            _includedItems.Add(item);
            return this;
        }

        public virtual void AddItem(Item item)
        {
            _includedItems.Add(item);
        }

        public virtual int TotalWeight()
        {
            return _includedItems.Sum(item => item.Weight);
        }

        public virtual int TotalValue()
        {
            return _includedItems.Sum(item => item.Value);
        }

        public virtual int ItemCount()
        {
            return _includedItems.Count();
        }

        public virtual string ItemNames()
        {
            StringBuilder sbNames = new StringBuilder();

            foreach (Item item in _includedItems)
            {
                if (sbNames.Length > 0)
                {
                    sbNames.Append(",");
                }
                sbNames.Append(item.Name);
            }

            return sbNames.ToString();
        }

        public static ItemGroup CopyOrNull(ItemGroup itemGroup)
        {
            return itemGroup == null ? null : new ItemGroup(itemGroup);
        }
    }
}
