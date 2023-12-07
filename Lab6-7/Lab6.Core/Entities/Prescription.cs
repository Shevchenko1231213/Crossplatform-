namespace Lab6.Core.Entities;

public sealed class Prescription
{
    public Guid Id { get; set; }

    public DateTime ReceivedOnUtc { get; set; }

    public DateTime RenewalOnUtc { get; set; }

    public DateTime SendToDoctorOnUtc { get; set; }

    public DateTime ProcessedOnUtc { get; set; }

    public DateTime ReceivedFromDoctorOnUtc { get; set; }

    public DateTime SentToCompanyOnUtc { get; set; }

    public Guid CustomerId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid CompanyId { get; set; }
}
