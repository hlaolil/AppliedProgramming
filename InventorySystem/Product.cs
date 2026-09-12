public class Product : InventoryItem //Product INHERITS from InventoryItem.
{
    public string Category { get; set; }
    // takes seven values. Six of them belong to the
    // parent, and one (category) belongs to this class.
    public Product(int id, string name, int quantity, decimal price,
                   StockLocation location, int reorderLevel, string category)
        : base(id, name, quantity, price, location, reorderLevel)
    {
        Category = category;
    }

    public override string GetItemType() //Overrides the abstract method in the parent class, 
    //so that this class can be instantiated.
    {
        return "Standard - " + Category; //Returns a string that describes the type of item, 
        //including the category.
    }
}