namespace Bankai.Domain.Entities;

/// <summary>
/// Represents a product in the Bankai catalog.
/// This entity lives in the Domain layer — no framework dependencies.
/// </summary>
public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    // Relationship: Many products belong to one Category
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    // Domain constructor enforces invariants
    public Product(string name, string description, decimal price, int stock, int categoryId)
    {
        SetName(name);
        SetDescription(description);
        SetPrice(price);
        SetStock(stock);
        CategoryId = categoryId;
    }

    // EF Core requires parameterless constructor
    protected Product() { }

    // Domain methods — business rules live here
    public void UpdateDetails(string name, string description, decimal price, int stock)
    {
        SetName(name);
        SetDescription(description);
        SetPrice(price);
        SetStock(stock);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
        Name = name.Trim();
    }

    private void SetDescription(string description)
        => Description = description?.Trim() ?? string.Empty;

    private void SetPrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        Price = price;
    }

    private void SetStock(int stock)
    {
        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative.", nameof(stock));
        Stock = stock;
    }
}
