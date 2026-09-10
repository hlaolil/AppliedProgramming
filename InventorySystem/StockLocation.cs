using System;

public struct StockLocation
{
    public int Aisle { get; set; }
    public int Shelf { get; set; }

    public StockLocation(int aisle, int shelf)
    {
        Aisle = aisle;
        Shelf = shelf;
    }

    public string Describe()
    {
        return "A" + Aisle + "-S" + Shelf;
    }

    public override string ToString()
    {
        return Describe();
    }
}