using System;
using System.Collections.Generic;
using System.Linq;

namespace KnapsackProblem.ProblemSet
{
    public abstract class ProblemSet : IProblemSet
    {
        public Knapsack Knapsack { get; set; }
        public List<Item> DataSet { get; set; }

        protected List<Item> SortByUnitProfitDescending(List<Item> items)
        {
            return items.OrderByDescending(p => (p.Value / p.Weight)).ToList();
        }

    }
}
