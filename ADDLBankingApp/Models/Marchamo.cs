namespace ADDLBankingApp.Models
{
    public partial class Marchamo
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string NumberPlate { get; set; }
        public string VehicleType { get; set; }

    }
}