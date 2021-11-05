namespace ADDLBankingApp.Models
{
    public partial class TimeDeposit
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal Percentage { get; set; }

    }
}