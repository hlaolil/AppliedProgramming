public class Product : InventoryItem
{
    public string Category { get; set; }

    public Product(int id, string name, int quantity, decimal price,
                   StockLocation location, int reorderLevel, string category)
        : base(id, name, quantity, price, location, reorderLevel)
    {
        Category = category;
    }

    public override string GetItemType()
    {
        return "Standard - " + Category;
    }
}