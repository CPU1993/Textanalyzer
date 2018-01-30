using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Textanalyzer.Data.Entities;

namespace Textanalyzer.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Text> Texts { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Query> Queries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Word>().HasOne(w => w.Sentence).WithMany(s => s.Words);
            builder.Entity<Sentence>().HasOne(s => s.Previous).WithOne(s => s.Next);
            builder.Entity<Sentence>().HasOne(s => s.Next).WithOne(s => s.Previous);
            builder.Entity<Sentence>().HasOne(s => s.Text).WithMany(t => t.Sentences);
        }

    }
}
