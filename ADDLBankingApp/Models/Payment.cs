namespace ADDLBankingApp.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int UserId { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Mount { get; set; }

    }
}