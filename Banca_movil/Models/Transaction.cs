namespace Banca_movil.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }       // Relación con Account
        public decimal Amount { get; set; }
        public string Type { get; set; }         // "Deposit", "Withdrawal", "Transfer"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }

        // Relación con cuenta
        public Account Account { get; set; }
    }
}
