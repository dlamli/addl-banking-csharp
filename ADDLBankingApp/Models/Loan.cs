namespace ADDLBankingApp.Models
{
    public partial class Loan
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}