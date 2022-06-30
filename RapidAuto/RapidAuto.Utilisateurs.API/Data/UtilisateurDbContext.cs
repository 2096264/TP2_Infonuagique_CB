using Microsoft.EntityFrameworkCore;
using RapidAuto.Utilisateurs.API.Models;

namespace RapidAuto.Utilisateurs.API.Data
{
    public class UtilisateurDbContext : DbContext
    {
        public UtilisateurDbContext(DbContextOptions<UtilisateurDbContext> options) : base(options)
        {

        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
    }
}
