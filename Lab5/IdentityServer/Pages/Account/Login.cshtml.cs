using Duende.IdentityServer.Extensions;
using IdentityServer.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account;


public class LoginModel(SignInManager<User> signInManager) : PageModel
{
    [BindProperty]
    public required string Username { get; set; }

    [BindProperty]
    public required string Password { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }


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

        var user = await signInManager.UserManager.FindByNameAsync(Username);
        if (user is not null)
        {
            var result = await signInManager.CheckPasswordSignInAsync(
                user,
                Password,
                false
            );
            if (result.Succeeded)
            {
                await signInManager.SignInWithClaimsAsync(user, true, [
                    new("full_name", user.FullName)
                ]);

                return ReturnUrl is null
                    ? RedirectToPage("/Index")
                    : Redirect(ReturnUrl);
            }
        }

        ModelState.AddModelError("", "Name or password is incorrect.");
        return Page();
    }
}