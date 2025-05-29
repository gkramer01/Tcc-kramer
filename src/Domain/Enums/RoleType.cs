using System.ComponentModel;

namespace Domain.Enums
{
    public enum RoleType
    {
        [Description("Administrator")] Admin = 1,
        [Description("Customer")] Customer = 2,
        [Description("Seller")] Seller = 3,
        [Description("Shopkeeper")] Shopkeeper = 4
    }
}