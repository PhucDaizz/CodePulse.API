﻿using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Blogpost> Blogposts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> Blogimages { get; set; }    
    }
}
