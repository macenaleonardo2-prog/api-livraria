using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Livro> Livros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    
        optionsBuilder.UseSqlite("Data Source=Livraria.db");
    }
}