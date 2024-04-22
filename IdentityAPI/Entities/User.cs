using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Entities
{
    public class User: IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
    }
}
