namespace ADDLBankingApp.Models
{
    public partial class Transfer
    {
        public int Id { get; set; }
        public int AccountOrigin { get; set; }
        public int AccountDestiny { get; set; }
        public System.DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}