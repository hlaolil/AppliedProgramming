//For items that goes off - milk, medicine, food.
public class PerishableProduct : InventoryItem /// Inherits from InventoryItem
{
    public DateTime ExpiryDate { get; set; }

    public PerishableProduct(int id, string name, int quantity, decimal price,
                             StockLocation location, int reorderLevel, DateTime expiryDate)
        : base(id, name, quantity, price, location, reorderLevel)
    {
        ExpiryDate = expiryDate;
    }

    public int DaysUntilExpiry()
    {
        return (ExpiryDate.Date - DateTime.Now.Date).Days;
    }

    public bool IsExpired()
    {
        return DaysUntilExpiry() < 0;
    }

    public bool IsExpiringSoon(int days)
    {
        return !IsExpired() && DaysUntilExpiry() <= days;
    }

    public override string GetItemType()
    {
        return "Perishable";
    }

    public override string GetDisplayLine()
    {
        string line = base.GetDisplayLine()
                    + "  exp:" + ExpiryDate.ToString("yyyy-MM-dd");

        if (IsExpired())
        {
            line += "  [EXPIRED]";
        }
        else if (IsExpiringSoon(7))
        {
            line += "  [" + DaysUntilExpiry() + " DAYS LEFT]";
        }

        return line;
    }
}