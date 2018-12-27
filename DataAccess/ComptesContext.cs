using Microsoft.EntityFrameworkCore;
using MonProjetBanking_Back.Models;

namespace MonProjetBanking_Back.DataAccess
{
    public class ComptesContext : DbContext
    {
        public ComptesContext(DbContextOptions<ComptesContext> options) : base(options)
        {            
        }
        public DbSet<Compte> Comptes { get; set; }
    }
}