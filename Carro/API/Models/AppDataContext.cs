using System;
using API.Models;
using Microsoft.EntityFrameworkCore;

public class AppDataContext : DbContext{
    public DbSet<Carro> Carros {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlite("Data Source=jean.db");
    }
}