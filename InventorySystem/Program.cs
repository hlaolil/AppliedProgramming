using System.Globalization;

// This is the entry point. It handles everything the user
// sees and types.
//
// It is the simplest class in the whole system. Its job is
// to load the data, show the menu over and over, pass the real work
// to Inventory, and save on the way out.

class Program
{
    private const string DataFile = "inventory.txt"; // This never changes 
    // while the program runs.
    // Specifying the file name once here means changing it later is a
    // one-line job.

    static void Main(string[] args) // The Main method that C# runs first when the program
    // starts. Every console program has exactly one.
    {
        // Create the inventory and populate it from the file.
        Inventory inventory = new Inventory();
        inventory.LoadFrom(DataFile);

        Console.WriteLine("Loaded " + inventory.Count + " item(s) from " + DataFile + ".");

        bool running = true; // A true/false flag that controls the loop below.
        // The save happens AFTER the loop finishes. If we
        // quit from inside the loop, we would jump straight out of the
        // program and never save - losing everything the user just did.

        while (running)
        {
            ShowMenu();
            string? choice = Console.ReadLine(); //Read the user's choice as a string from the console.

            Console.WriteLine();

            switch (choice) // compares "choice" against each case.
            // "break" ends that case so it does not fall into the next.
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
                    running = false; // This does not quit. It just sets the flag false,
                    // so the loop ends naturally and the save below runs.
                    break;
                default:  // "default" catches anything that matched no case.
                    Console.WriteLine("That is not a valid option.");
                    break;
            }
        }

        inventory.SaveTo(DataFile); // Only reached once the loop has ended
        Console.WriteLine("Inventory saved to " + DataFile + ". Goodbye.");
    }

    static void ShowMenu() //Display the menu of options.
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
        Console.Write("Choose an option: "); // Write, not WriteLine, so the cursor stays 
        // on the same line
        // and the user types their answer right after the prompt.
    }

    static void AddItem(Inventory inventory) // Asks the user for all the details of a 
    // new item, then builds
    // either a Product or a PerishableProduct and adds it.
    {
        int id = ReadInt("Id: ");

        if (inventory.FindById(id) != null) //Check that no two items have the same id. 
        //If so, print a message and return to menu.
        {
            Console.WriteLine("An item with that id already exists.");
            return;
        }

        Console.Write("Name: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name)) //check that the name is not empty or 
        //whitespace. If so, print a message and return to menu.
        {
            Console.WriteLine("The name cannot be empty.");
            return;
        }

        name = name.Replace("|", "-"); //replace any "|" characters with "-" 
        //to avoid breaking the file format.

        int quantity = ReadInt("Quantity: ");
        decimal price = ReadDecimal("Price: ");
        int aisle = ReadInt("Aisle: ");
        int shelf = ReadInt("Shelf: ");
        int reorderLevel = ReadInt("Reorder level: ");

        StockLocation location = new StockLocation(aisle, shelf); // Build the struct from the two numbers we just collected.

        Console.Write("Is this item perishable? (y/n): ");
        string? answer = Console.ReadLine();
        bool perishable = answer != null && answer.Trim().ToLower() == "y"; //check if the answer 
        //is not null or whitespace and is "y" (case-insensitive).

        if (perishable) //Branch on which type of item to build. 
        //If perishable, ask for the expiry date and build a PerishableProduct. 
        // Otherwise, ask for the category and build a Product.
        {
            DateTime expiry = ReadDate("Expiry date (yyyy-MM-dd): ");

            inventory.AddItem(new PerishableProduct(id, name, quantity, price,
                                                    location, reorderLevel, expiry));
        }
        else
        {
            Console.Write("Category: ");
            string? category = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(category)) // If they left blank, fill in something 
            // sensible rather than refusing the whole item.
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
            Console.WriteLine(item.GetDisplayLine()); //Print each result without checking 
            //what kind it is.
            // Standard and perishable items format themselves
            // differently, and each one handles that itself.

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
        while (true) //Keep asking until the user types something valid.

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
        while (true) //Keep asking until the user types something valid.
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
        while (true) //Keep asking until the user types something valid.
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