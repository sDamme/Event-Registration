namespace Event_Registration.Models
{
    public abstract class Guest
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public PaymentType PaymentType { get; set; }
        public abstract string ExtraInformation { get; set; }
    }

    public enum PaymentType
    {
        Cash,
        BankTransfer
    }
}
