using Duende.IdentityServer.Extensions;
using IdentityServer.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Pages.Account;


[BindProperties]
public class RegisterModel(UserManager<User> userManager) : PageModel
{
    [MaxLength(50)]
    public required string UserName { get; init; }

    [MaxLength(500)]
    public required string FullName { get; init; }

    [Length(8, 16)]
    public required string Password { get; init; }

    [Compare(nameof(Password))]
    public required string ConfirmationPassword { get; init; }

    [RegularExpression(@"\+380\d{9}", ErrorMessage = "Invalid Ukrainian phone number.")]
    public required string PhoneNumber { get; init; }

    [EmailAddress]
    public required string Email { get; init; }


    public IActionResult OnGet()
    {
        if (User.IsAuthenticated())
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (User.IsAuthenticated())
        {
            return RedirectToPage("/Index");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await userManager.CreateAsync(
            new()
            {
                UserName = UserName,
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                Email = Email
            },
            Password
        );
        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        foreach (var error in result.Errors)
        {
            ModelState.TryAddModelError("", error.Description);
        }
        return Page();
    }
}