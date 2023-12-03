using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Account;


public class LoginModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; init; }


    public IActionResult OnGet() => User.Identity?.IsAuthenticated is true
        ? RedirectToPage("/Index")
        : Challenge(new AuthenticationProperties
        {
            RedirectUri = ReturnUrl
        });
}