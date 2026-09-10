using System;

public abstract class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public StockLocation Location { get; set; }
    public int ReorderLevel { get; set; }

    protected InventoryItem(int id, string name, int quantity,
                            decimal price, StockLocation location, int reorderLevel)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
        ReorderLevel = reorderLevel;
        Location = location;
    }

    public decimal CalculateValue()
    {
        return Quantity * Price;
    }

    public abstract string GetItemType();

    public virtual string GetDisplayLine()
    {
        return string.Format("[{0}] {1,-15} qty:{2,4}  {3,8:0.00}  {4}  ({5})",
            Id, Name, Quantity, CalculateValue(), Location.Describe(), GetItemType());
    }
}