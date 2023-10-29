using Microsoft.AspNetCore.Identity;

namespace RestAPI_Library_Management_System.models
{
    public class ApplicationUser: IdentityUser
    {

        Patron Patron { get; set; }
    }
}
