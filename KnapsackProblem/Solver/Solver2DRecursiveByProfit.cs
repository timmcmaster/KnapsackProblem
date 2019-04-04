using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Solver
{
    /// <summary>
    /// Implements recursive dynamic programming algorithm for 0-1 knapsack problem with single objective
    /// see: https://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    public class Solver2DRecursiveByProfit : ISolver
    {
        // define max values for first N items at given weight, after choosing K
        private readonly MinWeightItemGroup[,] _minWeightItemGroup;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Solver2DRecursiveByProfit(Knapsack knapsack, List<Item> items)
        {
            _knapsack = knapsack;
            _items = items;

            // define max values for first N items at given weight
            var profitUpperBound = UtilFunctions.GetProfitUpperBound_KP(_items, _knapsack);
            var numberOfProfitValues = profitUpperBound + 1;
            var numberOfItemsInList = items.Count + 1;
            _minWeightItemGroup = new MinWeightItemGroup[numberOfItemsInList, numberOfProfitValues];
        }

        public void Solve()
        {
            CalculateRecursive();
        }

        public void CalculateRecursive()
        {
            // set current profit to highest value
            var currentProfit = _minWeightItemGroup.GetUpperBound(1);

            // calculate at this profit value
            MinWeightItemGroup optimalValueGroup = GetMinWeightValue(_items.Count, currentProfit);

            // if no solution at this profit value, find next lowest
            while (optimalValueGroup.TotalWeight() > _knapsack.Capacity)
            {
                Console.WriteLine("No solution at profit {0}, calculating at {1}", currentProfit, --currentProfit);

                optimalValueGroup = GetMinWeightValue(_items.Count, currentProfit);
            }

            WriteSolutionAndData(optimalValueGroup);
        }

        public MinWeightItemGroup GetMinWeightValue(int forFirstN, int atProfit)
        {
            // value at 0 items and 0 profit is empty collection, initial weight 0,
            // else at 0 items and profit above zero, initialise to > max weight
            if (forFirstN == 0)
            {
                return atProfit <= 0 ? new MinWeightItemGroup(0) : new MinWeightItemGroup(_knapsack.Capacity + 1);
            }

            // calculate the previous value
            if (_minWeightItemGroup[forFirstN - 1, atProfit] is null)
            {
                _minWeightItemGroup[forFirstN - 1, atProfit] = GetMinWeightValue(forFirstN - 1, atProfit);
            }

            // if current item profit is greater than profit being calculated, use previous value
            if (_items[forFirstN - 1].Value > atProfit)
            {
                _minWeightItemGroup[forFirstN, atProfit] = new MinWeightItemGroup(_minWeightItemGroup[forFirstN - 1, atProfit]);
            }
            else
            {
                var itemGroupExcludingCurrent = _minWeightItemGroup[forFirstN - 1, atProfit - _items[forFirstN - 1].Value];

                // if we haven't calculated max without current item, do it now
                if (itemGroupExcludingCurrent is null)
                {
                    itemGroupExcludingCurrent = GetMinWeightValue(forFirstN - 1, atProfit - _items[forFirstN - 1].Value);
                    _minWeightItemGroup[forFirstN - 1, atProfit - _items[forFirstN - 1].Value] = itemGroupExcludingCurrent;
                }

                int weightAtProfitExcludingCurrentItem = itemGroupExcludingCurrent.TotalWeight();
                int weightOfCurrentItem = _items[forFirstN - 1].Weight;
                int weightOnPreviousIteration = _minWeightItemGroup[forFirstN - 1, atProfit].TotalWeight();

                if (weightAtProfitExcludingCurrentItem + weightOfCurrentItem < weightOnPreviousIteration)
                {
                    _minWeightItemGroup[forFirstN, atProfit] = (MinWeightItemGroup)new MinWeightItemGroup(itemGroupExcludingCurrent).Add(_items[forFirstN - 1]);
                }
                else
                {
                    _minWeightItemGroup[forFirstN, atProfit] = new MinWeightItemGroup(_minWeightItemGroup[forFirstN - 1, atProfit]);
                }
            }

            return _minWeightItemGroup[forFirstN, atProfit];
        }

        private void WriteSolutionAndData(MinWeightItemGroup requiredValueGroup)
        {
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

            DumpArrayToLog(m => m.TotalWeight());
            //DumpArrayToLog(m => m.ItemCount());
        }

        private void DumpArrayToLog(Func<MinWeightItemGroup, int> getValue)
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
                    if (_minWeightItemGroup[i, j] is null)
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