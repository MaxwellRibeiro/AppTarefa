using AppTarefa.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTarefa.Banco
{
    public class BancoContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }

        public BancoContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Constants.CaminhoDoBanco}");
        }
    }
}
