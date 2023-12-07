namespace Lab6.Core.Entities;

public sealed class PrescriptionItem
{
    public Guid Id { get; set; }

    public uint Quantity { get; set; }

    public string Instruction { get; set; } = string.Empty;

    public Guid PrescriptionId { get; set; }

    public Guid MedicationId { get; set; }
}
