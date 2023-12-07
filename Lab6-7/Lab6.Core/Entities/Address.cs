namespace Lab6.Core.Entities;

public sealed class Address
{
    public Guid Id { get; set; }

    public string City { get; set; } = string.Empty;

    public string Zip { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;
}
