using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return items.OrderByDescending(p => (p.Value / p.Weight)).ToList();
        }

        /// <summary>
        /// Uses greedy algorithm to get approximation of highest value for profit
        /// </summary>
        /// <returns></returns>
        public static double LP_Approximation(List<Item> items, Knapsack knapsack)
        {
            MaxValueGroup group = new MaxValueGroup();

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
                    criticalItemFraction = (knapsack.Capacity - group.TotalWeight()) / (double)criticalItem.Weight;
                    break;
                }
            }

            // Add fractional value of next item
            double maxProfitValue = group.TotalValue() + criticalItem.Value * criticalItemFraction;
            return maxProfitValue;
        }

        public static int GetProfitUpperBound(List<Item> items, Knapsack knapsack)
        {
            // upper bound is max int below the LP approximation 
            return (int)Math.Floor(UtilFunctions.LP_Approximation(items,knapsack));
        }

    }
}
