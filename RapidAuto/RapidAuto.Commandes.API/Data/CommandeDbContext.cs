using Microsoft.EntityFrameworkCore;
using RapidAuto.Commandes.API.Models;

namespace RapidAuto.Commandes.API.Data
{
    public class CommandeDbContext : DbContext
    {
        public CommandeDbContext(DbContextOptions<CommandeDbContext> options) : base(options)
        {

        }

        public DbSet<Commande> Utilisateurs { get; set; }
    }
}
