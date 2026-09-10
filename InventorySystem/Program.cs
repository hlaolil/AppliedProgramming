namespace InventorySystem;

class Program
{
    static void Main(string[] args)
    {
        StockLocation loc = new StockLocation(3, 2);
        InventoryItem a = new Product(1, "Bandages", 10, 25m, loc, 5, "Dressings");
        InventoryItem b = new PerishableProduct(2, "Milk", 4, 18m, loc, 5,
                                        DateTime.Now.AddDays(3));

        Console.WriteLine(a.GetDisplayLine());
        Console.WriteLine(b.GetDisplayLine());
    }
}
