public class PerishableProduct : InventoryItem
{
    public DateTime ExpiryDate { get; set; }

    public PerishableProduct(int id, string name, int quantity, decimal price, 
                             StockLocation location, int reorderLevel, DateTime expiryDate)
        : base(id, name, quantity, price, location, reorderLevel)
    {
        ExpiryDate = expiryDate;
    }

    public override string GetItemType()
    {
        return "Perishable";
    }

    public bool IsExpiringSoon(int days)
    {
        return ExpiryDate <= DateTime.Now.AddDays(days);
    }

    public override string GetDisplayLine()
    {
        string line = base.GetDisplayLine() + "  exp:" + ExpiryDate.ToString("yyyy-MM-dd");

        if (IsExpiringSoon(7))
        {
            line += "  [EXPIRING SOON]";
        }

        return line;
    }
}