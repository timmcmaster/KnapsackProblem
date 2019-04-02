using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Solver
{
    /// <summary>
    /// Implements non-recursive dynamic programming algorithm for 0-1 knapsack problem with single objective
    /// Uses array of item groups at given profit and count (to give collection results)
    /// see: https://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    public class Solver2DNonRecursiveByProfitUsingItemGroup : ISolver
    {
        // define min weights for first N items at given profit, after choosing K
        private readonly ItemGroup[,] _minWeightItemGroup;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Solver2DNonRecursiveByProfitUsingItemGroup(Knapsack knapsack, List<Item> items)
        {
            _knapsack = knapsack;
            _items = items;

            // define max values for first N items at given weight
            var profitUpperBound = UtilFunctions.GetProfitUpperBound(_items, _knapsack);

            var numberOfProfitValues = profitUpperBound + 1;
            var numberOfItemsInList = items.Count + 1;
            _minWeightItemGroup = new ItemGroup[numberOfItemsInList, numberOfProfitValues];

        }

        public void Solve()
        {
            CalculateNonRecursive();
            WriteSolutionAndData();
        }

        public void CalculateNonRecursive()
        {
            // Initialise values after 0 items to weight of 0
            _minWeightItemGroup[0, 0] = new ItemGroup();

            for (int p = 1; p <= _minWeightItemGroup.GetUpperBound(1); p++)
            {
                // initialise item collection as null
                _minWeightItemGroup[0, p] = null;       // null item group will be treated as weight above capacity 
            }

            // for each number of items from 1 to numberOfItems
            for (int i = 1; i <= _minWeightItemGroup.GetUpperBound(0); i++)
            {
                // for each profit value from max down to profit of current item
                for (int profit = _minWeightItemGroup.GetUpperBound(1); profit >= 0; profit--)
                {
                    // Calculate m[i,j] which is the minimum weight that can be obtained at profit j after looking at first i elements in list

                    // if profit we are calculating at is less than profit of this item, use previous value
                    if (_items[i - 1].Value > profit)
                    {
                        _minWeightItemGroup[i, profit] = ItemGroup.CopyOrNull(_minWeightItemGroup[i - 1, profit]);
                    }
                    else
                    {
                        // if (weight at profit excluding this item) + weight of this item is less than previous min weight, then include this item
                        int weightAtProfitExcludingCurrentItem =
                            GetWeightOrDefault(_minWeightItemGroup[i - 1, profit - _items[i - 1].Value]);

                        int weightOfCurrentItem = _items[i - 1].Weight;
                        int weightOnPreviousIteration = GetWeightOrDefault(_minWeightItemGroup[i - 1, profit]);

                        if (weightAtProfitExcludingCurrentItem + weightOfCurrentItem < weightOnPreviousIteration)
                        {
                            // maximum value is maximum before we added this item to the list
                            var itemGroupExcludingCurrent =
                                ItemGroup.CopyOrNull(_minWeightItemGroup[i - 1, profit - _items[i - 1].Value]);
                            if (itemGroupExcludingCurrent == null)
                            {
                                _minWeightItemGroup[i, profit] = new ItemGroup().Add(_items[i - 1]);
                            }
                            else
                            {
                                _minWeightItemGroup[i, profit] = new ItemGroup(itemGroupExcludingCurrent).Add(_items[i - 1]);
                            }
                        }
                        else
                        {
                            _minWeightItemGroup[i, profit] = ItemGroup.CopyOrNull(_minWeightItemGroup[i - 1, profit]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the weight of the actual group
        /// or default where null value (which is Capacity + 1).
        /// </summary>
        /// <param name="itemGroup">The item group.</param>
        /// <returns></returns>
        private int GetWeightOrDefault(ItemGroup itemGroup)
        {
            return itemGroup?.TotalWeight() ?? _knapsack.Capacity + 1;
        }

        private void WriteSolutionAndData()
        {
            // Maximal Solution at any given iteration (value of i) is the highest value of profit where item group is not null
            for (int i = _minWeightItemGroup.GetUpperBound(0); i >= 0; i--)
            {
                for (int profit = _minWeightItemGroup.GetUpperBound(1); profit >= 0; profit--)
                {
                    if (_minWeightItemGroup[i, profit] != null)
                    {
                        Console.WriteLine("Optimal profit for first {0} items is {1} at weight {2}", i, profit, _minWeightItemGroup[i, profit].TotalWeight());
                        Console.WriteLine("Items: {0}, Count: {1}, Total weight: {2}",
                            _minWeightItemGroup[i, profit].ItemNames(),
                            _minWeightItemGroup[i, profit].ItemCount(),
                            _minWeightItemGroup[i, profit].TotalWeight());
                        break; // out of for p loop
                    }
                }
            }

            DumpArrayToLog(m => m.TotalWeight());
        }

        private void DumpArrayToLog(Func<ItemGroup, int> getValue)
        {
            StringBuilder sb = new StringBuilder();

            // write indices at top
            sb.Append("  |");

            for (int j = 0; j < _minWeightItemGroup.GetLength(1); j++)
            {
                sb.AppendFormat(" {0,4:###0}", j);
            }
            sb.Append("|");
            LogFile.WriteLine(sb.ToString());


            // for each item from 0 to number of items
            for (int i = 0; i < _minWeightItemGroup.GetLength(0); i++)
            {
                sb.Clear();
                sb.AppendFormat("{0,2:#0}", i);
                sb.Append("[");

                for (int j = 0; j < _minWeightItemGroup.GetLength(1); j++)
                {
                    if (_minWeightItemGroup[i, j] == null)
                    {
                        sb.Append("    x");
                    }
                    else
                    {
                        sb.AppendFormat(" {0,4:###0}", getValue(_minWeightItemGroup[i, j]));
                    }
                }
                sb.Append("]");

                LogFile.WriteLine(sb.ToString());
            }
        }
    }
}