namespace Bankai.Domain.Entities;

/// <summary>
/// Order entity — connects Users to Products.
/// Demonstrates second entity relationship.
/// </summary>
public class Order
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    public DateTime PlacedAt { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = "Pending";

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private readonly List<OrderItem> _items = new();

    public Order(int userId, IEnumerable<OrderItem> items)
    {
        UserId = userId;
        PlacedAt = DateTime.UtcNow;
        _items = items.ToList();
        TotalAmount = _items.Sum(i => i.LineTotal);
    }

    protected Order() { }

    public void MarkShipped() => Status = "Shipped";
    public void MarkDelivered() => Status = "Delivered";
    public void Cancel() => Status = "Cancelled";
}

/// <summary>Value object inside Order (Order Item line).</summary>
public class OrderItem
{
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal LineTotal => Quantity * UnitPrice;

    public OrderItem(int productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    protected OrderItem() { }
}
