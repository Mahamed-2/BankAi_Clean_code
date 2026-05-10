namespace Bankai.Domain.Entities;

/// <summary>
/// Represents a product category.
/// Domain entity — pure C#, no framework coupling.
/// </summary>
public class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;

    // One-to-many: a Category has many Products
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();
    private readonly List<Product> _products = new();

    public Category(string name)
    {
        SetName(name);
    }

    protected Category() { }

    public void Rename(string newName) => SetName(newName);

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));
        Name = name.Trim();
        Slug = name.Trim().ToLowerInvariant().Replace(' ', '-');
    }
}
