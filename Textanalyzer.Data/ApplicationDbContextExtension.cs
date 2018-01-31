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
            if (!context.Users.Any())
            {
                context.Users.Add(new ApplicationUser() { UserName = "test@test.at" });
            }

            if (!context.Texts.Any())
            {
                context.Texts.Add(new Text() { UserName = "test@test.at", Value = "Diktum ist ein Ausdruck für einen bedeutsamen, pointierten Ausspruch, eigentlich das Gesagte." });
                context.Texts.Add(new Text() { UserName = "test@test.at", Value = "Das ist kein Test. Er hat mehrere Sätze. Die zusammenhängen sollten." });
                context.Texts.Add(new Text() { UserName = "test@test.at", Value = "Das ist ein Test. Er hat mehrere Sätze. Die zusammenhängen sollten." });
                context.SaveChanges();
            }

            if (!context.Sentences.Any())
            {
                context.Sentences.Add(new Sentence() { Value = "Diktum ist ein Ausdruck für einen bedeutsamen, pointierten Ausspruch, eigentlich das Gesagte", TextID = 1 });
                context.Sentences.Add(new Sentence() { Value = "Das ist kein Test", TextID = 2 });
                context.Sentences.Add(new Sentence() { Value = "Er hat mehrere Sätze", TextID = 2 });
                context.Sentences.Add(new Sentence() { Value = "Die zusammenhängen sollten", TextID = 2 });
                context.Sentences.Add(new Sentence() { Value = "Das ist ein Test", TextID = 3 });
                context.Sentences.Add(new Sentence() { Value = "Er hat mehrere Sätze", TextID = 3 });
                context.Sentences.Add(new Sentence() { Value = "Die zusammenhängen sollten", TextID = 3 });
                context.SaveChanges();
            }

            if (!context.Words.Any())
            {
                context.Words.Add(new Word() { SentenceID = 1, Value = "Diktum" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "ist" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "ein" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "Ausdruck" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "für" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "einen" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "bedeutsamen" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "pointierten" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "Ausspruch" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "eigentlich" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "das" });
                context.Words.Add(new Word() { SentenceID = 1, Value = "Gesagte" });
                context.Words.Add(new Word() { SentenceID = 2, Value = "Das" });
                context.Words.Add(new Word() { SentenceID = 2, Value = "ist" });
                context.Words.Add(new Word() { SentenceID = 2, Value = "kein" });
                context.Words.Add(new Word() { SentenceID = 2, Value = "Test" });
                context.Words.Add(new Word() { SentenceID = 3, Value = "Er" });
                context.Words.Add(new Word() { SentenceID = 3, Value = "hat" });
                context.Words.Add(new Word() { SentenceID = 3, Value = "mehrere" });
                context.Words.Add(new Word() { SentenceID = 3, Value = "Sätze" });
                context.Words.Add(new Word() { SentenceID = 4, Value = "Die" });
                context.Words.Add(new Word() { SentenceID = 4, Value = "zusammenhängen" });
                context.Words.Add(new Word() { SentenceID = 4, Value = "sollten" });
                context.Words.Add(new Word() { SentenceID = 5, Value = "Das" });
                context.Words.Add(new Word() { SentenceID = 5, Value = "ist" });
                context.Words.Add(new Word() { SentenceID = 5, Value = "ein" });
                context.Words.Add(new Word() { SentenceID = 5, Value = "Test" });
                context.Words.Add(new Word() { SentenceID = 6, Value = "Er" });
                context.Words.Add(new Word() { SentenceID = 6, Value = "hat" });
                context.Words.Add(new Word() { SentenceID = 6, Value = "mehrere" });
                context.Words.Add(new Word() { SentenceID = 6, Value = "Sätze" });
                context.Words.Add(new Word() { SentenceID = 7, Value = "Die" });
                context.Words.Add(new Word() { SentenceID = 7, Value = "zusammenhängen" });
                context.Words.Add(new Word() { SentenceID = 7, Value = "sollten" });
                context.SaveChanges();
            }
        }
    }
}
