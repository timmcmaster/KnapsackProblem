using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Solver
{
    /// <summary>
    /// Implements non-recursive dynamic programming algorithm for 0-1 knapsack problem with single objective
    /// Uses integer array of weights at given profit and count
	/// Note that this will not give items in minimal collection
    /// see: https://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    public class Solver2DNonRecursiveByProfitUsingInt : ISolver
    {
        // define min weights for first N items at given profit, after choosing K
        private readonly int[,] _minWeightInt;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Solver2DNonRecursiveByProfitUsingInt(Knapsack knapsack, List<Item> items)
        {
            _knapsack = knapsack;
            _items = items;

            // define max values for first N items at given weight
            var profitUpperBound = UtilFunctions.GetProfitUpperBound(_items, _knapsack);

            var numberOfProfitValues = profitUpperBound + 1;
            var numberOfItemsInList = items.Count + 1;
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

            for (int p = 1; p <= _minWeightInt.GetUpperBound(1); p++)
            {
                // initialise weight to capacity + 1
                _minWeightInt[0, p] = _knapsack.Capacity + 1;
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
                    }
                }
            }
        }

        private void WriteSolutionAndData()
        {
            // Maximal Solution at any given iteration (value of i) is the highest value of p where total weight < capacity
            for (int i = _minWeightInt.GetUpperBound(0); i >= 0; i--)
            {
                for (int p = _minWeightInt.GetUpperBound(1); p >= 0; p--)
                {
                    if (_minWeightInt[i, p] <= _knapsack.Capacity)
                    {
                        Console.WriteLine("Optimal profit for first {0} items is {1} at weight {2}", i, p, _minWeightInt[i, p]);
                        break; // out of for p loop
                    }
                }
            }

            DumpArrayToLog(m => m);
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