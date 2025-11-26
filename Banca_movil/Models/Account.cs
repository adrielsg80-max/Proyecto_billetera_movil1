namespace Banca_movil.Models
{
    public class Account
    {
        
            public int Id { get; set; }
            public int UserId { get; set; }          // Relación con User
            public string AccountNumber { get; set; }
            public decimal Balance { get; set; }
            public string Currency { get; set; } = "USD";
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            // Relación con usuario
            public User User { get; set; }

            // Relación con transacciones
            public List<Transaction> Transactions { get; set; }
        }

    }

