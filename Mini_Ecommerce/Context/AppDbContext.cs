﻿using Microsoft.EntityFrameworkCore;
using Mini_Ecommerce.Models;

namespace Mini_Ecommerce.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Category>().HasKey(c => c.CategoryId);

        mb.Entity<Category>().
            Property(c => c.Name).
             HasMaxLength(100).
                IsRequired();

        mb.Entity<Category>().
            Property(c => c.Name).
             HasMaxLength(100).
                IsRequired();

        mb.Entity<Product>().
            Property(c => c.Description).
                HasMaxLength(255).
                    IsRequired();

        mb.Entity<Product>().
            Property(c => c.ImageURL).
                HasMaxLength(255).
                      IsRequired();

        mb.Entity<Product>().
            Property(c => c.Price).
                HasPrecision(12, 2);

        mb.Entity<Category>()
            .HasMany(g => g.Products)
                .WithOne(c => c.Category)
                    .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Category>().HasData(
           new Category
           {
               CategoryId = 1,
               Name = "Material Escolar",
           },
           new Category
           {
               CategoryId = 2,
               Name = "Acessórios",
           }
       );
    }   

}
