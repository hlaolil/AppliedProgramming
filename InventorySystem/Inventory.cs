public class Inventory
{
    private List<InventoryItem> items;

    public Inventory()
    {
        items = new List<InventoryItem>();
    }

    public int Count
    {
        get { return items.Count; }
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
    }

    public InventoryItem? FindById(int id)
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

    public bool RemoveItem(int id)
    {
        InventoryItem? found = FindById(id);

        if (found == null)
        {
            return false;
        }

        items.Remove(found);
        return true;
    }

    public bool UpdateQuantity(int id, int newQuantity)
    {
        InventoryItem? found = FindById(id);

        if (found == null)
        {
            return false;
        }

        found.Quantity = newQuantity;
        return true;
    }

    public List<InventoryItem> Search(string? term)
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

    public decimal GetTotalValue()
    {
        decimal total = 0;

        foreach (InventoryItem item in items)
        {
            total += item.CalculateValue();
        }

        return total;
    }

    public void DisplayAll()
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

    public void DisplayLowStock()
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

    public void LoadFrom(string path)
    {
        items = InventoryFile.Load(path);
    }

    public void SaveTo(string path)
    {
        InventoryFile.Save(items, path);
    }

    public void DisplayExpiring(int days)
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