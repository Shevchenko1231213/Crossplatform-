namespace Lab6.Core.Entities;

public sealed class Customer
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime JoinedOnUtc { get; set; }

    public string CardNumber { get; set; } = string.Empty;

    public DateTime CardExpiredOnUtc { get; set; }

    public Guid AddressId { get; set; }
}
