using Microsoft.AspNetCore.Identity;

namespace api.Model;

public class AppUser : IdentityUser
{
    public List<PortFolio> PortFolios {get;set;} = [];
}
