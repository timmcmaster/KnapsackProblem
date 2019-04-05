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
    public class Solver3DNonRecursiveByWeight : ISolver
    {
        private const int ItemsConsideredDimension = 0;
        private const int ItemsChosenDimension = 1;
        private const int WeightDimension = 2;

        // define max values for first N items at given weight, after choosing K
        private readonly ItemGroup[,,] _maxProfitItemGroup;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Solver3DNonRecursiveByWeight(Knapsack knapsack, List<Item> items)
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
            CalculateNonRecursive();
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
        public void CalculateNonRecursive()
        {
            // In all cases, after considering 0 items, item group is empty (and profit = 0)

            // for each number of items chosen from 0 to knapsack.AllowedItems
            for (int itemsChosen = 0; itemsChosen <= _maxProfitItemGroup.GetUpperBound(ItemsChosenDimension); itemsChosen++)
            {
                // for each weight from 0 to knapsack.Capacity
                for (int weight = 0; weight <= _maxProfitItemGroup.GetUpperBound(WeightDimension); weight++)
                {
                    // maximum value for first 0 items at any given weight and no. chosen is 0
                    _maxProfitItemGroup[0, itemsChosen, weight] = new ItemGroup();
                }
            }

            // for each number of items considered, from 1 to numberOfItems
            for (int i = 1; i <= _maxProfitItemGroup.GetUpperBound(ItemsConsideredDimension); i++)
            {
                Console.WriteLine("** Considering first {0} items in list:", i);
                
                // for each number of items chosen from 0 to knapsack.AllowedItems
                for (int itemsChosen = 0; itemsChosen <= _maxProfitItemGroup.GetUpperBound(ItemsChosenDimension); itemsChosen++)
                {
                    // for each weight from 0 to knapsack.Capacity
                    for (int weight = 0; weight <= _maxProfitItemGroup.GetUpperBound(WeightDimension); weight++)
                    {
                        // Calculate m[i,j,k] which is the maximum value that can be obtained
                        // at weight k after considering first i elements in list and choosing j items

                        // If no items chosen
                        if (itemsChosen == 0)
                        {
                            // profit is 0 and set is empty (regardless of number considered, or weight)
                            _maxProfitItemGroup[i, itemsChosen, weight] = new ItemGroup();
                        }
                        else if (itemsChosen > i) // haven't considered enough items to fulfill number chosen 
                        {
                            // calculation is kind of undefined
                            // HACK: leave at 0 and see if this works
                            _maxProfitItemGroup[i, itemsChosen, weight] = new ItemGroup();
                        }
                        else
                        {
                            // if current item weight is greater than weight being calculated, don't add the value
                            if (_items[i - 1].Weight > weight)
                            {
                                // maximum value is maximum before we considered this item
                                _maxProfitItemGroup[i, itemsChosen, weight] = new ItemGroup(_maxProfitItemGroup[i - 1, itemsChosen, weight]);
                            }
                            else
                            {
                                var itemGroupExcludingCurrent = _maxProfitItemGroup[i - 1, itemsChosen - 1, weight - _items[i - 1].Weight];

                                int profitAtWeightExcludingCurrentItem = itemGroupExcludingCurrent.TotalValue();
                                int profitOfCurrentItem = _items[i - 1].Value;
                                int profitOnPreviousIteration = _maxProfitItemGroup[i - 1, itemsChosen, weight].TotalValue();

                                // if (profit at weight excluding this item) + value of this item is more than previous max profit, then include this item
                                if (profitAtWeightExcludingCurrentItem + profitOfCurrentItem > profitOnPreviousIteration)
                                {
                                    _maxProfitItemGroup[i, itemsChosen, weight] = (ItemGroup)new ItemGroup(itemGroupExcludingCurrent).Add(_items[i - 1]);
                                }
                                else
                                {
                                    _maxProfitItemGroup[i, itemsChosen, weight] = new ItemGroup(_maxProfitItemGroup[i - 1, itemsChosen, weight]);
                                }
                            }
                        }

                        Console.WriteLine("Maximum value for choosing {0} item/s at weight {1} is {2}", itemsChosen, weight,
                                          _maxProfitItemGroup[i, itemsChosen, weight].TotalValue());
                        Console.WriteLine("Items: {0}, Count: {1}, Total weight: {2}", 
                                          _maxProfitItemGroup[i, itemsChosen, weight].ItemNames(),
                                          _maxProfitItemGroup[i, itemsChosen, weight].ItemCount(), 
                                          _maxProfitItemGroup[i, itemsChosen, weight].TotalWeight());
                    }

                }
            }
        }

        private ItemGroup GetSolution(int numberOfItems)
        {
            ItemGroup maxProfitGroup = new ItemGroup();

            int itemsUpperBound = _maxProfitItemGroup.GetUpperBound(ItemsConsideredDimension);

            // find maximum value where all items in list considered and count = allowed items
            for (int weight = 0; weight <= _maxProfitItemGroup.GetUpperBound(WeightDimension); weight++)
            {
                if (_maxProfitItemGroup[itemsUpperBound, numberOfItems, weight].TotalValue() >
                    maxProfitGroup.TotalValue())
                {
                    maxProfitGroup = _maxProfitItemGroup[itemsUpperBound, numberOfItems, weight];
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

        private void WriteSolutionAndData()
        {
            bool exactSolutionExists = Exact_K_SolutionExists();

            if (exactSolutionExists)
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

            //DumpArrayToLog(m => m.TotalValue());
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
                        sb.AppendFormat(" {0,4:###0}", getValue(_maxProfitItemGroup[i, j, k]));
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