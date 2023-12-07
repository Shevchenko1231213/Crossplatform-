using Lab6.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Core.Dal;

public sealed class LabContext : DbContext
{
    #region data
    private static List<Address> _addresses => new List<Address>()
    {
        new Address
        {
            Id = Guid.Parse("bbff9bbd-63f0-4c82-bbf7-73b5a7b8c0b3"),
            City = "Kiyv",
            Zip = "04116",
            State = string.Empty,
            Country = "Ukraine"
        },
    };

    private static List<Company> _companies => new List<Company>()
    {
        new Company
        {
            Id = Guid.Parse("b47a9243-34a4-443f-b269-617fe6cd0353"),
            Name = "Med_Company",
        }
    };

    private static List<Customer> _customers => new List<Customer>()
    {
        new Customer
        {
            Id = Guid.Parse("ebf9a13b-8ab9-4efe-a01d-e1056cf4597f"),
            FirstName = "Andrey",
            LastName = "Shevchenko",
            Phone = "380978673644",
            JoinedOnUtc = DateTime.UtcNow,
            CardNumber = "4149...",
            CardExpiredOnUtc = DateTime.UtcNow.AddYears(2),
            AddressId = Guid.Parse("bbff9bbd-63f0-4c82-bbf7-73b5a7b8c0b3")
        }
    };

    private static List<Doctor> _doctors => new List<Doctor>()
    {
        new Doctor
        {
            Id = Guid.Parse("b71e6833-f305-4671-8b5c-6dfe7e4e8e5e"),
            FirstName = "Andrew",
            LastName = "Petrov",
            Gender = "M",
            Phone = "380...",
            Email = "ashalet@gmail.com",
            AddressId = Guid.Parse("bbff9bbd-63f0-4c82-bbf7-73b5a7b8c0b3")
        }
    };

    private static List<Medication> _medications => new List<Medication>()
    {
        new Medication
        {
            Id = Guid.Parse("236b4b2f-2d23-4659-8f5b-94a5e7a31b9b"),
            Name = "pigylka",
            Cost = 1500,
            Description = ".net"
        },
        new Medication
        {
            Id = Guid.Parse("09c3c0aa-3031-4ff5-b207-666a3666a680"),
            Name = "pigylka2",
            Cost = 1600,
            Description = ".net"
        }
    };

    private static List<Prescription> _prescriptions => new List<Prescription>()
    {
        new Prescription
        {
            Id = Guid.Parse("f4797963-50d8-4ff3-8c0c-0c552d47d18f"),
            ReceivedOnUtc = DateTime.UtcNow.AddHours(-3),
            RenewalOnUtc = DateTime.UtcNow.AddHours(-2),
            SendToDoctorOnUtc = DateTime.UtcNow.AddHours(-2),
            ProcessedOnUtc = DateTime.UtcNow.AddHours(-1),
            ReceivedFromDoctorOnUtc= DateTime.UtcNow,
            SentToCompanyOnUtc = DateTime.UtcNow.AddHours(1),
            CustomerId = Guid.Parse("ebf9a13b-8ab9-4efe-a01d-e1056cf4597f"),
            DoctorId = Guid.Parse("b71e6833-f305-4671-8b5c-6dfe7e4e8e5e"),
            CompanyId = Guid.Parse("b47a9243-34a4-443f-b269-617fe6cd0353"),
        }
    };

    private static List<PrescriptionItem> _prescriptionItems => new List<PrescriptionItem>()
    {
        new PrescriptionItem
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            Instruction = "lab6",
            PrescriptionId = Guid.Parse("f4797963-50d8-4ff3-8c0c-0c552d47d18f"),
            MedicationId =  Guid.Parse("236b4b2f-2d23-4659-8f5b-94a5e7a31b9b")
        },
         new PrescriptionItem
        {
            Id = Guid.NewGuid(),
            Quantity = 2,
            Instruction = "lab6",
            PrescriptionId =  Guid.Parse("f4797963-50d8-4ff3-8c0c-0c552d47d18f"),
            MedicationId = Guid.Parse("09c3c0aa-3031-4ff5-b207-666a3666a680")
        }
    };
    #endregion

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Doctor> Doctors => Set<Doctor>();

    public DbSet<Medication> Medications => Set<Medication>();

    public DbSet<Prescription> Prescriptions => Set<Prescription>();

    public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();

    public LabContext(DbContextOptions<LabContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(options =>
        {
            options.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Address>().HasData(_addresses);

        modelBuilder.Entity<Company>(options =>
        {
            options.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Company>().HasData(_companies);

        modelBuilder.Entity<Customer>(options =>
        {
            options.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Customer>().HasData(_customers);

        modelBuilder.Entity<Doctor>(options =>
        {
            options.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Doctor>().HasData(_doctors);

        modelBuilder.Entity<Medication>(options =>
        {
            options.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Medication>().HasData(_medications);

        modelBuilder.Entity<Prescription>(options =>
        {
            options.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Prescription>().HasData(_prescriptions);

        modelBuilder.Entity<PrescriptionItem>(options =>
        {
            options.HasKey(x => x.Id);
        });

        modelBuilder.Entity<PrescriptionItem>().HasData(_prescriptionItems);
    }
}
