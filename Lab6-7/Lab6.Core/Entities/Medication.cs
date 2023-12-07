namespace Lab6.Core.Entities;

public sealed class Medication
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Cost { get; set; }

    public string Description { get; set; } = string.Empty;
}
