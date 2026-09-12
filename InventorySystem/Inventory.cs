public class Inventory // Represents a collection of inventory items, 
// //and provides methods to manipulate them.
{
    private List<InventoryItem> items;// The list of items in the inventory.

    public Inventory() //Creates a new, empty inventory.
    {
        items = new List<InventoryItem>();
    }

    public int Count //Returns the number of items in the inventory.
    {
        get { return items.Count; }
    }

    public void AddItem(InventoryItem item) //Adds an item to the inventory. The item can be any subclass of InventoryItem.
    {
        items.Add(item);
    }

    public InventoryItem? FindById(int id) //Searches for an item by its ID. Returns the item if found, or null if not found.
    {
        foreach (InventoryItem item in items)
        {
            if (item.Id == id)
            {
                return item;
            }
        }

        return null;
    }

    public bool RemoveItem(int id) //Removes an item from the inventory by its ID. 
    //Returns true if the item was found and removed, or false if not found.
    {
        InventoryItem? found = FindById(id);

        if (found == null)
        {
            return false;
        }

        items.Remove(found);
        return true;
    }

    public bool UpdateQuantity(int id, int newQuantity) //Updates the quantity of an item by its ID.
    {
        InventoryItem? found = FindById(id);

        if (found == null)
        {
            return false;
        }

        found.Quantity = newQuantity;
        return true;
    }

    public List<InventoryItem> Search(string? term) //Searches for items whose names contain the 
    //given term (case-insensitive).
    {
        List<InventoryItem> results = new List<InventoryItem>();

        if (term == null)
        {
            return results;
        }

        string needle = term.Trim().ToLower();

        foreach (InventoryItem item in items)
        {
            if (item.Name.ToLower().Contains(needle))
            {
                results.Add(item);
            }
        }

        return results;
    }

    public decimal GetTotalValue() //Calculates the total value of all items in the inventory,
    {
        decimal total = 0;

        foreach (InventoryItem item in items)
        {
            total += item.CalculateValue();
        }

        return total;
    }

    public void DisplayAll() //Prints a list of all items in the inventory, using each item's own display format.
    {
        if (items.Count == 0)
        {
            Console.WriteLine("The inventory is empty.");
            return;
        }

        foreach (InventoryItem item in items)
        {
            Console.WriteLine(item.GetDisplayLine());
        }
    }

    public void DisplayLowStock() //Prints a list of all items that are at or below their 
    //reorder level, using each item's own display format.
    {
        bool anyFound = false;

        foreach (InventoryItem item in items)
        {
            if (item.NeedsReorder())
            {
                Console.WriteLine(item.GetDisplayLine() + "   [LOW STOCK]");
                anyFound = true;
            }
        }

        if (!anyFound)
        {
            Console.WriteLine("No items are below their reorder level.");
        }
    }

    public void LoadFrom(string path) //Loads the inventory from a file, replacing any existing 
    //items.
    {
        items = InventoryFile.Load(path);
    }

    public void SaveTo(string path) //Saves the inventory to a file, overwriting any existing file.
    {
        InventoryFile.Save(items, path);
    }

    public void DisplayExpiring(int days) //Prints a list of all perishable items that are expired or expiring within the given number of days.
    {
        bool anyFound = false;

        foreach (InventoryItem item in items)
        {
            if (item is PerishableProduct perishable)
            {
                if (perishable.IsExpired() || perishable.IsExpiringSoon(days))
                {
                    Console.WriteLine(perishable.GetDisplayLine());
                    anyFound = true;
                }
            }
        }

        if (!anyFound)
        {
            Console.WriteLine("Nothing is expired or expiring within " + days + " days.");
        }
    }
}