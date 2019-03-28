using System;
using System.Collections.Generic;

namespace KnapsackProblem.Solver
{
    public class NonRecursiveSolver3D : ISolver
    {
        // define max values for first N items at given weight, after choosing K
        private readonly ItemGroup[,,] _maxValues;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public NonRecursiveSolver3D(Knapsack knapsack, List<Item> items)
        {
            _knapsack = knapsack;
            _items = items;

            // define max values for first N items at given weight
            var numberOfWeights = knapsack.Capacity + 1;
            var numberOfItemsInList = items.Count + 1;
            var numberOfItemsAllowed = knapsack.AllowedItems + 1;
            _maxValues = new ItemGroup[numberOfItemsInList, numberOfWeights, numberOfItemsAllowed];

        }

        public void Solve()
        {
            CalculateNonRecursive();
        }

        public void CalculateNonRecursive()
        {
            //// for each weight from 0 to knapsack.Capacity
            //for (int weight = 0; weight <= _knapsack.Capacity; weight++)
            //{
            //    // maximum value for first 0 items at any given weight is 0
            //    _maxValues[0, weight] = new ItemGroup();
            //}

            //// for each number of items from 1 to numberOfItems
            //for (int i = 1; i <= _items.Count; i++)
            //{
            //    // for each weight from 0 to knapsack.Capacity
            //    for (int weight = 0; weight <= _knapsack.Capacity; weight++)
            //    {
            //        // if current item weight is greater than weight being calculated, don't add the value
            //        if (_items[i-1].Weight > weight)
            //        {
            //            _maxValues[i, weight] = new ItemGroup(_maxValues[i - 1, weight]);
            //        }
            //        else
            //        {
            //            ItemGroup withCurrentItem = new ItemGroup(_maxValues[i - 1, weight - _items[i-1].Weight]);
            //            withCurrentItem.AddItem(_items[i-1]);

            //            // get higher of maximum for previous N _items or (maximum at weight excluding this item + value of this item)
            //            // return first item if totals are equal
            //            if (_maxValues[i - 1, weight].TotalValue() >= withCurrentItem.TotalValue())
            //            {
            //                _maxValues[i, weight] = new ItemGroup(_maxValues[i - 1, weight]);
            //            }
            //            else
            //            {
            //                _maxValues[i, weight] = withCurrentItem;
            //            }
            //        }

            //        Console.WriteLine("Maximum value for first {0} items at weight {1} is {2}", i, weight, _maxValues[i, weight].TotalValue());
            //        Console.WriteLine("Items: {0}, Count: {1}, Total weight: {2}", _maxValues[i, weight].ItemNames(), _maxValues[i, weight].ItemCount(), _maxValues[i, weight].TotalWeight());
            //    }
            //}

            //// if count is not equal to max count then exclude it
            //ItemGroup requiredValueGroup = _maxValues[_items.Count, _knapsack.Capacity];

            //Console.WriteLine("Maximum value for first {0} items at weight {1} is {2}", _items.Count, _knapsack.Capacity, requiredValueGroup.TotalValue());
            //Console.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            //Console.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            //Console.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            //Console.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());
        }

/*

        public void LogDataValues()
        {
            // put out all lists of 9 riders
            // where entire list has been searched 
            // not worried about total weight

            int itemsLeft = 9;
            int itemsChecked = _items.Count;

            // for each weight from 0 to knapsack.Capacity
            for (int weight = 0; weight <= _knapsack.Capacity; weight++)
            {
                var fullGroup = _maxValues[itemsChecked, weight, itemsLeft];
                LogFile.WriteLine("_maxValues[{0},{1},{2}]: {3}", itemsChecked, weight, itemsLeft, GroupAsText(fullGroup));

                // log previous iteration in number of riders
                var prevGroup = _maxValues[itemsChecked, weight, itemsLeft-1];
                LogFile.WriteLine("_maxValues[{0},{1},{2}]: {3}", itemsChecked, weight, itemsLeft-1, GroupAsText(prevGroup));
            }
        }

        public string GroupAsText(ItemGroup group)
        {
            if (@group is null)
            {
                return "null";
            }

            return string.Format("Items: {0}, Count: {1}, Total weight: {2}",
                @group.ItemNames(),
                @group.ItemCount(),
                @group.TotalWeight());
        }
 */

    }
}