using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Account;


[Authorize]
public class LogoutModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; init; }


    public IActionResult OnGet() => SignOut(
        new AuthenticationProperties
        {
            RedirectUri = ReturnUrl
        },
        CookieAuthenticationDefaults.AuthenticationScheme,
        OpenIdConnectDefaults.AuthenticationScheme
    );
}