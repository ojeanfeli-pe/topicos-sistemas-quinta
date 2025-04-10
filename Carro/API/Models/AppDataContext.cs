using System;
using API.Models;
using Microsoft.EntityFrameworkCore;

public class AppDataContext : DbContext{
    public DbSet<Carro> Carros {get;set;}
    public DbSet<Modelo> Modelos {get;set;}


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlite("Data Source=jean.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Modelo>().HasData(
            new Modelo() {Id = 1, Nome = "Sedan"},
            new Modelo() {Id = 2, Nome = "Hatch"},
            new Modelo() {Id = 3, Nome = "Hatchback"},
            new Modelo() {Id = 4, Nome = "SUV"}

        );
    }
}