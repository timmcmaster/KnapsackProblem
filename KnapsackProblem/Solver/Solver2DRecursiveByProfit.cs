using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem.Solver
{
    /// <summary>
    /// Implements recursive dynamic programming algorithm for 0-1 knapsack problem with single objective
    /// see: https://en.wikipedia.org/wiki/Knapsack_problem
    /// No constraint on total number of items to be selected
    /// </summary>
    public class Solver2DRecursiveByProfit : ISolver
    {
        // define max values for first N items at given weight, after choosing K
        private readonly MaxValueGroup[,] _maxValues;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Solver2DRecursiveByProfit(Knapsack knapsack, List<Item> items)
        {
            _knapsack = knapsack;
            _items = items;

            // define max values for first N items at given weight
            var numberOfWeights = knapsack.Capacity + 1;
            var numberOfItemsInList = items.Count + 1;
            _maxValues = new MaxValueGroup[numberOfItemsInList, numberOfWeights];
        }

        public void Solve()
        {
            CalculateRecursive();
        }

        public void CalculateRecursive()
        {
            MaxValueGroup requiredValueGroup = GetMaxValue(_items.Count, _knapsack.Capacity);

            LogFile.WriteLine("Maximum value after choosing {0} of first {1} items at weight {2} is {3}",
                _knapsack.AllowedItems,
                _items.Count,
                _knapsack.Capacity,
                requiredValueGroup.TotalValue()
            );
            LogFile.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            LogFile.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            LogFile.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            LogFile.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());

            Console.WriteLine("Maximum value after choosing {0} of first {1} items at weight {2} is {3}",
                _knapsack.AllowedItems,
                _items.Count,
                _knapsack.Capacity,
                requiredValueGroup.TotalValue()
            );
            Console.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            Console.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            Console.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            Console.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());

            DumpArrayToLog(m => m.TotalValue());
            DumpArrayToLog(m => m.ItemCount());
        }

        public MaxValueGroup GetMaxValue(int forFirstN, int atWeight)
        {
            // value at 0 items or 0 weight, or 0 items chosen is 0
            if (forFirstN == 0 || atWeight <= 0)
            {
                return new MaxValueGroup();
            }

            // calculate the previous value
            if (_maxValues[forFirstN - 1, atWeight] is null)
            {
                _maxValues[forFirstN - 1, atWeight] = GetMaxValue(forFirstN - 1, atWeight);
            }

            // if current item weight is greater than weight being calculated, don't add the value
            if (_items[forFirstN - 1].Weight > atWeight)
            {
                _maxValues[forFirstN, atWeight] = new MaxValueGroup(_maxValues[forFirstN - 1, atWeight]);
            }
            else
            {
                MaxValueGroup withoutCurrentItem = _maxValues[forFirstN - 1, atWeight - _items[forFirstN - 1].Weight];

                // if we haven't calculated max without current item, do it now
                if (withoutCurrentItem is null)
                {
                    withoutCurrentItem = GetMaxValue(forFirstN - 1, atWeight - _items[forFirstN - 1].Weight);
                    _maxValues[forFirstN - 1, atWeight - _items[forFirstN - 1].Weight] = withoutCurrentItem;
                }

                // add item to prev
                MaxValueGroup withCurrentItem = new MaxValueGroup(withoutCurrentItem).Add(_items[forFirstN - 1]);

                if (withCurrentItem.TotalValue() > _maxValues[forFirstN - 1, atWeight].TotalValue())
                {
                    _maxValues[forFirstN, atWeight] = withCurrentItem;
                }
                else
                {
                    _maxValues[forFirstN, atWeight] = new MaxValueGroup(_maxValues[forFirstN - 1, atWeight]);
                }
            }

            //LogFile.WriteLine("Maximum value after choosing {3} of first {0} items at weight {1} is {2}",
            //    forFirstN,
            //    atWeight,
            //    _maxValues[forFirstN, atWeight, itemsLeft].TotalValue(),
            //    itemsLeft);
            //LogFile.WriteLine("Items: {0}, Count: {1}, Total weight: {2}",
            //    _maxValues[forFirstN, atWeight, itemsLeft].ItemNames(),
            //    _maxValues[forFirstN, atWeight, itemsLeft].ItemCount(),
            //    _maxValues[forFirstN, atWeight, itemsLeft].TotalWeight());

            return _maxValues[forFirstN, atWeight];
        }

        protected int GetProfitUpperBound()
        {
            // worst case upper bound is double the LP approximation 
            return (int)Math.Ceiling(LP_Approximation() * 2);
        }

        /// <summary>
        /// Uses greedy algorithm to get approximation of highest value
        /// </summary>
        /// <returns></returns>
        protected double LP_Approximation()
        {
            MaxValueGroup group = new MaxValueGroup();

            List<Item> sortedItems = UtilFunctions.SortByUnitProfitDescending(_items);

            Item criticalItem = new Item("",0,0);
            double criticalItemFraction = 0;

            // Fill until we reach critical item
            foreach (Item item in sortedItems)
            {
                if (group.TotalWeight() + item.Weight <= _knapsack.Capacity)
                {
                    group.AddItem(item);
                }
                else // we have reached critical item
                {
                    criticalItem = item;
                    criticalItemFraction = (_knapsack.Capacity - group.TotalWeight()) / (double)criticalItem.Weight;
                    break;
                }
            }

            // Add fractional value of next item
            double maxProfitValue = group.TotalValue() + criticalItem.Value * criticalItemFraction;
            return maxProfitValue;
        }

        //protected int Greedy()
        //{
        //    MaxValueGroup group = new MaxValueGroup();

        //    List<Item> sortedItems = SortByUnitProfitDescending(_items);

        //    foreach (Item t in sortedItems)
        //    {
        //        if (group.TotalWeight() + t.Weight <= _knapsack.Capacity)
        //        {
        //            group.AddItem(t);
        //        }
        //    }

        //    return group.TotalValue();
        //}

        private void DumpArrayToLog(Func<MaxValueGroup, int> getValue)
        {
            StringBuilder sb = new StringBuilder();

            // write indices at top
            sb.Append("  |");

            for (int j = 0; j < _maxValues.GetLength(1); j++)
            {
                sb.AppendFormat(" {0,4:###0}", j);
            }
            sb.Append("]");
            LogFile.WriteLine(sb.ToString());


            // for each item from 0 to number of items
            for (int i = 0; i < _maxValues.GetLength(0); i++)
            {
                sb.Clear();
                sb.AppendFormat("{0,2:#0}", i);
                sb.Append("[");

                for (int j = 0; j < _maxValues.GetLength(1); j++)
                {
                    if (_maxValues[i, j] is null)
                    {
                        sb.Append("    x");
                    }
                    else
                    {
                        sb.AppendFormat(" {0,4:###0}", getValue(_maxValues[i, j]));
                    }
                }
                sb.Append("]");

                LogFile.WriteLine(sb.ToString());
            }
        }
    }
}