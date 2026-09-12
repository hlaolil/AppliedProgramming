public abstract class InventoryItem //"base class" (or parent class) for everything stored in
// the inventory. Holds the things that EVERY item has, no matter what kind it is.

{
    // Six properties that every item needs, whatever kind it is.
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public StockLocation Location { get; set; }
    public int ReorderLevel { get; set; }

    //Sets all six properties when an item is created.
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

    public decimal CalculateValue() //Calculates the total value of this item in stock. 
    //by multiplying the quantity by the price.
    {
        return Quantity * Price;
    }

    public bool NeedsReorder() //Checks whether the quantity is at or below the reorder level.
    {
        return Quantity <= ReorderLevel; //If the quantity is less than or equal to 
        //the reorder level, return true. Otherwise, return false.
    }

    public abstract string GetItemType(); //promises that every subclass will implement a 
    //method to return a string describing the type of item.

    public virtual string GetDisplayLine() //Builds a string that can be printed to 
    //show the item in a list.
    {
        return string.Format("[{0}] {1,-15} qty:{2,4}  {3,8:0.00}  {4}  ({5})", //The format string
        //has placeholders for the six properties, plus the calculated value.
            Id, Name, Quantity, CalculateValue(), Location.Describe(), GetItemType());
    }
}