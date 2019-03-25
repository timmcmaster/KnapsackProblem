﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackProblem
{
    public class Knapsack
    {
        public int Capacity { get; set; }
        public int AllowedItems { get; set; }
        public bool AllowMultiple { get; set; }

        public Knapsack(int capacity, int allowedItems, bool allowMultiple)
        {
            Capacity = capacity;
            AllowedItems = allowedItems;
            AllowMultiple = allowMultiple;
        }

    }
}
