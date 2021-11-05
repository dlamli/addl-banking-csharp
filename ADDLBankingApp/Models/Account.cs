namespace ADDLBankingApp.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CurrencyId { get; set; }
        public string Description { get; set; }
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public int CardId { get; set; }
        public string PhoneNumber { get; set; }

    }
}
