namespace ADDLBankingApp.Models
{
    public partial class Stadistic
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime Date { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }
        public string Page { get; set; }
        public string Action { get; set; }
    }
}