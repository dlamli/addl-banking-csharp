namespace ADDLBankingApp.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string Identification { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public System.DateTime Birthdate { get; set; }
        public string Status { get; set; }
        public int RoleId { get; set; }
    }
}