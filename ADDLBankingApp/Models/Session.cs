namespace ADDLBankingApp.Models
{
    public partial class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateExpiration { get; set; }
        public string Status { get; set; }
    }
}