public struct StockLocation
{
    public int Aisle { get; set; }
    public int Shelf { get; set; }

    public StockLocation(int aisle, int shelf)
    {
        Aisle = aisle;
        Shelf = shelf;
    }

    public string Describe() // Builds a readable label, for example
    {
        return "A" + Aisle + "-S" + Shelf;
    }

    public override string ToString() //Replace the built-in ToString method with our own.
    // Without this, printing a StockLocation would show the type name instead of something useful.

    {
        return Describe();
    }
}