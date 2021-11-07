namespace ADDLBankingApp.Models
{

    public partial class Card
    {
        public int Id { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CCV { get; set; }
        public System.DateTime DueDate { get; set; }
        public string Provider { get; set; }

    }
}
