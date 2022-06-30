using Microsoft.EntityFrameworkCore;
using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.Data
{
    public class VehiculeDbContext : DbContext
    {
        public VehiculeDbContext(DbContextOptions<VehiculeDbContext> options) : base(options)
        {

        }

        public DbSet<Vehicule> Vehicules { get; set; }
    }
}
