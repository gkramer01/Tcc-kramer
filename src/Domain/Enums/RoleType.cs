using System.ComponentModel;

namespace Domain.Enums
{
    public enum RoleType
    {
        [Description("Administrator")] Admin = 1,
        [Description("User")] User = 2
    }
}
