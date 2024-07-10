using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApiIdentityEndPoint.Models
{
	public class User : IdentityUser
	{
        public string Document { get; set; } = string.Empty;
    }
}
