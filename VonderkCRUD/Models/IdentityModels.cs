using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VonderkCRUD.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<VonderkCRUD.Producto> Productos { get; set; }

        public System.Data.Entity.DbSet<VonderkCRUD.Categoria> Categorias { get; set; }

        public System.Data.Entity.DbSet<VonderkCRUD.Marca> Marcas { get; set; }

        public System.Data.Entity.DbSet<VonderkCRUD.SlidePrincipal> SlidePrincipal { get; set; }

        public System.Data.Entity.DbSet<VonderkCRUD.Trabajo> Trabajos { get; set; }

        public System.Data.Entity.DbSet<VonderkCRUD.FichasTecnica> FichasTecnicas { get; set; }
    }
}