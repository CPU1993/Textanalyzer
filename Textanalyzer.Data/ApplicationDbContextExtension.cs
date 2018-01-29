using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Textanalyzer.Data.Data;
using System.IO;
using Textanalyzer.Data.Entities;
using Newtonsoft.Json;
using Textanalyzer.Data;

namespace Textanalyzer.Data
{
    public static class ApplicationDbContextExtension
    {
        public static bool AllMigrationsApplied(this ApplicationDbContext context)
        {
            var applied = context.GetService<IHistoryRepository>().GetAppliedMigrations().Select(m => m.MigrationId);
            var total = context.GetService<IMigrationsAssembly>().Migrations.Select(m => m.Key);
            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this ApplicationDbContext context)
        {
            if (!context.Texts.Any())
            {
                List<Text> texts = JsonConvert.DeserializeObject<List<Text>>(File.ReadAllText("../Textanalyzer.Data/seed" + Path.DirectorySeparatorChar + "texts.json"));
                context.AddRange(texts);
                context.SaveChanges();
            }

            if (!context.Sentences.Any())
            {
                List<Sentence> sentences = JsonConvert.DeserializeObject<List<Sentence>>(File.ReadAllText("../Textanalyzer.Data/seed" + Path.DirectorySeparatorChar + "sentences.json"));
                context.AddRange(sentences);
                context.SaveChanges();
            }

            if (!context.Words.Any())
            {
                List<Word> words = JsonConvert.DeserializeObject<List<Word>>(File.ReadAllText("../Textanalyzer.Data/seed" + Path.DirectorySeparatorChar + "words.json"));
                context.AddRange(words);
                context.SaveChanges();
            }
        }
    }
}
