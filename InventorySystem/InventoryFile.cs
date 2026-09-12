using System.Globalization;
// This class handles saving and loading the inventory to and from a text file.
public static class InventoryFile //A static class is a class that cannot be instantiated. 
//It is used to group related methods together.
{
    private const char Separator = '|';
    private const int FieldCount = 9;

    public static void Save(List<InventoryItem> items, string path) //Saves the given list of 
    //items to the given file path, overwriting any existing file.
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (InventoryItem item in items)
                {
                    writer.WriteLine(BuildLine(item));
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Could not save the inventory: " + ex.Message);
        }
    }

    public static List<InventoryItem> Load(string path) //Loads a list of items from the given 
    //file path, replacing any existing items.
    {
        List<InventoryItem> items = new List<InventoryItem>();

        if (!File.Exists(path))
        {
            return items;
        }

        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string? line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim().Length == 0)
                    {
                        continue;
                    }

                    InventoryItem? item = ParseLine(line);

                    if (item == null)
                    {
                        Console.WriteLine("Skipped an unreadable line.");
                    }
                    else
                    {
                        items.Add(item);
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Could not load the inventory: " + ex.Message);
        }

        return items;
    }

    private static string BuildLine(InventoryItem item) //Builds a line of text to represent the 
    //given item, using the separator character to separate fields.
    {
        string common = string.Join(Separator.ToString(),
            item.Id, item.Name, item.Quantity,
            item.Price.ToString(CultureInfo.InvariantCulture),
            item.Location.Aisle, item.Location.Shelf,
            item.ReorderLevel);

        if (item is PerishableProduct perishable)
        {
            return "P" + Separator + common + Separator
                 + perishable.ExpiryDate.ToString("yyyy-MM-dd");
        }

        Product product = (Product)item;
        return "S" + Separator + common + Separator + product.Category;
    }

    private static InventoryItem? ParseLine(string line) //Parses a line of text to create an 
    //InventoryItem object. Returns null if the line is invalid.
    {
        string[] parts = line.Split(Separator);

        if (parts.Length != FieldCount)
        {
            return null;
        }

        try
        {
            string tag = parts[0];
            int id = int.Parse(parts[1], CultureInfo.InvariantCulture);
            string name = parts[2];
            int quantity = int.Parse(parts[3], CultureInfo.InvariantCulture);
            decimal price = decimal.Parse(parts[4], CultureInfo.InvariantCulture);
            int aisle = int.Parse(parts[5], CultureInfo.InvariantCulture);
            int shelf = int.Parse(parts[6], CultureInfo.InvariantCulture);
            int reorderLevel = int.Parse(parts[7], CultureInfo.InvariantCulture);

            StockLocation location = new StockLocation(aisle, shelf);

            if (tag == "P")
            {
                DateTime expiry = DateTime.ParseExact(parts[8], "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);

                return new PerishableProduct(id, name, quantity, price,
                                             location, reorderLevel, expiry);
            }

            if (tag == "S")
            {
                return new Product(id, name, quantity, price,
                                   location, reorderLevel, parts[8]);
            }

            return null;
        }
        catch (FormatException)
        {
            return null;
        }
    }
}