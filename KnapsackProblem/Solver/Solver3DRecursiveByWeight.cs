using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnapsackProblem.Solver
{
    /// <summary>
    /// Implements non-recursive dynamic programming algorithm for 0-1 knapsack problem 
    /// with multiple objectives (capacity constraint and max items constraint)
    /// see: https://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    public class Solver3DRecursiveByWeight : ISolver
    {
        private const int ItemsConsideredDimension = 0;
        private const int ItemsChosenDimension = 1;
        private const int WeightDimension = 2;

        // define max values for first N items at given weight, after choosing K
        private readonly ItemGroup[,,] _maxProfitItemGroup;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Solver3DRecursiveByWeight(Knapsack knapsack, List<Item> items)
        {
            _knapsack = knapsack;
            _items = items;

            // define max values for first N items at given weight
            var weightUpperBound = _knapsack.Capacity;
            var numberOfWeights = weightUpperBound + 1;
            var numberOfItemsInList = items.Count + 1;
            var numberOfItemsAllowed = knapsack.AllowedItems + 1;
            _maxProfitItemGroup = new ItemGroup[numberOfItemsInList, numberOfItemsAllowed, numberOfWeights];
        }

        public void Solve()
        {
            CalculateRecursive();
            WriteSolutionAndData();
        }

        protected bool Exact_K_SolutionExists()
        {
            // Order Item list by weight ascending
            var sortedItems = _items.OrderBy(p => p.Weight).ToList();

            int minWeight = 0;

            for (int i = 0; i < _knapsack.AllowedItems; i++)
            {
                minWeight = minWeight + sortedItems[i].Weight;
            }

            return (minWeight <= _knapsack.Capacity);

        }

        private ItemGroup GetSolution(int numberOfItems)
        {
            ItemGroup maxProfitGroup = new ItemGroup();

            int itemsUpperBound = _maxProfitItemGroup.GetUpperBound(ItemsConsideredDimension);

            // find maximum value where all items in list considered and count = allowed items
            for (int weight = 0; weight <= _maxProfitItemGroup.GetUpperBound(WeightDimension); weight++)
            {
                var solutionAtWeightW = GetMaxProfitValue(itemsUpperBound,numberOfItems, weight);

                if (solutionAtWeightW.TotalValue() > maxProfitGroup.TotalValue())
                {
                    maxProfitGroup = solutionAtWeightW;
                }
            }

            return maxProfitGroup;
        }

        private ItemGroup GetOptimalSolution()
        {
            ItemGroup maxProfitGroup = new ItemGroup();

            int itemsUpperBound = _maxProfitItemGroup.GetUpperBound(ItemsConsideredDimension);

            // find maximum value where all items in list considered
            for (int itemsChosen = _maxProfitItemGroup.GetUpperBound(ItemsChosenDimension); itemsChosen >= 0; itemsChosen--)
            {
                var solutionWithNItems = GetSolution(itemsChosen);

                if (solutionWithNItems.TotalValue() > maxProfitGroup.TotalValue())
                {
                    maxProfitGroup = solutionWithNItems;
                }
            }

            return maxProfitGroup;
        }

        public void CalculateRecursive()
        {
            if (Exact_K_SolutionExists())
            {
                ItemGroup maxProfitGroup = GetSolution(_knapsack.AllowedItems);

                Console.WriteLine("Solution exists for exactly {0} items", _knapsack.AllowedItems);
                Console.WriteLine("  No of items:  {0}", maxProfitGroup.ItemCount());
                Console.WriteLine("  Item names:   {0}", maxProfitGroup.ItemNames());
                Console.WriteLine("  Total weight: {0}", maxProfitGroup.TotalWeight());
                Console.WriteLine("  Total value:  {0}", maxProfitGroup.TotalValue());
            }

            ItemGroup optimalGroup = GetOptimalSolution();

            Console.WriteLine("Optimal value solution:");
            Console.WriteLine("  No of items:  {0}", optimalGroup.ItemCount());
            Console.WriteLine("  Item names:   {0}", optimalGroup.ItemNames());
            Console.WriteLine("  Total weight: {0}", optimalGroup.TotalWeight());
            Console.WriteLine("  Total value:  {0}", optimalGroup.TotalValue());
        }


        public ItemGroup GetMaxProfitValue(int itemsConsidered, int itemsChosen, int currentWeight)
        {
            // In all cases, after considering 0 items or choosing 0 items, item group is empty (and profit = 0)
            if ((itemsConsidered == 0) || (itemsChosen == 0))
            {
                return new ItemGroup();
            }

            // haven't considered enough items to fulfill number chosen 
            if (itemsChosen > itemsConsidered)
            {
                // calculation is kind of undefined
                // HACK: leave at 0 and see if this works
                _maxProfitItemGroup[itemsConsidered, itemsChosen, currentWeight] = new ItemGroup();
                return _maxProfitItemGroup[itemsConsidered, itemsChosen, currentWeight];
            }

            // calculate the value prior to considering this item
            if (_maxProfitItemGroup[itemsConsidered - 1, itemsChosen, currentWeight] is null)
            {
                _maxProfitItemGroup[itemsConsidered - 1, itemsChosen, currentWeight] 
                    = GetMaxProfitValue(itemsConsidered - 1, itemsChosen, currentWeight);
            }

            // if current item weight is greater than weight being calculated, don't add the value
            if (_items[itemsConsidered - 1].Weight > currentWeight)
            {
                // maximum value is maximum before we considered this item
                _maxProfitItemGroup[itemsConsidered, itemsChosen, currentWeight]
                    = new ItemGroup(_maxProfitItemGroup[itemsConsidered - 1, itemsChosen, currentWeight]);
            }
            else
            {
                var itemGroupExcludingCurrent =
                    _maxProfitItemGroup[itemsConsidered - 1, itemsChosen - 1,
                                        currentWeight - _items[itemsConsidered - 1].Weight];

                // if we haven't calculated max without current item, do it now
                if (itemGroupExcludingCurrent is null)
                {
                    itemGroupExcludingCurrent = GetMaxProfitValue(itemsConsidered - 1,
                                                                  itemsChosen - 1,
                                                                  currentWeight - _items[itemsConsidered - 1].Weight);

                    _maxProfitItemGroup[itemsConsidered - 1, itemsChosen - 1,
                                        currentWeight - _items[itemsConsidered - 1].Weight] =
                        itemGroupExcludingCurrent;
                }

                int profitAtWeightExcludingCurrentItem = itemGroupExcludingCurrent.TotalValue();
                int profitOfCurrentItem = _items[itemsConsidered - 1].Value;

                int profitOnPreviousIteration =
                    _maxProfitItemGroup[itemsConsidered - 1, itemsChosen, currentWeight].TotalValue();

                // if (profit at weight excluding this item) + value of this item is more than previous max profit, then include this item
                if (profitAtWeightExcludingCurrentItem + profitOfCurrentItem > profitOnPreviousIteration)
                {
                    _maxProfitItemGroup[itemsConsidered, itemsChosen, currentWeight]
                        = (ItemGroup) new ItemGroup(itemGroupExcludingCurrent).Add(_items[itemsConsidered - 1]);
                }
                else
                {
                    _maxProfitItemGroup[itemsConsidered, itemsChosen, currentWeight]
                        = new ItemGroup(_maxProfitItemGroup[itemsConsidered - 1, itemsChosen, currentWeight]
                        );
                }
            }

            return _maxProfitItemGroup[itemsConsidered, itemsChosen, currentWeight];
        }


        private void WriteSolutionAndData()
        {
            DumpArrayToLog(m => m.TotalValue());
            //DumpArrayToLog(m => m.ItemCount());
        }

        private void DumpArrayToLog(Func<ItemGroup,int> getValue)
        {
            StringBuilder sb = new StringBuilder();

            // for each items chosen from 0 to number of items
            for (int i = 0; i < _maxProfitItemGroup.GetLength(ItemsConsideredDimension); i++)
            {
                LogFile.WriteLine("After considering {0} items:", i);

                // write indices at top
                sb.Append("  |");

                for (int j = 0; j < _maxProfitItemGroup.GetLength(WeightDimension); j++)
                {
                    sb.AppendFormat(" {0,4:###0}", j);
                }

                sb.Append("|");
                LogFile.WriteLine(sb.ToString());
                sb.Clear();


                // for each items chosen from 0 to number of items
                for (int j = 0; j < _maxProfitItemGroup.GetLength(ItemsChosenDimension); j++)
                {
                    sb.AppendFormat("{0,2:#0}", j);
                    sb.Append("[");

                    for (int k = 0; k < _maxProfitItemGroup.GetLength(WeightDimension); k++)
                    {
                        if (_maxProfitItemGroup[i, j, k] is null)
                        {
                            sb.Append("    x");
                        }
                        else
                        {
                            sb.AppendFormat(" {0,4:###0}", getValue(_maxProfitItemGroup[i, j, k]));
                        }
                    }

                    sb.Append("]");

                    LogFile.WriteLine(sb.ToString());
                    sb.Clear();
                }

                LogFile.WriteLine("");
            }
        }
    }
}