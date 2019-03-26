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
        //private readonly MaxValueGroup[,] _maxValues;
        private readonly int[,] _maxValues;
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
            _maxValues = new int[numberOfItemsInList, numberOfProfitValues];

        }

        public void Solve()
        {
            CalculateNonRecursive();
            WriteSolutionAndData();
        }

        public void CalculateNonRecursive()
        {
            // Initialise values after 0 items
            _maxValues[0, 0] = 0;
            for (int p = 1; p <= _maxValues.GetUpperBound(1); p++)
            {
                // initialise weight to capacity + 1
                _maxValues[0, p] = _knapsack.Capacity + 1;
            }

            // for each number of items from 1 to numberOfItems
            for (int i = 1; i <= _maxValues.GetUpperBound(0); i++)
            {
                // for each profit value from max down to profit of current item
                for (int p = _maxValues.GetUpperBound(1); p >= _items[i-1].Value; p--)
                {
                    // Calculate m[i,j] which is the minimum weight that can be obtained at profit j after looking at first i elements in list

                    // if (weight at profit excluding this item) + weight of this item is less than previous min weight, then include this item
                    int weightAtProfitExcludingCurrentItem = _maxValues[i - 1, p - _items[i - 1].Value];
                    int weightOfCurrentItem = _items[i - 1].Weight;
                    int weightOnPreviousIteration = _maxValues[i - 1, p];

                    if (weightAtProfitExcludingCurrentItem + weightOfCurrentItem < weightOnPreviousIteration)
                    {
                        // maximum value is maximum before we added this item to the list
                        _maxValues[i, p] = weightAtProfitExcludingCurrentItem + weightOfCurrentItem;
                    }
                    else
                    {
                        _maxValues[i, p] = weightOnPreviousIteration;
                    }

                    //Console.WriteLine("Minimum weight for first {0} items at profit {1} is {2}", i, p,
                    //    _maxValues[i, p]);
                    //Console.WriteLine("Items: {0}, Count: {1}, Total weight: {2}", _maxValues[i, weight].ItemNames(),
                    //    _maxValues[i, weight].ItemCount(), _maxValues[i, weight].TotalWeight());
                }
            }
        }

        private void WriteSolutionAndData()
        {
            //MaxValueGroup requiredValueGroup = _maxValues[_items.Count, _knapsack.Capacity];
            //int minValue = _maxValues[_items.Count, _knapsack.Capacity];

            //Console.WriteLine("Maximum value for first {0} items at weight {1} is {2}", _items.Count, _knapsack.Capacity, requiredValueGroup.TotalValue());
            //Console.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            //Console.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            //Console.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            //Console.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());

            DumpArrayToLog(m => m);
            //DumpArrayToLog(m => m.ItemCount());
        }

        private void DumpArrayToLog(Func<int,int> getValue)
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
                    sb.AppendFormat(" {0,4:###0}", getValue(_maxValues[i, j]));
                }
                sb.Append("]");
                
                LogFile.WriteLine(sb.ToString());
            }
        }
    }
}