using System.Collections.Generic;

namespace KnapsackProblem.ProblemSet
{
    public class KellererBookProblemSet : ProblemSet
    {
        public KellererBookProblemSet()
        {
            Knapsack = new Knapsack(9, 3, false);
            var items = InitialiseItems();
            DataSet = SortByUnitProfitDescending(items);
        }

        public List<Item> InitialiseItems()
        {
            // List for this wikipedia example (https://en.wikipedia.org/wiki/Knapsack_problem)
            return new List<Item>
            {
                new Item("a", 2, 6),
                new Item("b", 3, 5),
                new Item("c", 6, 8),
                new Item("d", 7, 9),
                new Item("e", 5, 6),
                new Item("f", 9, 7),
                new Item("g", 4, 3),
            };
        }

    }
}