using System.Collections.Generic;

namespace KnapsackProblem.ProblemSet
{
    public class WikipediaProblemSet : ProblemSet
    {
        public WikipediaProblemSet()
        {
            Knapsack = new Knapsack(67, 99, false);
            var items = InitialiseItems();
            DataSet = SortByUnitProfitDescending(items);
        }

        public List<Item> InitialiseItems()
        {
            // List for this wikipedia example (https://en.wikipedia.org/wiki/Knapsack_problem)
            return new List<Item>
            {
                new Item("a", 23, 505),
                new Item("b", 26, 352),
                new Item("c", 20, 458),
                new Item("d", 18, 220),
                new Item("e", 32, 354),
                new Item("f", 27, 414),
                new Item("g", 29, 498),
                new Item("h", 26, 545),
                new Item("i", 30, 473),
                new Item("j", 27, 543)
            };
        }

    }
}
