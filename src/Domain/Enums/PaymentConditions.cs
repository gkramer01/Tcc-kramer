using System.ComponentModel;

namespace Domain.Enums
{
    public enum PaymentConditions
    {
        [Description("Cash")] Cash = 1,
        [Description("CreditCard")] CreditCard = 2,
        [Description("DebitCard")] DebitCard = 3,
        [Description("Pix")] Pix = 4,
        [Description("Paypal")] Paypal = 5
    }
}
