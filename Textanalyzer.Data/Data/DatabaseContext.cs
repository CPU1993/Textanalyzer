using Microsoft.EntityFrameworkCore;
using Textanalyzer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Textanalyzer.Data.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Text> Texts { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<Word> Words { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {            
        }
    }
}
