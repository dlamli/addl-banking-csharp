namespace ADDLBankingApp.Models
{
    public partial class SinpeM
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string AccountTarget { get; set; }
        public System.DateTime TransactionDate { get; set; }
    }
}