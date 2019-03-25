using System.Collections.Generic;

namespace KnapsackProblem.ProblemSet
{
    public interface IProblemSet
    {
        Knapsack Knapsack { get; set; }
        List<Item> DataSet { get; set; }
    }
}
