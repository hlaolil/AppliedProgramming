using System.Globalization;

class Program
{
    private const string DataFile = "inventory.txt";

    static void Main(string[] args)
    {
        Inventory inventory = new Inventory();
        inventory.LoadFrom(DataFile);

        Console.WriteLine("Loaded " + inventory.Count + " item(s) from " + DataFile + ".");

        bool running = true;

        while (running)
        {
            ShowMenu();
            string? choice = Console.ReadLine();

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    inventory.DisplayAll();
                    break;
                case "2":
                    AddItem(inventory);
                    break;
                case "3":
                    SearchItem(inventory);
                    break;
                case "4":
                    UpdateQuantity(inventory);
                    break;
                case "5":
                    RemoveItem(inventory);
                    break;
                case "6":
                    inventory.DisplayLowStock();
                    break;
                case "7":
                    inventory.DisplayExpiring(7);
                    break;
                case "8":
                    Console.WriteLine("Total stock value: "
                        + inventory.GetTotalValue().ToString("0.00"));
                    break;
                case "9":
                    Console.WriteLine("Items held: " + inventory.Count);
                    break;
                case "10":
                    running = false;
                    break;
                default:
                    Console.WriteLine("That is not a valid option.");
                    break;
            }
        }

        inventory.SaveTo(DataFile);
        Console.WriteLine("Inventory saved to " + DataFile + ". Goodbye.");
    }

    static void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Inventory Management System ===");
        Console.WriteLine("1. View all items");
        Console.WriteLine("2. Add an item");
        Console.WriteLine("3. Search by name");
        Console.WriteLine("4. Update a quantity");
        Console.WriteLine("5. Remove an item");
        Console.WriteLine("6. View low stock");
        Console.WriteLine("7. View expiring stock");
        Console.WriteLine("8. View total value");
        Console.WriteLine("9. Count items");
        Console.WriteLine("10. Save and exit");
        Console.Write("Choose an option: ");
    }

    static void AddItem(Inventory inventory)
    {
        int id = ReadInt("Id: ");

        if (inventory.FindById(id) != null)
        {
            Console.WriteLine("An item with that id already exists.");
            return;
        }

        Console.Write("Name: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("The name cannot be empty.");
            return;
        }

        name = name.Replace("|", "-");

        int quantity = ReadInt("Quantity: ");
        decimal price = ReadDecimal("Price: ");
        int aisle = ReadInt("Aisle: ");
        int shelf = ReadInt("Shelf: ");
        int reorderLevel = ReadInt("Reorder level: ");

        StockLocation location = new StockLocation(aisle, shelf);

        Console.Write("Is this item perishable? (y/n): ");
        string? answer = Console.ReadLine();
        bool perishable = answer != null && answer.Trim().ToLower() == "y";

        if (perishable)
        {
            DateTime expiry = ReadDate("Expiry date (yyyy-MM-dd): ");

            inventory.AddItem(new PerishableProduct(id, name, quantity, price,
                                                    location, reorderLevel, expiry));
        }
        else
        {
            Console.Write("Category: ");
            string? category = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(category))
            {
                category = "Uncategorised";
            }

            category = category.Replace("|", "-");

            inventory.AddItem(new Product(id, name, quantity, price,
                                          location, reorderLevel, category));
        }

        Console.WriteLine("Item added.");
    }

    static void SearchItem(Inventory inventory)
    {
        Console.Write("Name to search for: ");
        string? term = Console.ReadLine();

        List<InventoryItem> results = inventory.Search(term);

        if (results.Count == 0)
        {
            Console.WriteLine("No items matched that name.");
            return;
        }

        foreach (InventoryItem item in results)
        {
            Console.WriteLine(item.GetDisplayLine());
        }
    }

    static void UpdateQuantity(Inventory inventory)
    {
        int id = ReadInt("Id of the item to update: ");
        int quantity = ReadInt("New quantity: ");

        if (inventory.UpdateQuantity(id, quantity))
        {
            Console.WriteLine("Quantity updated.");
        }
        else
        {
            Console.WriteLine("No item found with id " + id + ".");
        }
    }

    static void RemoveItem(Inventory inventory)
    {
        int id = ReadInt("Id of the item to remove: ");

        if (inventory.RemoveItem(id))
        {
            Console.WriteLine("Item removed.");
        }
        else
        {
            Console.WriteLine("No item found with id " + id + ".");
        }
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            int value;

            if (int.TryParse(Console.ReadLine(), out value))
            {
                return value;
            }

            Console.WriteLine("Please enter a whole number.");
        }
    }

    static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            decimal value;

            if (decimal.TryParse(Console.ReadLine(), out value))
            {
                return value;
            }

            Console.WriteLine("Please enter a number.");
        }
    }

    static DateTime ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            DateTime value;

            if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            {
                return value;
            }

            Console.WriteLine("Please use the format yyyy-MM-dd, for example 2026-12-31.");
        }
    }
}