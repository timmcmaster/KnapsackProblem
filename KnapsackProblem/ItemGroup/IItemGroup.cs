namespace KnapsackProblem
{
    public interface IItemGroup
    {
        IItemGroup Add(Item item);
        void AddItem(Item item);
        int TotalWeight();
        int TotalValue();
        int ItemCount();
        string ItemNames();
    }
}