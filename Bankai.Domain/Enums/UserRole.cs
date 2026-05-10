namespace Bankai.Domain.Enums;

/// <summary>
/// Defines user roles for Role-Based Access Control (RBAC).
/// VG requirement: Admin can manage products; User can only view and order.
/// </summary>
public enum UserRole
{
    User = 0,
    Admin = 1
}
