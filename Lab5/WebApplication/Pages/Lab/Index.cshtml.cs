using Labs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Lab;


[Authorize]
public class IndexModel : PageModel
{
    private static readonly Func<string, string>[] _runMethods =
    [
        Lab1.Run, Lab2.Run, Lab3.Run
    ];


    [BindProperty(SupportsGet = true)]
    public required int Number { get; init; }

    [BindProperty]
    public string? Input { get; set; }

    [BindProperty]
    public string? Output { get; set; }


    public void OnGet()
    {
        if (ModelState.IsValid && !(Number >= 1 && Number <= _runMethods.Length))
        {
            ModelState.AddModelError("", $"Invalid laboratory work ¹{Number}.");
        }
    }


    public void OnPost()
    {
        if (ModelState.IsValid && !(Number >= 1 && Number <= _runMethods.Length))
        {
            ModelState.AddModelError("", $"Invalid laboratory work ¹{Number}.");
        }
        if (ModelState.IsValid)
        {
            Output = _runMethods[Number - 1](Input ?? "");
        }
    }
}