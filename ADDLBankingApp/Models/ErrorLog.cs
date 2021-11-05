namespace ADDLBankingApp.Models
{
    public partial class ErrorLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime Date { get; set; }
        public string Source { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public string Page { get; set; }
        public string Action { get; set; }

    }
}