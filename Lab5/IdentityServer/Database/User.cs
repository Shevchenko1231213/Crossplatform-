using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Database;


public class User : IdentityUser
{
    public required string FullName { get; set; }
}