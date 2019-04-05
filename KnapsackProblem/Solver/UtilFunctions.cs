using System;
using System.Collections.Generic;
using System.Linq;

namespace KnapsackProblem.Solver
{
    public static class UtilFunctions
    {
        /// <summary>
        /// Sorts the given list by unit profit descending.
        /// Required for greedy algorithm and LPApprox algorithm
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static List<Item> SortByUnitProfitDescending(List<Item> items)
        {
            return items.OrderByDescending(p => p.ProfitPerUnitWeight()).ToList();
        }

        /// <summary>
        /// Conventional 2-approximation algorithm for 0-1KP
        /// Result, r holds to outcome that optimal solution, z* is less than or equal to 2r
        /// i.e. 2r provides upper bound for profit
        /// </summary>
        /// <returns></returns>
        public static int Conventional2Approx_KP(List<Item> items, Knapsack knapsack)
        {
            ItemGroup group = new ItemGroup();

            List<Item> sortedItems = UtilFunctions.SortByUnitProfitDescending(items);

            // Fill until we reach critical item
            foreach (Item item in sortedItems)
            {
                if (group.TotalWeight() + item.Weight <= knapsack.Capacity)
                {
                    group.AddItem(item);
                }
                else // we have reached critical item
                {
                    var criticalItem = item;

                    // 2-approximation solution is one with higher profit value of group or criticalItem
                    if (criticalItem.Value > group.TotalValue())
                    {
                        group = (ItemGroup) new ItemGroup().Add(criticalItem);
                    }

                    break;
                }
            }

            return group.TotalValue();
        }

        /// <summary>
        /// Uses greedy algorithm to get approximation of highest value for profit
        /// </summary>
        /// <returns></returns>
        public static double LPRelaxedApprox_KP(List<Item> items, Knapsack knapsack)
        {
            ItemGroup group = new ItemGroup();

            List<Item> sortedItems = UtilFunctions.SortByUnitProfitDescending(items);

            Item criticalItem = new Item("", 0, 0);
            double criticalItemFraction = 0;

            // Fill until we reach critical item
            foreach (Item item in sortedItems)
            {
                if (group.TotalWeight() + item.Weight <= knapsack.Capacity)
                {
                    group.AddItem(item);
                }
                else // we have reached critical item
                {
                    criticalItem = item;
                    criticalItemFraction = (knapsack.Capacity - group.TotalWeight()) / (double) criticalItem.Weight;
                    break;
                }
            }

            // Add fractional value of next item
            double maxProfitValue = group.TotalValue() + criticalItem.Value * criticalItemFraction;
            return maxProfitValue;
        }

        public static int GetProfitUpperBound_KP(List<Item> items, Knapsack knapsack)
        {
            // conventional 2-approximation solution 
            int conv2Approx = UtilFunctions.Conventional2Approx_KP(items, knapsack);
            int conv2UpperBound = conv2Approx * 2;

            Console.WriteLine("Conventional 2-approx value: {0}, UB: {1}", conv2Approx, conv2UpperBound);

            // LP approximation 
            double lpApprox = UtilFunctions.LPRelaxedApprox_KP(items, knapsack);

            // upper bound is max int below the LP approximation 
            int lpApproxUpperBound = (int) Math.Floor(lpApprox);

            Console.WriteLine("LP approx value: {0}, UB: {1}", lpApprox, lpApproxUpperBound);

            return lpApproxUpperBound;
        }

        public static int Conventional2Approx_kKP(List<Item> items, Knapsack knapsack)
        {
            return 0;
        }

        public static double LPRelaxedApprox_kKP(List<Item> items, Knapsack knapsack)
        {
            ItemGroup group = new ItemGroup();

            List<Item> sortedItems = UtilFunctions.SortByUnitProfitDescending(items);

            Item criticalItem = new Item("", 0, 0);
            double criticalItemFraction = 0;

            // Fill until we reach critical item
            foreach (Item item in sortedItems)
            { 
                // if count will be within max count
                if (group.ItemCount() < knapsack.AllowedItems)
                {

                }

                // if weight will be within capacity
                if (group.TotalWeight() + item.Weight <= knapsack.Capacity)
                {
                    group.AddItem(item);
                }
                else // we have reached critical item
                {
                    criticalItem = item;
                    criticalItemFraction = (knapsack.Capacity - group.TotalWeight()) / (double)criticalItem.Weight;
                    break;
                }
            }

            // Add fractional value of next item
            double maxProfitValue = group.TotalValue() + criticalItem.Value * criticalItemFraction;
            return maxProfitValue;
        }
    }
}