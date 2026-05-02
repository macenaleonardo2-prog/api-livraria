using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();


app.MapPost("/livro", async (Livro livro, AppDbContext db) =>
{
    db.Livros.Add(livro);
    await db.SaveChangesAsync();
    return Results.Created($"/livros/{livro.Id}", livro);
});


app.MapGet("/livros", async (AppDbContext db) => 
    await db.Livros.ToListAsync());


app.MapGet("/livros/{autor}", async (string autor, AppDbContext db) =>
{
    var livros = await db.Livros
        .Where(l => l.Autor.ToLower().Contains(autor.ToLower()))
        .ToListAsync();
    return livros.Any() ? Results.Ok(livros) : Results.NotFound();
});


app.MapDelete("/livros/{id}", async (int id, AppDbContext db) =>
{
    var livro = await db.Livros.FindAsync(id);
    if (livro is null) return Results.NotFound();

    db.Livros.Remove(livro);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public int AnoPublicacao { get; set; }
}


public class AppDbContext : DbContext
{
    public DbSet<Livro> Livros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
  
        optionsBuilder.UseSqlite("Data Source=Livraria.db");
    }
}