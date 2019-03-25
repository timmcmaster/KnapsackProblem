using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackProblem
{
    public class Calculator
    {
        // define max values for first N items at given weight, after choosing K
        private readonly MaxValueGroup[,,] _maxValues;
        private readonly Knapsack _knapsack;
        private readonly List<Item> _items;

        public Calculator(Knapsack knapsack, List<Item> items)
        {
            _knapsack = knapsack;
            _items = items;

            // define max values for first N items at given weight
            var numberOfWeights = knapsack.Capacity + 1;
            var numberOfItemsInList = items.Count + 1;
            var numberOfItemsAllowed = knapsack.AllowedItems + 1;
            _maxValues = new MaxValueGroup[numberOfItemsInList, numberOfWeights, numberOfItemsAllowed];

        }

        public void Calculate()
        {
            CalculateRecursive();
        }

        /*
        public void CalculateNonRecursive()
        {
            // for each weight from 0 to knapsack.Capacity
            for (int weight = 0; weight <= _knapsack.Capacity; weight++)
            {
                // maximum value for first 0 items at any given weight is 0
                _maxValues[0, weight] = new MaxValueGroup();
            }

            // for each number of items from 1 to numberOfItems
            for (int i = 1; i <= _items.Count; i++)
            {
                // for each weight from 0 to knapsack.Capacity
                for (int weight = 0; weight <= _knapsack.Capacity; weight++)
                {
                    // if current item weight is greater than weight being calculated, don't add the value
                    if (_items[i-1].Weight > weight)
                    {
                        _maxValues[i, weight] = new MaxValueGroup(_maxValues[i - 1, weight]);
                    }
                    else
                    {
                        MaxValueGroup withCurrentItem = new MaxValueGroup(_maxValues[i - 1, weight - _items[i-1].Weight]);
                        withCurrentItem.AddItem(_items[i-1]);

                        // get higher of maximum for previous N _items or (maximum at weight excluding this item + value of this item)
                        // return first item if totals are equal
                        if (_maxValues[i - 1, weight].TotalValue() >= withCurrentItem.TotalValue())
                        {
                            _maxValues[i, weight] = new MaxValueGroup(_maxValues[i - 1, weight]);
                        }
                        else
                        {
                            _maxValues[i, weight] = withCurrentItem;
                        }
                    }

                    Console.WriteLine("Maximum value for first {0} items at weight {1} is {2}", i, weight, _maxValues[i, weight].TotalValue());
                    Console.WriteLine("Items: {0}, Count: {1}, Total weight: {2}", _maxValues[i, weight].ItemNames(), _maxValues[i, weight].ItemCount(), _maxValues[i, weight].TotalWeight());
                }
            }

            // if count is not equal to max count then exclude it
            MaxValueGroup requiredValueGroup = _maxValues[_items.Count, _knapsack.Capacity];

            Console.WriteLine("Maximum value for first {0} items at weight {1} is {2}", _items.Count, _knapsack.Capacity, requiredValueGroup.TotalValue());
            Console.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            Console.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            Console.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            Console.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());
        }
        */

        public void CalculateRecursive()
        {
            MaxValueGroup requiredValueGroup = GetMaxValue(_items.Count, _knapsack.Capacity, _knapsack.AllowedItems);

            LogFile.WriteLine("Maximum value after choosing {0} of first {1} items at weight {2} is {3}",
                _knapsack.AllowedItems,
                _items.Count,
                _knapsack.Capacity,
                requiredValueGroup.TotalValue()
            );
            LogFile.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            LogFile.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            LogFile.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            LogFile.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());

            Console.WriteLine("Maximum value after choosing {0} of first {1} items at weight {2} is {3}",
                _knapsack.AllowedItems,
                _items.Count,
                _knapsack.Capacity,
                requiredValueGroup.TotalValue()
            );
            Console.WriteLine("No of items: {0}", requiredValueGroup.ItemCount());
            Console.WriteLine("Item names: {0}", requiredValueGroup.ItemNames());
            Console.WriteLine("Total weight: {0}", requiredValueGroup.TotalWeight());
            Console.WriteLine("Total value: {0}", requiredValueGroup.TotalValue());

        }

        public MaxValueGroup GetMaxValue(int forFirstN, int atWeight, int itemsLeft)
        {
            // value at 0 items or 0 weight, or 0 items chosen is 0
            if (forFirstN == 0 || atWeight <= 0 || itemsLeft == 0)
            {
                return new MaxValueGroup();
            }

            // calculate the previous value
            if (_maxValues[forFirstN - 1, atWeight, itemsLeft] is null)
            {
                _maxValues[forFirstN - 1, atWeight, itemsLeft] = GetMaxValue(forFirstN - 1, atWeight, itemsLeft);
            }

            // if current item weight is greater than weight being calculated, don't add the value
            if (_items[forFirstN - 1].Weight > atWeight)
            {
                _maxValues[forFirstN, atWeight, itemsLeft] =
                    new MaxValueGroup(_maxValues[forFirstN - 1, atWeight, itemsLeft]);
            }
            else
            {
                MaxValueGroup withoutCurrentItem =
                    _maxValues[forFirstN - 1, atWeight - _items[forFirstN - 1].Weight, itemsLeft - 1];
                // if we haven't calculated max without current item, do it now
                if (withoutCurrentItem is null)
                {
                    withoutCurrentItem = GetMaxValue(forFirstN - 1, atWeight - _items[forFirstN - 1].Weight,
                        itemsLeft - 1);
                    _maxValues[forFirstN - 1, atWeight - _items[forFirstN - 1].Weight, itemsLeft - 1] =
                        withoutCurrentItem;
                }

                // add item to prev
                MaxValueGroup withCurrentItem = new MaxValueGroup(withoutCurrentItem);
                withCurrentItem.AddItem(_items[forFirstN - 1]);

                if (_maxValues[forFirstN - 1, atWeight, itemsLeft].TotalValue() >= withCurrentItem.TotalValue())
                {
                    _maxValues[forFirstN, atWeight, itemsLeft] =
                        new MaxValueGroup(_maxValues[forFirstN - 1, atWeight, itemsLeft]);
                }
                else
                {
                    _maxValues[forFirstN, atWeight, itemsLeft] = withCurrentItem;
                }
            }

            //LogFile.WriteLine("Maximum value after choosing {3} of first {0} items at weight {1} is {2}",
            //    forFirstN,
            //    atWeight,
            //    _maxValues[forFirstN, atWeight, itemsLeft].TotalValue(),
            //    itemsLeft);
            //LogFile.WriteLine("Items: {0}, Count: {1}, Total weight: {2}",
            //    _maxValues[forFirstN, atWeight, itemsLeft].ItemNames(),
            //    _maxValues[forFirstN, atWeight, itemsLeft].ItemCount(),
            //    _maxValues[forFirstN, atWeight, itemsLeft].TotalWeight());

            return _maxValues[forFirstN, atWeight, itemsLeft];
        }

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

        public string GroupAsText(MaxValueGroup group)
        {
            if (group is null)
            {
                return "null";
            }

            return string.Format("Items: {0}, Count: {1}, Total weight: {2}",
                    group.ItemNames(),
                    group.ItemCount(),
                    group.TotalWeight());
        }
    }
}