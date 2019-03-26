using System.Collections.Generic;

namespace KnapsackProblem.ProblemSet
{
    public class MultiConstraintProblemSet : ProblemSet
    {
        public MultiConstraintProblemSet()
        {
            Knapsack = new Knapsack(15, 3, false);
            var items = InitialiseItems();
            DataSet = SortByUnitProfitDescending(items);
        }

        public List<Item> InitialiseItems()
        {
            // List for multi-constraint example (https://ideone.com/wKzqXk)
            return new List<Item>
            {
                new Item("a", 4, 6),
                new Item("b", 3, 4),
                new Item("c", 5, 5),
                new Item("d", 7, 3),
                new Item("e", 7, 7),
            };
        }

    }
}
