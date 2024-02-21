using ApiCrud.Estudantes;
using Microsoft.EntityFrameworkCore;


namespace ApiCrud.Data;

public class AppDbContext : DbContext
{
    public DbSet<Estudante2> Estudantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=banco.sqlite");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();
        
        base.OnConfiguring(optionsBuilder);
    }
}