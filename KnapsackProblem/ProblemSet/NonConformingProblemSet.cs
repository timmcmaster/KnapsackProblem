using System.Collections.Generic;

namespace KnapsackProblem.ProblemSet
{
    public class NonConformingProblemSet : IProblemSet
    {
        public Knapsack Knapsack { get; set; }
        public List<Item> DataSet { get; set; }

        public NonConformingProblemSet()
        {
            Knapsack = new Knapsack(15, 3, false);
            DataSet = InitialiseItems();
        }

        public List<Item> InitialiseItems()
        {
            // List for multi-constraint example (https://ideone.com/wKzqXk)
            return new List<Item>
            {
                new Item("a", 1, 200),
                new Item("b", 1, 9),
                new Item("c", 1, 8),
                new Item("d", 1, 4),
                new Item("e", 1, 5),
                new Item("f", 1, 6),
                new Item("g", 1, 7),
                new Item("h", 1, 1),
                new Item("i", 1, 2),
                new Item("j", 1, 3)
            };
        }

    }
}