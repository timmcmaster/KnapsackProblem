namespace KnapsackProblem
{
    public class Item
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }

        public Item(string name, int weight, int value)
        {
            Name = name;
            Weight = weight;
            Value = value;
        }

        public double ProfitPerUnitWeight()
        {
            return this.Value / (double) this.Weight;
        }
    }
}
