using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Entities
{
    public class IdentityDbContext : IdentityDbContext<
                                    User, // Tipo de la entidad User
                                    Role, // Tipo de la entidad Role
                                    Guid, // Tipo de la clave primaria de User y Role
                                    IdentityUserClaim<Guid>, // Tipo para los claims de User
                                    IdentityUserRole<Guid>, // Tipo para los roles de User
                                    IdentityUserLogin<Guid>, // Tipo para los logins de User
                                    IdentityRoleClaim<Guid>, // Tipo para los claims de Role
                                    IdentityUserToken<Guid>>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        }

    }
}
