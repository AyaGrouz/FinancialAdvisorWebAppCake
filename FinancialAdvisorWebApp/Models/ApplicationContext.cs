using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class ApplicationContext : DbContext
    {

        public DbSet<QUESTION> Questions { get; set; }
        public DbSet<CHOICE> Choices { get; set; }
        public DbSet<INVESTISSEUR> Investisseurs { get; set; }
        public DbSet<QUESTIONNAIRE> Questionnaires { get; set; }
        public DbSet<FACIAL_EMOTIONS> Facial_Emotions { get; set; }
        public DbSet<SPEECH_EMOTIONS> Speech_Emotions { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QUEST_INVEST>()
        .HasKey(bc => new { bc.ID_INVEST, bc.ID_QUESTIONNAIRE });
            modelBuilder.Entity<QUEST_INVEST>()
                .HasOne(bc => bc.INVESTISSEUR)
                .WithMany(b => b.Quest_Invest)
                .HasForeignKey(bc => bc.ID_INVEST);
            modelBuilder.Entity<QUEST_INVEST>()
                .HasOne(bc => bc.QUESTIONNAIRE)
                .WithMany(c => c.Quest_Invest)
                .HasForeignKey(bc => bc.ID_QUESTIONNAIRE);
        }
    }
}
