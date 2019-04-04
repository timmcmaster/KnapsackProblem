using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem
{
    /// <summary>
    /// Item group where total weight is initialised to max int and recalculated as needed
    /// </summary>
    /// <seealso cref="KnapsackProblem.IItemGroup" />
    public class MinWeightItemGroup : IItemGroup
    {
        private readonly List<Item> _includedItems;
        private int _totalWeight;

        public MinWeightItemGroup(int initialWeight) : base()
        {
            _includedItems = new List<Item>();
            _totalWeight = initialWeight;
        }

        public MinWeightItemGroup(MinWeightItemGroup group): base()
        {
            _includedItems = new List<Item>(group._includedItems);
            _totalWeight = group.TotalWeight();
        }

        public IItemGroup Add(Item item)
        {
            _includedItems.Add(item);
            RecalculateWeight();
            return this;
        }

        public virtual void AddItem(Item item)
        {
            _includedItems.Add(item);
            RecalculateWeight();
        }

        public int TotalWeight()
        {
            return _totalWeight;
        }

        protected void RecalculateWeight()
        {
            _totalWeight = _includedItems.Sum(item => item.Weight);
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
    }
}