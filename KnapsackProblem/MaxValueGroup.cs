using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackProblem
{
    public class MaxValueGroup
    {
        private readonly List<Item> _includedItems;

        public MaxValueGroup()
        {
            _includedItems = new List<Item>();
        }

        public MaxValueGroup(MaxValueGroup item)
        {
            _includedItems = new List<Item>(item._includedItems);
        }

        public void AddItem(Item item)
        {
            _includedItems.Add(item);
        }

        public int TotalWeight()
        {
            return _includedItems.Sum(item => item.Weight);
        }

        public int TotalValue()
        {
            return _includedItems.Sum(item => item.Value);
        }

        public int ItemCount()
        {
            return _includedItems.Count();
        }

        public string ItemNames()
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
