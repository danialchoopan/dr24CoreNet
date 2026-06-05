namespace dr24CoreNet.Domain.Entities;

public class Wallet
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public decimal Balance { get; set; }
    public List<WalletTransaction> Transactions { get; set; } = new();
}

public class WalletTransaction
{
    public int Id { get; set; }
    public int WalletId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCredit { get; set; } // true for deposit, false for withdrawal
}

public class ChatRoom
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    public List<ChatMessage> Messages { get; set; } = new();
}

public class ChatMessage
{
    public int Id { get; set; }
    public int ChatRoomId { get; set; }
    public string SenderId { get; set; } = string.Empty; // User role/ID
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}

public class Prescription
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    public DateTime IssuedAt { get; set; }
    public List<PrescriptionItem> Items { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}

public class PrescriptionItem
{
    public int Id { get; set; }
    public int PrescriptionId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
}
