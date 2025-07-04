using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonRole> PersonRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cartoon> Cartoons { get; set; }
        public DbSet<CountryCartoon> CountryCartoons { get; set; }
        public DbSet<CartoonGenre> CartoonGenres { get; set; }
        public DbSet<CartoonCollection> CartoonCollections { get; set; }
        public DbSet<CartoonStudio> CartoonStudios { get; set; }
        public DbSet<CartoonDirector> CartoonDirectors { get; set; }
        public DbSet<CartoonActor> CartoonActors { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<CartoonSlider> CartoonSliders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.CartoonId })
                .IsUnique(); 
        }
    }
  
}

