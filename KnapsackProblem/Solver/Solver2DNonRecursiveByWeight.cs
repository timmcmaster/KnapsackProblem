using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace KnapsackProblem.Solver
{
    /// <summary>
    /// Implements non-recursive dynamic programming algorithm for 0-1 knapsack problem with single objective
    /// see: https://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    public class Solver2DNonRecursiveByWeight : ISolver
    {
        // define max values for first N items at given weight, after choosing K
        private readonly MaxValueGroup[,] _maxValues;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Solver2DNonRecursiveByWeight(Knapsack knapsack, List<Item> items)
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
            CalculateNonRecursive();
            WriteSolutionAndData();
        }

        public void CalculateNonRecursive()
        {
            // for each weight from 0 to knapsack.Capacity
            for (int weight = 0; weight <= _knapsack.Capacity; weight++)
            {
                // maximum value for first 0 items at any given weight is 0
                _maxValues[0, weight] = new MaxValueGroup();
            }

            // for each number of items from 1 to numberOfItems
            for (int i = 1; i <= _items.Count; i++)
            {
                // for each weight from 0 to knapsack.Capacity
                for (int weight = 0; weight <= _knapsack.Capacity; weight++)
                {
                    // Calculate m[i,j] which is the maximum value that can be obtained at weight j after looking at first i elements in list

                    // if current item weight is greater than weight being calculated, don't add the value
                    if (_items[i - 1].Weight > weight)
                    {
                        // maximum value is maximum before we added this item to the list
                        _maxValues[i, weight] = new MaxValueGroup(_maxValues[i - 1, weight]);
                    }
                    else
                    {
                        // get the value that would result in weight we are at, then add new item 
                        MaxValueGroup withCurrentItem =
                            new MaxValueGroup(_maxValues[i - 1, weight - _items[i - 1].Weight]).Add(_items[i - 1]);

                        // if including this item results in a higher value, use that value
                        if (withCurrentItem.TotalValue() > _maxValues[i - 1, weight].TotalValue())
                        {
                            _maxValues[i, weight] = withCurrentItem;
                        }
                        else
                        {
                            _maxValues[i, weight] = new MaxValueGroup(_maxValues[i - 1, weight]);
                        }
                    }

                    Console.WriteLine("Maximum value for first {0} items at weight {1} is {2}", i, weight,
                        _maxValues[i, weight].TotalValue());
                    Console.WriteLine("Items: {0}, Count: {1}, Total weight: {2}", _maxValues[i, weight].ItemNames(),
                        _maxValues[i, weight].ItemCount(), _maxValues[i, weight].TotalWeight());
                }
            }
        }

        private void WriteSolutionAndData()
        {
            MaxValueGroup requiredValueGroup = _maxValues[_items.Count, _knapsack.Capacity];

            Console.WriteLine("Maximum value for first {0} items at weight {1} is {2}", _items.Count, _knapsack.Capacity, requiredValueGroup.TotalValue());
            Console.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            Console.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            Console.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            Console.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());

            DumpArrayToLog(m => m.TotalValue());
            DumpArrayToLog(m => m.ItemCount());
        }

        private void DumpArrayToLog(Func<MaxValueGroup,int> getValue)
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