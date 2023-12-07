using Lab6.Core.Auth;
using Lab6.Core.Dal;
using Lab6.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Core.Services;

public record AuthRequest(
    string FirstName,
    string LastName,
    string Phone
);

public record GetCustomersRequest(
    string SearchColumn,
    string SearchPhrase
);

public record FilteredResponse(
    Guid Id,
    Guid DoctorId,
    Guid PrescriptionId,
    Guid MedicationId,
    string CompanyName,
    uint Quantity,
    decimal Cost
);

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Phone,
    DateTime JoinedOnUtc,
    string CardNumber,
    DateTime CardExpiredOnUtc,
    Guid AddressId
);

public sealed class CoreService
{
    private readonly LabContext _context;
    private readonly JwtTokenProvider _jwtTokenProvider;

    public CoreService(LabContext context, JwtTokenProvider jwtTokenProvider)
        => (_context, _jwtTokenProvider) = (context, jwtTokenProvider);

    public async Task<string> LoginAsync(AuthRequest request)
    {
        JsonWebToken acess_token;

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            var customerByPhone = await _context
                .Customers.FirstOrDefaultAsync(x => x.FirstName == request.FirstName &&
                                                    x.LastName == request.LastName &&
                                                    x.Phone == request.Phone);

            if (customerByPhone is null)
                return string.Empty;

            acess_token = _jwtTokenProvider.CreateToken(customerByPhone.Id.ToString("N"), customerByPhone.FirstName);

            return acess_token.AccessToken;
        }

        var customer = await _context
             .Customers.FirstOrDefaultAsync(x => x.FirstName == request.FirstName &&
                                                    x.LastName == request.LastName);

        if (customer is null)
            return string.Empty;

        acess_token = _jwtTokenProvider.CreateToken(customer.Id.ToString("N"), customer.FirstName);

        return acess_token.AccessToken;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        var customer = await _context
            .Customers.FirstOrDefaultAsync(x => x.FirstName == request.FirstName &&
                                                    x.LastName == request.LastName);

        if (customer is not null)
            return "Already registered!";

        var customerToAdd = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            AddressId = _context.Addresses.FirstOrDefault()!.Id
        };

        _context.Customers.Add(customerToAdd);

        await _context.SaveChangesAsync();

        return _jwtTokenProvider.CreateToken(customerToAdd.Id.ToString("N"), customerToAdd.FirstName).AccessToken;
    }

    public async Task<List<FilteredResponse>> GetListAsync()
    {
        IQueryable<Customer> _customers = _context.Customers;
        IQueryable<Prescription> _prescriptions = _context.Prescriptions;
        IQueryable<Doctor> _doctors = _context.Doctors;
        IQueryable<PrescriptionItem> _prescriptionItems = _context.PrescriptionItems;
        IQueryable<Medication> _medications = _context.Medications;
        IQueryable<Company> _companies = _context.Companies;

        return await (from customer in _context.Customers
                      join prescription in _context.Prescriptions
                        on customer.Id equals prescription.CustomerId
                      join doctor in _context.Doctors
                        on prescription.DoctorId equals doctor.Id
                      join prescriptionItem in _context.PrescriptionItems
                        on prescription.Id equals prescriptionItem.PrescriptionId
                      join medication in _context.Medications
                        on prescriptionItem.MedicationId equals medication.Id
                      join company in _context.Companies
                        on prescription.CompanyId equals company.Id
                      select new FilteredResponse(customer.Id, doctor.Id, prescription.Id, medication.Id, company.Name, prescriptionItem.Quantity, medication.Cost)).ToListAsync();
    }

    public async Task<List<FilteredResponse>> GetCustomersByFiltersAsync(GetCustomersRequest request)
    {
        var customers = await GetListAsync();

        if (!string.IsNullOrEmpty(request.SearchColumn) && !string.IsNullOrEmpty(request.SearchPhrase))
            customers = customers.Where(customer => FilterCustomerByRequest(customer, request)).ToList();

        return customers;
    }

    private bool FilterCustomerByRequest(FilteredResponse customer, GetCustomersRequest request)
    {
        var propertyValue = customer.GetType().GetProperty(request.SearchColumn)?.GetValue(customer, null)?.ToString();
        return propertyValue != null && propertyValue.Contains(request.SearchPhrase, StringComparison.OrdinalIgnoreCase);
    }
}