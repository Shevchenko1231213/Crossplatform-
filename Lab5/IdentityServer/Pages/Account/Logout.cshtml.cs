using Duende.IdentityServer.Services;
using IdentityServer.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account;


[Authorize]
public class LogoutModel(
    IIdentityServerInteractionService interactionService,
    SignInManager<User> signInManager
) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? LogoutId { get; init; }


    public async Task<IActionResult> OnGet()
    {
        var context = await interactionService.GetLogoutContextAsync(LogoutId);
        await signInManager.SignOutAsync();
        return context?.PostLogoutRedirectUri is null
            ? RedirectToPage("/Index")
            : Redirect(context.PostLogoutRedirectUri);
    }
}