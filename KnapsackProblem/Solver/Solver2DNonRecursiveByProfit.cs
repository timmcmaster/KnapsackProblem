using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Solver
{
    /// <summary>
    /// Implements non-recursive dynamic programming algorithm for 0-1 knapsack problem with single objective
    /// see: https://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    public class Solver2DNonRecursiveByProfit : ISolver
    {
        // define max values for first N items at given weight, after choosing K
        private readonly ItemGroup[,] _minWeightItemGroup;
        private readonly int[,] _minWeightInt;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Solver2DNonRecursiveByProfit(Knapsack knapsack, List<Item> items)
        {
            _knapsack = knapsack;
            _items = items;

            // define max values for first N items at given weight
            var profitUpperBound = UtilFunctions.GetProfitUpperBound(_items, _knapsack);

            var numberOfProfitValues = profitUpperBound + 1;
            var numberOfItemsInList = items.Count + 1;
            _minWeightItemGroup = new ItemGroup[numberOfItemsInList, numberOfProfitValues];
            _minWeightInt = new int[numberOfItemsInList, numberOfProfitValues];

        }

        public void Solve()
        {
            CalculateNonRecursive();
            WriteSolutionAndData();
        }

        public void CalculateNonRecursive()
        {
            // Initialise values after 0 items
            _minWeightInt[0, 0] = 0;
            _minWeightItemGroup[0, 0] = new ItemGroup();

            for (int p = 1; p <= _minWeightInt.GetUpperBound(1); p++)
            {
                // initialise weight to capacity + 1
                _minWeightInt[0, p] = _knapsack.Capacity + 1;
                _minWeightItemGroup[0, p] = null;
            }

            // for each number of items from 1 to numberOfItems
            for (int i = 1; i <= _minWeightInt.GetUpperBound(0); i++)
            {
                // for each profit value from max down to profit of current item
                for (int profit = _minWeightInt.GetUpperBound(1); profit >= 0; profit--)
                {
                    // Calculate m[i,j] which is the minimum weight that can be obtained at profit j after looking at first i elements in list

                    // if profit we are calculating at is less than profit of this item, use previous value
                    if (_items[i - 1].Value > profit)
                    {
                        _minWeightItemGroup[i, profit] = _minWeightItemGroup[i - 1, profit] == null ? null : new ItemGroup(_minWeightItemGroup[i - 1, profit]);
                        _minWeightInt[i, profit] = _minWeightInt[i - 1, profit];
                    }
                    else
                    {
                        // if (weight at profit excluding this item) + weight of this item is less than previous min weight, then include this item
                        int weightAtProfitExcludingCurrentItem = _minWeightInt[i - 1, profit - _items[i - 1].Value];
                        int weightOfCurrentItem = _items[i - 1].Weight;
                        int weightOnPreviousIteration = _minWeightInt[i - 1, profit];

                        if (weightAtProfitExcludingCurrentItem + weightOfCurrentItem < weightOnPreviousIteration)
                        {
                            // maximum value is maximum before we added this item to the list
                            _minWeightInt[i, profit] = weightAtProfitExcludingCurrentItem + weightOfCurrentItem;
                        }
                        else
                        {
                            _minWeightInt[i, profit] = weightOnPreviousIteration;
                        }

                        // if (weight at profit excluding this item) + weight of this item is less than previous min weight, then include this item
                        weightAtProfitExcludingCurrentItem = _minWeightInt[i - 1, profit - _items[i - 1].Value];
                        weightOfCurrentItem = _items[i - 1].Weight;
                        weightOnPreviousIteration = _minWeightInt[i - 1, profit];
                        ItemGroup withCurrentItem = _minWeightItemGroup[i - 1, profit - _items[i - 1].Value].Add(_items[i-1]);
                        ItemGroup previousIteration = _minWeightItemGroup[i - 1, profit];

                        if (withCurrentItem.TotalWeight() < previousIteration.TotalWeight())
                        {
                            // maximum value is maximum before we added this item to the list
                            _minWeightItemGroup[i, profit] = new ItemGroup(withCurrentItem);
                        }
                        else
                        {
                            _minWeightItemGroup[i, profit] = previousIteration == null ? null : new ItemGroup(previousIteration);
                        }

                    }

                    //Console.WriteLine("Minimum weight for first {0} items at profit {1} is {2}", i, p,
                    //    _minWeightInt[i, p]);
                    //Console.WriteLine("Items: {0}, Count: {1}, Total weight: {2}", _minWeightInt[i, weight].ItemNames(),
                    //    _minWeightInt[i, weight].ItemCount(), _minWeightInt[i, weight].TotalWeight());
                }
            }

            // Maximal Solution at any given iteration (value of i) is the highest value of p where total weight < capacity
            //int[] optimalProfit = new int[_minWeightInt.GetUpperBound(0)];
            //int[] weightAtOptimalProfit = new int[_minWeightInt.GetUpperBound(0)];

            for (int i = _minWeightInt.GetUpperBound(0); i >= 0; i--)
            {
                for (int p = _minWeightInt.GetUpperBound(1); p >= 0; p--)
                {
                    if (_minWeightItemGroup[i, p] != null)
                    {
                        //optimalProfit[i] = p;
                        //weightAtOptimalProfit[i] = _minWeightInt[i, p];

                        Console.WriteLine("Optimal profit for first {0} items is {1} at weight {2}", i, p, _minWeightItemGroup[i, p].TotalWeight());
                        Console.WriteLine("Items: {0}, Count: {1}, Total weight: {2}", 
                            _minWeightItemGroup[i, p].ItemNames(),
                            _minWeightItemGroup[i, p].ItemCount(), 
                            _minWeightItemGroup[i, p].TotalWeight());
                        break; // out of for p loop
                    }

                    if (_minWeightInt[i, p] <= _knapsack.Capacity)
                    {
                        //optimalProfit[i] = p;
                        //weightAtOptimalProfit[i] = _minWeightInt[i, p];
                        
                        Console.WriteLine("Optimal profit for first {0} items is {1} at weight {2}", i, p,  _minWeightInt[i, p]);
                        break; // out of for p loop
                    }
                }
            }
        }

        private void WriteSolutionAndData()
        {
            //ItemGroup requiredValueGroup = _minWeightInt[_items.Count, _knapsack.Capacity];
            //int minValue = _minWeightInt[_items.Count, _knapsack.Capacity];

            //Console.WriteLine("Maximum value for first {0} items at weight {1} is {2}", _items.Count, _knapsack.Capacity, requiredValueGroup.TotalValue());
            //Console.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            //Console.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            //Console.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            //Console.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());

            DumpArrayToLog(m => m);
            //DumpArrayToLog(m => m.ItemCount());
        }

        private void DumpArrayToLog(Func<int, int> getValue)
        {
            StringBuilder sb = new StringBuilder();

            // write indices at top
            sb.Append("  |");

            for (int j = 0; j < _minWeightInt.GetLength(1); j++)
            {
                sb.AppendFormat(" {0,4:###0}", j);
            }
            sb.Append("]");
            LogFile.WriteLine(sb.ToString());


            // for each item from 0 to number of items
            for (int i = 0; i < _minWeightInt.GetLength(0); i++)
            {
                sb.Clear();
                sb.AppendFormat("{0,2:#0}", i);
                sb.Append("[");

                for (int j = 0; j < _minWeightInt.GetLength(1); j++)
                {
                    sb.AppendFormat(" {0,4:###0}", getValue(_minWeightInt[i, j]));
                }
                sb.Append("]");

                LogFile.WriteLine(sb.ToString());
            }
        }
    }
}